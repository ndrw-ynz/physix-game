using System;
using TMPro;
using UnityEngine;

public class VectorDirectionFormulaDisplay : MonoBehaviour
{
	[Header("Input Fields")]
	[SerializeField] private TMP_InputField directionInputNumerator;
	[SerializeField] private TMP_InputField directionInputDenominator;

    [Header("Result Field")]
	[SerializeField] private TMP_InputField directionResultField;

	public float? resultValue { get; private set; }

	public void OnValueChange()
	{
		bool isNumeratorEvaluated = ExpressionEvaluator.Evaluate($"{directionInputNumerator.text}", out double numeratorResult);
		bool isDenominatorEvaluated = ExpressionEvaluator.Evaluate($"{directionInputDenominator.text}", out double denominatorResult);

		if (isNumeratorEvaluated == true && isDenominatorEvaluated == true)
		{
			resultValue = (float) (Math.Atan2(numeratorResult, denominatorResult) * (180 / Math.PI));
			resultValue = (float) Math.Round((float) resultValue, 4);
			directionResultField.text = $"{resultValue}";
		}
		else
		{
			resultValue = null;
			directionResultField.text = "N/A";
		}
	}
}
