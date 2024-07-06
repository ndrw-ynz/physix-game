using TMPro;
using UnityEngine;

public class CalcInputField : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

	private void Start()
	{
		CalcDigitButton.DigitInsertEvent += InsertDigit;
		CalcOperatorButton.OperatorInsertEvent += InsertOperator;
		CalcClearEntryButton.ClearResultFieldEvent += ClearResultField;
		CalcCalculateButton.CalculateResultEvent += ClearAllResultField;
	}

	private void InsertDigit(int digit)
	{
		inputField.text += digit.ToString();
	}

	private void InsertOperator(string op)
	{
		if (!string.IsNullOrEmpty(inputField.text))
		{
			char lastChar = inputField.text[inputField.text.Length - 1];
			switch (op)
			{
				// Checking for opening parenthesis insertion '('
				case "(":
					inputField.text += op;
					break;

				// Checking for closing parentheses insertion ')'
				case ")":
					// Need to keep track of ( parentheses count
					int openParenthesesCount = 0;
					foreach (char c in inputField.text)
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
						inputField.text += op;
					}
					break;

				// Checking for operators '+', '-', and '/'
				case "+":
				case "-":
				case "/":
				case "x":
					// If previous char is an operator, replace with current
					if (lastChar == '+' || lastChar == '-' || lastChar == '/' || lastChar == 'x')
					{
						inputField.text = inputField.text.Remove(inputField.text.Length - 1) + op;
					}
					// If previous char is a digit or a parentheses, append.
					if (char.IsDigit(lastChar) || lastChar == ')')
					{
						inputField.text += op;
					}
					break;

				case "^2":
					if (char.IsDigit(lastChar) || lastChar == ')')
					{
						inputField.text += "^2";
					}
					break;

				case ".":
					// Check if the current number segment already contains a decimal point
					int lastOperatorIndex = inputField.text.LastIndexOfAny(new char[] { '+', '-', '/', '*', '(', ')' });
					string currentNumberSegment = lastOperatorIndex == -1 ? inputField.text : inputField.text.Substring(lastOperatorIndex + 1);

					if (!currentNumberSegment.Contains("."))
					{
						if (string.IsNullOrEmpty(inputField.text) || lastChar == '+' || lastChar == '-' || lastChar == '/' || lastChar == '*' || lastChar == '(')
						{
							inputField.text += "0" + op;
						}
						else if (char.IsDigit(lastChar))
						{
							inputField.text += op;
						}
					}
					break;
			}
		}
		// If the string is empty, the only possible symbol is (
		else
		{
			if (op == "(")
			{
				inputField.text += op;
			}
		}
	}

	private void ClearResultField()
	{
		if (inputField.text != "")
		{
			inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
		}
	}

	private void ClearAllResultField()
	{
		inputField.text = "";
	}

}