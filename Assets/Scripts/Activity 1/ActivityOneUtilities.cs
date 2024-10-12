using System;
using System.Collections.Generic;
using UnityEngine;

public class ScientificNotationAnswerSubmissionResults
{
	public bool isCoefficientValueCorrect;
	public bool isExponentValueCorrect;

	public bool isAllCorrect()
	{
		return (
			isCoefficientValueCorrect &&
			isExponentValueCorrect
			);
	}
}

public static class ActivityOneUtilities
{
    public static ScientificNotationAnswerSubmissionResults ValidateScientificNotationSubmission(ScientificNotationAnswerSubmission answer, float givenNumericalValue, string givenUnitOfMeasurement)
    {
		Dictionary<string, int> unitPowers = new Dictionary<string, int>()
		{
			{ "Teragram", 12 },
			{ "Gigagram", 9 },
			{ "Megagram", 6 },
			{ "Kilogram", 3 },
			{ "Hectogram", 2 },
			{ "Dekagram", 1 }
		};

		if (unitPowers.TryGetValue(givenUnitOfMeasurement, out int power))
		{
			// First, get the base form from the given numerical value and unit of measurement.
			double baseForm = givenNumericalValue * Math.Pow(10, power);
			string scientificFormat;
			if (baseForm == Math.Floor(baseForm))
			{
				scientificFormat = baseForm.ToString("0.0E+0");
			}
			else
			{
				scientificFormat = baseForm.ToString("0.#####E+0");
			}
			// Second, extract the coefficient and exponent value from the base form.
			string[] parts = scientificFormat.Split('E');
			float coefficientValue = float.Parse(parts[0]);
			int exponentValue = int.Parse(parts[1]);

			ScientificNotationAnswerSubmissionResults results = new ScientificNotationAnswerSubmissionResults();
			results.isCoefficientValueCorrect = coefficientValue == answer.coefficientValue;
			results.isExponentValueCorrect = exponentValue == answer.exponentValue;

			return results;
		} else
		{
			return null;
		}
	}
}