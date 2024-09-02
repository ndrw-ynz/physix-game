using System;

public class AppleForceTypeSubmissionStatusDisplay : ForceTypeSubmissionStatusDisplay
{
	public static event Action ProceedEvent;
	public static event Action<ForceObjectMotionType> UpdateAppleEnvionmentStateEvent;

	protected override void OnEnable()
	{
		base.OnEnable();
		proceedButton.onClick.AddListener(() => ProceedEvent?.Invoke());
		proceedButton.onClick.AddListener(() => UpdateAppleEnvionmentStateEvent?.Invoke(displayedForceObjectMotionType));
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		proceedButton.onClick.RemoveAllListeners();
	}
}