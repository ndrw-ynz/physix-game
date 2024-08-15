using System;
using UnityEngine;

/// <summary>
/// A utility class containing functions for validating submitted answers for Activity Eight.
/// </summary>
public static class ActivityEightUtilities
{
	/// <summary>
	/// Validates submitted Moment of Inertia value based from <c>MomentOfInertiaData</c>.
	/// </summary>
	/// <param name="submittedMomentOfInertia"></param>
	/// <param name="momentOfInertiaData"></param>
	/// <returns></returns>
	public static bool ValidateMomentOfInertiaSubmission(float? submittedMomentOfInertia, MomentOfInertiaData momentOfInertiaData)
	{
		if (submittedMomentOfInertia == null) return false;

		string equationText = "";
		switch (momentOfInertiaData.inertiaObjectType)
		{
			case InertiaObjectType.SlenderRodCenter:
				equationText = $"1/12 * ({momentOfInertiaData.mass}) * ({momentOfInertiaData.length})^2";
				break;
			case InertiaObjectType.SlenderRodEnd:
				equationText = $"1/3 * ({momentOfInertiaData.mass}) * ({momentOfInertiaData.length})^2";
				break;
			case InertiaObjectType.RectangularPlateCenter:
				equationText = $"1/2 * {momentOfInertiaData.mass} * ({momentOfInertiaData.plateLengthA}^2 + {momentOfInertiaData.plateLengthB}^2)";
				break;
			case InertiaObjectType.RectangularPlateEdge:
				equationText = $"1/3 * {momentOfInertiaData.mass} * {momentOfInertiaData.plateLengthA}^2";
				break;
			case InertiaObjectType.HollowCylinder:
				equationText = $"1/2 * {momentOfInertiaData.mass} * ({momentOfInertiaData.innerRadius} + {momentOfInertiaData.outerRadius})";
				break;
			case InertiaObjectType.SolidCylinder:
				equationText = $"1/12 * {momentOfInertiaData.mass} * {momentOfInertiaData.radius}^2";
				break;
			case InertiaObjectType.ThinWalledHollowCylinder:
				equationText = $"{momentOfInertiaData.mass} * {momentOfInertiaData.radius}^2";
				break;
			case InertiaObjectType.SolidSphere:
				equationText = $"2/5 * {momentOfInertiaData.mass} * {momentOfInertiaData.radius}^2";
				break;
			case InertiaObjectType.ThinWalledHollowSphere:
				equationText = $"2/3 * {momentOfInertiaData.mass} * {momentOfInertiaData.radius}^2";
				break;
			case InertiaObjectType.SolidDisk:
				equationText = $"1/2 * {momentOfInertiaData.mass} * {momentOfInertiaData.radius}^2";
				break;
		}

		bool canEvaluate = ExpressionEvaluator.Evaluate(equationText, out float result);
		if (canEvaluate)
		{
			float resultValue = (float)Math.Round(result, 4);
			return Mathf.Abs((float)submittedMomentOfInertia - resultValue) <= 0.0001;
		}
		return false;
	}

	/// <summary>
	/// Validates submitted Torque magnitude value based from <c>TorqueData</c>.
	/// </summary>
	/// <param name="submittedTorqueMagnitude"></param>
	/// <param name="torqueData"></param>
	/// <returns></returns>
	public static bool ValidateTorqueMagnitudeSubmission(float? submittedTorqueMagnitude, TorqueData torqueData)
	{
		if (submittedTorqueMagnitude == null) return false;

		// Formula: F * r
		float computationResult = torqueData.force * torqueData.distanceVector;

		return Mathf.Abs((float)submittedTorqueMagnitude - computationResult) <= 0.0001;
	}
}