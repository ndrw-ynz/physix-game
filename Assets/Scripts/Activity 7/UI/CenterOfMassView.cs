using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CenterOfMassView : MonoBehaviour
{
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
	[SerializeField] private GameObject MassTimesXCoords;
	[SerializeField] private GameObject MassTimesYCoords;
	[SerializeField] private GameObject CenterOfMassXDir;
	[SerializeField] private GameObject CenterOfMassYDir;
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

		MassTimesXCoords.gameObject.SetActive(true);
		CenterOfMassXDir.gameObject.SetActive(true);

		MassTimesYCoords.gameObject.SetActive(false);
		CenterOfMassYDir.gameObject.SetActive(false);
	}

	public void OnRightPageButtonClick()
	{
		leftPageButton.gameObject.SetActive(true);
		rightPageButton.gameObject.SetActive(false);

		MassTimesXCoords.gameObject.SetActive(false);
		CenterOfMassXDir.gameObject.SetActive(false);

		MassTimesYCoords.gameObject.SetActive(true);
		CenterOfMassYDir.gameObject.SetActive(true);
	}
	#endregion
}