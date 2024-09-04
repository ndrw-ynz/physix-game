using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SumEquationDisplay : MonoBehaviour
{
	[Header("Number Fields")]
	[SerializeField] private List<TMP_InputField> numberInputFields;
	[SerializeField] private TMP_InputField resultField;
	public float? sumValue { get; private set; }

	public void OnValueChange()
	{
		string sumEquation = "0";
		for (int i = 0; i < numberInputFields.Count; i++)
		{
			string inputText = numberInputFields[i].text;
			if (string.IsNullOrEmpty(inputText))
			{
				sumEquation += "+0";
			}
			else
			{
				sumEquation += $"+{inputText}";
			}
		}

		bool canEvaluate = ExpressionEvaluator.Evaluate(sumEquation, out float result);
		if (canEvaluate)
		{
			sumValue = (float) Math.Round(result, 4);
			resultField.text = $"{result}";
		}
		else
		{
			sumValue = null;
			resultField.text = "N/A";
		}
	}
}