using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VarianceAnswerSubmission
{
	public float? massSumValue;
	public float? meanValue;
	public List<float?> squaredDeviationValues;
	public float? varianceValue;

	public VarianceAnswerSubmission(
		float? massSumValue,
		float? meanValue,
		List<float?> squaredDeviationValues,
		float? varianceValue
		)
	{
		this.massSumValue = massSumValue;
		this.meanValue = meanValue;
		this.squaredDeviationValues = squaredDeviationValues;
		this.varianceValue = varianceValue;
	}
}

public class VarianceView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;
	public event Action<VarianceAnswerSubmission> SubmitAnswerEvent;

	[Header("Given Numerical Value Display Container")]
	[SerializeField] private VerticalLayoutGroup numericalValueContainers;

	[Header("Equation Displays")]
	[SerializeField] private ContainerMassSumEquationDisplay massSumEquationDisplay;
	[SerializeField] private QuotientEquationDisplay meanEquationDisplay;
	[SerializeField] private VerticalLayoutGroup squaredDeviationEquationContainer;
	[SerializeField] private ContainerVarianceEquationDisplay varianceEquationDisplay;

	[Header("Prefabs")]
	[SerializeField] private GivenVariableDisplay givenVariableDisplayPrefab;
	[SerializeField] private ContainerSquaredDeviationEquationDisplay squaredDeviationEquationDisplayPrefab;

	[Header("Calculation Page List")]
	[SerializeField] private List<GameObject> calculationPages;

	[Header("Interactive Buttons")]
	[SerializeField] private Button leftPageButton;
	[SerializeField] private Button rightPageButton;

	private int currentPageIndex;

	public void SetupVarianceView(List<float> numericalContainerValues)
	{
		for (int i = 0; i < numericalContainerValues.Count; i++)
		{
			GivenVariableDisplay numericalValueDisplay = Instantiate(givenVariableDisplayPrefab, numericalValueContainers.transform, false);
			numericalValueDisplay.SetupGivenVariableDisplay($"Container No. {i+1} : ", $"{numericalContainerValues[i]}");

			ContainerSquaredDeviationEquationDisplay squaredDeviationDisplay = Instantiate(squaredDeviationEquationDisplayPrefab, squaredDeviationEquationContainer.transform, false);
			squaredDeviationDisplay.SetupHeaderText($"Calculate the Squared Devation of Container No. {i+1}");
		}

		massSumEquationDisplay.SetupEquationDisplay(numericalContainerValues.Count);
		varianceEquationDisplay.SetupEquationDisplay(numericalContainerValues.Count);
	}

	private void OnEnable()
	{
		OpenViewEvent?.Invoke();
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

		if (currentPageIndex >= calculationPages.Count-1)
		{
			rightPageButton.gameObject.SetActive(false);
		}
		leftPageButton.gameObject.SetActive(true);
	}

	public void OnSubmitButtonClick()
	{
		List<float?> squaredDeviationValues = new List<float?>();
		foreach (ContainerSquaredDeviationEquationDisplay squaredDeviationDisplay in squaredDeviationEquationContainer.GetComponentsInChildren<ContainerSquaredDeviationEquationDisplay>())
		{
			squaredDeviationValues.Add(squaredDeviationDisplay.resultValue);
		}

		VarianceAnswerSubmission submission = new VarianceAnswerSubmission(
			massSumValue: massSumEquationDisplay.resultValue,
			meanValue: meanEquationDisplay.quotientValue,
			squaredDeviationValues: squaredDeviationValues,
			varianceValue: varianceEquationDisplay.resultValue
			);

		SubmitAnswerEvent?.Invoke(submission);
	}

	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitViewEvent?.Invoke();
	}
}