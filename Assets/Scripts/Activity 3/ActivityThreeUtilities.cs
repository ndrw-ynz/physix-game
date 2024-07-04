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
}
