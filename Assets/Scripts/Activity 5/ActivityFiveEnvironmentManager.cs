using System.Collections.Generic;
using UnityEngine;

public class ActivityFiveEnvironmentManager : ActivityEnvironmentManager
{
	[Header("Views")]
	[SerializeField] private ForceMotionView appleMotionView;

	[Header("Submission Status Displays")]
	[Header("Apple Force Submission Status Displays")]
	[SerializeField] private ForceSubmissionStatusDisplay appleForceSubmissionStatusDisplay;
	[SerializeField] private ForceDiagramSubmissionStatusDisplay appleForceDiagramSubmissionStatusDisplay;

	[Header("Environment Cameras")]
	[SerializeField] private Camera appleCamera;
	[SerializeField] private Camera appleTreeCamera;

	[Header("Apple Tree Area Game Objects")]
	[SerializeField] private GameObject appleOnBranch;
	[SerializeField] private AppleFallMotionAnimate fallingApple;
	[SerializeField] private GameObject appleTreeAreaIndicatorEffect;
	[SerializeField] private InteractableViewOpenerObject interactableApples;

	private AppleMotionEnvironmentStateMachine appleMotionEnvironmentStateMachine;
	private Queue<AppleMotionEnvironmentState> appleMotionEnvironmentStateQueue;

	public void Start()
	{
		appleMotionView.OpenViewEvent += UpdateAppleEnvironmentStateMachine;
		appleMotionView.QuitViewEvent += () => appleMotionEnvironmentStateMachine.TransitionToState(AppleMotionEnvironmentState.None);
		appleForceSubmissionStatusDisplay.ProceedEvent += DequeueAppleEnvironmentStateQueue;
		appleForceDiagramSubmissionStatusDisplay.ProceedEvent += DequeueAppleEnvironmentStateQueue;
		
		// Initialize environment state queues
		InitializeEnvironmentStateQueues();

		// Initialize values for apple tree environment state machine
		appleMotionEnvironmentStateMachine = new AppleMotionEnvironmentStateMachine(this);
		appleMotionEnvironmentStateMachine.Initialize(AppleMotionEnvironmentState.None);
	}

	private void InitializeEnvironmentStateQueues()
	{
		// Initialize and enqueue default content of each environment state queues
		appleMotionEnvironmentStateQueue = new Queue<AppleMotionEnvironmentState>();
		appleMotionEnvironmentStateQueue.Enqueue(AppleMotionEnvironmentState.OnBranch);
		appleMotionEnvironmentStateQueue.Enqueue(AppleMotionEnvironmentState.Falling);
		appleMotionEnvironmentStateQueue.Enqueue(AppleMotionEnvironmentState.Falling);

		// Enqueue addition environment states based on difficulty configuration
		switch (ActivityFiveManager.difficultyConfiguration)
		{
			case Difficulty.Medium:
				appleMotionEnvironmentStateQueue.Enqueue(AppleMotionEnvironmentState.Falling);
				break;
			case Difficulty.Hard:
				appleMotionEnvironmentStateQueue.Enqueue(AppleMotionEnvironmentState.Falling);
				appleMotionEnvironmentStateQueue.Enqueue(AppleMotionEnvironmentState.Falling);
				break;
		}
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

	private void DequeueAppleEnvironmentStateQueue()
	{
		appleMotionEnvironmentStateQueue.Dequeue();
		UpdateAppleEnvironmentStateMachine();
	}

	private void UpdateAppleEnvironmentStateMachine()
	{
		if (appleMotionEnvironmentStateQueue.Count == 0)
		{
			// Deactivate area effect and interactable apples
			appleTreeAreaIndicatorEffect.gameObject.SetActive(false);
			interactableApples.SetInteractable(false);

			appleMotionEnvironmentStateMachine.TransitionToState(AppleMotionEnvironmentState.None);
		}
		else
		{
			appleMotionEnvironmentStateMachine.TransitionToState(appleMotionEnvironmentStateQueue.Peek());
		}
	}
}