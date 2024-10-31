using System;
using TMPro;
using UnityEngine;

public class CentripetalAccelerationFormulaDisplay : MonoBehaviour
{
	[Header("Input Fields")]
	[SerializeField] private TMP_InputField numeratorConstantField;
	[SerializeField] private TMP_InputField piField;
	[SerializeField] private TMP_InputField radiusField;
	[SerializeField] private TMP_InputField periodField;

	[Header("Result Field")]
	[SerializeField] private TMP_InputField resultField;

	public float? resultValue { get; private set; }

	public void OnValueChange()
	{
		bool canEvaluate = ExpressionEvaluator.Evaluate($"({numeratorConstantField.text} * ({piField.text})^2 * {radiusField.text}) / ({periodField.text}^2)", out float result);
		result = (float) Math.Round(result, 2);
		if (canEvaluate)
		{
			resultValue = result;
			resultField.text = $"{result}";
		}
		else
		{
			resultValue = null;
			resultField.text = "N/A";
		}
	}

	public void ResetState()
	{
		// Clear all number input fields.
		numeratorConstantField.text = "0";
		piField.text = "0";
		radiusField.text = "0";
		periodField.text = "0";

		// Clear result field
		resultField.text = "";

		// Set result value to null.
		resultValue = null;
	}
}
