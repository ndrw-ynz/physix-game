using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuantitiesAnswerSubmissionResults
{
	public bool hasUnsolvedQuantities;
	public bool isScalarListCorrect;
	public bool isVectorListCorrect;

	public bool isAllCorrect()
	{
		return !hasUnsolvedQuantities && isScalarListCorrect && isVectorListCorrect;
	}
}

public class CartesianComponentsAnswerSubmissionResults
{
	public bool isVectorXComponentCorrect;
	public bool isVectorYComponentCorrect;

	public bool isAllCorrect()
	{
		return isVectorXComponentCorrect && isVectorYComponentCorrect;
	}
}

public static class ActivityTwoUtilities
{
	public static QuantitiesAnswerSubmissionResults ValidateQuantitiesSubmission(QuantitiesAnswerSubmission answer)
	{
		// Set initial results to correct values
		QuantitiesAnswerSubmissionResults results = new QuantitiesAnswerSubmissionResults();
		results.hasUnsolvedQuantities = false;
		results.isScalarListCorrect = true;
		results.isVectorListCorrect = true;

		// Checking of submitted unsolved quantities.
		foreach (DraggableQuantityText quantity in answer.unsolvedQuantities)
		{
			if (quantity.quantityType == QuantityType.Scalar)
			{
				results.isScalarListCorrect = false;
				results.hasUnsolvedQuantities = true;
			} else
			{
				results.isVectorListCorrect = false;
				results.hasUnsolvedQuantities = true;
			}
		}

		// count number of scalar and vector. then, check if scalar = ok , and if scalar thuz far

		// Checking of submitted scalar quantities.
		foreach (DraggableQuantityText quantity in answer.scalarQuantities)
		{
			if (quantity.quantityType != QuantityType.Scalar)
			{
				results.isScalarListCorrect = false;
				results.isVectorListCorrect = false;
			}
		}

		// Checking of submitted vector quantities.
		foreach (DraggableQuantityText quantity in answer.vectorQuantities)
		{
			if (quantity.quantityType != QuantityType.Vector)
			{
				results.isScalarListCorrect = false;
				results.isVectorListCorrect = false;
			}
		}

		return results;
	}

	public static CartesianComponentsAnswerSubmissionResults ValidateCartesianComponentsSubmission(CartesianComponentsAnswerSubmission answer, VectorData givenVectorData)
	{
		CartesianComponentsAnswerSubmissionResults results = new CartesianComponentsAnswerSubmissionResults();

		// Compute and validate x-component
		if (answer.vectorXComponent != null)
		{
			ExpressionEvaluator.Evaluate($"{givenVectorData.magnitude} * cos({givenVectorData.angleMeasure}*(pi/180))", out float computedXComponent);
			results.isVectorXComponentCorrect = Mathf.Abs(computedXComponent - (float)answer.vectorXComponent) <= 0.0001;
		} else
		{
			results.isVectorXComponentCorrect = false;
		}

		// Compute and validate y-component
		if (answer.vectorYComponent != null)
		{
			ExpressionEvaluator.Evaluate($"{givenVectorData.magnitude} * sin({givenVectorData.angleMeasure}*(pi/180))", out float computedYComponent);
			results.isVectorYComponentCorrect = Mathf.Abs(computedYComponent - (float)answer.vectorYComponent) <= 0.0001;
		} else
		{
			results.isVectorYComponentCorrect = false;
		}

		return results;
	}

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