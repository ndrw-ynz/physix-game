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
	[SerializeField] private TMP_InputField gravitationConstantCoefficient;
	[SerializeField] private TMP_InputField gravitationConstantExponent;
	[Header("Mass One Input Fields")]
	[SerializeField] private TMP_InputField massOneCoefficient;
	[SerializeField] private TMP_InputField massOneExponent;
	[Header("Mass Two Input Fields")]
	[SerializeField] private TMP_InputField massTwoCoefficient;
	[SerializeField] private TMP_InputField massTwoExponent;
	[Header("Distance Input Fields")]
	[SerializeField] private TMP_InputField distanceCoefficient;
	[SerializeField] private TMP_InputField distanceExponent;
	[Header("Result Field")]
	[SerializeField] private TMP_InputField resultField;
	[Header("Gravity Formula Type")]
	public GravityFormulaType gravityFormulaType;

	public double? resultValue { get; private set; }

	public void OnValueChange()
	{
		// Check for empty or null input
		if (
			string.IsNullOrEmpty(gravitationConstantCoefficient.text) ||
			string.IsNullOrEmpty(gravitationConstantExponent.text) ||
			string.IsNullOrEmpty(massOneCoefficient.text) ||
			string.IsNullOrEmpty(massOneExponent.text) ||
			string.IsNullOrEmpty(massTwoCoefficient.text) ||
			string.IsNullOrEmpty(massTwoExponent.text) ||
			string.IsNullOrEmpty(distanceCoefficient.text) ||
			string.IsNullOrEmpty(distanceExponent.text)
			)
		{
			resultField.text = "N/A";
			resultValue = null;
			return;
		}

		// Compute value from scientific notation
		double gravitationConstantValue = EvaluateScientificNotation(gravitationConstantCoefficient.text, gravitationConstantExponent.text);
		double massOneValue = EvaluateScientificNotation(massOneCoefficient.text, massOneExponent.text);
		double massTwoValue = EvaluateScientificNotation(massTwoCoefficient.text, massTwoExponent.text);
		double distanceValue = EvaluateScientificNotation(distanceCoefficient.text, distanceExponent.text);

		// Calculate result based on formula type.
		double result = 0;
		switch (gravityFormulaType)
		{
			case GravityFormulaType.GravitationalForce:
				// Formula: gravitationConstant * ( (massOne * massTwo) / distance ^ 2 )
				result = gravitationConstantValue * massOneValue * massTwoValue / Math.Pow(distanceValue, 2);
				break;
			case GravityFormulaType.GravitationalPotentialEnergy:
				// Formula: gravitationConstant * ( (massOne * massTwo) / distance )
				result = gravitationConstantValue * massOneValue * massTwoValue / distanceValue;
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

	private double EvaluateScientificNotation(string coefficient, string exponent)
	{
		ExpressionEvaluator.Evaluate($"{coefficient} * (10 ^ ({exponent}))", out double result);
		Debug.Log(result);
		return result;
	}

	public void ResetState()
	{
		// Set to default values.
		gravitationConstantCoefficient.text = "0";
		gravitationConstantExponent.text = "0";
		massOneCoefficient.text = "0";
		massOneExponent.text = "0";
		massTwoCoefficient.text = "0";
		massTwoExponent.text = "0";
		distanceCoefficient.text = "0";
		distanceExponent.text = "0";
		// Clear result field
		resultField.text = "0";
		// Set result val to null.
		resultValue = null;
	}
}