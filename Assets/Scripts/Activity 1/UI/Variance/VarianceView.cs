using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VarianceView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;

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

	public void SetupVarianceView(List<BoxContainer> boxContainerList)
	{
		for (int i = 0; i < boxContainerList.Count; i++)
		{
			GivenVariableDisplay numericalValueDisplay = Instantiate(givenVariableDisplayPrefab, numericalValueContainers.transform, false);
			numericalValueDisplay.SetupGivenVariableDisplay($"Container No. {i+1} : ", $"{boxContainerList[i].numericalValue}");

			ContainerSquaredDeviationEquationDisplay squaredDeviationDisplay = Instantiate(squaredDeviationEquationDisplayPrefab, squaredDeviationEquationContainer.transform, false);
			squaredDeviationDisplay.SetupHeaderText($"Calculate the Squared Devation of Container No. {i+1}");
		}

		massSumEquationDisplay.SetupEquationDisplay(boxContainerList.Count);
		varianceEquationDisplay.SetupEquationDisplay(boxContainerList.Count);
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

	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitViewEvent?.Invoke();
	}
}