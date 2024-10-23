using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VectorAdditionView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;

	[Header("Vector Info Display Container")]
	[SerializeField] private VerticalLayoutGroup vectorDisplayContainer;

	[Header("Equation Displays")]
	[SerializeField] private ComponentSumEquationDisplay xComponentSumDisplay;
	[SerializeField] private ComponentSumEquationDisplay yComponentSumDisplay;
	[SerializeField] private VectorMagnitudeFormulaDisplay vectorMagnitudeDisplay;
	[SerializeField] private VectorDirectionFormulaDisplay vectorDirectionDisplay;

	[Header("Prefabs")]
	[SerializeField] private VectorInfoDisplay vectorInfoDisplayPrefab;

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

	public void SetupVectorAdditionView(List<VectorData> givenVectorData)
	{
		// Setup vector info displays
		for (int i = 0; i < givenVectorData.Count; i++)
		{
			VectorInfoDisplay vectorInfoDisplay = Instantiate(vectorInfoDisplayPrefab, vectorDisplayContainer.transform, false);
			vectorInfoDisplay.SetupVectorInfoDisplay(i + 1, givenVectorData[i]);
		}

		// Setup equation displays
		xComponentSumDisplay.SetupEquationDisplay(givenVectorData.Count);
		yComponentSumDisplay.SetupEquationDisplay(givenVectorData.Count);
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
