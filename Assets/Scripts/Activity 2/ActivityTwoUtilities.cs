using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class ActivityTwoUtilities
{
	public static float EvaluateNumericalExpressions(DraggableNumericalExpression[] numericalExpressions)
	{
		float currentValue = numericalExpressions.Length > 0 ? 1 : 0;
		foreach (DraggableNumericalExpression expression in numericalExpressions)
		{
			string expressionText = expression.numericalExpression;
			expressionText = System.Text.RegularExpressions.Regex.Replace(expressionText, @"\bsin\(([^)]+)\)", m => $"sin({m.Groups[1].Value}*pi/180)");
			expressionText = System.Text.RegularExpressions.Regex.Replace(expressionText, @"\bcos\(([^)]+)\)", m => $"cos({m.Groups[1].Value}*pi/180)");
			expressionText = System.Text.RegularExpressions.Regex.Replace(expressionText, @"\btan\(([^)]+)\)", m => $"tan({m.Groups[1].Value}*pi/180)");

			bool canEvaluate = ExpressionEvaluator.Evaluate(expressionText, out float value);
			if (canEvaluate) currentValue *= value;
		}
		return currentValue;
	}

	public static bool ValidateValueSubmission(string submittedText, float correctValue)
	{
		bool canEvaluate = ExpressionEvaluator.Evaluate(submittedText, out float value);
		return canEvaluate && Mathf.Abs((float)(value - correctValue)) <= 0.0001;
	}

	public static bool ValidateComponentInputFields(List<float> trueComponentValues, HorizontalLayoutGroup componentContainer)
	{
		TMP_InputField[] inputFields = componentContainer.GetComponentsInChildren<TMP_InputField>();
		List<float> submittedComponentValues = inputFields.Select(field =>
		{
			if (float.TryParse(field.text, out float value))
			{
				return value;
			}
			else
			{
				return 0;
			}
		}).ToList();

		var trueComponentCounts = trueComponentValues.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
		var submittedComponentCounts = submittedComponentValues.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());

		return trueComponentCounts.Count == submittedComponentCounts.Count
			&& !trueComponentCounts.Except(submittedComponentCounts).Any();
	}

	public static bool ValidateMagnitudeSubmission(float xSum, float ySum, TMP_InputField magnitudeResultField)
	{
		// Evaluate magnitude result field
		bool isMagnitudeResultEvaluated = ExpressionEvaluator.Evaluate(magnitudeResultField.text, out double magnitudeResult);
		if (isMagnitudeResultEvaluated == false) return false;

		// Evaluate magnitude equation with xSum and ySum
		bool canEvaluateMagnitude = ExpressionEvaluator.Evaluate($"sqrt(({xSum})^2+({ySum})^2)", out double computationResult);
		if (canEvaluateMagnitude)
		{
			computationResult = Math.Round(computationResult, 4);
			return Math.Abs(computationResult - magnitudeResult) <= 0.0001;
		}
		return false;
	}

	public static bool ValidateDirectionSubmission(float xSum, float ySum, TMP_InputField directionResultField)
	{
		// Evaluate direction result field
		bool isDirectionResultEvaluated = ExpressionEvaluator.Evaluate(directionResultField.text, out double directionResult);
		if (isDirectionResultEvaluated == false) return false;

		// Evaluate direction equation with xSum and ySum
		double computationResult = Math.Atan2(ySum, xSum) * (180 / Math.PI);
		computationResult = Math.Round(computationResult, 4);
		return Math.Abs(computationResult - directionResult) <= 0.0001;
	}
}