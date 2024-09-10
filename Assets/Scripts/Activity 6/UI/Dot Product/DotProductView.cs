using System;
using TMPro;
using UnityEngine;

public class DotProductView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;

	[Header("Vector Given Fields")]
	[SerializeField] private TMP_InputField satelliteDishVector;
	[SerializeField] private TMP_InputField targetObjectVector;

	[Header("Dot Product Equation Displays")]
	[SerializeField] private ProductEquationDisplay xCoordScalarProductDisplay;
	[SerializeField] private ProductEquationDisplay yCoordScalarProductDisplay;
	[SerializeField] private ProductEquationDisplay zCoordScalarProductDisplay;
	[SerializeField] private SumEquationDisplay dotProductDisplay;

	private void OnEnable()
	{
		OpenViewEvent?.Invoke();
	}

	public void SetupDotProductView(DotProductData data)
	{
		ClearAllFields();
		satelliteDishVector.text = $"({data.satelliteDishVector.x}, {data.satelliteDishVector.y}, {data.satelliteDishVector.z})";
		targetObjectVector.text = $"({data.targetObjectVector.x}, {data.targetObjectVector.y}, {data.targetObjectVector.z})";
	}

	private void ClearAllFields()
	{
		xCoordScalarProductDisplay.ResetState();
		yCoordScalarProductDisplay.ResetState();
		zCoordScalarProductDisplay.ResetState();
		dotProductDisplay.ResetState();
	}

	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitViewEvent?.Invoke();
	}
}