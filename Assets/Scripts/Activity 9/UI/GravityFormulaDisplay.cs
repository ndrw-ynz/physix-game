using System;
using TMPro;
using UnityEngine;

public enum GravityFormulaType
{
    GravitationalForce,
    GravitationalPotentialEnergy
}

public class GravityFormulaDisplay : MonoBehaviour
{
	[Header("Input Fields")]
	[Header("Gravitation Constant Input Fields")]
	[SerializeField] private TMP_InputField gravitationConstantCoefficientInputField;
	[SerializeField] private TMP_InputField gravitationConstantExponentInputField;
	[Header("Mass One Input Fields")]
	[SerializeField] private TMP_InputField massOneCoefficientInputField;
	[SerializeField] private TMP_InputField massOneExponentInputField;
	[Header("Mass Two Input Fields")]
	[SerializeField] private TMP_InputField massTwoCoefficientInputField;
	[SerializeField] private TMP_InputField massTwoExponentInputField;
	[Header("Distance Input Fields")]
	[SerializeField] private TMP_InputField distanceCoefficientnputField;
	[SerializeField] private TMP_InputField distanceExponentInputField;
	[Header("Result Field")]
	[SerializeField] private TMP_InputField resultField;
	[Header("Gravity Formula Type")]
	public GravityFormulaType gravityFormulaType;

	public double? resultValue { get; private set; }

	public void OnValueChange()
	{
		// Check for empty or null input
		if (
			!float.TryParse(gravitationConstantCoefficientInputField.text, out float gravitationConstantCoefficient) ||
			!int.TryParse(gravitationConstantExponentInputField.text, out int gravitationConstantExponent) ||
			!float.TryParse(massOneCoefficientInputField.text, out float massOneCoefficient) ||
			!int.TryParse(massOneExponentInputField.text, out int massOneExponent) ||
			!float.TryParse(massTwoCoefficientInputField.text, out float massTwoCoefficient) ||
			!int.TryParse(massTwoExponentInputField.text, out int massTwoExponent) ||
			!float.TryParse(distanceCoefficientnputField.text, out float distanceCoefficient) ||
			!int.TryParse(distanceExponentInputField.text, out int distanceExponent)
			)
		{
			resultField.text = "N/A";
			resultValue = null;
			return;
		}

		// Compute value from scientific notation
		double gravitationConstantValue = ActivityNineUtilities.EvaluateScientificNotation(gravitationConstantCoefficient, gravitationConstantExponent);
		double massOneValue = ActivityNineUtilities.EvaluateScientificNotation(massOneCoefficient, massOneExponent);
		double massTwoValue = ActivityNineUtilities.EvaluateScientificNotation(massTwoCoefficient, massTwoExponent);
		double distanceValue = ActivityNineUtilities.EvaluateScientificNotation(distanceCoefficient, distanceExponent);

		// Calculate result based on formula type.
		double result = 0;
		switch (gravityFormulaType)
		{
			case GravityFormulaType.GravitationalForce:
				// Formula: gravitationConstant * ( (massOne * massTwo) / distance ^ 2 )
				result = gravitationConstantValue * massOneValue * massTwoValue / Math.Pow(distanceValue, 2);
				break;
			case GravityFormulaType.GravitationalPotentialEnergy:
				// Formula: - gravitationConstant * ( (massOne * massTwo) / distance )
				result = - gravitationConstantValue * massOneValue * massTwoValue / distanceValue;
				break;
		}

		// Update and display result.
		if (double.IsFinite(result))
		{
			resultValue = result;
			resultField.text = $"{result}";
		}
		else
		{
			resultField.text = "N/A";
			resultValue = null;
		}
	}

	public void ResetState()
	{
		// Set to default values.
		gravitationConstantCoefficientInputField.text = "0";
		gravitationConstantExponentInputField.text = "0";
		massOneCoefficientInputField.text = "0";
		massOneExponentInputField.text = "0";
		massTwoCoefficientInputField.text = "0";
		massTwoExponentInputField.text = "0";
		distanceCoefficientnputField.text = "0";
		distanceExponentInputField.text = "0";
		// Clear result field
		resultField.text = "0";
		// Set result val to null.
		resultValue = null;
	}
}