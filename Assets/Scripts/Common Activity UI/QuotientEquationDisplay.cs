using System;
using TMPro;
using UnityEngine;

public class QuotientEquationDisplay : MonoBehaviour
{
	[Header("Number Fields")]
	[SerializeField] private TMP_InputField numeratorField;
	[SerializeField] private TMP_InputField denominatorField;
	[SerializeField] private TMP_InputField resultField;
	public float? quotientValue { get; private set; }

	public void OnValueChange()
	{
		string numerator = numeratorField.text;
		if (string.IsNullOrEmpty(numerator)) numerator = "0";
		string denominator = denominatorField.text;
		if (string.IsNullOrEmpty(denominator)) denominator = "0";

		bool canEvaluate = ExpressionEvaluator.Evaluate($"{numerator}/{denominator}", out float result);
		result = (float)Math.Round(result, 4);
		if (canEvaluate)
		{
			quotientValue = result;
			resultField.text = $"{result}";
		}
		else
		{
			quotientValue = null;
			resultField.text = "N/A";
		}
	}
}