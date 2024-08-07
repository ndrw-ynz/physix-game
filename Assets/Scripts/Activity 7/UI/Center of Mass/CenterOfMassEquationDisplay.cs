using System;
using TMPro;
using UnityEngine;

public class CenterOfMassEquationDisplay : MonoBehaviour
{
    [Header("Input Fields")]
    [SerializeField] private TMP_InputField numeratorInputField;
    [SerializeField] private TMP_InputField denominatorInputField;
    [Header("Result Field")]
    [SerializeField] private TMP_InputField centerOfMassResultInputField;
	public float? centerOfMassValue { get; private set; }

	public void OnInputFieldChange()
	{
		string numerator = numeratorInputField.text;
		if (string.IsNullOrEmpty(numerator)) numerator = "0";
		string denominator = denominatorInputField.text;
		if (string.IsNullOrEmpty(denominator)) denominator = "0";

		bool canEvaluate = ExpressionEvaluator.Evaluate($"{numerator}/{denominator}", out float result);
		result = (float) Math.Round(result, 2);
		if (canEvaluate)
		{
			centerOfMassValue = result;
			centerOfMassResultInputField.text = $"{result}";
		}
		else
		{
			centerOfMassValue = null;
			centerOfMassResultInputField.text = "N/A";
		}
	}
}