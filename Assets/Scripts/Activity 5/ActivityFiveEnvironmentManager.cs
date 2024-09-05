using System.Collections.Generic;
using UnityEngine;

public class ActivityFiveEnvironmentManager : ActivityEnvironmentManager
{
	[Header("Views")]
	[SerializeField] private ForceMotionView appleMotionView;
	[SerializeField] private ForceMotionView rockMotionView;

	[Header("Submission Status Displays")]
	[Header("Apple Force Submission Status Displays")]
	[SerializeField] private ForceSubmissionStatusDisplay appleForceSubmissionStatusDisplay;
	[SerializeField] private ForceDiagramSubmissionStatusDisplay appleForceDiagramSubmissionStatusDisplay;
	[Header("Rock Force Submission Status Displays")]
	[SerializeField] private ForceSubmissionStatusDisplay rockForceSubmissionStatusDisplay;
	[SerializeField] private ForceDiagramSubmissionStatusDisplay rockForceDiagramSubmissionStatusDisplay;

	[Header("Environment Cameras")]
	[SerializeField] private Camera appleOnBranchCamera;
	[SerializeField] private Camera appleTreeCamera;
	[SerializeField] private Camera stationaryRockCamera;
	[SerializeField] private Camera rollingRockCamera;
	[SerializeField] private Camera bouncingRockCamera;
	[SerializeField] private Camera flyingRockCamera;
	[SerializeField] private Camera stationaryBoatCamera;
	[SerializeField] private Camera boatMovingRightCamera;
	[SerializeField] private Camera boatMovingLeftCamera;

	[Header("Apple Tree Area Game Objects")]
	[SerializeField] private GameObject appleOnBranch;
	[SerializeField] private AppleFallMotionAnimate fallingApple;
	[SerializeField] private GameObject appleTreeAreaIndicatorEffect;
	[SerializeField] private InteractableViewOpenerObject interactableApples;

	[Header("Rock Area Game Objects")]
	[SerializeField] private GameObject stationaryRock;
	[SerializeField] private RollingRockMotionAnimate rollingRock;
	[SerializeField] private BouncingRockMotionAnimate bouncingRock;
	[SerializeField] private FlyingRockAnimate flyingRock;
	[SerializeField] private GameObject rockAreaIndicatorEffect;
	[SerializeField] private InteractableViewOpenerObject interactableRock;

	[Header("Boat River Area Game Objects")]
	[SerializeField] private BoatMotionAnimate stationaryBoat;
	[SerializeField] private BoatMotionAnimate boatMovingRight;
	[SerializeField] private BoatMotionAnimate boatMovingLeft;
	[SerializeField] private GameObject boatAreaIndicatorEffect;
	[SerializeField] private InteractableViewOpenerObject interactableBoat;

	private AppleMotionEnvironmentStateMachine appleMotionEnvironmentStateMachine;
	private Queue<AppleMotionEnvironmentState> appleMotionEnvironmentStateQueue;
	private RockMotionEnvironmentStateMachine rockMotionEnvironmentStateMachine;
	private Queue<RockMotionEnvironmentState> rockMotionEnvironmentStateQueue;

	public void Start()
	{
		appleMotionView.OpenViewEvent += UpdateAppleEnvironmentStateMachine;
		appleMotionView.QuitViewEvent += () => appleMotionEnvironmentStateMachine.TransitionToState(AppleMotionEnvironmentState.None);
		appleForceSubmissionStatusDisplay.ProceedEvent += DequeueAppleEnvironmentStateQueue;
		appleForceDiagramSubmissionStatusDisplay.ProceedEvent += DequeueAppleEnvironmentStateQueue;

		rockMotionView.OpenViewEvent += UpdateRockEnvironmentStateMachine;
		rockMotionView.QuitViewEvent += () => rockMotionEnvironmentStateMachine.TransitionToState(RockMotionEnvironmentState.None);
		rockForceSubmissionStatusDisplay.ProceedEvent += DequeueRockEnvironmentStateQueue;
		rockForceDiagramSubmissionStatusDisplay.ProceedEvent += DequeueRockEnvironmentStateQueue;

		// Initialize environment state queues
		InitializeEnvironmentStateQueues();

		// Initialize values for apple tree environment state machine
		appleMotionEnvironmentStateMachine = new AppleMotionEnvironmentStateMachine(this);
		appleMotionEnvironmentStateMachine.Initialize(AppleMotionEnvironmentState.None);
		// Initialize values for rock environment state machine
		rockMotionEnvironmentStateMachine = new RockMotionEnvironmentStateMachine(this);
		rockMotionEnvironmentStateMachine.Initialize(RockMotionEnvironmentState.None);
	}

	private void InitializeEnvironmentStateQueues()
	{
		// Initialize and enqueue default content of each environment state queues
		appleMotionEnvironmentStateQueue = new Queue<AppleMotionEnvironmentState>();
		appleMotionEnvironmentStateQueue.Enqueue(AppleMotionEnvironmentState.OnBranch);
		appleMotionEnvironmentStateQueue.Enqueue(AppleMotionEnvironmentState.Falling);
		appleMotionEnvironmentStateQueue.Enqueue(AppleMotionEnvironmentState.Falling);

		rockMotionEnvironmentStateQueue = new Queue<RockMotionEnvironmentState>();
		rockMotionEnvironmentStateQueue.Enqueue(RockMotionEnvironmentState.Stationary);
		rockMotionEnvironmentStateQueue.Enqueue(RockMotionEnvironmentState.Rolling);
		rockMotionEnvironmentStateQueue.Enqueue(RockMotionEnvironmentState.Rolling);
		rockMotionEnvironmentStateQueue.Enqueue(RockMotionEnvironmentState.Bouncing);
		rockMotionEnvironmentStateQueue.Enqueue(RockMotionEnvironmentState.Flying);
		rockMotionEnvironmentStateQueue.Enqueue(RockMotionEnvironmentState.Flying);

		// Enqueue addition environment states based on difficulty configuration
		switch (ActivityFiveManager.difficultyConfiguration)
		{
			case Difficulty.Medium:
				appleMotionEnvironmentStateQueue.Enqueue(AppleMotionEnvironmentState.Falling);

				rockMotionEnvironmentStateQueue.Enqueue(RockMotionEnvironmentState.Rolling);
				rockMotionEnvironmentStateQueue.Enqueue(RockMotionEnvironmentState.Flying);
				break;
			case Difficulty.Hard:
				appleMotionEnvironmentStateQueue.Enqueue(AppleMotionEnvironmentState.Falling);
				appleMotionEnvironmentStateQueue.Enqueue(AppleMotionEnvironmentState.Falling);

				rockMotionEnvironmentStateQueue.Enqueue(RockMotionEnvironmentState.Rolling);
				rockMotionEnvironmentStateQueue.Enqueue(RockMotionEnvironmentState.Flying);
				rockMotionEnvironmentStateQueue.Enqueue(RockMotionEnvironmentState.Rolling);
				rockMotionEnvironmentStateQueue.Enqueue(RockMotionEnvironmentState.Flying);
				break;
		}
	}

	public void SetAppleOnBranchState(bool isActive)
	{
		appleOnBranchCamera.gameObject.SetActive(isActive);
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

	public void SetRockStationaryState(bool isActive)
	{
		stationaryRockCamera.gameObject.SetActive(isActive);
		stationaryRock.gameObject.SetActive(isActive);
	}

	public void SetRockRollingState(bool isActive)
	{
		rollingRockCamera.gameObject.SetActive(isActive);
		rollingRock.gameObject.SetActive(isActive);
	}

	public void SetRockBouncingState(bool isActive)
	{
		bouncingRockCamera.gameObject.SetActive(isActive);
		bouncingRock.gameObject.SetActive(isActive);
	}

	public void SetRockFlyingState(bool isActive)
	{
		flyingRockCamera.gameObject.SetActive(isActive);
		flyingRock.gameObject.SetActive(isActive);
	}

	private void DequeueRockEnvironmentStateQueue()
	{
		rockMotionEnvironmentStateQueue.Dequeue();
		UpdateRockEnvironmentStateMachine();
	}

	private void UpdateRockEnvironmentStateMachine()
	{
		if (rockMotionEnvironmentStateQueue.Count == 0)
		{
			// Deactivate area effect and interactable rock
			rockAreaIndicatorEffect.gameObject.SetActive(false);
			interactableRock.SetInteractable(false);

			rockMotionEnvironmentStateMachine.TransitionToState(RockMotionEnvironmentState.None);
		}
		else
		{
			rockMotionEnvironmentStateMachine.TransitionToState(rockMotionEnvironmentStateQueue.Peek());
		}
	}

	public void SetBoatStationaryState(bool isActive)
	{
		stationaryBoatCamera.gameObject.SetActive(isActive);
		stationaryBoat.gameObject.SetActive(isActive);
	}

	public void SetBoatMovingRightState(bool isActive)
	{
		boatMovingRightCamera.gameObject.SetActive(isActive);
		boatMovingRight.gameObject.SetActive(isActive);
	}

	public void SetBoatMovingLeftState(bool isActive)
	{
		boatMovingLeftCamera.gameObject.SetActive(isActive);
		boatMovingLeft.gameObject.SetActive(isActive);
	}
}