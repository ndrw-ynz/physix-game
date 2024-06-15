using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
