using System;
using TMPro;
using UnityEngine;

public class TotalDepthFormulaDisplay : MonoBehaviour
{
	[Header("Input Fields")]
	[SerializeField] private TMP_InputField initialVelocityField;
	[SerializeField] private TMP_InputField timeFieldOne;
	[SerializeField] private TMP_InputField gravitationalConstantField;
	[SerializeField] private TMP_InputField timeFieldTwo;
	[SerializeField] private TMP_InputField denominatorField;
	[Header("Result Field")]
	[SerializeField] private TMP_InputField resultField;

	public float? resultValue { get; private set; }

	public void OnValueChange()
	{
		bool canEvaluate = ExpressionEvaluator.Evaluate($"{initialVelocityField.text} * {timeFieldOne.text} + ( ({gravitationalConstantField.text} * {timeFieldTwo.text}^2) / {denominatorField.text} )", out float result);
		result = (float) Math.Round(result, 4);
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
		initialVelocityField.text = "0";
		timeFieldOne.text = "0";
		gravitationalConstantField.text = "0";
		timeFieldTwo.text = "0";
		denominatorField.text = "0";

		// Clear result field
		resultField.text = "";

		// Set result value to null.
		resultValue = null;
	}
}
