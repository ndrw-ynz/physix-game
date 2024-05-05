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
		_placeholderText.text = dragObject.GetValue().ToString();
	}

	private void InsertOperand(int operand)
	{
		_placeholderText.text += operand.ToString();
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
					// bool isEmpty = false;
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

							// Ensure non-empty parentheses upon closing...
							/*int index = _placeholderText.text.LastIndexOf('(', _placeholderText.text.IndexOf(c));
							Debug.Log(index);
							if (index >= 0)
							{
								string substr = _placeholderText.text.Substring(index + 1, _placeholderText.text.IndexOf(c) - index - 1);
								bool hasDigit = false;
								Debug.Log("substring : " + substr);
								foreach (char chr in substr)
								{
									if (char.IsDigit(chr))
									{
										hasDigit = true;
										Debug.Log("has digit, therefore aint empty");
										break;
									}
								}
								if (!hasDigit)
								{
									Debug.Log("this EMPTY AFFFF!!!!");
									isEmpty = true;
								}
							}*/
						}
					}

					// Previous char must not be an operator +-/
					if (lastChar == '+' || lastChar == '-' || lastChar == '/')
					{
						return;
					} 
					// Number of open parentheses must be > 0 to add
					else if (openParenthesesCount > 0) // && !isEmpty)
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
	}
}

