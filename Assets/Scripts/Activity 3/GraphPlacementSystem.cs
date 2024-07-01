using UnityEngine;

public class GraphPlacementSystem : MonoBehaviour
{
	[Header("Indicators")]
    [SerializeField] private GameObject mouseIndicator;
	[SerializeField] private GameObject pointIndicator;
	[Header("Graph Manager")]
    [SerializeField] private GraphManager graphManager;

	private int numGridRecordRows = 9;
	private int numGridRecordCols = 9;
	private GameObject[,] gridRecord;

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
			
			// Remove duplicate point
			GameObject dupicatePoint = DuplicatePointOnColumn(gridPosition.x);
			if (dupicatePoint)
			{
				Destroy(dupicatePoint);
			}

			// Update point on line renderer on current graph
			graphManager.currentGraph.UpdateColumnPointOnGraph(gridPosition.x, gridPosition.z);
			
			GameObject newPoint = Instantiate(pointIndicator);
			newPoint.transform.position = graphManager.currentGraph.graphGrid.CellToWorld(gridPosition);
			gridRecord[gridPosition.z, gridPosition.x] = newPoint;
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
}