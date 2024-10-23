using System;
using TMPro;
using UnityEngine;

public class VectorComponentFormulaDisplay : MonoBehaviour
{
	[Header("Number Input Fields")]
	[SerializeField] private TMP_InputField magnitudeInputField;
	[SerializeField] private TMP_InputField angleInputField;
    [Header("Trigonometric Function Dropdown")]
    [SerializeField] private TMP_Dropdown trigonometricFunctionDropdown;
	[Header("Result Field")]
	[SerializeField] private TMP_InputField resultField;

	public float? resultValue { get; private set; }

	public void OnValueChange()
	{
		// Get the trigonometric function from the dropdown
		int selectedIndex = trigonometricFunctionDropdown.value;
		string selectedTrigonometricFunc = trigonometricFunctionDropdown.options[selectedIndex].text;

		// Evaluate formula based from input
		// Formulas:
		// Formula for x-component - magnitude * cos(angle)
		// Formula for y-component - magnitude * sin(angle)
		bool canEvaluate = ExpressionEvaluator.Evaluate($"{magnitudeInputField.text} * {selectedTrigonometricFunc}({angleInputField.text}*(pi/180))", out float componentValue);
		componentValue = (float) Math.Round(componentValue, 4);
		if (canEvaluate)
		{
			resultValue = componentValue;
			resultField.text = $"{componentValue}";
		}
		else
		{
			resultValue = null;
			resultField.text = "N/A";
		}
	}

	public void ResetState()
	{
		// Clear all number input fields.
		magnitudeInputField.text = "0";
		angleInputField.text = "0";

		// Reset dropdown choice
		trigonometricFunctionDropdown.value = 0;
		trigonometricFunctionDropdown.RefreshShownValue();

		// Clear result field
		resultField.text = "";

		// Set result value to null.
		resultValue = null;
	}
}
