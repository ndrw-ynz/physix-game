using System;
using UnityEngine;

public static class ActivityNineUtilities
{
	public static double EvaluateScientificNotation(float coefficient, int exponent)
	{
		ExpressionEvaluator.Evaluate($"{coefficient} * (10 ^ ({exponent}))", out double result);
		return result;
	}

	/// <summary>
	/// Validates submitted gravitational force value based from <c>GravityData</c>.
	/// </summary>
	/// <param name="submittedGravitationalForce"></param>
	/// <param name="gravityData"></param>
	/// <returns></returns>
	public static bool ValidateGravitationalForceSubmission(double? submittedGravitationalForce, GravityData gravityData)
	{
		if (submittedGravitationalForce == null) return false;

		// Formula: gravitationConstant * ( (massOne * massTwo) / distance ^ 2 )
		double gravitationConstantValue = EvaluateScientificNotation(6.6743f, -11);
		double planetMassValue = EvaluateScientificNotation(gravityData.planetMassSNCoefficient, gravityData.planetMassSNExponent);
		double orbittingObjectMassValue = EvaluateScientificNotation(gravityData.orbittingObjectMassSNCoefficient, gravityData.orbittingObjectMassSNExponent);
		double distanceValue = EvaluateScientificNotation((float) gravityData.distanceBetweenObjects, 3);
		
		double computationResult = gravitationConstantValue * planetMassValue * orbittingObjectMassValue / Math.Pow(distanceValue, 2);

		return Math.Abs((double)submittedGravitationalForce - computationResult) <= 0.0001;
	}

	/// <summary>
	/// Validates submitted GPE value based from <c>GravityData</c>.
	/// </summary>
	/// <param name="submittedGPE"></param>
	/// <param name="gravityData"></param>
	/// <returns></returns>
	public static bool ValidateGPESubmission(double? submittedGPE, GravityData gravityData)
	{
		if (submittedGPE == null) return false;

		// Formula: - gravitationConstant * ( (massOne * massTwo) / distance )
		double gravitationConstantValue = EvaluateScientificNotation(6.6743f, -11);
		double planetMassValue = EvaluateScientificNotation(gravityData.planetMassSNCoefficient, gravityData.planetMassSNExponent);
		double orbittingObjectMassValue = EvaluateScientificNotation(gravityData.orbittingObjectMassSNCoefficient, gravityData.orbittingObjectMassSNExponent);
		double distanceValue = EvaluateScientificNotation((float)gravityData.distanceBetweenObjects, 3);

		double computationResult = - gravitationConstantValue * planetMassValue * orbittingObjectMassValue / distanceValue;

		return Math.Abs((double)submittedGPE - computationResult) <= 0.0001;
	}
}