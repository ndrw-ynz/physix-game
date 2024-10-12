using System;
using TMPro;
using UnityEngine;

public class ContainerSquaredDeviationEquationDisplay : MonoBehaviour
{
	[Header("Header Text")]
	[SerializeField] private TextMeshProUGUI headerText;

	[Header("Input Fields")]
	[SerializeField] private TMP_InputField dataValueField;
	[SerializeField] private TMP_InputField meanValueField;

	[Header("Result Field")]
	[SerializeField] private TMP_InputField resultField;

	public float? resultValue { get; private set; }

	public void SetupHeaderText(string text)
	{
		headerText.text = text;
	}

	public void OnValueChange()
	{
		bool canEvaluate = ExpressionEvaluator.Evaluate($"({dataValueField.text} - {meanValueField.text}) ^ 2", out float result);
		result = (float) Math.Round(result, 4);
		if (canEvaluate)
		{
			resultValue = result;
			resultField.text = $"{result}";
		}
		else
		{
			resultValue = null;
			resultField.text = "N/A";
		}
	}
}
