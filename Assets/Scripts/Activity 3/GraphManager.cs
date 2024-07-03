using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GraphsSubActivitySO;
using Random = UnityEngine.Random;

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

    public void SetupGraphs(GraphsSubActivitySO graphsSO)
    {
        foreach (GraphDataset graphDataset in graphsSO.datasets)
        {
            string rawStringDataValues = graphDataset.dataset[Random.Range(0, graphDataset.dataset.Count)];
			List<int> graphPointValues = rawStringDataValues.Split(',').Select(int.Parse).ToList();
			switch (graphDataset.datasetType)
            {
                case DatasetType.Position:
                    positionVsTimeGraph.InitializeGraph(graphPointValues);
					break;
                case DatasetType.Velocity:
					velocityVsTimeGraph.InitializeGraph(graphPointValues);
					break;
                case DatasetType.Acceleration:
					accelerationVsTimeGraph.InitializeGraph(graphPointValues);
					break;
            }
        }
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