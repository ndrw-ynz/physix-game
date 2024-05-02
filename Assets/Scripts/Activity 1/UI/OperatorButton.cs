using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OperatorButton : MonoBehaviour
{
	public static event Action<string> OperatorButtonEvent;
	private TextMeshProUGUI _operatorText;
	private string _operator;

	public void Initialize()
	{
		_operator = transform.name;
		_operatorText = GetComponentInChildren<TextMeshProUGUI>();
		_operatorText.text = transform.name;
	}

	public void OnClick()
	{
		OperatorButtonEvent?.Invoke(_operator);
	}
}
