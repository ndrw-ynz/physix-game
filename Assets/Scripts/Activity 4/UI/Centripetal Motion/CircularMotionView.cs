using System;
using TMPro;
using UnityEngine;

public class CircularMotionView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;
	public event Action<float?> SubmitAnswerEvent;

	[Header("Text Displays")]
	[SerializeField] private TextMeshProUGUI testCountText;

	[Header("Given Value Fields")]
	[SerializeField] private TMP_InputField givenRadius;
	[SerializeField] private TMP_InputField givenPeriod;

	[Header("Formula Displays")]
	[SerializeField] private CentripetalAccelerationFormulaDisplay centripetalAccelerationFormulaDisplay;

	private void OnEnable()
	{
		OpenViewEvent?.Invoke();
	}

	public void UpdateTestCountTextDisplay(int currentNumTests, int totalNumTests)
	{
		testCountText.text = $"<color=yellow>Number of Tests Solved: {currentNumTests} / {totalNumTests}</color>";
	}

	public void SetupCircularMotionView(CircularMotionCalculationData data)
	{
		// Update given value text fields
		givenRadius.text = $"{data.radius} kilometers";
		givenPeriod.text = $"{data.period} seconds";

		// Reset state of formula displays
		centripetalAccelerationFormulaDisplay.ResetState();
	}

	public void OnSubmitButtonClick()
	{
		SubmitAnswerEvent?.Invoke(centripetalAccelerationFormulaDisplay.resultValue);
	}

	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitViewEvent?.Invoke();
	}
}
