using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropHandler : MonoBehaviour, IDropHandler
{
	private TextMeshProUGUI _placeholderText;
	// todo: to make it easier, make use of bools for checking if operator or operand to avoid regex use
	private bool _isPrevOperator;
	private bool _isParentheses;

	public void Start()
	{
		_placeholderText = GetComponentInChildren<TextMeshProUGUI>();
		_placeholderText.text = "";

		OperandButton.OperandButtonEvent += InsertOperand;
		OperatorButton.OperatorButtonEvent += InsertOperator;
		_isPrevOperator = true;
	}

	public void OnDrop(PointerEventData eventData)
	{
		Draggable dragObject = eventData.pointerDrag.GetComponent<Draggable>();
		_placeholderText.text = dragObject.GetValue().ToString();
	}

	private void InsertOperand(int operand)
	{
		_placeholderText.text += operand.ToString();
		_isPrevOperator = false;
	}

	private void InsertOperator(string op)
	{
		if (!string.IsNullOrEmpty(_placeholderText.text))
		{
			char lastChar = _placeholderText.text[_placeholderText.text.Length - 1];

			if (char.IsDigit(lastChar) || lastChar == ')')
			{
				// Append the operator directly
				_placeholderText.text += op;
			}
			else if (lastChar == '(' || "+-*/".Contains(lastChar))
			{
				if (lastChar == '(')
				{
					if (op != "-")
					{
						_placeholderText.text += op;
					}
				}
				else
				{
					_placeholderText.text = _placeholderText.text.Remove(_placeholderText.text.Length - 1) + op;
				}
			}
		}
	}



}

