using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElasticInelasticCollisionAnswerSubmission
{
	public float? cubeOneInitialMomentum { get; set; }
	public float? cubeTwoInitialMomentum { get; set; }
	public float? cubeOneFinalMomentum { get; set; }
	public float? cubeTwoFinalMomentum {get; set;}
	public float? netInitialMomentum { get; set; }
	public float? netFinalMomentum { get; set; }
	public CollisionType? collisionType { get; set; }

	public ElasticInelasticCollisionAnswerSubmission(
		float? cubeOneInitialMomentum,
		float? cubeTwoInitialMomentum,
		float? cubeOneFinalMomentum,
		float? cubeTwoFinalMomentum,
		float? netInitialMomentum,
		float? netFinalMomentum,
		CollisionType? collisionType
		)
	{
		this.cubeOneInitialMomentum = cubeOneInitialMomentum;
		this.cubeTwoInitialMomentum = cubeTwoInitialMomentum;
		this.cubeOneFinalMomentum = cubeOneFinalMomentum;
		this.cubeTwoFinalMomentum = cubeTwoFinalMomentum;
		this.netInitialMomentum = netInitialMomentum;
		this.netFinalMomentum = netFinalMomentum;
		this.collisionType = collisionType;
	}
}

public class ElasticInelasticCollisionView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;
	public static event Action<ElasticInelasticCollisionAnswerSubmission> SubmitAnswerEvent;

	[Header("Text")]
	[SerializeField] private TextMeshProUGUI calibrationTestText;


	[Header("Given Fields")]
	[Header("Given Fields - Cube One")]
	[SerializeField] private TMP_InputField givenCubeOneMass;
	[SerializeField] private TMP_InputField givenCubeOneInitialVelocity;
	[SerializeField] private TMP_InputField givenCubeOneFinalVelocity;

	[Header("Given Fields - Cube Two")]
	[SerializeField] private TMP_InputField givenCubeTwoMass;
	[SerializeField] private TMP_InputField givenCubeTwoInitialVelocity;
	[SerializeField] private TMP_InputField givenCubeTwoFinalVelocity;


	[Header("Input Fields")]
	[Header("Input Fields - Cube's Initial Momentum")]
	[SerializeField] private TMP_InputField cubeOneInitialMomentumMultiplicand;
	[SerializeField] private TMP_InputField cubeOneInitialMomentumMultiplicator;
	[SerializeField] private TMP_InputField cubeTwoInitialMomentumMultiplicand;
	[SerializeField] private TMP_InputField cubeTwoInitialMomentumMultiplicator;

	[Header("Input Fields - Cube's Final Momentum")]
	[SerializeField] private TMP_InputField cubeOneFinalMomentumMultiplicand;
	[SerializeField] private TMP_InputField cubeOneFinalMomentumMultiplicator;
	[SerializeField] private TMP_InputField cubeTwoFinalMomentumMultiplicand;
	[SerializeField] private TMP_InputField cubeTwoFinalMomentumMultiplicator;

	[Header("Input Fields - Net Momentum")]
	[SerializeField] private TMP_InputField netInitialMomentumAddendOne;
	[SerializeField] private TMP_InputField netInitialMomentumAddendTwo;
	[SerializeField] private TMP_InputField netFinalMomentumAddendOne;
	[SerializeField] private TMP_InputField netFinalMomentumAddendTwo;


	[Header("Result Fields")]
	[Header("Result Fields - Initial Momentum")]
	[SerializeField] private TMP_InputField cubeOneInitialMomentumResultField;
	[SerializeField] private TMP_InputField cubeTwoInitialMomentumResultField;

	[Header("Result Fields - Final Momentum")]
	[SerializeField] private TMP_InputField cubeOneFinalMomentumResultField;
	[SerializeField] private TMP_InputField cubeTwoFinalMomentumResultField;

	[Header("Result Fields - Net Momentum")]
	[SerializeField] private TMP_InputField netInitialMomentumResultField;
	[SerializeField] private TMP_InputField netFinalMomentumResultField;


	[Header("Calculation Displays")]
	[SerializeField] private GameObject initialMomentumCalculations;
	[SerializeField] private GameObject finalMomentumCalculations;
	[SerializeField] private GameObject netMomentumCalculations;
	[SerializeField] private GameObject collisionTypeChoice;


	[Header("Interactive Buttons")]
	[SerializeField] private CollisionTypeButton elasticButton;
	[SerializeField] private CollisionTypeButton inelasticButton;
	[SerializeField] private Button leftPageButton;
	[SerializeField] private Button rightPageButton;

	private void OnEnable()
	{
		OpenViewEvent?.Invoke();
	}


	#region View Setup
	public void SetupElasicInelasticCollisionView(CollisionData collisionData)
	{
		switch (ActivitySevenManager.difficultyConfiguration)
		{
			case Difficulty.Easy:
				SetGivenFields(collisionData, "kg", "m/s");
				break;
			case Difficulty.Medium: case Difficulty.Hard:
				SetGivenFields(collisionData, "g", "km/s", 1000, 0.001f);
				break;
		}

		// Display default view
		OnLeftPageButtonClick();
		ClearAllInputFields();
	}

	private void SetGivenFields(
		CollisionData collisionData, 
		string massUnit, 
		string velocityUnit, 
		float massMultiplier = 1, 
		float velocityMultiplier = 1
		)
	{
		// Cube One Given Fields
		CollisionObject cubeOne = collisionData.cubeOne;
		givenCubeOneMass.text = $"{cubeOne.mass * massMultiplier} {massUnit}";
		givenCubeOneInitialVelocity.text = $"{cubeOne.initialVelocity * velocityMultiplier} {velocityUnit}";
		givenCubeOneFinalVelocity.text = $"{cubeOne.finalVelocity * velocityMultiplier} {velocityUnit}";

		// Cube Two Given Fields
		CollisionObject cubeTwo = collisionData.cubeTwo;
		givenCubeTwoMass.text = $"{cubeTwo.mass * massMultiplier} {massUnit}";
		givenCubeTwoInitialVelocity.text = $"{cubeTwo.initialVelocity * velocityMultiplier} {velocityUnit}";
		givenCubeTwoFinalVelocity.text = $"{cubeTwo.finalVelocity * velocityMultiplier} {velocityUnit}";
	}

	public void UpdateCalibrationTestTextDisplay(int testNumber, int totalTests)
	{
		calibrationTestText.text = $"Calibration Test: {testNumber} / {totalTests}";
	}

	#endregion

	#region Buttons
	public void OnLeftPageButtonClick()
	{
		leftPageButton.gameObject.SetActive(false);
		rightPageButton.gameObject.SetActive(true);

		initialMomentumCalculations.gameObject.SetActive(true);
		finalMomentumCalculations.gameObject.SetActive(true);

		netMomentumCalculations.gameObject.SetActive(false);
		collisionTypeChoice.gameObject.SetActive(false);
	}

	public void OnRightPageButtonClick()
	{
		leftPageButton.gameObject.SetActive(true);
		rightPageButton.gameObject.SetActive(false);

		initialMomentumCalculations.gameObject.SetActive(false);
		finalMomentumCalculations.gameObject.SetActive(false);

		netMomentumCalculations.gameObject.SetActive(true);
		collisionTypeChoice.gameObject.SetActive(true);
	}

	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitViewEvent?.Invoke();
	}

	public void OnSubmitButtonClick()
	{
		// Checking collisionType
		CollisionType? collisionType;
		if (elasticButton.isClicked == true && inelasticButton.isClicked == false)
		{
			collisionType = elasticButton.collisionType;
		} else if (inelasticButton.isClicked == true && elasticButton.isClicked == false)
		{
			collisionType = inelasticButton.collisionType;
		} else
		{
			collisionType = null;
		}

		// Storing answers in ElasticInelasticCollisionAnswerSubmission instance
		ElasticInelasticCollisionAnswerSubmission submission = new ElasticInelasticCollisionAnswerSubmission(
			cubeOneInitialMomentum: float.Parse(cubeOneInitialMomentumResultField.text),
			cubeTwoInitialMomentum: float.Parse(cubeTwoInitialMomentumResultField.text),
			cubeOneFinalMomentum: float.Parse(cubeOneFinalMomentumResultField.text),
			cubeTwoFinalMomentum: float.Parse(cubeTwoFinalMomentumResultField.text),
			netInitialMomentum: float.Parse(netInitialMomentumResultField.text),
			netFinalMomentum: float.Parse(netFinalMomentumResultField.text),
			collisionType: collisionType
			);

		SubmitAnswerEvent?.Invoke(submission);
	}
	#endregion

	private void ClearAllInputFields()
	{
		cubeOneInitialMomentumMultiplicand.text = "";
		cubeOneInitialMomentumMultiplicator.text = "";
		cubeTwoInitialMomentumMultiplicand.text = "";
		cubeTwoInitialMomentumMultiplicator.text = "";

		cubeOneFinalMomentumMultiplicand.text = "";
		cubeOneFinalMomentumMultiplicator.text = "";
		cubeTwoFinalMomentumMultiplicand.text = "";
		cubeTwoFinalMomentumMultiplicator.text = "";

		netInitialMomentumAddendOne.text = "";
		netInitialMomentumAddendTwo.text = "";
		netFinalMomentumAddendOne.text = "";
		netFinalMomentumAddendTwo.text = "";
	}
}