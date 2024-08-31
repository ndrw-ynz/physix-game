using UnityEngine;

public abstract class ForceTypeSubmissionStatusDisplay : SubmissionStatusDisplay
{
	[Header("Force Type Answer Displays")]
	[SerializeField] ForceTypeAnswerDisplay upForceTypeAnswerDisplay;
	[SerializeField] ForceTypeAnswerDisplay downForceTypeAnswerDisplay;
	[SerializeField] ForceTypeAnswerDisplay leftForceTypeAnswerDisplay;
	[SerializeField] ForceTypeAnswerDisplay rightForceTypeAnswerDisplay;

	public void UpdateForceDiagramDisplay(ForceTypeAnswerSubmission answer, ForceTypeAnswerSubmissionResults results)
    {
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