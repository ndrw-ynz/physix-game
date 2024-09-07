using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProductEquationDisplay : MonoBehaviour
{
	[Header("Number Fields")]
	[SerializeField] private List<TMP_InputField> numberInputFields;
	[SerializeField] private TMP_InputField resultField;
	public float? productValue { get; private set; }

	public void OnValueChange()
	{
		string productEquation = "1";
		for (int i = 0; i < numberInputFields.Count; i++)
		{
			string inputText = numberInputFields[i].text;
			if (string.IsNullOrEmpty(inputText)) { 
				productEquation += "*0"; 
			} else
			{
				productEquation += $"*{inputText}";
			}
		}

		bool canEvaluate = ExpressionEvaluator.Evaluate(productEquation, out float result);
		if (canEvaluate)
		{
			productValue = (float) Math.Round(result,4);
			resultField.text = $"{result}";
		}
		else
		{
			productValue = null;
			resultField.text = "N/A";
		}
	}
}