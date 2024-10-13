using System;
using System.Collections.Generic;
using System.Linq;

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

public class VarianceAnswerSubmissionResults
{
	public bool isMassSumValueCorrect;
	public bool isMeanValueCorrect;
	public List<bool> squaredDeviationsResult;
	public bool isVarianceValueCorrect;

	public bool isAllCorrect()
	{
		return (
			isMassSumValueCorrect &&
			isMeanValueCorrect &&
			squaredDeviationsResult.All(x => x) &&
			isVarianceValueCorrect
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

	public static VarianceAnswerSubmissionResults ValidateVarianceSubmission(VarianceAnswerSubmission answer, List<float> numericalContainerValues)
	{
		VarianceAnswerSubmissionResults results = new VarianceAnswerSubmissionResults();

		// Validate sum of masses
		float computedSum = numericalContainerValues.Sum();
		if (answer.massSumValue == null)
		{
			results.isMassSumValueCorrect = false;
		} else
		{
			results.isMassSumValueCorrect = Math.Abs((float)answer.massSumValue - computedSum) <= 0.01;
		}

		// Validate mean
		float computedMean = computedSum / numericalContainerValues.Count;
		if (answer.meanValue == null)
		{
			results.isMeanValueCorrect = false;
		} else
		{
			results.isMeanValueCorrect = Math.Abs((float)answer.meanValue - computedMean) <= 0.01;
		}

		// Validate squared deviations
		List<bool> squaredDeviationResults = new List<bool>();
		List<float> computedSquaredDeviations = new List<float>();
		for (int i = 0; i < numericalContainerValues.Count; i++)
		{
			if (answer.squaredDeviationValues[i] == null)
			{
				computedSquaredDeviations.Add(0f);
				squaredDeviationResults.Add(false);
				continue;
			}

			float computedDeviation = (float) Math.Pow(numericalContainerValues[i] - computedMean, 2);
			computedSquaredDeviations.Add(computedDeviation);
			squaredDeviationResults.Add(Math.Abs((float)answer.squaredDeviationValues[i] - computedDeviation) <= 0.01);
		}
		results.squaredDeviationsResult = squaredDeviationResults;

		// Validate variance
		float computedVariance = computedSquaredDeviations.Sum() / (numericalContainerValues.Count - 1);
		if (answer.varianceValue == null)
		{
			results.isVarianceValueCorrect = false;
		} else
		{
			results.isVarianceValueCorrect = Math.Abs((float)answer.varianceValue - computedVariance) <= 0.01;
		}

		return results;
	}
}