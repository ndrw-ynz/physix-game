using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
	[Header("Interactive Graph Camera")]
    public Camera interactiveGraphCamera;
    [Header("Grid")]
    public Grid graphGrid;
    [Header("Line Renderer")]
    public LineRenderer graphLineRenderer;
	[Header("Graph Point Indicator")]
	[SerializeField] private GameObject pointIndicator;
	[Header("Grid Record")]
	public GameObject[,] gridRecord;
	[Header("Grid Record Characteristics")]
	public int numGridRecordRows = 9;
	public int numGridRecordCols = 9;
	public int gridRowBoundary = 8;
	public int gridColumnBoundary = 4;

	public void InitializeGraph(List<int> pointValues)
	{
		graphLineRenderer.startWidth = 0.2f;
		graphLineRenderer.endWidth = 0.2f;
		graphLineRenderer.positionCount = pointValues.Count;

		gridRecord = new GameObject[numGridRecordRows, numGridRecordCols];
		
		for (int i = 0; i < pointValues.Count; i++)
		{
			Vector3Int gridPosition = new Vector3Int(i, 0, pointValues[i]);

			GameObject newPoint = Instantiate(pointIndicator);
			newPoint.transform.position = graphGrid.CellToWorld(gridPosition);
			gridRecord[gridPosition.z + gridColumnBoundary, gridPosition.x] = newPoint;

			graphLineRenderer.SetPosition(i, new Vector3(i, 0, pointValues[i]));
		}
	}

	public void UpdateGraphPoints(List<int> pointValues)
	{
		for (int i = 0; i < pointValues.Count; i++)
		{
			// Remove duplicate point
			GameObject dupicatePoint = GetDuplicatePointOnColumn(i);
			if (dupicatePoint)
			{
				Destroy(dupicatePoint);
			}

			// Update point on line renderer on current graph			
			UpdateColumnPointOnGraph(i, pointValues[i]);

			GameObject newPoint = Instantiate(pointIndicator);
			newPoint.transform.position = graphGrid.CellToWorld(new Vector3Int(i, 0, pointValues[i]));
			gridRecord[gridColumnBoundary, i] = newPoint;
		}
	}

	public void UpdateColumnPointOnGraph(int columnValue, int rowValue)
	{
		graphLineRenderer.SetPosition(columnValue, new Vector3(columnValue, 0, rowValue));
	}

	public Vector3[] GetGraphPoints()
	{
		int positionCount = graphLineRenderer.positionCount;
		Vector3[] positions = new Vector3[positionCount];

		for (int i = 0; i < positionCount; i++)
		{
			positions[i] = graphLineRenderer.GetPosition(i);
		}

		return positions;
	}

	public GameObject GetDuplicatePointOnColumn(int column)
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
}