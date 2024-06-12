using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VectorLineRenderer : MonoBehaviour
{
    private LineRenderer _lineRenderer;
	private Vector3 _targetPoint;

	private void Start()
	{
		_lineRenderer = GetComponent<LineRenderer>();
	}

	public void SetupVector(Vector3 point)
	{
		_lineRenderer.positionCount = 2;
		_targetPoint = point;
	}

	private void Update()
	{
		_lineRenderer.SetPosition(0, Vector3.zero);
		_lineRenderer.SetPosition(1, _targetPoint);
	}
}
