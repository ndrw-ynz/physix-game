using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableComputationResult : Draggable
{
	public static event Action<float> EvaluateMathExpression;
	private float _expressionValue;
	private void Start()
	{
		DropHandler.UpdateHandlerText += OnTextUpdate;
		Initialize();
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
