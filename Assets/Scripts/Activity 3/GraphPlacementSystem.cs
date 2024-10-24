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

	private void Start()
	{
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
			Graph currentGraph = graphManager.currentGraph;

			Vector3 mousePosition = graphManager.GetSelectedMapPosition();
			Vector3Int gridPosition = currentGraph.graphGrid.WorldToCell(mousePosition);
			Debug.Log(gridPosition);

			// Exit if gridPosition goes outside boundary.
			if (Math.Abs(gridPosition.z) > currentGraph.gridColumnBoundary || gridPosition.x < 0 || gridPosition.x > currentGraph.gridRowBoundary)
			{
				return;
			}
			
			// Remove duplicate point
			GameObject dupicatePoint = DuplicatePointOnColumn(currentGraph, gridPosition.x);
			if (dupicatePoint)
			{
				Destroy(dupicatePoint);
			}

			// Update point on line renderer on current graph
			currentGraph.UpdateColumnPointOnGraph(gridPosition.x, gridPosition.z); 

			GameObject newPoint = Instantiate(pointIndicator);
			newPoint.transform.position = currentGraph.graphGrid.CellToWorld(gridPosition);
			currentGraph.gridRecord[gridPosition.z + currentGraph.gridColumnBoundary, gridPosition.x] = newPoint;
		}
	}

	private GameObject DuplicatePointOnColumn(Graph graph, int column)
	{
		for (int y = 0; y < graph.numGridRecordRows; y++)
		{
			if (graph.gridRecord[y, column])
			{
				return graph.gridRecord[y, column];
			}
		}
		return null;
	}
}