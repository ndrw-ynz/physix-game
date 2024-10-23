using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VectorAdditionSubmissionStatusDisplay : SubmissionStatusDisplay
{
	public event Action ProceedEvent;

	[Header("Calculation Page Displays")]
	[SerializeField] private List<GameObject> pageCalculationDisplays;

	[Header("Status Border Displays")]
	[SerializeField] private Image sumXComponentStatusBorderDisplay;
	[SerializeField] private Image sumYComponentStatusBorderDisplay;
	[SerializeField] private Image vectorMagnitudeStatusBorderDisplay;
	[SerializeField] private Image vectorDirectionStatusBorderDisplay;

	[Header("Calculation References")]
	[SerializeField] private GameObject sumXComponentCalculationReference;
	[SerializeField] private GameObject sumYComponentCalculationReference;
	[SerializeField] private GameObject vectorMagnitudeCalculationReference;
	[SerializeField] private GameObject vectorDirectionCalculationReference;

	[Header("Interactive Buttons")]
	[SerializeField] private Button leftPageButton;
	[SerializeField] private Button rightPageButton;

	// GameObject clones
	private GameObject sumXComponentClone;
	private GameObject sumYComponentClone;
	private GameObject vectorMagnitudeClone;
	private GameObject vectorDirectionClone;

	private int currentPageIndex;

	public void UpdateStatusBorderDisplaysFromResults(VectorAdditionAnswerSubmissionResults results)
	{
		sumXComponentStatusBorderDisplay.color = (
			results.isXComponentCorrect
			) ?
			new Color32(175, 255, 155, 255) :
			new Color32(200, 75, 55, 255);

		sumYComponentStatusBorderDisplay.color = (
			results.isYComponentCorrect
			) ?
			new Color32(175, 255, 155, 255) :
			new Color32(200, 75, 55, 255);

		vectorMagnitudeStatusBorderDisplay.color = (
			results.isVectorMagnitudeCorrect
			) ?
			new Color32(175, 255, 155, 255) :
			new Color32(200, 75, 55, 255);

		vectorDirectionStatusBorderDisplay.color = (
			results.isVectorDirectionCorrect
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
		// Sum of x-component
		sumXComponentClone = Instantiate(sumXComponentCalculationReference);
		sumXComponentClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(sumXComponentClone, sumXComponentStatusBorderDisplay.gameObject);

		// Sum of y-component
		sumYComponentClone = Instantiate(sumYComponentCalculationReference);
		sumYComponentClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(sumYComponentClone, sumYComponentStatusBorderDisplay.gameObject);

		// Vector magnitude calculation
		vectorMagnitudeClone = Instantiate(vectorMagnitudeCalculationReference);
		vectorMagnitudeClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(vectorMagnitudeClone, vectorMagnitudeStatusBorderDisplay.gameObject);

		// Vector direction calculation
		vectorDirectionClone = Instantiate(vectorDirectionCalculationReference);
		vectorDirectionClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(vectorDirectionClone, vectorDirectionStatusBorderDisplay.gameObject);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		proceedButton.onClick.RemoveAllListeners();

		Destroy(sumXComponentClone);
		Destroy(sumYComponentClone);
		Destroy(vectorMagnitudeClone);
		Destroy(vectorDirectionClone);
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
