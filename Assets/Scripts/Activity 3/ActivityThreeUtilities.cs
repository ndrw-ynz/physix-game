using System.Collections.Generic;
using UnityEngine;

public class GraphsAnswerSubmissionResults
{
    public bool isPositionVsTimeGraphCorrect;
    public bool isVelocityVsTimeGraphCorrect;
    public bool isAccelerationVsTimeGraphCorrect;

	public bool isAllCorrect()
	{
		return isPositionVsTimeGraphCorrect && isVelocityVsTimeGraphCorrect && isAccelerationVsTimeGraphCorrect;
	}
}

public static class ActivityThreeUtilities
{
    public static GraphsAnswerSubmissionResults ValidateGraphSubmission(GraphsAnswerSubmission answer, List<int> correctPositionValues, List<int> correctVelocityValues, List<int> correctAccelerationValues)
    {
		GraphsAnswerSubmissionResults results = new GraphsAnswerSubmissionResults();
		results.isPositionVsTimeGraphCorrect = ValidateGraphSubmission(answer.positionVsTimeGraph, correctPositionValues);
		results.isVelocityVsTimeGraphCorrect = ValidateGraphSubmission(answer.velocityVsTimeGraph, correctVelocityValues);
		results.isAccelerationVsTimeGraphCorrect = ValidateGraphSubmission(answer.accelerationVsTimeGraph, correctAccelerationValues);

		return results;
    }

    private static bool ValidateGraphSubmission(Graph graph, List<int> correctGraphPoints)
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

    public static bool ValidateAccelerationSubmission(float? accelerationAnswer, AccelerationCalculationData givenData)
    {
		if (accelerationAnswer == null) return false;

		ExpressionEvaluator.Evaluate($"({givenData.finalVelocity} - {givenData.initialVelocity}) / {givenData.totalTime}", out float computationResult);
        return Mathf.Abs((float) accelerationAnswer - computationResult) <= 0.0001;
	}

    public static bool ValidateTotalDepthSubmission(float? totalDepthAnswer, TotalDepthCalculationData givenData)
    {
		if (totalDepthAnswer == null) return false;

        ExpressionEvaluator.Evaluate($"{givenData.initialVelocity} * {givenData.totalTime} + ( (-9.81 * {givenData.totalTime}^2) / 2 )", out float computationResult);
        return Mathf.Abs((float) totalDepthAnswer - computationResult) <= 0.0001;
    }
}