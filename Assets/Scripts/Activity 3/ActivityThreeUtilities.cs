using System.Collections.Generic;
using UnityEngine;

public static class ActivityThreeUtilities
{
    public static bool ValidateGraphSubmission(List<int> correctGraphPoints, Graph graph)
    {
		Vector3[] graphPoints = graph.GetGraphPoints();

        for (int i = 0; i < graphPoints.Length; i++)
        {
            if (correctGraphPoints[i] != graphPoints[i].z)
            {
                return false;
            }
        }

        return true;
    }

    public static bool ValidateAccelerationSubmission(float submittedAcceleration, float initialVelocity, float finalVelocity, float totalTime)
    {
		ExpressionEvaluator.Evaluate($"({finalVelocity} - {initialVelocity}) / ({totalTime} * (1/60))", out float computationResult);
        return Mathf.Abs(submittedAcceleration - computationResult) <= 0.0001;
	}

    public static bool ValidateFreeFallSubmission(float submittedFreeFall, float totalTime)
    {
        ExpressionEvaluator.Evaluate($"-9.81*({totalTime}*60)^2 / 2", out float computationResult);
        Debug.Log(computationResult);
        return Mathf.Abs(submittedFreeFall - computationResult) <= 0.0001;
    }
}