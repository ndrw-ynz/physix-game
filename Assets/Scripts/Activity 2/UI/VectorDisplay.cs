using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VectorDisplay : MonoBehaviour
{
	// this just contains information about the VIEW DISPLAYS FOR UI
	// list all the elements and then link them up! then create methods for use of the display.
	// seven components here...
	[Header("Text Information")]
	public TextMeshProUGUI vectorTextInfo;
	[Header("Equation Holders")]
	public NumericalExpressionHolder xComponentEquationHolder;
	public NumericalExpressionHolder yComponentEquationHolder;
	[Header("Prefabs")]
	public DraggableNumericalExpression draggableNumericalExpressionPrefab;
	[Header("Choice Holders")]
	public NumericalExpressionHolder choiceHolderLeft;
	public NumericalExpressionHolder choiceHolderRight;
	[Header("n-Component Input Text")]
	public TMP_InputField xComponentInputText;
	public TMP_InputField yComponentInputText;
	[Header("Submit Button")]
	public Button submitButton;

	public void SetupVectorDisplay(VectorInfo vectorInfo)
	{
		vectorTextInfo.text = $"{vectorInfo.magnitudeValue}m {vectorInfo.directionValue}°";

		SetupChoiceHolders(vectorInfo);
	}

	private void SetupChoiceHolders(VectorInfo vectorInfo)
	{
		// Add two true magnitude values for left and right choice holder.
		DraggableNumericalExpression trueMagnitudeExpressionLeft = Instantiate(draggableNumericalExpressionPrefab);
		trueMagnitudeExpressionLeft.Initialize($"{vectorInfo.magnitudeValue}");
		trueMagnitudeExpressionLeft.transform.SetParent(choiceHolderLeft.transform, false);
		
		DraggableNumericalExpression trueMagnitudeExpressionRight = Instantiate(draggableNumericalExpressionPrefab);
		trueMagnitudeExpressionRight.Initialize($"{vectorInfo.magnitudeValue}");
		trueMagnitudeExpressionRight.transform.SetParent(choiceHolderRight.transform, false);
		
		// Add two true trigonometric expressions - cos and sin for left and right choice holder respectively.
		DraggableNumericalExpression trueTrigonometricExpressionX = Instantiate(draggableNumericalExpressionPrefab);
		trueTrigonometricExpressionX.Initialize($"cos({vectorInfo.directionValue})");
		trueTrigonometricExpressionX.transform.SetParent(choiceHolderLeft.transform, false);

		DraggableNumericalExpression trueTrigonometricExpressionY = Instantiate(draggableNumericalExpressionPrefab);
		trueTrigonometricExpressionY.Initialize($"sin({vectorInfo.directionValue})");
		trueTrigonometricExpressionY.transform.SetParent(choiceHolderRight.transform, false);

		// Create four fake counterparts for left and right choice holder.
		for (int i = 0; i < 2; i++)
		{
			int randomizedDirectionValue = 0;
			switch (vectorInfo.directionType)
			{
				case DirectionType.Cardinal:
					HashSet<int> cardinalDirectionsSet = new HashSet<int> { 0, 90, 180, 270 };
					cardinalDirectionsSet.Remove(vectorInfo.directionValue);

					List<int> cardinalDirectionsList = new List<int>(cardinalDirectionsSet);
					randomizedDirectionValue = cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
					break;
				case DirectionType.Standard:
					HashSet<int> standardDirectionsSet = new HashSet<int> { 0, 30, 45, 60, 90, 120, 135, 1150, 180, 210, 225, 240, 270, 300, 315, 330 };
					standardDirectionsSet.Remove(vectorInfo.directionValue);

					List<int> standardDirectionsList = new List<int>(standardDirectionsSet);
					randomizedDirectionValue = standardDirectionsList[Random.Range(0, standardDirectionsList.Count)];
					break;
				case DirectionType.FullRange:
					randomizedDirectionValue = Random.Range(0, 360);
					break;
			}

			DraggableNumericalExpression fakeTrigonometricExpressionX = Instantiate(draggableNumericalExpressionPrefab);
			fakeTrigonometricExpressionX.Initialize($"sin({randomizedDirectionValue})");
			fakeTrigonometricExpressionX.transform.SetParent(choiceHolderLeft.transform, false);

			DraggableNumericalExpression fakeTrigonometricExpressionY = Instantiate(draggableNumericalExpressionPrefab);
			fakeTrigonometricExpressionY.Initialize($"cos({randomizedDirectionValue})");
			fakeTrigonometricExpressionY.transform.SetParent(choiceHolderRight.transform, false);
		}
	}
}
