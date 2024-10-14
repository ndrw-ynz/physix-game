using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VarianceSubmissionStatusDisplay : SubmissionStatusDisplay
{
	public event Action ProceedEvent;

	[Header("Prefabs")]
	[SerializeField] private Image statusBorderDisplayPrefab;

	[Header("Calculation Page Displays")]
	[SerializeField] private List<GameObject> pageCalculationDisplays;

	[Header("Status Border Displays")]
	[SerializeField] private Image sumOfMassStatusBorderDisplay;
	[SerializeField] private Image meanStatusBorderDisplay;
	[SerializeField] private VerticalLayoutGroup squaredDeviationStatusBorderContainer;
	[SerializeField] private Image varianceStatusBorderDisplay;

	[Header("Calculation References")]
	[SerializeField] private GameObject sumOfMassCalculationReference;
	[SerializeField] private GameObject meanCalculationReference;
	[SerializeField] private VerticalLayoutGroup squaredDeviationCalculationReferences;
	[SerializeField] private GameObject varianceCalculationReference;

	[Header("Interactive Buttons")]
	[SerializeField] private Button leftPageButton;
	[SerializeField] private Button rightPageButton;

	// GameObject clones
	private GameObject sumOfMassClone;
	private GameObject meanClone;
	private GameObject varianceClone;

	private int currentPageIndex;

	public void UpdateStatusBorderDisplaysFromResult(VarianceAnswerSubmissionResults results)
	{
		sumOfMassStatusBorderDisplay.color = (
			results.isMassSumValueCorrect
			) ?
			new Color32(175, 255, 155, 255) :
			new Color32(200, 75, 55, 255);

		meanStatusBorderDisplay.color = (
			results.isMeanValueCorrect
			) ?
			new Color32(175, 255, 155, 255) :
			new Color32(200, 75, 55, 255);

		for (int i = 0; i < squaredDeviationStatusBorderContainer.transform.childCount; i++)
		{
			squaredDeviationStatusBorderContainer.transform.GetChild(i).GetComponent<Image>().color = results.squaredDeviationsResult[i]
				? new Color32(175, 255, 155, 255)
				: new Color32(200, 75, 55, 255);
		}

		varianceStatusBorderDisplay.color = (
			results.isVarianceValueCorrect
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
		// Sum of mass calculation
		sumOfMassClone = Instantiate(sumOfMassCalculationReference);
		sumOfMassClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(sumOfMassClone, sumOfMassStatusBorderDisplay.gameObject);

		// Mean calculation
		meanClone = Instantiate(meanCalculationReference);
		meanClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(meanClone, meanStatusBorderDisplay.gameObject);

		// Squared deviation calculations
		ContainerSquaredDeviationEquationDisplay[] squaredDeviationEquationDisplays = squaredDeviationCalculationReferences.GetComponentsInChildren<ContainerSquaredDeviationEquationDisplay>();
		if (squaredDeviationStatusBorderContainer.transform.childCount == 0)
		{
			// Initialize page two calculation status border displays, if empty
			for (int i = 0; i < squaredDeviationEquationDisplays.Length; i++)
			{
				Image display = Instantiate(statusBorderDisplayPrefab, squaredDeviationStatusBorderContainer.transform, false);
			}
		}

		for (int i = 0; i < squaredDeviationEquationDisplays.Length; i++)
		{
			ContainerSquaredDeviationEquationDisplay squaredDeviationClone = Instantiate(squaredDeviationEquationDisplays[i], squaredDeviationStatusBorderContainer.transform.GetChild(i).transform, false);
			squaredDeviationClone.gameObject.SetActive(true);
			UIUtilities.CenterChildInParent(squaredDeviationClone.gameObject, squaredDeviationStatusBorderContainer.transform.GetChild(i).gameObject);
		}

		// Variance calculation
		varianceClone = Instantiate(varianceCalculationReference);
		varianceClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(varianceClone, varianceStatusBorderDisplay.gameObject);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		proceedButton.onClick.RemoveAllListeners();

		Destroy(sumOfMassClone);
		Destroy(meanClone);

		for (int i = 0; i < squaredDeviationStatusBorderContainer.transform.childCount; i++)
		{
			Image squaredDeviationContainer = squaredDeviationStatusBorderContainer.transform.GetChild(i).GetComponent<Image>();
			Destroy(squaredDeviationContainer.transform.GetChild(0).gameObject);
		}
		Destroy(varianceClone);
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
