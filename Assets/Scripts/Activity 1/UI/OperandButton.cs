using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OperandButton : MonoBehaviour
{
	private TextMeshProUGUI _operandText;
    private int _operand;

	public void Initialize()
    {
        _operand = int.Parse(transform.name);
        _operandText = GetComponentInChildren<TextMeshProUGUI>();
        _operandText.text = transform.name;
    }

}
