using UnityEngine;

public class GraphPlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject mouseIndicator;
	[SerializeField] private GameObject pointIndicator;
    [SerializeField] private GraphManager graphManager;
	[SerializeField] private Grid grid;

	private void Start()
	{
		graphManager.OnMouseClick += PlacePoint;
	}

	private void Update()
	{
		if (graphManager.interactiveGraphCamera)
		{
			Vector3 mousePosition = graphManager.GetSelectedMapPosition();
			Vector3Int gridPosition = grid.WorldToCell(mousePosition);
			mouseIndicator.transform.position = mousePosition;
			pointIndicator.transform.position = grid.CellToWorld(gridPosition);
		} else
		{
			mouseIndicator.transform.position = new Vector3(0, -1, 0);
			pointIndicator.transform.position = new Vector3(0, -1, 0);
		}
	}

	private void PlacePoint()
	{
		if (graphManager.interactiveGraphCamera)
		{
			Vector3 mousePosition = graphManager.GetSelectedMapPosition();
			Vector3Int gridPosition = grid.WorldToCell(mousePosition);
			GameObject newPoint = Instantiate(pointIndicator);
			newPoint.transform.position = grid.CellToWorld(gridPosition);
		}
	}
}