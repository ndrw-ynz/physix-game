using System;
using System.Collections.Generic;
using UnityEngine;

public class WorkGraphInterpretationView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;
	public event Action<float?> SubmitAnswerEvent;

	[Header("Graph Components")]
	[SerializeField] private GraphCoordinatePlotter graphCoordinatePlotter;
	[SerializeField] private LineRenderer graphLineRenderer;

	[Header("Calculation Displays")]
	[SerializeField] private GameObject constantWorkCalculationDisplay;
	[SerializeField] private GameObject linearWorkCalculationDisplay;

	[Header("Equation Displays")]
	[SerializeField] private ProductEquationDisplay constantWorkEquationDisplay;
	[SerializeField] private ProductEquationDisplay linearWorkEquationDisplay;

	private void OnEnable()
	{
		OpenViewEvent?.Invoke();
	}

	public void SetupWorkGraphInterpretationView(Dictionary<ForceDisplacementCurveType, List<Vector2>> data, ForceDisplacementCurveType graphType)
    {
		// With data, setup line renderer and graph coordinate plotter
		SetupGraphDisplay(data[graphType]);

		// Clear fields
		ClearAllFields();

		// Update displayed equation
		UpdateDisplayedWorkGraphEquation(graphType);
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

	public void OnSubmitButtonClick()
	{
		float? submission = constantWorkCalculationDisplay.activeSelf ? constantWorkEquationDisplay.productValue : linearWorkEquationDisplay.productValue;

		SubmitAnswerEvent?.Invoke(submission);
	}

	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitViewEvent?.Invoke();
	}
}