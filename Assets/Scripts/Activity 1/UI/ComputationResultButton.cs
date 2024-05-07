using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComputationResultButton : MonoBehaviour
{
	private TextMeshProUGUI _placeholderText;
	private float _expressionValue;
	public void Initialize()
	{
		_placeholderText = GetComponentInChildren<TextMeshProUGUI>();
		DropHandler.UpdateHandlerText += OnTextUpdate;
	}

	private void OnTextUpdate(string mathExp)
	{
		ExpressionEvaluator.Evaluate(mathExp, out float result);
		if (result == 0)
		{
			Debug.Log("Invalid expression! Cannot evaluate.");
			_expressionValue = 0;
			_placeholderText.text = "Invalid";

		} else
		{
			Debug.Log("Result: " + result.ToString());
			_expressionValue = (float) Math.Round(result, 4);
			_placeholderText.text = _expressionValue.ToString();
		}
	}
}
