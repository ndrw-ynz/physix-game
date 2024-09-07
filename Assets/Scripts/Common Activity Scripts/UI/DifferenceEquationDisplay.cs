using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DifferenceEquationDisplay : MonoBehaviour
{
	[Header("Number Fields")]
	[SerializeField] private List<TMP_InputField> numberInputFields;
	[SerializeField] private TMP_InputField resultField;
	public float? differenceValue { get; private set; }

	public void OnValueChange()
	{
		string differenceEquation = "";
		for (int i = 0; i < numberInputFields.Count; i++)
		{
			if (i != 0)
			{
				differenceEquation += "-";
			}

			string inputText = numberInputFields[i].text;
			if (string.IsNullOrEmpty(inputText))
			{
				differenceEquation += "0";
			}
			else
			{
				differenceEquation += $"{inputText}";
			}
		}

		bool canEvaluate = ExpressionEvaluator.Evaluate(differenceEquation, out float result);
		if (canEvaluate)
		{
			differenceValue = (float)Math.Round(result, 4);
			resultField.text = $"{result}";
		}
		else
		{
			differenceValue = null;
			resultField.text = "N/A";
		}
	}
}