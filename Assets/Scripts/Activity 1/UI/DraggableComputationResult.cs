using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableComputationResult : Draggable
{
	public static event Action<float> EvaluateMathExpression;
	private void Start()
	{
		DropHandler.UpdateHandlerText += OnTextUpdate;
	}

	private void OnTextUpdate(string mathExp)
	{
		ExpressionEvaluator.Evaluate(mathExp, out float result);
		if (result == 0)
		{
			Debug.Log("Invalid expression! Cannot evaluate.");

		} else
		{
			Debug.Log("parsing result: " + result.ToString());
		}
	}
}
