using System;
using TMPro;
using UnityEngine;

public class Kinematics1DView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;

	[Header("Text Displays")]
	[SerializeField] private TextMeshProUGUI accelerationHeaderText;
	[SerializeField] private TextMeshProUGUI totalDepthHeaderText;
	[SerializeField] private TextMeshProUGUI testCountText;

	[Header("Given Value Displays")]
	[SerializeField] private GameObject givenAccelerationValuesDisplay;
	[SerializeField] private GameObject givenTotalDepthValuesDisplay;

	[Header("Given Acceleration Value Fields")]
	[SerializeField] private TMP_InputField givenInitialVelocityOne;
	[SerializeField] private TMP_InputField givenFinalVelocity;
	[SerializeField] private TMP_InputField givenTimeOne;

	[Header("Given Total Depth Value Fields")]
	[SerializeField] private TMP_InputField givenInitialVelocityTwo;
	[SerializeField] private TMP_InputField givenTimeTwo;

	[Header("Calculation Displays")]
	[SerializeField] private GameObject accelerationCalcDisplay;
	[SerializeField] private GameObject totalDepthCalcDisplay;

	[Header("Calculation Formulas")]
	[SerializeField] private AccelerationFormulaDisplay accelerationFormulaDisplay;
	[SerializeField] private TotalDepthFormulaDisplay totalDepthFormulaDisplay;

	public void UpdateTestCountTextDisplay(int currentNumTests, int totalNumTests)
	{
		testCountText.text = $"<color=yellow>Number of Tests Solved: {currentNumTests} / {totalNumTests}</color>";
	}

	public void UpdateAccelerationInfo(AccelerationCalculationData accelerationCalculationData)
	{
		givenInitialVelocityOne.text = $"{accelerationCalculationData.initialVelocity} m/s";
		givenFinalVelocity.text = $"{accelerationCalculationData.finalVelocity} m/s";
		givenTimeOne.text = $"{accelerationCalculationData.totalTime} seconds";
		accelerationFormulaDisplay.ResetState();
	}

	public void UpdateTotalDepthInfo(TotalDepthCalculationData totalDepthCalculationData)
	{
		givenInitialVelocityTwo.text = $"{totalDepthCalculationData.initialVelocity} m/s";
		givenTimeTwo.text = $"{totalDepthCalculationData.totalTime} seconds";
		totalDepthFormulaDisplay.ResetState();
	}

	public void DisplayTotalDepthInfo()
	{
		// Switch header text
		accelerationHeaderText.gameObject.SetActive(false);
		totalDepthHeaderText.gameObject.SetActive(true);

		// Switch given value displays
		givenAccelerationValuesDisplay.SetActive(false);
		givenTotalDepthValuesDisplay.SetActive(true);

		// Switch calculation displays
		accelerationCalcDisplay.SetActive(false);
		totalDepthCalcDisplay.SetActive(true);
	}

	private void OnEnable()
	{
		OpenViewEvent?.Invoke();
	}

	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitViewEvent?.Invoke();
	}
}
