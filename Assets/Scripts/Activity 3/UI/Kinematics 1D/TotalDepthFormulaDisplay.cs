using System;
using System.Collections;
using System.Collections.Generic;
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
}
