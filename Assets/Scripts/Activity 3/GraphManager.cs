using System;
using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public event Action OnMouseClick;

    [Header("Graph")]
    public Graph positionVsTimeGraph;
    public Graph velocityVsTimeGraph;
    public Graph accelerationVsTimeGraph;
    public Graph currentGraph;
    [Header("Layer Mask")]
    [SerializeField] private LayerMask placementLayerMask;

    private Vector3 lastMousePosition;

    public void SetupGraphs(List<int> positionValues, List<int> velocityValues, List<int> accelerationValues)
    {
        positionVsTimeGraph.InitializeGraph(positionValues);
		velocityVsTimeGraph.InitializeGraph(velocityValues);
		accelerationVsTimeGraph.InitializeGraph(accelerationValues);
    }

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
        {
            OnMouseClick?.Invoke();
        }
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