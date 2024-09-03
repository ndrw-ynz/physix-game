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

public class ForceTypeAnswerSubmission
{
	public ForceType? upForceType { get; private set; }
	public ForceType? downForceType { get; private set; }
	public ForceType? leftForceType {get; private set;}
	public ForceType? rightForceType { get; private set; }

	public ForceTypeAnswerSubmission(
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

	[Header("Submission Status Displays")]
	[Header("Apple Force Submission Status Displays")]
	[SerializeField] private ForceSubmissionStatusDisplay appleForceSubmissionStatusDisplay;
	[SerializeField] private ForceDiagramSubmissionStatusDisplay appleForceDiagramSubmissionStatusDisplay;

	// queue for apple motion
	private ForceMotionViewStateMachine appleForceMotionSubActivityStateMachine;
	private Queue<ActivityFiveSubActivityState> appleForceMotionSubActivityStateQueue;

	// given data - force
	private ForceData appleForceGivenData;
	private Queue<ForceObjectMotionType> appleForceDiagramMotionTypeQueue;

	private void Start()
	{
		ConfigureLevelData(Difficulty.Easy);

		appleMotionView.OpenViewEvent += UpdateAppleSubActivityStateMachine;
		appleMotionView.SubmitForceAnswerEvent += CheckAppleForceAnswer;
		appleMotionView.SubmitForceTypesAnswerEvent += CheckAppleForceTypeAnswers;
		appleForceSubmissionStatusDisplay.ProceedEvent += UpdateAppleSubActivityStateQueue;
		appleForceDiagramSubmissionStatusDisplay.ProceedEvent += UpdateAppleForceDiagramStateQueue;

		// Initialize sub activity state queues
		InitializeSubActivityStateQueues();

		// Initialize force diagram motion type queues
		InitializeAppleForceDiagramMotionTypeQueue();

		// Initialize values for apple motion sub activity state machine
		appleForceMotionSubActivityStateMachine = new ForceMotionViewStateMachine(appleMotionView);
		appleForceMotionSubActivityStateMachine.Initialize(appleForceMotionSubActivityStateQueue.Peek());

		// Update state machine
		UpdateAppleSubActivityStateMachine();
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

		// Enqueue additional force calculations based from difficulty configuration
		switch (difficultyConfiguration)
		{
			case Difficulty.Medium:
				appleForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceCalculation);
				break;
			case Difficulty.Hard:
				appleForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceCalculation);
				appleForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceCalculation);
				break;
		}
	}

	#region Apple Motion
	private void InitializeAppleForceDiagramMotionTypeQueue()
	{
		appleForceDiagramMotionTypeQueue = new Queue<ForceObjectMotionType>();

		appleForceDiagramMotionTypeQueue.Enqueue(ForceObjectMotionType.Apple_OnBranch);
		appleForceDiagramMotionTypeQueue.Enqueue(ForceObjectMotionType.Apple_Falling);
	}

	private ForceData GenerateNewForceGivenData(ForceSubActivitySO forceSO)
	{
		ForceData forceData = new ForceData();

		forceData.acceleration = (float) Math.Round(Random.Range(forceSO.accelerationMinVal, forceSO.accelerationMaxVal), 3);
		forceData.mass = (float) Math.Round(Random.Range(forceSO.massMinVal, forceSO.massMaxVal), 3);

		return forceData;
	}

	private void CheckAppleForceAnswer(float? answer)
	{
		bool result = ActivityFiveUtilities.ValidateForceSubmission(answer, appleForceGivenData);
		// add metrics alter
		DisplayForceSubmissionResults(result, appleForceSubmissionStatusDisplay);
	}

	private void CheckAppleForceTypeAnswers(ForceTypeAnswerSubmission answer)
	{
		ForceTypeAnswerSubmissionResults results = ActivityFiveUtilities.ValidateForceTypeSubmission(appleForceDiagramMotionTypeQueue.Peek(), answer);
		// add metrics alter
		DisplayForceTypeSubmissionResults(answer, results, appleForceDiagramSubmissionStatusDisplay);
	}

	private void DisplayForceTypeSubmissionResults(ForceTypeAnswerSubmission answer, ForceTypeAnswerSubmissionResults results, ForceDiagramSubmissionStatusDisplay forceTypeSubmissionStatusDisplay)
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

	private void UpdateAppleForceDiagramStateQueue()
	{
		appleForceDiagramMotionTypeQueue.Dequeue();
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
}