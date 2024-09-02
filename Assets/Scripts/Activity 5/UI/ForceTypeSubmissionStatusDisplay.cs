using UnityEngine;

public abstract class ForceTypeSubmissionStatusDisplay : SubmissionStatusDisplay
{
	[Header("Force Type Answer Displays")]
	[SerializeField] private ForceTypeAnswerDisplay upForceTypeAnswerDisplay;
	[SerializeField] private ForceTypeAnswerDisplay downForceTypeAnswerDisplay;
	[SerializeField] private ForceTypeAnswerDisplay leftForceTypeAnswerDisplay;
	[SerializeField] private ForceTypeAnswerDisplay rightForceTypeAnswerDisplay;

	public ForceObjectMotionType displayedForceObjectMotionType { get; private set; }

	public void UpdateForceDiagramDisplay(ForceTypeAnswerSubmission answer, ForceTypeAnswerSubmissionResults results)
    {
		displayedForceObjectMotionType = answer.forceObjectMotionType;
        UpdateForceDiagramText(answer);
        UpdateForceDiagramStatusColors(results);
    }

    private void UpdateForceDiagramText(ForceTypeAnswerSubmission answer)
    {
		upForceTypeAnswerDisplay.UpdateTextDisplay(answer.upForceType);
		downForceTypeAnswerDisplay.UpdateTextDisplay(answer.downForceType);
		leftForceTypeAnswerDisplay.UpdateTextDisplay(answer.leftForceType);
		rightForceTypeAnswerDisplay.UpdateTextDisplay(answer.rightForceType);
	}

	private void UpdateForceDiagramStatusColors(ForceTypeAnswerSubmissionResults results)
    {
		upForceTypeAnswerDisplay.UpdateStatusBorderDisplay(results.isUpForceTypeCorrect);
		downForceTypeAnswerDisplay.UpdateStatusBorderDisplay(results.isDownForceTypeCorrect);
		leftForceTypeAnswerDisplay.UpdateStatusBorderDisplay(results.isLeftForceTypeCorrect);
		rightForceTypeAnswerDisplay.UpdateStatusBorderDisplay(results.isRightForceTypeCorrect);
	}
}