using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GravityAnswerSubmission
{
	public double? gravitationalForce { get; private set; }
	public double? GPE { get; private set; }

	public GravityAnswerSubmission(
		double? gravitationalForce,
		double? GPE
		)
	{
		this.gravitationalForce = gravitationalForce;
		this.GPE = GPE;
	}
}

public class GravityView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;
	public event Action<GravityAnswerSubmission> SubmitAnswerEvent;
	public event Action<OrbittingObjectType> UpdateDisplayedOrbittingObjectEvent;

	[Header("Text")]
	[SerializeField] private TextMeshProUGUI calibrationTestText;

	[Header("Orbitting Object Render Raw Images")]
	[SerializeField] private RawImage spaceshipRenderImage;
	[SerializeField] private RawImage satelliteOneRenderImage;
	[SerializeField] private RawImage satelliteTwoRenderImage;

	[Header("Given Variable Displays")]
	[SerializeField] private GivenVariableDisplay planetMassDisplay;
	[SerializeField] private GivenVariableDisplay orbittingObjectMassDisplay;
	[SerializeField] private GivenVariableDisplay centerPointDistanceDisplay;

	[Header("Gravity Formula Displays")]
	[SerializeField] private GravityFormulaDisplay gravitationalForceFormulaDisplay;
	[SerializeField] private GravityFormulaDisplay GPEFormulaDisplay;

	[Header("Calculation Displays")]
	[SerializeField] private GameObject gravitationalForceCalculationDisplay;
	[SerializeField] private GameObject GPECalculationDisplay;

	[Header("Interactive Buttons")]
	[SerializeField] private Button leftPageButton;
	[SerializeField] private Button rightPageButton;

	private void OnEnable()
	{
		OpenViewEvent?.Invoke();
	}

	public void SetupGravityView(GravityData data)
	{
		ClearAllFields();

		UpdateDisplayedObjectRenderImage(data.orbittingObjectType);

		planetMassDisplay.SetupGivenVariableDisplay("Planet Mass: ", $"{Math.Round(data.planetMassSNCoefficient, 4)} x 10 ^ {data.planetMassSNExponent} kg");
		orbittingObjectMassDisplay.SetupGivenVariableDisplay("Orbitting Object Mass: ", $"{Math.Round(data.orbittingObjectMassSNCoefficient, 4)} x 10 ^ {data.orbittingObjectMassSNExponent} kg");
		centerPointDistanceDisplay.SetupGivenVariableDisplay("Center Point Distance: ", $"{Math.Round(data.distanceBetweenObjects, 4)} km");

		// Display default view
		OnLeftPageButtonClick();
	}

	private void UpdateDisplayedObjectRenderImage(OrbittingObjectType orbittingObjectType)
	{
		// Disable all.
		spaceshipRenderImage.gameObject.SetActive(false);
		satelliteOneRenderImage.gameObject.SetActive(false);
		satelliteTwoRenderImage.gameObject.SetActive(false);

		// Only activate specific orbitting object type
		switch(orbittingObjectType)
		{
			case OrbittingObjectType.Spaceship:
				spaceshipRenderImage.gameObject.SetActive(true);
				break;
			case OrbittingObjectType.SatelliteOne:
				satelliteOneRenderImage.gameObject.SetActive(true);
				break;
			case OrbittingObjectType.SatelliteTwo:
				satelliteTwoRenderImage.gameObject.SetActive(true);
				break;
		}

		UpdateDisplayedOrbittingObjectEvent?.Invoke(orbittingObjectType);
	}

	/// <summary>
	/// Updates the content of Calibration Test Text display, used in updating current test number and total tests.
	/// </summary>
	/// <param name="testNumber"></param>
	/// <param name="totalTests"></param>
	public void UpdateCalibrationTestTextDisplay(int testNumber, int totalTests)
	{
		calibrationTestText.text = $"Calculation Test: {testNumber} / {totalTests}";
	}

	#region Buttons
	public void OnLeftPageButtonClick()
	{
		SceneSoundManager.Instance.PlaySFX("Click");

		leftPageButton.gameObject.SetActive(false);
		rightPageButton.gameObject.SetActive(true);

		gravitationalForceCalculationDisplay.gameObject.SetActive(true);

		GPECalculationDisplay.gameObject.SetActive(false);
	}

	public void OnRightPageButtonClick()
	{
		SceneSoundManager.Instance.PlaySFX("Click");

		leftPageButton.gameObject.SetActive(true);
		rightPageButton.gameObject.SetActive(false);

		gravitationalForceCalculationDisplay.gameObject.SetActive(false);

		GPECalculationDisplay.gameObject.SetActive(true);
	}

	public void OnSubmitButtonClick()
	{
		SceneSoundManager.Instance.PlaySFX("Click");

		GravityAnswerSubmission submission = new GravityAnswerSubmission(
			gravitationalForce: gravitationalForceFormulaDisplay.resultValue,
			GPE: GPEFormulaDisplay.resultValue
			);

		SubmitAnswerEvent?.Invoke(submission);
	}

	/// <summary>
	/// Quit button click event action for quitting <c>GravityView</c>.
	/// </summary>
	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitViewEvent?.Invoke();
	}

	#endregion

	private void ClearAllFields()
	{
		gravitationalForceFormulaDisplay.ResetState();
		GPEFormulaDisplay.ResetState();
	}
}