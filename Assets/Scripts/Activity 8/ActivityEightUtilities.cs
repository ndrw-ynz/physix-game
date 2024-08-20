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

	/// <summary>
	/// Validates submitted Summation of Downward Forces value based from <c>EquilibriumData</c>.
	/// </summary>
	/// <param name="submittedSummationOfDownwardForces"></param>
	/// <param name="equilibriumData"></param>
	/// <returns></returns>
	public static bool ValidateSummationOfDownwardForcesSubmission(float? submittedSummationOfDownwardForces, EquilibriumData equilibriumData)
	{
		if (submittedSummationOfDownwardForces == null) return false;

		// Formula: -redBoxWeight + -blueBoxWeight + -weighingApparatusWeight
		float computationResult = -equilibriumData.redBoxWeight + -equilibriumData.blueBoxWeight + -equilibriumData.weighingApparatusWeight;

		return Mathf.Abs((float)submittedSummationOfDownwardForces - computationResult) <= 0.0001;
	}

	/// <summary>
	/// Validates submitted Upward Force value based from <c>EquilibriumData</c>.
	/// </summary>
	/// <param name="submittedUpwardForce"></param>
	/// <param name="equilibriumData"></param>
	/// <returns></returns>
	public static bool ValidateUpwardForceSubmission(float? submittedUpwardForce, EquilibriumData equilibriumData)
	{
		if (submittedUpwardForce == null) return false;

		// submittedUpwardForce should be equal to fulcrumForce.
		return Mathf.Abs((float)submittedUpwardForce - equilibriumData.fulcrumForce) <= 0.0001;
	}

	/// <summary>
	/// Validates submitted Summation of Total Forces value based from <c>EquilibriumData</c>.
	/// </summary>
	/// <param name="submittedSummationOfTotalForces"></param>
	/// <param name="equilibriumData"></param>
	/// <returns></returns>
	public static bool ValidateSummationOfTotalForcesSubmission(float? submittedSummationOfTotalForces, EquilibriumData equilibriumData)
	{
		if (submittedSummationOfTotalForces == null) return false;

		// Formula: -redBoxWeight + -blueBoxWeight + -weighingApparatusWeight + fulcrumForce
		float computedSummationDownwardForces = -equilibriumData.redBoxWeight + -equilibriumData.blueBoxWeight + -equilibriumData.weighingApparatusWeight;
		float upwardForce = equilibriumData.fulcrumForce;

		float computationResult = computedSummationDownwardForces + upwardForce;

		return Mathf.Abs((float)submittedSummationOfTotalForces - computationResult) <= 0.0001;
	}

	/// <summary>
	/// Validates submitted <c>EquilibriumType</c> based from <c>EquilibriumData</c>.
	/// </summary>
	/// <param name="submittedEquilibriumType"></param>
	/// <param name="equilibriumData"></param>
	/// <returns></returns>
	public static bool ValidateEquilibriumTypeSubmission(EquilibriumType? submittedEquilibriumType, EquilibriumData equilibriumData)
	{
		if (submittedEquilibriumType == null) return false;

		// Condition 1: Summation of Total Forces = 0
		float summationOfTotalForces = -equilibriumData.redBoxWeight + -equilibriumData.blueBoxWeight + -equilibriumData.weighingApparatusWeight + equilibriumData.fulcrumForce;
		bool isTotalForceOnEquilibrium = Mathf.Abs(summationOfTotalForces) <= 0.001;

		// Condition 2: counterClockwiseTorque = clockwiseTorque
		float counterclockwiseTorque = equilibriumData.redBoxWeight * equilibriumData.redBoxDistance;
		float clockwiseTorque = equilibriumData.blueBoxWeight * equilibriumData.blueBoxDistance;
		bool isTorqueOnEquilibrium = Mathf.Abs(counterclockwiseTorque - clockwiseTorque) <= 0.001;

		bool computedEquilibriumResult = isTotalForceOnEquilibrium && isTorqueOnEquilibrium;

		if (
			(computedEquilibriumResult == true && submittedEquilibriumType == EquilibriumType.InEquilibrium) ||
			(computedEquilibriumResult == false && submittedEquilibriumType == EquilibriumType.NotInEquilibrium)
			)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}