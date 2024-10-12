using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContainerVarianceEquationDisplay : MonoBehaviour
{
	[Header("Numerator Equation Container")]
	[SerializeField] private HorizontalLayoutGroup numeratorEquationContainer;

	[Header("Denominator Field")]
	[SerializeField] private TMP_InputField denominatorField;

	[Header("Result Field")]
	[SerializeField] private TMP_InputField resultField;

	[Header("Prefabs")]
	[SerializeField] private TMP_InputField numberInputFieldPrefab;
	[SerializeField] private TextMeshProUGUI plusSignTextPrefab;

	public float? resultValue { get; private set; }

	public void SetupEquationDisplay(int addendsCount)
	{
		for (int i = 0; i < addendsCount; i++)
		{
			TMP_InputField numberInputField = Instantiate(numberInputFieldPrefab, numeratorEquationContainer.transform, false);
			numberInputField.onValueChanged.AddListener((_) => UpdateEquationResultField());

			if (i + 1 < addendsCount)
			{
				TextMeshProUGUI plusSignText = Instantiate(plusSignTextPrefab, numeratorEquationContainer.transform, false);
			}
		}
	}

	public void UpdateEquationResultField()
	{
		float numeratorSum = 0;
		TMP_InputField[] numberInputFields = numeratorEquationContainer.GetComponentsInChildren<TMP_InputField>();
		foreach (TMP_InputField numberInputField in numberInputFields)
		{
			numeratorSum += float.Parse(numberInputField.text);
		}

		bool canEvaluate = ExpressionEvaluator.Evaluate($"{numeratorSum} / {denominatorField.text}", out float result);
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
