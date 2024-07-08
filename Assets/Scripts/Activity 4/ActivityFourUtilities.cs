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
}