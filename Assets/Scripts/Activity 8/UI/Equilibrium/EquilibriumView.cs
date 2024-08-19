using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class contains the components for displaying the
/// information related to the Equilibrium subactivity for Activity Eight.
/// </summary>
public class EquilibriumView : MonoBehaviour
{
	public static event Action QuitViewEvent;

	[Header("Text")]
	[SerializeField] private TextMeshProUGUI calibrationTestText;


	[Header("Given Fields")]
	[SerializeField] private TMP_InputField givenWeightWeighingApparatus;
	[SerializeField] private TMP_InputField givenWeightRedBox;
	[SerializeField] private TMP_InputField givenDistanceRedBox;
	[SerializeField] private TMP_InputField givenWeightBlueBox;
	[SerializeField] private TMP_InputField givenDistanceBlueBox;
	[SerializeField] private TMP_InputField givenForceFulcrum;


	[Header("Input Fields")]
	[Header("Input Fields - Equilibrium on Exerted Forces")]
	[SerializeField] private TMP_InputField summationDownwardForceAddendOne;
	[SerializeField] private TMP_InputField summationDownwardForceAddendTwo;
	[SerializeField] private TMP_InputField summationDownwardForceAddendThree;
	[SerializeField] private TMP_InputField summationTotalForceAddendOne;
	[SerializeField] private TMP_InputField summationTotalForceAddendTwo;

	[Header("Input Fields - Equilibrium on Torques")]
	[SerializeField] private TMP_InputField torqueCounterclockwiseMultiplicand;
	[SerializeField] private TMP_InputField torqueCounterclockwiseMultiplicator;
	[SerializeField] private TMP_InputField torqueClockwiseMultiplicand;
	[SerializeField] private TMP_InputField torqueClockwiseMultiplicator;


	[Header("Result Fields")]
	[Header("Result Fields - Equilibrium on Exerted Forces")]
	[SerializeField] private TMP_InputField summationDownwardForcesResultField;
	[SerializeField] private TMP_InputField upwardForceResultField;
	[SerializeField] private TMP_InputField summationForcesResultField;

	[Header("Result Fields - Equilibrium on Torques")]
	[SerializeField] private TMP_InputField torqueCounterclockwiseResultField;
	[SerializeField] private TMP_InputField torqueClockwiseResultField;


	[Header("Calculation Displays")]
	[SerializeField] private GameObject forceEquilibriumCalculations;
	[SerializeField] private GameObject torqueEquilibriumCalculations;
	[SerializeField] private GameObject equilibriumTypeSelection;

	[Header("Interactive Buttons")]
	[SerializeField] private EquilibriumTypeButton inEquilibriumButton;
	[SerializeField] private EquilibriumTypeButton notInEquilibriumButton;
	[SerializeField] private Button leftPageButton;
	[SerializeField] private Button rightPageButton;

	/// <summary>
	/// Setup state of <c>EquilibriumView</c> in displaying given information
	/// related to the calculation of equilibrium.
	/// </summary>
	/// <param name="data"></param>
	public void SetupEquilibriumView(EquilibriumData data)
	{
		// Clear all input and result fields.
		ClearAllFields();

		// Setup given fields based on EquilibriumData and Difficulty.
		switch (ActivityEightManager.difficultyConfiguration)
		{
			case Difficulty.Easy:
				SetGivenFields(data, "N", "m");
				break;
			case Difficulty.Medium:
			case Difficulty.Hard:
				SetGivenFields(data, "mN", "mm", 1000, 1000);
				break;
		}

		// Display default view.
		OnLeftPageButtonClick();
	}

	/// <summary>
	/// Sets the displayed values for the given fields of <c>EquilibriumView</c>.
	/// </summary>
	/// <param name="equilibriumData"></param>
	/// <param name="forceUnit"></param>
	/// <param name="distanceUnit"></param>
	/// <param name="forceMultiplier"></param>
	/// <param name="distanceMultiplier"></param>
	private void SetGivenFields(
		EquilibriumData equilibriumData,
		string forceUnit,
		string distanceUnit,
		float forceMultiplier = 1,
		float distanceMultiplier = 1
		)
	{
		givenWeightWeighingApparatus.text = $"{equilibriumData.weighingApparatusWeight * forceMultiplier} {forceUnit}";
		givenWeightRedBox.text = $"{equilibriumData.redBoxWeight * forceMultiplier} {forceUnit}";
		givenDistanceRedBox.text = $"{equilibriumData.redBoxDistance * distanceMultiplier} {distanceUnit}";
		givenWeightBlueBox.text = $"{equilibriumData.blueBoxWeight * forceMultiplier} {forceUnit}";
		givenDistanceBlueBox.text = $"{equilibriumData.blueBoxDistance * distanceMultiplier} {distanceUnit}";
		givenForceFulcrum.text = $"{equilibriumData.fulcrumForce * forceMultiplier} {forceUnit}";
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

		forceEquilibriumCalculations.gameObject.SetActive(true);

		torqueEquilibriumCalculations.gameObject.SetActive(false);
		equilibriumTypeSelection.gameObject.SetActive(false);
	}

	public void OnRightPageButtonClick()
	{
		leftPageButton.gameObject.SetActive(true);
		rightPageButton.gameObject.SetActive(false);

		forceEquilibriumCalculations.gameObject.SetActive(false);

		torqueEquilibriumCalculations.gameObject.SetActive(true);
		equilibriumTypeSelection.gameObject.SetActive(true);
	}
	/// <summary>
	/// Quit button click event action for quitting <c>TorqueView</c>.
	/// </summary>
	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitViewEvent?.Invoke();
	}
	#endregion

	private void ClearAllFields()
	{
		// Clear all Input Fields
		summationDownwardForceAddendOne.text = "";
		summationDownwardForceAddendTwo.text = "";
		summationDownwardForceAddendThree.text = "";
		summationTotalForceAddendOne.text = "";
		summationTotalForceAddendTwo.text = "";

		torqueCounterclockwiseMultiplicand.text = "";
		torqueCounterclockwiseMultiplicator.text = "";
		torqueClockwiseMultiplicand.text = "";
		torqueClockwiseMultiplicator.text = "";

		// Clear all Result Fields
		summationDownwardForcesResultField.text = "";
		upwardForceResultField.text = "";
		summationForcesResultField.text = "";
		torqueCounterclockwiseResultField.text = "";
		torqueClockwiseResultField.text = "";
	}
}