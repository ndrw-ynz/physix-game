using System;
using UnityEngine;

public class ProjectileMotionSubmissionResults
{
	public bool isMaximumHeightCorrect;
	public bool isHorizontalRangeCorrect;
	public bool isTimeOfFlightCorrect;

	public bool isAllCorrect()
	{
		return isMaximumHeightCorrect && isHorizontalRangeCorrect && isTimeOfFlightCorrect;
	}
}

public static class ActivityFourUtilities
{
	public static ProjectileMotionSubmissionResults ValidateProjectileMotionSubmission(ProjectileMotionAnswerSubmission answer, ProjectileMotionCalculationData givenData)
	{
		ProjectileMotionSubmissionResults results = new ProjectileMotionSubmissionResults();

		// Validate maximum height submission
		if (answer.maximumHeight != null)
		{
			ExpressionEvaluator.Evaluate($"(- ({givenData.initialVelocity}^2) * (sin({givenData.angleMeasure}*(pi/180)))^2) / (2 * -9.81)) + {givenData.initialHeight}", out float computedMaximumHeight);
			computedMaximumHeight = (float) Math.Round(computedMaximumHeight, 2);
			results.isMaximumHeightCorrect = Mathf.Abs(computedMaximumHeight - (float) answer.maximumHeight) <= 0.0001;
		} else
		{
			results.isMaximumHeightCorrect = false;
		}

		// Validate horizontal range submission
		if (answer.horizontalRange != null)
		{
			ExpressionEvaluator.Evaluate($"(- ({givenData.initialVelocity})^2 * sin(2*{givenData.angleMeasure}*(pi/180))) / (-9.81)", out float computedHorizontalRange);
			computedHorizontalRange = (float)Math.Round(computedHorizontalRange, 2);
			results.isHorizontalRangeCorrect = Mathf.Abs(computedHorizontalRange - (float)answer.horizontalRange) <= 0.0001;
		}
		else
		{
			results.isHorizontalRangeCorrect = false;
		}

		// Validate time of flight answer submission
		if (answer.timeOfFlight != null)
		{
			ExpressionEvaluator.Evaluate($"-(2 * {givenData.initialVelocity} * sin({givenData.angleMeasure}*(pi/180))) / (-9.81)", out float computedTimeOfFlight);
			computedTimeOfFlight = (float)Math.Round(computedTimeOfFlight, 2);
			results.isTimeOfFlightCorrect = Mathf.Abs(computedTimeOfFlight - (float)answer.timeOfFlight) <= 0.0001;
		}
		else
		{
			results.isTimeOfFlightCorrect = false;
		}

		return results;
	}

	public static bool ValidateCentripetalAccelerationSubmission(float? centripetalAccelerationAnswer, CircularMotionCalculationData givenData)
	{
		if (centripetalAccelerationAnswer == null) return false;

		ExpressionEvaluator.Evaluate($"(4*(pi^2)*{givenData.radius*1000})/({givenData.period}^2)", out float computedCentripetalAcceleration);
		computedCentripetalAcceleration = (float)Math.Round(computedCentripetalAcceleration, 4);
		return Mathf.Abs((float)centripetalAccelerationAnswer - computedCentripetalAcceleration) <= 0.01;
	}
}