using System;
using UnityEngine;

public static class ActivityNineUtilities
{
	public static double EvaluateScientificNotation(float coefficient, int exponent)
	{
		ExpressionEvaluator.Evaluate($"{coefficient} * (10 ^ ({exponent}))", out double result);
		result = Math.Round(result, 4);
		return result;
	}
}