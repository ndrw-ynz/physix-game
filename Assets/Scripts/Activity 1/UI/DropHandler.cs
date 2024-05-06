using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropHandler : MonoBehaviour, IDropHandler
{
	public static event Action<string> UpdateHandlerText;
	private TextMeshProUGUI _placeholderText;
	public void Start()
	{
		_placeholderText = GetComponentInChildren<TextMeshProUGUI>();
		_placeholderText.text = "";

		OperandButton.OperandButtonEvent += InsertOperand;
		OperatorButton.OperatorButtonEvent += InsertOperator;
	}

	public void OnDrop(PointerEventData eventData)
	{
		Draggable dragObject = eventData.pointerDrag.GetComponent<Draggable>();
		InsertOperand(dragObject.GetValue());
	}

	private void InsertOperand(float operand)
	{
		_placeholderText.text += operand.ToString();
		// Invoke action of event upon attempt of appending an operand.
		UpdateHandlerText?.Invoke(_placeholderText.text);
	}

	private void InsertOperator(string op)
	{
		// Check if the text is not empty
		if (!string.IsNullOrEmpty(_placeholderText.text))
		{
			char lastChar = _placeholderText.text[_placeholderText.text.Length - 1];
			switch (op)
			{
				// Checking for opening parenthesis insertion '('
				case "(":
					_placeholderText.text += op;
					break;

				// Checking for closing parentheses insertion ')'
				case ")":
					// Need to keep track of ( parentheses count
					int openParenthesesCount = 0;
					foreach (char c in _placeholderText.text)
					{
						if (c == '(')
						{
							openParenthesesCount++;
						}
						else if (c == ')')
						{
							if (openParenthesesCount == 0)
							{
								return;
							}
							openParenthesesCount--;
						}
					}

					// Previous char must not be an operator +-/
					if (lastChar == '+' || lastChar == '-' || lastChar == '/')
					{
						return;
					} 
					// Number of open parentheses must be > 0 to add
					else if (openParenthesesCount > 0)
					{
						_placeholderText.text += op;
					}
					break;

				// Checking for operators '+', '-', and '/'
				case "+": case "-": case "/":
					// If previous char is an operator, replace with current
					if (lastChar == '+' || lastChar == '-' || lastChar == '/')
					{
						_placeholderText.text = _placeholderText.text.Remove(_placeholderText.text.Length - 1) + op;
					}
					// If previous char is a digit or a parentheses, append.
					if (char.IsDigit(lastChar) || lastChar == ')')
					{
						_placeholderText.text += op;
					}
					break;

				case "x^2":
					if (char.IsDigit(lastChar) || lastChar == ')')
					{
						_placeholderText.text += "^2";
					}
					break;
			}		
		}
		// If the string is empty, the only possible symbol is (
		else
		{
			if (op == "(")
			{
				_placeholderText.text += op;
			}
		}
		// Invoke action of event upon attempt of appending operator.
		UpdateHandlerText?.Invoke(_placeholderText.text);
	}

	public void ClearEntry()
	{
		if (_placeholderText.text != "") _placeholderText.text = _placeholderText.text.Substring(0, _placeholderText.text.Length - 1);
		UpdateHandlerText?.Invoke(_placeholderText.text);
	}
}

