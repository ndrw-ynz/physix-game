using System;
using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public event Action OnMouseClick;

    public bool canEditGraph;

    [Header("Graph")]
    public Graph positionVsTimeGraph;
    public Graph velocityVsTimeGraph;
    public Graph accelerationVsTimeGraph;
    public Graph currentGraph;
    [Header("Views")]
    [SerializeField] private GraphEditorUI graphEditorUI;
    [SerializeField] private GraphViewerUI graphViewerUI;
	[Header("Layer Mask")]
    [SerializeField] private LayerMask placementLayerMask;

    private Vector3 lastMousePosition;

	private void Start()
	{
        graphEditorUI.QuitGraphEditorEvent += ClearGraph;
        graphViewerUI.QuitGraphViewerEvent += ClearGraph;
	}

	public void SetupGraphs(List<int> positionValues)
    {
        positionVsTimeGraph.InitializeGraph(positionValues);
		velocityVsTimeGraph.InitializeGraph(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 });
		accelerationVsTimeGraph.InitializeGraph(new List<int> { 0, 0, 0, 0, 0, 0, 0 });
    }

    public void UpdateGraphs(List<int> positionValues)
    {
        positionVsTimeGraph.UpdateGraphPoints(positionValues);
        velocityVsTimeGraph.UpdateGraphPoints(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 });
        accelerationVsTimeGraph.UpdateGraphPoints(new List<int> { 0, 0, 0, 0, 0, 0, 0 });
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
        {
            OnMouseClick?.Invoke();
        }
	}

    public void DisplayGraph(Graph graph)
    {
        // Disable all graph cameras
		positionVsTimeGraph.interactiveGraphCamera.enabled = false;
		velocityVsTimeGraph.interactiveGraphCamera.enabled = false;
		accelerationVsTimeGraph.interactiveGraphCamera.enabled = false;

        graph.interactiveGraphCamera.enabled = true;
        currentGraph = graph;
    }

    public void ClearGraph()
    {
        currentGraph.interactiveGraphCamera.enabled = false;
        currentGraph = null;
        canEditGraph = false;
    }

	public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = currentGraph.interactiveGraphCamera.nearClipPlane;
        RaycastHit hit;
        Ray ray = currentGraph.interactiveGraphCamera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out hit, 100, placementLayerMask))
        {
            lastMousePosition = hit.point;
        }
        return lastMousePosition;
    }
}