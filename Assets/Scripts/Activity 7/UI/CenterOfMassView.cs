using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CenterOfMassAnswerSubmission
{
	public int?[] massTimesXCoordinates { get; private set; }
	public int?[] massTimesYCoordinates { get; private set; }
	public int? sumOfMassTimesXCoordinates { get; private set; }
	public int? sumOfMassTimesYCoordinates { get; private set; }
	public int? totalMassX { get; private set; }
	public int? totalMassY { get; private set; }
	public float? centerOfMassX { get; private set; }
	public float? centerOfMassY { get; private set; }

	public void SetMassTimesCoordinates(MassCoordinateProductDisplay[] xMassCoordinateProductDisplays, MassCoordinateProductDisplay[] yMassCoordinateProductDisplays)
	{
		massTimesXCoordinates = new int?[xMassCoordinateProductDisplays.Length];
		for (int i = 0; i < xMassCoordinateProductDisplays.Length; i++)
		{
			massTimesXCoordinates[i] = xMassCoordinateProductDisplays[i].productValue;
		}

		massTimesYCoordinates = new int?[yMassCoordinateProductDisplays.Length];
		for (int i = 0; i < yMassCoordinateProductDisplays.Length; i++)
		{
			massTimesYCoordinates[i] = yMassCoordinateProductDisplays[i].productValue;
		}
	}

	public void SetSumOfMassTimesCoordinates(int sumOfMassTimesXCoordinates, int sumOfMassTimesYCoordinates)
	{
		this.sumOfMassTimesXCoordinates = sumOfMassTimesXCoordinates;
		this.sumOfMassTimesYCoordinates = sumOfMassTimesYCoordinates;
	}

	public void SetTotalMass(int totalMassX, int totalMassY)
	{
		this.totalMassX = totalMassX;
		this.totalMassY = totalMassY;
	}

	public void SetCenterOfMass(float? centerOfMassX, float? centerOfMassY)
	{
		this.centerOfMassX = centerOfMassX;
		this.centerOfMassY = centerOfMassY;
	}
}

public class CenterOfMassView : MonoBehaviour
{
	public static event Action<CenterOfMassAnswerSubmission> SubmitAnswerEvent;

	[Header("Graph Coordinate Plotter")]
	[SerializeField] private GraphCoordinatePlotter graphCoordinatePlotter;

	[Header("Mass Coordinate Component Container")]
	[SerializeField] private VerticalLayoutGroup massCoordinateComponentContainer;

	[Header("Mass Coordinate Product Containers")]
	[SerializeField] private HorizontalLayoutGroup XMassCoordinateProductContainer;
	[SerializeField] private HorizontalLayoutGroup YMassCoordinateProductContainer;

	[Header("Sum of Mass Coordinate Product Containers")]
	[SerializeField] private HorizontalLayoutGroup XSumOfMassCoordinateProductContainer;
	[SerializeField] private HorizontalLayoutGroup YSumOfMassCoordinateProductContainer;

	[Header("Sum of Mass Containers")]
	[SerializeField] private HorizontalLayoutGroup XSumOfMassContainer;
	[SerializeField] private HorizontalLayoutGroup YSumOfMassContainer;

	[Header("Sum Result Fields")]
	[SerializeField] private TMP_InputField XMassCoordinateProductSumResultField;
	[SerializeField] private TMP_InputField YMassCoordinateProductSumResultField;
	[SerializeField] private TMP_InputField XSumOfMassResultField;
	[SerializeField] private TMP_InputField YSumOfMassResultField;

	[Header("Prefabs")]
	[SerializeField] private MassCoordinateComponentDisplay massCoordinateComponentDisplayPrefab;
	[SerializeField] private MassCoordinateProductDisplay massCoordinateProductDisplayPrefab;
	[SerializeField] private TMP_InputField numberInputFieldPrefab;
	[SerializeField] private TextMeshProUGUI plusSignTextPrefab;

	[Header("Calculation Displays")]
	[SerializeField] private GameObject massTimesXCoords;
	[SerializeField] private GameObject massTimesYCoords;
	[SerializeField] private GameObject centerOfMassXDir;
	[SerializeField] private GameObject centerOfMassYDir;

	[Header("Equation Displays")]
	[SerializeField] private CenterOfMassEquationDisplay xCenterOfMassEquationDisplay;
	[SerializeField] private CenterOfMassEquationDisplay yCenterOfMassEquationDisplay;

	[Header("Interactive Buttons")]
    [SerializeField] private Button leftPageButton;
	[SerializeField] private Button rightPageButton;

	#region View Setup
	public void SetupCenterOfMassView(List<MassCoordinatePair> massCoordinatePairs)
	{
		// Setting up mass coordinate components for UI display of given values
		for (int i = 0; i < massCoordinatePairs.Count; i++)
		{
			MassCoordinateComponentDisplay massCoordinateComponentDisplay = Instantiate(massCoordinateComponentDisplayPrefab);
			massCoordinateComponentDisplay.SetupInputFields(massCoordinatePairs[i], i + 1);
			massCoordinateComponentDisplay.transform.SetParent(massCoordinateComponentContainer.transform, false);
		}

		// Setting up mass coordinate product displays in calculation display
		SetupMassCoordinateProductContainers(massCoordinatePairs.Count, XMassCoordinateProductContainer);
		SetupMassCoordinateProductContainers(massCoordinatePairs.Count, YMassCoordinateProductContainer);

		// Setting up LEQ of XY Sum of Mass Coordinate Products
		SetupLeftSumEquationContainer(massCoordinatePairs.Count, XSumOfMassCoordinateProductContainer, XMassCoordinateProductSumResultField);
		SetupLeftSumEquationContainer(massCoordinatePairs.Count, YSumOfMassCoordinateProductContainer, YMassCoordinateProductSumResultField);

		// Setting up LEQ of XY Sum of Mass
		SetupLeftSumEquationContainer(massCoordinatePairs.Count, XSumOfMassContainer, XSumOfMassResultField);
		SetupLeftSumEquationContainer(massCoordinatePairs.Count, YSumOfMassContainer, YSumOfMassResultField);

		// Setup graph points
		for (int i = 0; i < massCoordinatePairs.Count; i++)
		{
			graphCoordinatePlotter.PlacePoint(massCoordinatePairs[i].coordinate);
		}
	}

	private void SetupMassCoordinateProductContainers(int coordinatePairsCount, HorizontalLayoutGroup massCoordinateProductContainer)
	{
		for (int i = 0; i < coordinatePairsCount; i++)
		{
			MassCoordinateProductDisplay massCoordinateProductDisplay = Instantiate(massCoordinateProductDisplayPrefab);
			massCoordinateProductDisplay.SetupText(i + 1);
			massCoordinateProductDisplay.transform.SetParent(massCoordinateProductContainer.transform, false);
		}
	}

	private void SetupLeftSumEquationContainer(int addendsCount, HorizontalLayoutGroup leftSumEquationContainer, TMP_InputField sumResultField)
	{
		for (int i = 0; i < addendsCount; i++)
		{
			TMP_InputField numberInputField = Instantiate(numberInputFieldPrefab);
			numberInputField.transform.SetParent(leftSumEquationContainer.transform, false);
			numberInputField.onValueChanged.AddListener((_) => UpdateSumResultField(leftSumEquationContainer, sumResultField));

			if (i + 1 < addendsCount)
			{
				TextMeshProUGUI plusSignText = Instantiate(plusSignTextPrefab);
				plusSignText.transform.SetParent(leftSumEquationContainer.transform, false);
			}
		}
	}
	private void UpdateSumResultField(HorizontalLayoutGroup leftSumEquationContainer, TMP_InputField sumResultField)
	{
		float sum = 0;
		TMP_InputField[] numberInputFields = leftSumEquationContainer.GetComponentsInChildren<TMP_InputField>();
		foreach (TMP_InputField numberInputField in numberInputFields)
		{
			bool isEvaluated = ExpressionEvaluator.Evaluate(numberInputField.text, out float expressionResult);
			if (isEvaluated) sum += expressionResult;
		}
		sumResultField.text = $"{sum}";
	}
	#endregion

	#region Buttons
	public void OnLeftPageButtonClick()
	{
		leftPageButton.gameObject.SetActive(false);
		rightPageButton.gameObject.SetActive(true);

		massTimesXCoords.gameObject.SetActive(true);
		centerOfMassXDir.gameObject.SetActive(true);

		massTimesYCoords.gameObject.SetActive(false);
		centerOfMassYDir.gameObject.SetActive(false);
	}

	public void OnRightPageButtonClick()
	{
		leftPageButton.gameObject.SetActive(true);
		rightPageButton.gameObject.SetActive(false);

		massTimesXCoords.gameObject.SetActive(false);
		centerOfMassXDir.gameObject.SetActive(false);

		massTimesYCoords.gameObject.SetActive(true);
		centerOfMassYDir.gameObject.SetActive(true);
	}

	public void OnSubmitButtonClick()
	{
		CenterOfMassAnswerSubmission centerOfMassAnswerSubmission = new CenterOfMassAnswerSubmission();

		// Set MassTimesCoordinates for X and Y
		MassCoordinateProductDisplay[] XMassCoordinateProductDisplays = XMassCoordinateProductContainer.GetComponentsInChildren<MassCoordinateProductDisplay>();
		MassCoordinateProductDisplay[] YMassCoordinateProductDisplays = YMassCoordinateProductContainer.GetComponentsInChildren<MassCoordinateProductDisplay>();
		centerOfMassAnswerSubmission.SetMassTimesCoordinates(XMassCoordinateProductDisplays, YMassCoordinateProductDisplays);

		// Set SumOfMassTimesCoordinates for X and Y
		int SumOfMassTimesXCoordinates = int.Parse(XMassCoordinateProductSumResultField.text);
		int SumOfMassTimesYCoordinates = int.Parse(YMassCoordinateProductSumResultField.text);
		centerOfMassAnswerSubmission.SetSumOfMassTimesCoordinates(SumOfMassTimesXCoordinates, SumOfMassTimesYCoordinates);

		// Set Total Mass for X and Y
		int totalMassX = int.Parse(XSumOfMassResultField.text);
		int totalMassY = int.Parse(YSumOfMassResultField.text);
		centerOfMassAnswerSubmission.SetTotalMass(totalMassX, totalMassY);

		// Set Center of Mass for X and Y
		float? centerOfMassX = xCenterOfMassEquationDisplay.centerOfMassValue;
		float? centerOfMassY = yCenterOfMassEquationDisplay.centerOfMassValue;
		centerOfMassAnswerSubmission.SetCenterOfMass(centerOfMassX, centerOfMassY);

		SubmitAnswerEvent?.Invoke(centerOfMassAnswerSubmission);
	}
	#endregion
}