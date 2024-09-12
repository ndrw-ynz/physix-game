using System;
using UnityEngine;

public class DotProductAnswerSubmissionResults
{
    public bool isXCoordScalarProductCorrect;
	public bool isYCoordScalarProductCorrect;
	public bool isZCoordScalarProductCorrect;
	public bool isDotProductCorrect;

	public bool isAllCorrect()
	{
		return (
			isXCoordScalarProductCorrect &&
			isYCoordScalarProductCorrect &&
			isZCoordScalarProductCorrect &&
			isDotProductCorrect
			);
	}
}

public class WorkSubActivityAnswerSubmissionResults
{
	public bool isForceCorrect;
	public bool isWorkCorrect;

	public bool isAllCorrect()
	{
		return isForceCorrect && isWorkCorrect;
	}
}

public static class ActivitySixUtilities
{
	public static DotProductAnswerSubmissionResults ValidateDotProductSubmission(DotProductAnswerSubmission answer, DotProductData givenData)
	{
		DotProductAnswerSubmissionResults results = new DotProductAnswerSubmissionResults();
		
		// Validate scalar products first
		// Formula: scalar product = Ax * Bx, wherein x is any coordinate
		float calculatedXCoordScalarProduct = givenData.satelliteDishVector.x * givenData.targetObjectVector.x;
		float calculatedYCoordScalarProduct = givenData.satelliteDishVector.y * givenData.targetObjectVector.y;
		float calculatedZCoordScalarProduct = givenData.satelliteDishVector.z * givenData.targetObjectVector.z;

		results.isXCoordScalarProductCorrect = Math.Abs((float)answer.xCoordScalarProduct - calculatedXCoordScalarProduct) <= 0.1;
		results.isYCoordScalarProductCorrect = Math.Abs((float)answer.yCoordScalarProduct - calculatedYCoordScalarProduct) <= 0.1;
		results.isZCoordScalarProductCorrect = Math.Abs((float)answer.zCoordScalarProduct - calculatedZCoordScalarProduct) <= 0.1;
		
		// Validate dot product
		// Formula: xCoordScalarProduct + yCoordScalarProduct + zCoordScalarProduct;
		float calculatedDotProduct = calculatedXCoordScalarProduct + calculatedYCoordScalarProduct + calculatedZCoordScalarProduct;
		results.isDotProductCorrect = Math.Abs((float)answer.dotProduct - calculatedDotProduct) <= 0.1;

		return results;
	}

	public static WorkSubActivityAnswerSubmissionResults ValidateWorkSubActivitySubmission(WorkSubActivityAnswerSubmission answer, WorkSubActivityData givenData)
	{
		WorkSubActivityAnswerSubmissionResults results = new WorkSubActivityAnswerSubmissionResults();

		// Validate force
		// Formula: Force = mass * acceleration
		float calculatedForce = givenData.mass * givenData.acceleration;
		Debug.Log(answer.force);
		Debug.Log(calculatedForce);
		results.isForceCorrect = Math.Abs((float)answer.force - calculatedForce) <= 0.1;

		// Validate work
		// Formula (Linear work): Work = Force * displacement
		// Formula (Angular work): Work = Force * displacement * cos(angle)
		string workFormulaExpression;
		if (givenData.workSubActivityState == WorkSubActivityState.LinearWork)
		{
			workFormulaExpression = $"{calculatedForce} * {givenData.displacement}";
		} else
		{
			workFormulaExpression = $"{calculatedForce} * {givenData.displacement} * cos({givenData.angleMeasure})";
		}
		ExpressionEvaluator.Evaluate(workFormulaExpression, out float calculatedWork);
		results.isWorkCorrect = Math.Abs((float)answer.work - calculatedWork) <= 0.1;

		return results;
	}
}