using System;
using TMPro;
using UnityEngine;

public class CartesianComponentsView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;

	[Header("Display Text")]
	[SerializeField] private TextMeshProUGUI numberOfVectorsText;

	[Header("Line Renderer")]
	[SerializeField] private LineRenderer lineRenderer;

	[Header("Given Fields")]
	[SerializeField] private TMP_InputField givenMagnitude;
	[SerializeField] private TMP_InputField givenAngleMeasure;

	[Header("Vector Component Formula DIsplays")]
	[SerializeField] private VectorComponentFormulaDisplay xComponentFormulaDisplay;
	[SerializeField] private VectorComponentFormulaDisplay yComponentFormulaDisplay;

	private void OnEnable()
	{
		OpenViewEvent?.Invoke();
	}

	public void UpdateNumberOfVectorsTextDisplay(int numOfVectors, int totalVectors)
	{
		numberOfVectorsText.text = $"Extracted Vector Components: {numOfVectors} / {totalVectors}";
	}

	public void UpdateCartesianComponentsView(VectorData vectorData)
	{
		// Update given fields
		givenMagnitude.text = $"{vectorData.magnitude} m";
		givenAngleMeasure.text = $"{vectorData.angleMeasure} °";

		// Update line renderer
		UpdateLineRenderer(vectorData.magnitude, vectorData.angleMeasure);

		xComponentFormulaDisplay.ResetState();
		yComponentFormulaDisplay.ResetState();
	}

	private void UpdateLineRenderer(int magnitudeValue, int directionValue)
	{
		float directionRadians = directionValue * Mathf.Deg2Rad;

		float x = magnitudeValue * Mathf.Cos(directionRadians);
		float z = magnitudeValue * Mathf.Sin(directionRadians);
		Vector3 targetPoint = new Vector3(x, 0, z);

		lineRenderer.positionCount = 2;
		lineRenderer.SetPosition(0, Vector3.zero);
		lineRenderer.SetPosition(1, targetPoint);
	}

	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitViewEvent?.Invoke();
	}
}
