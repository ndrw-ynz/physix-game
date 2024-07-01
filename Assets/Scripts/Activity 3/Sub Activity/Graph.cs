using System.Collections;
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

	private void Start()
	{
		graphLineRenderer.startWidth = 0.2f;
		graphLineRenderer.endWidth = 0.2f;
		graphLineRenderer.positionCount = 9;
		SetupGraphLineRenderer(9);
	}

	private void SetupGraphLineRenderer(int numOfPoints)
	{
		for (int i = 0; i < numOfPoints; i++)
		{
			graphLineRenderer.SetPosition(i, new Vector3(i,0,i));
		}
	}

	public void UpdateColumnPointOnGraph(int columnValue, int rowValue)
	{
		graphLineRenderer.SetPosition(columnValue, new Vector3(columnValue, 0, rowValue));
	}
}