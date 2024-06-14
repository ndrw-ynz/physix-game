using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

		submitButton.onClick.AddListener(() => CheckSubmission(vectorInfo));
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

	private void CheckSubmission(VectorInfo vectorInfo)
	{
		// Check submitted equation for x component
		DraggableNumericalExpression[] xComponentNumericalExpressions = xComponentEquationHolder.expressionHolder.GetComponentsInChildren<DraggableNumericalExpression>();
		float xComponentEquationResult = EvaluateNumericalExpressions(xComponentNumericalExpressions);
		Debug.Log($"Resulting x equation value: {xComponentEquationResult}");

		// Check submitted value for x component
		bool isXValueCorrect = IsValueSubmissionCorrect(xComponentInputText.text, vectorInfo.vectorComponents.x);
		Debug.Log($"Result of submitted x: {isXValueCorrect}");

		// Check submitted equation for y component
		DraggableNumericalExpression[] yComponentNumericalExpressions = yComponentEquationHolder.expressionHolder.GetComponentsInChildren<DraggableNumericalExpression>();
		float yComponentEquationResult = EvaluateNumericalExpressions(yComponentNumericalExpressions);
		Debug.Log($"Resulting y equation value: {yComponentEquationResult}");

		// Check submitted value for y component
		bool isYValueCorrect = IsValueSubmissionCorrect(yComponentInputText.text, vectorInfo.vectorComponents.y);
		Debug.Log($"Result of submitted y: {isYValueCorrect}");
	}

	private float EvaluateNumericalExpressions(DraggableNumericalExpression[] numericalExpressions)
	{
		float currentValue = numericalExpressions.Length == 2 ? 1 : 0; // beware of errors here.
		foreach (DraggableNumericalExpression expression in numericalExpressions)
		{
			string expressionText = expression.numericalExpression;
			expressionText = System.Text.RegularExpressions.Regex.Replace(expressionText, @"\bsin\(([^)]+)\)", m => $"sin({ConvertToRadians(m.Groups[1].Value)})");
			expressionText  = System.Text.RegularExpressions.Regex.Replace(expressionText, @"\bcos\(([^)]+)\)", m => $"cos({ConvertToRadians(m.Groups[1].Value)})");
			expressionText = System.Text.RegularExpressions.Regex.Replace(expressionText, @"\btan\(([^)]+)\)", m => $"tan({ConvertToRadians(m.Groups[1].Value)})");

			bool canEvaluate = ExpressionEvaluator.Evaluate(expressionText, out float value);
			if (canEvaluate) currentValue *= value;
		}
		return currentValue;
	}

	private string ConvertToRadians(string degrees)
	{
		double degreesValue;
		if (double.TryParse(degrees, out degreesValue))
		{
			double radians = degreesValue * (Math.PI / 180);
			return radians.ToString();
		}
		else
		{
			throw new ArgumentException("Invalid degrees value");
		}
	}

	private bool IsValueSubmissionCorrect(string submittedText, float correctValue)
	{
		bool canEvaluate = ExpressionEvaluator.Evaluate(submittedText, out float value);
		return canEvaluate && value == correctValue;
	}
}