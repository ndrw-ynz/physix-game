using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OperatorButton : MonoBehaviour
{
	private TextMeshProUGUI _operatorText;
	private string _operator;

	public void Initialize()
	{
		_operator = transform.name;
		_operatorText = GetComponentInChildren<TextMeshProUGUI>();
		_operatorText.text = transform.name;
	}

}
