using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElasticInelasticCollisionView : MonoBehaviour
{
	[Header("Given Fields")]
	[Header("Given Fields - Cube One")]
	[SerializeField] private TMP_InputField givenCubeOneMass;
	[SerializeField] private TMP_InputField givenCubeOneInitialVelocity;
	[SerializeField] private TMP_InputField givenCubeOneFinalVelocity;

	[Header("Given Fields - Cube Two")]
	[SerializeField] private TMP_InputField givenCubeTwoMass;
	[SerializeField] private TMP_InputField givenCubeTwoInitialVelocity;
	[SerializeField] private TMP_InputField givenCubeTwoFinalVelocity;


	[Header("Result Fields")]
	[Header("Result Fields - Initial Momentum")]
	[SerializeField] private TMP_InputField cubeOneInitialMomentumReultField;
	[SerializeField] private TMP_InputField cubeTwoInitialMomentumReultField;

	[Header("Result Fields - Final Momentum")]
	[SerializeField] private TMP_InputField cubeOneFinalMomentumResultField;
	[SerializeField] private TMP_InputField cubeTwoFinalMomentumResultField;

	[Header("Result Fields - Momentum Sums")]
	[SerializeField] private TMP_InputField sumOfInitialMomentumResultField;
	[SerializeField] private TMP_InputField sumOfFinalMomentumResultField;


	[Header("Calculation Displays")]
	[SerializeField] private GameObject initialMomentumCalculations;
	[SerializeField] private GameObject finalMomentumCalculations;
	[SerializeField] private GameObject momentumSumCalculations;
	[SerializeField] private GameObject collisionTypeChoice;


	[Header("Interactive Buttons")]
	[SerializeField] private CollisionTypeButton elasticButton;
	[SerializeField] private CollisionTypeButton inelasticButton;
	[SerializeField] private Button leftPageButton;
	[SerializeField] private Button rightPageButton;

	#region Buttons
	public void OnLeftPageButtonClick()
	{
		leftPageButton.gameObject.SetActive(false);
		rightPageButton.gameObject.SetActive(true);

		initialMomentumCalculations.gameObject.SetActive(true);
		finalMomentumCalculations.gameObject.SetActive(true);

		momentumSumCalculations.gameObject.SetActive(false);
		collisionTypeChoice.gameObject.SetActive(false);
	}

	public void OnRightPageButtonClick()
	{
		leftPageButton.gameObject.SetActive(true);
		rightPageButton.gameObject.SetActive(false);

		initialMomentumCalculations.gameObject.SetActive(false);
		finalMomentumCalculations.gameObject.SetActive(false);

		momentumSumCalculations.gameObject.SetActive(true);
		collisionTypeChoice.gameObject.SetActive(true);
	}

	#endregion
}