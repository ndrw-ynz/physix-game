using System;

public class AppleForceTypeSubmissionStatusDisplay : ForceTypeSubmissionStatusDisplay
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