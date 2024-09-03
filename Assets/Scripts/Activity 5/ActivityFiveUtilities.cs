using System;

public class ForceTypeAnswerSubmissionResults
{
	public bool isUpForceTypeCorrect;
	public bool isDownForceTypeCorrect;
	public bool isLeftForceTypeCorrect;
	public bool isRightForceTypeCorrect;

	public bool isAllCorrect()
	{
		return (
			isUpForceTypeCorrect &&
			isDownForceTypeCorrect &&
			isLeftForceTypeCorrect &&
			isRightForceTypeCorrect
			);
	}
}

public static class ActivityFiveUtilities
{
    public static ForceTypeAnswerSubmissionResults ValidateForceTypeSubmission(ForceObjectMotionType forceObjectMotionType, ForceTypeAnswerSubmission submission)
    {
		ForceTypeAnswerSubmissionResults results = new ForceTypeAnswerSubmissionResults();
		switch (forceObjectMotionType)
		{
			case ForceObjectMotionType.Apple_OnBranch:
				results.isUpForceTypeCorrect = submission.upForceType == ForceType.TensionForce;
				results.isDownForceTypeCorrect = submission.downForceType == ForceType.GravitationalForce;
				results.isLeftForceTypeCorrect = submission.leftForceType == null;
				results.isRightForceTypeCorrect = submission.rightForceType == null;
				break;
			case ForceObjectMotionType.Apple_Falling:
				results.isUpForceTypeCorrect = submission.upForceType == null;
				results.isDownForceTypeCorrect = submission.downForceType == ForceType.GravitationalForce;
				results.isLeftForceTypeCorrect = submission.leftForceType == null;
				results.isRightForceTypeCorrect = submission.rightForceType == null;
				break;
		}
		return results;
	}

	public static bool ValidateForceSubmission(float? submittedForce, ForceData forceData) 
	{
		if (submittedForce == null) return false;
		// Formula: Force = mass * acceleration
		return Math.Abs((float)submittedForce - (forceData.mass * forceData.acceleration)) <= 0.0001;
	}
}