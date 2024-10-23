using System;
using TMPro;
using UnityEngine;

public class VectorMagnitudeFormulaDisplay : MonoBehaviour
{
	[Header("Input Fields")]
	[SerializeField] private TMP_InputField magnitudeInputFieldOne;
	[SerializeField] private TMP_InputField magnitudeInputFieldTwo;

    [Header("Result Field")]
	[SerializeField] private TMP_InputField magnitudeResultField;

	public float? resultValue { get; private set; }

	public void OnValueChange()
	{
		bool canEvaluate = ExpressionEvaluator.Evaluate($"sqrt(({magnitudeInputFieldOne.text})^2 + ({magnitudeInputFieldTwo.text})^2)", out float magnitudeResult);
		magnitudeResult = (float) Math.Round(magnitudeResult, 4);

		if (canEvaluate)
		{
			resultValue = magnitudeResult;
			magnitudeResultField.text = $"{magnitudeResult}";
		}
		else
		{
			resultValue = null;
			magnitudeResultField.text = "N/A";
		}
	}
}
