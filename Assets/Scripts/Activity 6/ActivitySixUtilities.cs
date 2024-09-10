using System;

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
}