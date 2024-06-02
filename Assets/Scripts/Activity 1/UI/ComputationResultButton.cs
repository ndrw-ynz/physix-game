using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComputationResultButton : MonoBehaviour
{
	public static event Action ComputationResultButtonClickEvent;
	public GridLayoutGroup numberBankHolder;
	public Draggable draggablePrefab;
	private TextMeshProUGUI _placeholderText;
	private float _expressionValue;
	private bool _isExpressionValid;
	public void Initialize()
	{
		_placeholderText = GetComponentInChildren<TextMeshProUGUI>();
		ComputationDropHandler.UpdateHandlerText += OnTextUpdate;
		_isExpressionValid = false;
	}

	private void OnTextUpdate(string mathExp)
	{
		ExpressionEvaluator.Evaluate(mathExp, out float result);
		if (result == 0)
		{
			Debug.Log("Invalid expression! Cannot evaluate.");
			_expressionValue = 0;
			_placeholderText.text = "Invalid";
			_isExpressionValid = false;

		} else
		{
			Debug.Log("Result: " + result.ToString());
			_expressionValue = (float) Math.Round(result, 4);
			_placeholderText.text = _expressionValue.ToString();
			_isExpressionValid = true;
		}
	}

	public void OnClick()
	{
		if (_isExpressionValid)
		{
			Draggable newDraggable = Instantiate(draggablePrefab);
			newDraggable.transform.SetParent(numberBankHolder.transform, false);
			newDraggable.SetValue(_expressionValue);
			ComputationResultButtonClickEvent?.Invoke();
		}
	}
}
