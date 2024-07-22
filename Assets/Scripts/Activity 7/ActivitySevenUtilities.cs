using System;

public static class ActivitySevenUtilities
{
	#region Center Of Mass
	public static bool ValidateMassTimesCoordinatesSubmission(int?[] submittedMassTimesCoordinates, int[] massValues, int[] coordinateValues)
	{
		for (int i = 0; i < submittedMassTimesCoordinates.Length; i++)
		{
			int currentProduct = massValues[i] * coordinateValues[i];
			if (submittedMassTimesCoordinates[i] != currentProduct) return false;
		}
		return true;
	}

	public static bool ValidateSumOfMassTimesCoordinatesSubmission(int? submittedSumOfMassTimesCoordinates, int[] massValues, int[] coordinateValues)
	{
		int expectedSumOfProducts = 0;
		for (int i = 0; i < massValues.Length; i++)
		{
			expectedSumOfProducts += massValues[i] * coordinateValues[i];
		}
		return submittedSumOfMassTimesCoordinates == expectedSumOfProducts;
	}

	public static bool ValidateTotalMassSubmission(int? submittedTotalMass, int[] massValues)
	{
		int expectedTotalMass = 0;
		for (int i = 0; i < massValues.Length; i++)
		{
			expectedTotalMass += massValues[i];
		}
		return submittedTotalMass == expectedTotalMass;
	}

	public static bool ValidateCenterOfMassSubmission(float? submittedCenterOfMass, int[] massValues, int[] coordinateValues)
	{
		int totalMass = 0;
		int sumOfProducts = 0;

		for (int i = 0; i < massValues.Length; i++)
		{
			sumOfProducts += massValues[i] * coordinateValues[i];
			totalMass += massValues[i];
		}

		float expectedCenterOfMass = (float) Math.Round((float) sumOfProducts/totalMass, 2);

		return submittedCenterOfMass == expectedCenterOfMass;
	}

	#endregion
}