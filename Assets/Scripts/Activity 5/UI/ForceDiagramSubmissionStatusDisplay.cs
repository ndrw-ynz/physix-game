using UnityEngine;
using System;

public class ForceDiagramSubmissionStatusDisplay : SubmissionStatusDisplay
{
	public event Action ProceedEvent;

	[Header("Force Type Answer Displays")]
	[SerializeField] private ForceTypeAnswerDisplay upForceTypeAnswerDisplay;
	[SerializeField] private ForceTypeAnswerDisplay downForceTypeAnswerDisplay;
	[SerializeField] private ForceTypeAnswerDisplay leftForceTypeAnswerDisplay;
	[SerializeField] private ForceTypeAnswerDisplay rightForceTypeAnswerDisplay;

	public void UpdateForceDiagramDisplay(ForceDiagramAnswerSubmission answer, ForceDiagramAnswerSubmissionResults results)
    {
        UpdateForceDiagramText(answer);
        UpdateForceDiagramStatusColors(results);
    }

    private void UpdateForceDiagramText(ForceDiagramAnswerSubmission answer)
    {
		upForceTypeAnswerDisplay.UpdateTextDisplay(answer.upForceType);
		downForceTypeAnswerDisplay.UpdateTextDisplay(answer.downForceType);
		leftForceTypeAnswerDisplay.UpdateTextDisplay(answer.leftForceType);
		rightForceTypeAnswerDisplay.UpdateTextDisplay(answer.rightForceType);
	}

	private void UpdateForceDiagramStatusColors(ForceDiagramAnswerSubmissionResults results)
    {
		upForceTypeAnswerDisplay.UpdateStatusBorderDisplay(results.isUpForceTypeCorrect);
		downForceTypeAnswerDisplay.UpdateStatusBorderDisplay(results.isDownForceTypeCorrect);
		leftForceTypeAnswerDisplay.UpdateStatusBorderDisplay(results.isLeftForceTypeCorrect);
		rightForceTypeAnswerDisplay.UpdateStatusBorderDisplay(results.isRightForceTypeCorrect);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		proceedButton.onClick.AddListener(() => ProceedEvent?.Invoke());
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		proceedButton.onClick.RemoveAllListeners();
	}
}