using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorkGraphInterpretationView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;

	[Header("Graph Components")]
	[SerializeField] private GraphCoordinatePlotter graphCoordinatePlotter;
	[SerializeField] private LineRenderer graphLineRenderer;

	[Header("Calculation Displays")]
	[SerializeField] private GameObject constantWorkCalculationDisplay;
	[SerializeField] private GameObject linearWorkCalculationDisplay;

	[Header("Equation Displays")]
	[SerializeField] private ProductEquationDisplay constantWorkEquationDisplay;
	[SerializeField] private ProductEquationDisplay linearWorkEquationDisplay;

	public void SetupWorkGraphInterpretationView(ref Dictionary<ForceDisplacementCurveType, List<Vector2>> data)
    {
		List<ForceDisplacementCurveType> keys = data.Keys.ToList();

		// Randomly select one key
		int randomIndex = Random.Range(0, keys.Count);
		ForceDisplacementCurveType randomType = keys[randomIndex];

		// With data, setup line renderer and graph coordinate plotter
		List<Vector2> graphPoints = data[randomType];
		SetupGraphDisplay(graphPoints);

		// Remove the selected key-value pair
		data.Remove(randomType);

		// Clear fields
		ClearAllFields();

		// Update displayed equation
		UpdateDisplayedWorkGraphEquation(randomType);
    }

	private void SetupGraphDisplay(List<Vector2> graphPoints)
	{
		// Remove all points from graph
		graphCoordinatePlotter.RemoveAllPoints();

		// Add points to graph
		for (int i = 0; i < graphPoints.Count; i++)
		{
			graphCoordinatePlotter.PlacePoint(graphPoints[i]);

			// Update displayed line on graph line renderer
			graphLineRenderer.SetPosition(i, new Vector3(graphPoints[i].x, 0, graphPoints[i].y));
		}
	}

	private void UpdateDisplayedWorkGraphEquation(ForceDisplacementCurveType curveType)
	{
		switch (curveType)
		{
			case ForceDisplacementCurveType.ConstantForceGraph:
				constantWorkCalculationDisplay.gameObject.SetActive(true);
				linearWorkCalculationDisplay.gameObject.SetActive(false);
				break;
			case ForceDisplacementCurveType.LinearlyIncreasingForceGraph:
			case ForceDisplacementCurveType.LinearlyDecreasingForceGraph:
				constantWorkCalculationDisplay.gameObject.SetActive(false);
				linearWorkCalculationDisplay.gameObject.SetActive(true);
				break;
		}
	}

	private void ClearAllFields()
	{
		constantWorkEquationDisplay.ResetState();
		linearWorkEquationDisplay.ResetState();
	}
}