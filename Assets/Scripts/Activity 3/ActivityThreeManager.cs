using UnityEngine;

public class ActivityThreeManager : MonoBehaviour
{
	[Header("Managers")]
	[SerializeField] private GraphManager graphManager;

    [Header("Cameras")]
	[SerializeField] private Camera mainCamera;
	[SerializeField] private Camera positionVsTimeGraphCamera;
	[SerializeField] private Camera velocityVsTimeGraphCamera;
	[SerializeField] private Camera accelerationVsTimeGraphCamera;

	[Header("Views")]
    [SerializeField] private ViewGraphs viewGraphs;
    [SerializeField] private ViewGraphEdit viewGraphEdit;
    
	private void Start()
	{
        GraphEditButton.InitiateGraphEditViewSwitch += ChangeViewToGraphEditView;
		ViewGraphEdit.InitiateGraphViewSwitch += ChangeViewToGraphView;
	}

    private void ChangeViewToGraphEditView(Graph graph)
    {
		mainCamera.enabled = false;
		viewGraphEdit.gameObject.SetActive(true);
		viewGraphEdit.interactiveGraphCamera = graph.interactiveGraphCamera;
		viewGraphs.gameObject.SetActive(false);
		graphManager.currentGraph = graph;
    }

	private void ChangeViewToGraphView()
	{
		mainCamera.enabled = true;
		viewGraphEdit.gameObject.SetActive(false);
		viewGraphEdit.interactiveGraphCamera = null;
		viewGraphs.gameObject.SetActive(true);
		graphManager.currentGraph = null;
	}
}