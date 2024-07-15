using UnityEngine;

public static class ActivityFourUtilities
{
	public static bool ValidateMaximumHeightSubmission(float submittedMaximumHeight, float projectileInitialVelocity, float projectileAngle)
	{
		ExpressionEvaluator.Evaluate($"-({projectileInitialVelocity})^2*sin({projectileAngle}*(pi/180))/(2*-9.81)", out float computationResult);
		Debug.Log($"computed result: {computationResult}");
		Debug.Log($"submitted : {submittedMaximumHeight}");
		return Mathf.Abs(submittedMaximumHeight - computationResult) <= 0.0001;
	}

	public static bool ValidateHorizontalRangeSubmission(float submittedHorizontalRange, float projectileInitialVelocity)
	{
		ExpressionEvaluator.Evaluate($"-({projectileInitialVelocity})^2*sin(pi/2)/(2*-9.81)", out float computationResult);
		Debug.Log($"computed result: {computationResult}");
		Debug.Log($"submitted : {submittedHorizontalRange}");
		return Mathf.Abs(submittedHorizontalRange - computationResult) <= 0.0001;
	}

	public static bool ValidateTimeOfFlightSubmission(float timeOfFlight, float projectileInitialVelocity, float projectileAngle)
	{
		ExpressionEvaluator.Evaluate($"-({projectileInitialVelocity})*sin({projectileAngle}*(pi/180))/-9.81", out float computationResult);
		Debug.Log($"computed result: {computationResult}");
		Debug.Log($"submitted : {timeOfFlight}");
		return Mathf.Abs(timeOfFlight - computationResult) <= 0.0001;
	}

	public static bool ValidateCentripetalAccelerationSubmission(float centripetalAcceleration, float satelliteRadius, float satelliteTimePeriod)
	{
		ExpressionEvaluator.Evaluate($"(4*(pi^2)*{satelliteRadius})/({satelliteTimePeriod}^2)", out float computationResult);
		Debug.Log($"Submitted: {centripetalAcceleration}");
		Debug.Log($"Computed Result: {computationResult}");
		return Mathf.Abs(centripetalAcceleration - computationResult) <= 0.0001;
	}
}