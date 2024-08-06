using System;
using System.Collections.Generic;
using UnityEngine;

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

	#region Momentum-Impulse and Net Force
	public static bool ValidateMomentumImpulse(float? submittedMomentumImpulse, float mass, float velocity)
	{
		if (submittedMomentumImpulse == null) return false;
		return Mathf.Abs(
			(float)(submittedMomentumImpulse - (mass * velocity))
			) <= 0.0001;
	}

	public static bool ValidateNetForce(float? submittedNetForce, float mass, float velocity, float totalTime)
	{
		if (submittedNetForce == null) return false;
		return Mathf.Abs(
			(float)submittedNetForce - (float)Math.Round(mass * velocity / totalTime, 4)
			) <= 0.0001;
	}
	#endregion

	#region Elastic Inelastic Collision

	public static int FindGCD(int a, int b)
	{
		if (b > a)
		{
			int temp = a;
			a = b;
			b = temp;
		}

		while (b > 0)
		{
			int remainder = a % b;
			a = b;
			b = remainder;
		}

		return a;
	}

	public static bool ValidateNetMomentum(float? submittedNetMomentum, float cubeOneMass, float cubeOneVelocity, float cubeTwoMass, float cubeTwoVelocity)
	{
		if (submittedNetMomentum == null) return false;
		return Mathf.Abs(
			(float)(submittedNetMomentum) - (cubeOneMass * cubeOneVelocity + cubeTwoMass * cubeTwoVelocity)
			) <= 0.0001;
	}

	public static bool ValidateCollisionType(CollisionType? submittedCollisionType, CollisionData collisionData)
	{
		if (submittedCollisionType == null) return false;

		CollisionObject cubeOne = collisionData.cubeOne;
		CollisionObject cubeTwo = collisionData.cubeTwo;

		float initialMomentumSum = (cubeOne.mass * cubeOne.initialVelocity + cubeTwo.mass * cubeTwo.initialVelocity);
		float finalMomentumSum = (cubeOne.mass * cubeOne.finalVelocity + cubeTwo.mass * cubeTwo.finalVelocity);

		bool isElastic = Mathf.Abs(initialMomentumSum - finalMomentumSum) <= 0.0001;

		if (
			(isElastic == true && submittedCollisionType == CollisionType.Elastic) ||
			(isElastic == false && submittedCollisionType == CollisionType.Inelastic)
			)
		{
			return true;
		} else
		{
			return false;
		}
	}
	#endregion
}