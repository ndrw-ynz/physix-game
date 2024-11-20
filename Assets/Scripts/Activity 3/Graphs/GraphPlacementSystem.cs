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

		if (graphManager.currentGraph && graphManager.canEditGraph)
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
		if (graphManager.currentGraph && graphManager.canEditGraph)
		{
			SceneSoundManager.Instance.PlaySFX("Click");

			Graph currentGraph = graphManager.currentGraph;

			Vector3 mousePosition = graphManager.GetSelectedMapPosition();
			Vector3Int gridPosition = currentGraph.graphGrid.WorldToCell(mousePosition);

			// Exit if gridPosition goes outside boundary.
			if (Math.Abs(gridPosition.z) > currentGraph.gridColumnBoundary || gridPosition.x < 0 || gridPosition.x > currentGraph.graphLineRenderer.positionCount-1)
			{
				return;
			}
			
			// Remove duplicate point
			GameObject dupicatePoint = currentGraph.GetDuplicatePointOnColumn(gridPosition.x);
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
}