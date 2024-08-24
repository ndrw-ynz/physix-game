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
		double? gravitationalPotentialEnergy
		)
	{
		this.gravitationalForce = gravitationalForce;
		this.GPE = gravitationalPotentialEnergy;
	}
}

public class GravityView : MonoBehaviour
{
	public static event Action QuitViewEvent;
	public static event Action<GravityAnswerSubmission> SubmitAnswerEvent;

	[Header("Text")]
	[SerializeField] private TextMeshProUGUI calibrationTestText;

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

	public void SetupGravityView(GravityData data)
	{
		ClearAllFields();

		planetMassDisplay.SetupGivenVariableDisplay("Planet Mass: ", $"{Math.Round(data.planetMassSNCoefficient, 4)} x 10 ^ {data.planetMassSNExponent} kg");
		orbittingObjectMassDisplay.SetupGivenVariableDisplay("Orbitting Object Mass: ", $"{Math.Round(data.orbittingObjectMassSNCoefficient, 4)} x 10 ^ {data.orbittingObjectMassSNExponent} kg");
		centerPointDistanceDisplay.SetupGivenVariableDisplay("Center Point Distance: ", $"{Math.Round(data.distanceBetweenObjects, 4)} km");

		// Display default view
		OnLeftPageButtonClick();
	}

	/// <summary>
	/// Updates the content of Calibration Test Text display, used in updating current test number and total tests.
	/// </summary>
	/// <param name="testNumber"></param>
	/// <param name="totalTests"></param>
	public void UpdateCalibrationTestTextDisplay(int testNumber, int totalTests)
	{
		calibrationTestText.text = $"Calibration Test: {testNumber} / {totalTests}";
	}

	#region Buttons
	public void OnLeftPageButtonClick()
	{
		leftPageButton.gameObject.SetActive(false);
		rightPageButton.gameObject.SetActive(true);

		gravitationalForceCalculationDisplay.gameObject.SetActive(true);

		GPECalculationDisplay.gameObject.SetActive(false);
	}

	public void OnRightPageButtonClick()
	{
		leftPageButton.gameObject.SetActive(true);
		rightPageButton.gameObject.SetActive(false);

		gravitationalForceCalculationDisplay.gameObject.SetActive(false);

		GPECalculationDisplay.gameObject.SetActive(true);
	}

	public void OnSubmitButtonClick()
	{
		GravityAnswerSubmission submission = new GravityAnswerSubmission(
			gravitationalForce: gravitationalForceFormulaDisplay.resultValue,
			gravitationalPotentialEnergy: GPEFormulaDisplay.resultValue
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