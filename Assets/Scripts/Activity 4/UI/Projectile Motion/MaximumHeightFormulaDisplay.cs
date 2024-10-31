using System;
using TMPro;
using UnityEngine;

public class MaximumHeightFormulaDisplay : MonoBehaviour
{
	[Header("Input Fields")]
	[SerializeField] private TMP_InputField initialVelocityField;
	[SerializeField] private TMP_InputField angleMeasureField;
	[SerializeField] private TMP_InputField denominatorConstantField;
	[SerializeField] private TMP_InputField gravitationalConstantField;
	[SerializeField] private TMP_InputField initialHeightField;

	[Header("Result Field")]
	[SerializeField] private TMP_InputField resultField;

	public float? resultValue { get; private set; }

	public void OnValueChange()
	{
		bool canEvaluate = ExpressionEvaluator.Evaluate($"(- ({initialVelocityField.text}^2) * (sin({angleMeasureField.text}*(pi/180)))^2) / ({denominatorConstantField.text} * {gravitationalConstantField.text})) + {initialHeightField.text}", out float result);
		result = (float)Math.Round(result, 2);
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
		denominatorConstantField.text = "0";
		initialVelocityField.text = "0";
		angleMeasureField.text = "0";
		gravitationalConstantField.text = "0";
		initialHeightField.text = "0";

		// Clear result field
		resultField.text = "";

		// Set result value to null.
		resultValue = null;
	}
}
