using System;
using UnityEngine;

public class GraphPlacementSystem : MonoBehaviour
{
	[Header("Indicators")]
    [SerializeField] private GameObject mouseIndicator;
	[SerializeField] private GameObject pointIndicator;
	[Header("Graph Manager")]
    [SerializeField] private GraphManager graphManager;
	[Header("Graphs")]
	[SerializeField] private Graph positionVsTimeGraph;
	[SerializeField] private Graph velocityVsTimeGraph;
	[SerializeField] private Graph accelerationVsTimeGraph;

	private int numGridRecordRows = 9;
	private int numGridRecordCols = 9;
	private GameObject[,] gridRecord;

	private int gridRowBoundary = 8;
	private int gridColumnBoundary = 4;

	private void Start()
	{
		gridRecord = new GameObject[numGridRecordRows, numGridRecordCols];
		graphManager.OnMouseClick += PlacePoint;
	}

	private void Update()
	{
		if (graphManager.currentGraph)
		{
			Vector3 mousePosition = graphManager.GetSelectedMapPosition();
			Vector3Int gridPosition = graphManager.currentGraph.graphGrid.WorldToCell(mousePosition);
			mouseIndicator.transform.position = mousePosition;
			pointIndicator.transform.position = graphManager.currentGraph.graphGrid.CellToWorld(gridPosition);
		} else
		{
			mouseIndicator.transform.position = new Vector3(0, -1, 0);
			pointIndicator.transform.position = new Vector3(0, -1, 0);
		}
	}

	private void PlacePoint()
	{
		if (graphManager.currentGraph)
		{
			Vector3 mousePosition = graphManager.GetSelectedMapPosition();
			Vector3Int gridPosition = graphManager.currentGraph.graphGrid.WorldToCell(mousePosition);
			Debug.Log(gridPosition);

			// Exit if gridPosition goes outside boundary.
			if (Math.Abs(gridPosition.z) > gridColumnBoundary || gridPosition.x < 0 || gridPosition.x > gridRowBoundary)
			{
				return;
			}
			
			// Remove duplicate point
			GameObject dupicatePoint = DuplicatePointOnColumn(gridPosition.x);
			if (dupicatePoint)
			{
				Destroy(dupicatePoint);
			}

			// Update point on line renderer on current graph
			graphManager.currentGraph.UpdateColumnPointOnGraph(gridPosition.x, gridPosition.z); 

			UpdatePointsFromPosition();

			GameObject newPoint = Instantiate(pointIndicator);
			newPoint.transform.position = graphManager.currentGraph.graphGrid.CellToWorld(gridPosition);
			gridRecord[gridPosition.z+4, gridPosition.x] = newPoint;
		}
	}

	private GameObject DuplicatePointOnColumn(int column)
	{
		for (int y = 0; y < numGridRecordRows; y++)
		{
			if (gridRecord[y, column])
			{
				return gridRecord[y, column];
			}
		}
		return null;
	}

	// REMOVE THIS ONE
	private void UpdatePointsFromPosition()
	{
		// Fetch points from position.
		Vector3[] positionPoints = positionVsTimeGraph.GetGraphPoints();
		float[] timeValues = new float[positionPoints.Length];
		float[] positionValues = new float[positionPoints.Length];

		for (int i = 0; i < positionPoints.Length; i ++)
		{
			timeValues[i] = positionPoints[i].z;
			positionValues[i] = positionPoints[i].x;
		}

		// Compute velocity and acceleration values.
		int[] velocityValues = ComputeDiscreteVelocities(timeValues, positionValues);
		int[] accelerationValues = ComputeDiscreteAccelerations(timeValues, velocityValues);

		// Update velocity and acceleration vs. time graphs.
		for (int i = 0; i < timeValues.Length; i++)
		{
			velocityVsTimeGraph.UpdateColumnPointOnGraph(i, velocityValues[i]);
			accelerationVsTimeGraph.UpdateColumnPointOnGraph(i, accelerationValues[i]);
		}
	}

	// NOO. REMOVE THIS ONE.
	private int[] ComputeDiscreteVelocities(float[] times, float[] positions)
	{
		int n = times.Length;
		int[] velocities = new int[n];

		// Forward difference for the first point
		float firstPoint = (positions[1] - positions[0]) / (times[1] - times[0]);
		if (IsDiscreteInteger(firstPoint))
		{
			velocities[0] = Mathf.RoundToInt(firstPoint);
		} else
		{
			Debug.Log("not discrete: " + firstPoint);
			velocities[0] = 0;
		}

		// Central difference for the interior points
		for (int i = 1; i < n - 1; i++)
		{
			float calculatedPoint = (positions[i + 1] - positions[i - 1]) / (times[i + 1] - times[i - 1]);
			if (IsDiscreteInteger(calculatedPoint))
			{
				velocities[i] = Mathf.RoundToInt((positions[i + 1] - positions[i - 1]) / (times[i + 1] - times[i - 1]));
			} else
			{
				Debug.Log("not discrete: " + calculatedPoint);
				velocities[i] = 0;
			}
		}

		// Backward difference for the last point
		float lastPoint = (positions[n - 1] - positions[n - 2]) / (times[n - 1] - times[n - 2]);
		if (IsDiscreteInteger(lastPoint))
		{
			velocities[n - 1] = Mathf.RoundToInt(lastPoint);
		} else
		{
			Debug.Log("not discrete: " + lastPoint);
			velocities[n - 1] = 0;
		}

		return velocities;
	}

	// NO. REMOVE THIS ONE.
	private int[] ComputeDiscreteAccelerations(float[] times, int[] velocities)
	{
		int n = times.Length;
		int[] accelerations = new int[n];

		// Forward difference for the first point
		float firstPoint = (velocities[1] - velocities[0]) / (times[1] - times[0]);
		if (IsDiscreteInteger(firstPoint))
		{
			accelerations[0] = Mathf.RoundToInt(firstPoint);
		} else
		{
			Debug.Log("not discrete: " + firstPoint);
			accelerations[0] = 0;
		}

		// Central difference for the interior points
		for (int i = 1; i < n - 1; i++)
		{
			float calculatedPoint = (velocities[i + 1] - velocities[i - 1]) / (times[i + 1] - times[i - 1]);
			if (IsDiscreteInteger(calculatedPoint))
			{
				accelerations[i] = Mathf.RoundToInt(calculatedPoint);
			} else
			{
				Debug.Log("not discrete: " + calculatedPoint);
				accelerations[i] = 0;
			}
		}

		// Backward difference for the last point
		float lastPoint = (velocities[n - 1] - velocities[n - 2]) / (times[n - 1] - times[n - 2]);
		if (IsDiscreteInteger(lastPoint))
		{
			accelerations[n - 1] = Mathf.RoundToInt(lastPoint);
		} else
		{
			Debug.Log("not discrete: " + lastPoint);
			accelerations[n - 1] = 0;
		}

		return accelerations;
	}

	// NO. REMOVE THIS ONE.
	public static bool IsDiscreteInteger(float value, float tolerance = 1e-6f)
	{
		// Get the nearest integer to the float value
		int nearestInteger = Mathf.RoundToInt(value);
		// Check if the absolute difference is within the specified tolerance
		return Mathf.Abs(value - nearestInteger) < tolerance;
	}
}