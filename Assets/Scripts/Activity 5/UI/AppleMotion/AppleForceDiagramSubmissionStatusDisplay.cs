using System;

public class AppleForceDiagramSubmissionStatusDisplay : ForceDiagramSubmissionStatusDisplay
{
	public static event Action ProceedEvent;

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