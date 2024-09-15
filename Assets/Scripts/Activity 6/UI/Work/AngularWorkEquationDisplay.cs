using System;
using TMPro;
using UnityEngine;

public class AngularWorkEquationDisplay : MonoBehaviour
{
	[Header("Input Fields")]
	[SerializeField] private TMP_InputField multiplicandInputField;
	[SerializeField] private TMP_InputField multiplierInputField;
	[SerializeField] private TMP_InputField angleCosInputField;

	[Header("Result Field")]
	[SerializeField] private TMP_InputField angularWorkResultInputField;
	public float? angularWorkValue { get; private set; }

	public void OnInputFieldChange()
	{
		string multiplier = multiplierInputField.text;
		if (string.IsNullOrEmpty(multiplier)) multiplier = "0";
		string multiplicand = multiplicandInputField.text;
		if (string.IsNullOrEmpty(multiplicand)) multiplicand = "0";
		string angleCos = angleCosInputField.text;
		if (string.IsNullOrEmpty(angleCos)) angleCos = "0";

		bool canEvaluate = ExpressionEvaluator.Evaluate($"{multiplier} * {multiplicand} * cos({angleCos}*(pi/180))", out float result);
		result = (float) Math.Round(result, 2);
		if (canEvaluate)
		{
			angularWorkValue = result;
			angularWorkResultInputField.text = $"{result}";
		}
		else
		{
			angularWorkValue = null;
			angularWorkResultInputField.text = "N/A";
		}
	}

	public void ResetState()
	{
		multiplicandInputField.text = "0";
		multiplierInputField.text = "0";
		angleCosInputField.text = "0";
		angularWorkValue = 0;
	}
}