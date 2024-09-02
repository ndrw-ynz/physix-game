using UnityEngine;

public class ActivityFiveEnvironmentManager : ActivityEnvironmentManager
{
	[Header("Environment Cameras")]
	[SerializeField] private Camera appleCamera;
	[SerializeField] private Camera appleTreeCamera;

	[Header("Apple Tree Area Game Objects")]
	[SerializeField] private GameObject appleOnBranch;
	[SerializeField] private AppleFallMotionAnimate fallingApple;
	[SerializeField] private GameObject appleTreeAreaIndicatorEffect;
	[SerializeField] private InteractableViewOpenerObject interactableApples;

	private AppleMotionEnvironmentStateMachine appleMotionEnvironmentStateMachine;
	private AppleMotionEnvironmentState currentAppleMotionEnvironmentState;
	public void Start()
	{
		AppleMotionView.OpenViewEvent += () => appleMotionEnvironmentStateMachine.TransitionToState(currentAppleMotionEnvironmentState);
		AppleMotionView.QuitViewEvent += () => appleMotionEnvironmentStateMachine.TransitionToState(AppleMotionEnvironmentState.None);
		AppleForceTypeSubmissionStatusDisplay.UpdateAppleEnvionmentStateEvent += UpdateAppleEnvironmentStateMachine;
		
		// Initialize values for apple tree environment state machine
		appleMotionEnvironmentStateMachine = new AppleMotionEnvironmentStateMachine(this);
		appleMotionEnvironmentStateMachine.Initialize(AppleMotionEnvironmentState.None);
		currentAppleMotionEnvironmentState = AppleMotionEnvironmentState.OnBranch;
	}

	public void SetAppleOnBranchState(bool isActive)
	{
		appleCamera.gameObject.SetActive(isActive);
		appleOnBranch.gameObject.SetActive(isActive);
	}

	public void SetAppleFallingState(bool isActive)
	{
		appleTreeCamera.gameObject.SetActive(isActive);
		fallingApple.gameObject.SetActive(isActive);
	}

	private void UpdateAppleEnvironmentStateMachine(ForceObjectMotionType clearedForceObjectMotionType)
	{
		switch (clearedForceObjectMotionType)
		{
			case ForceObjectMotionType.Apple_OnBranch:
				currentAppleMotionEnvironmentState = AppleMotionEnvironmentState.Falling;
				break;
			case ForceObjectMotionType.Apple_Falling:
				break;
		}
		appleMotionEnvironmentStateMachine.TransitionToState(currentAppleMotionEnvironmentState);
	}
}