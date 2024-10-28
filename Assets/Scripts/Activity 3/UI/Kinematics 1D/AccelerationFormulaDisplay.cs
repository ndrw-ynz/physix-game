using System;
using TMPro;
using UnityEngine;

public class AccelerationFormulaDisplay : MonoBehaviour
{
	[Header("Input Fields")]
	[SerializeField] private TMP_InputField finalVelocityField;
	[SerializeField] private TMP_InputField initialVelocityField;
	[SerializeField] private TMP_InputField timeField;

	[Header("Result Field")]
	[SerializeField] private TMP_InputField resultField;

	public float? resultValue { get; private set; }

	public void OnValueChange()
	{
		bool canEvaluate = ExpressionEvaluator.Evaluate($"({finalVelocityField.text} - {initialVelocityField.text}) / {timeField.text}", out float result);
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
}
