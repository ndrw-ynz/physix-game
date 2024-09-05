using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ForceObjectMotionType
{
	Apple_OnBranch,
	Apple_Falling,
	Rock_Stationary,
	Rock_RollingRight,
	Rock_Bouncing,
	Rock_Flying,
	Boat_Stationary,
	Boat_MovingRight,
	Boat_MovingLeft
}

public enum ActivityFiveSubActivityState
{
	SolveForceDiagram,
	SolveForceCalculation,
	None
}

public class ForceData
{
	public float acceleration;
	public float mass;
}

public class ForceDiagramAnswerSubmission
{
	public ForceType? upForceType { get; private set; }
	public ForceType? downForceType { get; private set; }
	public ForceType? leftForceType {get; private set;}
	public ForceType? rightForceType { get; private set; }

	public ForceDiagramAnswerSubmission(
		ForceType? upForceType,
		ForceType? downForceType,
		ForceType? leftForceType,
		ForceType? rightForceType
		)
	{
		this.upForceType = upForceType;
		this.downForceType = downForceType;
		this.leftForceType = leftForceType;
		this.rightForceType = rightForceType;
	}
}

public class ActivityFiveManager : MonoBehaviour
{
	public static Difficulty difficultyConfiguration;

	[Header("Input Reader")]
	[SerializeField] InputReader inputReader;

	[Header("Level Data - Force")]
	[SerializeField] private ForceSubActivitySO forceLevelOne;
	[SerializeField] private ForceSubActivitySO forceLevelTwo;
	[SerializeField] private ForceSubActivitySO forceLevelThree;
	private ForceSubActivitySO currentForceLevel;

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

	// queue for apple motion
	private ForceMotionViewStateMachine appleForceMotionSubActivityStateMachine;
	private Queue<ActivityFiveSubActivityState> appleForceMotionSubActivityStateQueue;
	// queue for rock motion
	private ForceMotionViewStateMachine rockForceMotionSubActivityStateMachine;
	private Queue<ActivityFiveSubActivityState> rockForceMotionSubActivityStateQueue;


	// given data - force apple
	private ForceData appleForceGivenData;
	private Queue<ForceObjectMotionType> appleForceDiagramStateQueue;
	// given data - force rock
	private ForceData rockForceGivenData;
	private Queue<ForceObjectMotionType> rockForceDiagramStateQueue;

	private void Start()
	{
		ConfigureLevelData(Difficulty.Easy);

		appleMotionView.OpenViewEvent += UpdateAppleSubActivityStateMachine;
		appleMotionView.SubmitForceAnswerEvent += (answer) => CheckForceAnswer(
			answer,
			appleForceGivenData,
			appleForceSubmissionStatusDisplay
			);
		appleMotionView.SubmitForceDiagramAnswerEvent += (answer) => CheckForceDiagramAnswer(
			answer,
			appleForceDiagramStateQueue,
			appleForceDiagramSubmissionStatusDisplay
			);
		appleForceSubmissionStatusDisplay.ProceedEvent += UpdateAppleSubActivityStateQueue;
		appleForceDiagramSubmissionStatusDisplay.ProceedEvent += UpdateAppleForceDiagramStateQueue;


		rockMotionView.OpenViewEvent += UpdateRockSubActivityStateMachine;
		rockMotionView.SubmitForceAnswerEvent += (answer) => CheckForceAnswer(
			answer,
			rockForceGivenData,
			rockForceSubmissionStatusDisplay
			);
		rockMotionView.SubmitForceDiagramAnswerEvent += (answer) => CheckForceDiagramAnswer(
			answer,
			rockForceDiagramStateQueue,
			rockForceDiagramSubmissionStatusDisplay
			);
		rockForceSubmissionStatusDisplay.ProceedEvent += UpdateRockSubActivityStateQueue;
		rockForceDiagramSubmissionStatusDisplay.ProceedEvent += UpdateRockForceDiagramStateQueue;


		// Initialize sub activity state queues
		InitializeSubActivityStateQueues();

		// Initialize force diagram motion type queues
		InitializeForceDiagramStateQueues();

		// Initialize values for force motion sub activity state machines
		appleForceMotionSubActivityStateMachine = new ForceMotionViewStateMachine(appleMotionView);
		appleForceMotionSubActivityStateMachine.Initialize(appleForceMotionSubActivityStateQueue.Peek());
		rockForceMotionSubActivityStateMachine = new ForceMotionViewStateMachine(rockMotionView);
		rockForceMotionSubActivityStateMachine.Initialize(rockForceMotionSubActivityStateQueue.Peek());

		// Update state machine
		UpdateAppleSubActivityStateMachine();
		UpdateRockSubActivityStateMachine();
	}

	private void ConfigureLevelData(Difficulty difficulty)
	{
		difficultyConfiguration = difficulty;

		switch (difficulty)
		{
			case Difficulty.Easy:
				currentForceLevel = forceLevelOne;
				break;
			case Difficulty.Medium:
				currentForceLevel = forceLevelTwo;
				break;
			case Difficulty.Hard:
				currentForceLevel = forceLevelThree;
				break;
		}
	}

	private void InitializeSubActivityStateQueues()
	{
		// First initialize default content of queues
		appleForceMotionSubActivityStateQueue = new Queue<ActivityFiveSubActivityState>();
		appleForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceDiagram);
		appleForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceDiagram);
		appleForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceCalculation);

		rockForceMotionSubActivityStateQueue = new Queue<ActivityFiveSubActivityState>();
		rockForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceDiagram);
		rockForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceDiagram);
		rockForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceCalculation);
		rockForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceDiagram);
		rockForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceDiagram);
		rockForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceCalculation);


		// Enqueue additional force calculations based from difficulty configuration
		switch (difficultyConfiguration)
		{
			case Difficulty.Medium:
				appleForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceCalculation);

				rockForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceCalculation);
				break;
			case Difficulty.Hard:
				appleForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceCalculation);
				appleForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceCalculation);

				rockForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceCalculation);
				rockForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceCalculation);
				break;
		}
	}

	private void InitializeForceDiagramStateQueues()
	{
		// Initialize queue for apple force
		appleForceDiagramStateQueue = new Queue<ForceObjectMotionType>();

		appleForceDiagramStateQueue.Enqueue(ForceObjectMotionType.Apple_OnBranch);
		appleForceDiagramStateQueue.Enqueue(ForceObjectMotionType.Apple_Falling);


		// Initialize queue for rock force
		rockForceDiagramStateQueue = new Queue<ForceObjectMotionType>();

		rockForceDiagramStateQueue.Enqueue(ForceObjectMotionType.Rock_Stationary);
		rockForceDiagramStateQueue.Enqueue(ForceObjectMotionType.Rock_RollingRight);
		rockForceDiagramStateQueue.Enqueue(ForceObjectMotionType.Rock_Bouncing);
		rockForceDiagramStateQueue.Enqueue(ForceObjectMotionType.Rock_Flying);
	}

	private ForceData GenerateNewForceGivenData(ForceSubActivitySO forceSO)
	{
		ForceData forceData = new ForceData();

		forceData.acceleration = (float) Math.Round(Random.Range(forceSO.accelerationMinVal, forceSO.accelerationMaxVal), 3);
		forceData.mass = (float) Math.Round(Random.Range(forceSO.massMinVal, forceSO.massMaxVal), 3);

		return forceData;
	}

	private void CheckForceAnswer(float? answer, ForceData forceGivenData, ForceSubmissionStatusDisplay forceSubmissionStatusDisplay)
	{
		bool result = ActivityFiveUtilities.ValidateForceSubmission(answer, forceGivenData);
		// add metrics alter
		DisplayForceSubmissionResults(result, forceSubmissionStatusDisplay);
	}


	private void CheckForceDiagramAnswer(ForceDiagramAnswerSubmission answer, Queue<ForceObjectMotionType> forceDiagramStateQueue, ForceDiagramSubmissionStatusDisplay forceDiagramSubmissionStatusDisplay)
	{
		ForceTypeAnswerSubmissionResults results = ActivityFiveUtilities.ValidateForceTypeSubmission(forceDiagramStateQueue.Peek(), answer);
		// add metrics alter
		DisplayForceTypeSubmissionResults(answer, results, forceDiagramSubmissionStatusDisplay);
	}

	private void DisplayForceTypeSubmissionResults(ForceDiagramAnswerSubmission answer, ForceTypeAnswerSubmissionResults results, ForceDiagramSubmissionStatusDisplay forceTypeSubmissionStatusDisplay)
	{
		if (results.isAllCorrect())
		{
			forceTypeSubmissionStatusDisplay.SetSubmissionStatus(true, "correct");
		}
		else
		{
			forceTypeSubmissionStatusDisplay.SetSubmissionStatus(false, "wrong");
		}

		forceTypeSubmissionStatusDisplay.UpdateForceDiagramDisplay(answer, results);

		forceTypeSubmissionStatusDisplay.gameObject.SetActive(true);
	}

	private void DisplayForceSubmissionResults(bool isCorrect, SubmissionStatusDisplay submissionStatusDisplay)
	{
		if (isCorrect)
		{
			submissionStatusDisplay.SetSubmissionStatus(true, "correct");
		} else
		{
			submissionStatusDisplay.SetSubmissionStatus(false, "wrong");
		}

		submissionStatusDisplay.gameObject.SetActive(true);
	}

	#region Apple Motion

	private void UpdateAppleForceDiagramStateQueue()
	{
		appleForceDiagramStateQueue.Dequeue();
		UpdateAppleSubActivityStateQueue();
	}

	private void UpdateAppleSubActivityStateQueue()
	{
		appleForceMotionSubActivityStateQueue.Dequeue();
		UpdateAppleSubActivityStateMachine();
	}

	private void UpdateAppleSubActivityStateMachine()
	{
		if (appleForceMotionSubActivityStateQueue.Count == 0)
		{
			appleForceMotionSubActivityStateMachine.TransitionToState(ActivityFiveSubActivityState.None);
		} else
		{
			ActivityFiveSubActivityState queueSubActivityHead = appleForceMotionSubActivityStateQueue.Peek();

			// Do manager handling stuff
			switch (queueSubActivityHead)
			{
				case ActivityFiveSubActivityState.SolveForceDiagram:
					appleMotionView.ResetForceDiagram();
					break;
				case ActivityFiveSubActivityState.SolveForceCalculation:
					appleForceGivenData = GenerateNewForceGivenData(currentForceLevel);
					appleMotionView.SetupForceCalculationDisplay(appleForceGivenData);
					break;
			}

			appleForceMotionSubActivityStateMachine.TransitionToState(queueSubActivityHead);
		}
	}
	#endregion

	#region Rock Motion

	private void UpdateRockForceDiagramStateQueue()
	{
		rockForceDiagramStateQueue.Dequeue();
		UpdateRockSubActivityStateQueue();
	}

	private void UpdateRockSubActivityStateQueue()
	{
		rockForceMotionSubActivityStateQueue.Dequeue();
		UpdateRockSubActivityStateMachine();
	}

	private void UpdateRockSubActivityStateMachine()
	{
		if (rockForceMotionSubActivityStateQueue.Count == 0)
		{
			rockForceMotionSubActivityStateMachine.TransitionToState(ActivityFiveSubActivityState.None);
		}
		else
		{
			ActivityFiveSubActivityState queueSubActivityHead = rockForceMotionSubActivityStateQueue.Peek();

			// Do manager handling stuff
			switch (queueSubActivityHead)
			{
				case ActivityFiveSubActivityState.SolveForceDiagram:
					rockMotionView.ResetForceDiagram();
					break;
				case ActivityFiveSubActivityState.SolveForceCalculation:
					rockForceGivenData = GenerateNewForceGivenData(currentForceLevel);
					rockMotionView.SetupForceCalculationDisplay(rockForceGivenData);
					break;
			}

			rockForceMotionSubActivityStateMachine.TransitionToState(queueSubActivityHead);
		}
	}
	#endregion
}