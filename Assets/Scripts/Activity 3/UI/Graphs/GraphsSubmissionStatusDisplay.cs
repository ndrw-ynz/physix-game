using System;
using UnityEngine;
using UnityEngine.UI;

public class GraphsSubmissionStatusDisplay : SubmissionStatusDisplay
{
	public event Action ProceedEvent;

	[Header("Graphs Status Border Displays")]
	[SerializeField] private Image positionGraphStatusBorderDisplay;
	[SerializeField] private Image velocityGraphStatusBorderDisplay;
	[SerializeField] private Image accelerationGraphStatusBorderDisplay;

	public void UpdateStatusBorderDisplayFromResults(GraphsAnswerSubmissionResults results)
	{
		positionGraphStatusBorderDisplay.color = results.isPositionVsTimeGraphCorrect ? new Color32(175, 255, 155, 255) : new Color32(200, 75, 55, 255);
		velocityGraphStatusBorderDisplay.color = results.isVelocityVsTimeGraphCorrect ? new Color32(175, 255, 155, 255) : new Color32(200, 75, 55, 255);
		accelerationGraphStatusBorderDisplay.color = results.isAccelerationVsTimeGraphCorrect ? new Color32(175, 255, 155, 255) : new Color32(200, 75, 55, 255);
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
