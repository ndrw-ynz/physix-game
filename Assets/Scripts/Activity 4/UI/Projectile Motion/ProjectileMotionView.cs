using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileMotionView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;

	[Header("Text Displays")]
	[SerializeField] private TextMeshProUGUI testCountText;

	[Header("Given Value Fields")]
	[SerializeField] private TMP_InputField givenInitialVelocity;
	[SerializeField] private TMP_InputField givenInitialHeight;
	[SerializeField] private TMP_InputField givenAngleMeasure;

	[Header("Formula Displays")]
	[SerializeField] private MaximumHeightFormulaDisplay maximumHeightFormulaDisplay;
	[SerializeField] private HorizontalRangeFormulaDisplay horizontalRangeFormulaDisplay;
	[SerializeField] private TimeOfFlightFormulaDisplay timeOfFlightFormulaDisplay;

	[Header("Calculation Page List")]
	[SerializeField] private List<GameObject> calculationPages;

	[Header("Interactive Buttons")]
	[SerializeField] private Button leftPageButton;
	[SerializeField] private Button rightPageButton;

	private int currentPageIndex;

	private void OnEnable()
	{
		OpenViewEvent?.Invoke();
	}

	public void UpdateTestCountTextDisplay(int currentNumTests, int totalNumTests)
	{
		testCountText.text = $"<color=yellow>Number of Tests Solved: {currentNumTests} / {totalNumTests}</color>";
	}

	public void SetupProjectileMotionView(ProjectileMotionCalculationData data)
	{
		// Reset display state
		ResetViewDisplayState();

		// Update given value text fields
		givenInitialVelocity.text = $"{data.initialVelocity} m/s";
		givenInitialHeight.text = $"{data.initialHeight} m";
		givenAngleMeasure.text = $"{data.angleMeasure} °";

		// Reset state of formula displays
		maximumHeightFormulaDisplay.ResetState();
		horizontalRangeFormulaDisplay.ResetState();
		timeOfFlightFormulaDisplay.ResetState();
	}

	private void ResetViewDisplayState()
	{
		currentPageIndex = 0;
		for (int i = 0; i < calculationPages.Count; i++)
		{
			calculationPages[i].SetActive(false);
		}
		calculationPages[0].SetActive(true);
	}

	public void OnLeftPageButtonClick()
	{
		calculationPages[currentPageIndex].gameObject.SetActive(false);
		currentPageIndex--;
		calculationPages[currentPageIndex].gameObject.SetActive(true);

		if (currentPageIndex <= 0)
		{
			leftPageButton.gameObject.SetActive(false);
		}
		rightPageButton.gameObject.SetActive(true);
	}

	public void OnRightPageButtonClick()
	{
		calculationPages[currentPageIndex].gameObject.SetActive(false);
		currentPageIndex++;
		calculationPages[currentPageIndex].gameObject.SetActive(true);

		if (currentPageIndex >= calculationPages.Count - 1)
		{
			rightPageButton.gameObject.SetActive(false);
		}
		leftPageButton.gameObject.SetActive(true);
	}

	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitViewEvent?.Invoke();
	}
}
