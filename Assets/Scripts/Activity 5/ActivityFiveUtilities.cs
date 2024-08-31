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
    public static ForceTypeAnswerSubmissionResults ValidateForceTypeSubmission(ForceTypeAnswerSubmission submission)
    {
		ForceTypeAnswerSubmissionResults results = new ForceTypeAnswerSubmissionResults();
		switch (submission.forceObjectMotionType)
		{
			case ForceObjectMotionType.Apple_OnBranch:
				results.isUpForceTypeCorrect = submission.upForceType == ForceType.TensionForce;
				results.isDownForceTypeCorrect = submission.downForceType == ForceType.GravitationalForce;
				results.isLeftForceTypeCorrect = submission.leftForceType == null;
				results.isRightForceTypeCorrect = submission.rightForceType == null;
				break;
		}
		return results;
	}
}