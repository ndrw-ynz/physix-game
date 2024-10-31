using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileMotionSubmissionStatusDisplay : SubmissionStatusDisplay
{
	public event Action ProceedEvent;

	[Header("Calculation Page Displays")]
	[SerializeField] private List<GameObject> pageCalculationDisplays;

	[Header("Status Border Displays")]
	[SerializeField] private Image maximumHeightStatusBorderDisplay;
	[SerializeField] private Image horizontalRangeStatusBorderDisplay;
	[SerializeField] private Image timeOfFlightStatusBorderDisplay;

	[Header("Calculation References")]
	[SerializeField] private GameObject maximumHeightCalculationReference;
	[SerializeField] private GameObject horizontalRangeCalculationReference;
	[SerializeField] private GameObject timeOfFlightCalculationReference;

	[Header("Interactive Buttons")]
	[SerializeField] private Button leftPageButton;
	[SerializeField] private Button rightPageButton;

	// GameObject clones
	private GameObject maximumHeightClone;
	private GameObject horizontalRangeClone;
	private GameObject timeOfFlightClone;

	private int currentPageIndex;

	public void UpdateStatusBorderDisplaysFromResults(ProjectileMotionSubmissionResults results)
	{
		maximumHeightStatusBorderDisplay.color = (
			results.isMaximumHeightCorrect
			) ?
			new Color32(175, 255, 155, 255) :
			new Color32(200, 75, 55, 255);

		horizontalRangeStatusBorderDisplay.color = (
			results.isHorizontalRangeCorrect
			) ?
			new Color32(175, 255, 155, 255) :
			new Color32(200, 75, 55, 255);

		timeOfFlightStatusBorderDisplay.color = (
			results.isTimeOfFlightCorrect
			) ?
			new Color32(175, 255, 155, 255) :
			new Color32(200, 75, 55, 255);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		proceedButton.onClick.AddListener(() => ProceedEvent?.Invoke());

		// Reset display to default state
		ResetPageState();

		// Create references and attach to associated parents
		// Maximum height calculation
		maximumHeightClone = Instantiate(maximumHeightCalculationReference);
		maximumHeightClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(maximumHeightClone, maximumHeightStatusBorderDisplay.gameObject);

		// Horizontal range calculation
		horizontalRangeClone = Instantiate(horizontalRangeCalculationReference);
		horizontalRangeClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(horizontalRangeClone, horizontalRangeStatusBorderDisplay.gameObject);

		// Time of flight calculation
		timeOfFlightClone = Instantiate(timeOfFlightCalculationReference);
		timeOfFlightClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(timeOfFlightClone, timeOfFlightStatusBorderDisplay.gameObject);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		proceedButton.onClick.RemoveAllListeners();

		Destroy(maximumHeightClone);
		Destroy(horizontalRangeClone);
		Destroy(timeOfFlightClone);
	}

	private void ResetPageState()
	{
		pageCalculationDisplays[0].gameObject.SetActive(true);
		for (int i = 1; i < pageCalculationDisplays.Count; i++)
		{
			pageCalculationDisplays[i].gameObject.SetActive(false);
		}
		currentPageIndex = 0;
		leftPageButton.gameObject.SetActive(false);
		rightPageButton.gameObject.SetActive(true);
	}

	public void OnLeftPageButtonClick()
	{
		pageCalculationDisplays[currentPageIndex].gameObject.SetActive(false);
		currentPageIndex--;
		pageCalculationDisplays[currentPageIndex].gameObject.SetActive(true);

		if (currentPageIndex <= 0)
		{
			leftPageButton.gameObject.SetActive(false);
		}
		rightPageButton.gameObject.SetActive(true);
	}

	public void OnRightPageButtonClick()
	{
		pageCalculationDisplays[currentPageIndex].gameObject.SetActive(false);
		currentPageIndex++;
		pageCalculationDisplays[currentPageIndex].gameObject.SetActive(true);

		if (currentPageIndex >= pageCalculationDisplays.Count - 1)
		{
			rightPageButton.gameObject.SetActive(false);
		}
		leftPageButton.gameObject.SetActive(true);
	}
}
