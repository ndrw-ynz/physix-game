using UnityEngine;

public class GraphPlacementSystem : MonoBehaviour
{
	[Header("Indicators")]
    [SerializeField] private GameObject mouseIndicator;
	[SerializeField] private GameObject pointIndicator;
	[Header("Graph Manager")]
    [SerializeField] private GraphManager graphManager;

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
			Vector3 mousePosition = graphManager.GetSelectedMapPosition();
			Vector3Int gridPosition = graphManager.currentGraph.graphGrid.WorldToCell(mousePosition);
			Debug.Log(gridPosition);
			GameObject newPoint = Instantiate(pointIndicator);
			newPoint.transform.position = graphManager.currentGraph.graphGrid.CellToWorld(gridPosition);
		}
	}
}