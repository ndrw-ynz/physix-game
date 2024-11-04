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

public class ActivityFiveManager : ActivityManager
{
	public static Difficulty difficultyConfiguration;

	[Header("Level Data - Force")]
	[SerializeField] private ForceSubActivitySO forceLevelOne;
	[SerializeField] private ForceSubActivitySO forceLevelTwo;
	[SerializeField] private ForceSubActivitySO forceLevelThree;
	private ForceSubActivitySO currentForceLevel;

	[Header("Views")]
	[SerializeField] private ForceMotionView appleMotionView;
	[SerializeField] private ForceMotionView rockMotionView;
	[SerializeField] private ForceMotionView boatMotionView;
	[SerializeField] private ActivityFivePerformanceView performanceView;

	[Header("Submission Status Displays")]
	[Header("Apple Force Submission Status Displays")]
	[SerializeField] private ForceSubmissionStatusDisplay appleForceSubmissionStatusDisplay;
	[SerializeField] private ForceDiagramSubmissionStatusDisplay appleForceDiagramSubmissionStatusDisplay;
	[Header("Rock Force Submission Status Displays")]
	[SerializeField] private ForceSubmissionStatusDisplay rockForceSubmissionStatusDisplay;
	[SerializeField] private ForceDiagramSubmissionStatusDisplay rockForceDiagramSubmissionStatusDisplay;
	[Header("Rock Force Submission Status Displays")]
	[SerializeField] private ForceSubmissionStatusDisplay boatForceSubmissionStatusDisplay;
	[SerializeField] private ForceDiagramSubmissionStatusDisplay boatForceDiagramSubmissionStatusDisplay;

	// queue for apple motion
	private ForceMotionSubActivityStateMachine appleForceMotionSubActivityStateMachine;
	private Queue<ActivityFiveSubActivityState> appleForceMotionSubActivityStateQueue;
	// queue for rock motion
	private ForceMotionSubActivityStateMachine rockForceMotionSubActivityStateMachine;
	private Queue<ActivityFiveSubActivityState> rockForceMotionSubActivityStateQueue;
	// queue for boat motion
	private ForceMotionSubActivityStateMachine boatForceMotionSubActivityStateMachine;
	private Queue<ActivityFiveSubActivityState> boatForceMotionSubActivityStateQueue;

	// given data - force apple
	private ForceData appleForceGivenData;
	private Queue<ForceObjectMotionType> appleForceDiagramStateQueue;
	// given data - force rock
	private ForceData rockForceGivenData;
	private Queue<ForceObjectMotionType> rockForceDiagramStateQueue;
	// given data - force rock
	private ForceData boatForceGivenData;
	private Queue<ForceObjectMotionType> boatForceDiagramStateQueue;

	// Variables for tracking which view is currently active
	bool isAppleMotionViewActive;
	bool isRockMotionViewActive;
	bool isBoatMotionViewActive;

	// Gameplay performance metrics variables
	// Apple Motion Sub Activity
	private float appleMotionGameplayDuartion;
	private bool isAppleMotionSubActivityFinished;
	private int numIncorrectAppleMotionForceDiagramSubmission;
	private int numCorrectAppleMotionForceDiagramSubmission;
	private int numIncorrectAppleMotionForceSubmission;
	private int numCorrectAppleMotionForceSubmission;
	// Rock Motion Sub Activity
	private float rockMotionGameplayDuartion;
	private bool isRockMotionSubActivityFinished;
	private int numIncorrectRockMotionForceDiagramSubmission;
	private int numCorrectRockMotionForceDiagramSubmission;
	private int numIncorrectRockMotionForceSubmission;
	private int numCorrectRockMotionForceSubmission;
	// Boat Motion Sub Activity
	private float boatMotionGameplayDuartion;
	private bool isBoatMotionSubActivityFinished;
	private int numIncorrectBoatMotionForceDiagramSubmission;
	private int numCorrectBoatMotionForceDiagramSubmission;
	private int numIncorrectBoatMotionForceSubmission;
	private int numCorrectBoatMotionForceSubmission;

	protected override void Start()
	{
		base.Start();

		ConfigureLevelData(difficultyConfiguration);

		SubscribeForceMotionEvents();
	
		// Initialize sub activity state queues
		InitializeSubActivityStateQueues();

		// Initialize force diagram motion type queues
		InitializeForceDiagramStateQueues();

		// Initialize values for force motion sub activity state machines
		appleForceMotionSubActivityStateMachine = new ForceMotionSubActivityStateMachine(appleMotionView);
		appleForceMotionSubActivityStateMachine.Initialize(appleForceMotionSubActivityStateQueue.Peek());
		rockForceMotionSubActivityStateMachine = new ForceMotionSubActivityStateMachine(rockMotionView);
		rockForceMotionSubActivityStateMachine.Initialize(rockForceMotionSubActivityStateQueue.Peek());
		boatForceMotionSubActivityStateMachine = new ForceMotionSubActivityStateMachine(boatMotionView);
		boatForceMotionSubActivityStateMachine.Initialize(boatForceMotionSubActivityStateQueue.Peek());

		// Update state machines
		UpdateSubActivityStateMachine(
			appleForceMotionSubActivityStateMachine,
			appleForceMotionSubActivityStateQueue,
			appleMotionView,
			ref appleForceGivenData,
			ref isAppleMotionSubActivityFinished
			);
		UpdateSubActivityStateMachine(
			rockForceMotionSubActivityStateMachine,
			rockForceMotionSubActivityStateQueue,
			rockMotionView,
			ref rockForceGivenData,
			ref isRockMotionSubActivityFinished
			);
		UpdateSubActivityStateMachine(
			boatForceMotionSubActivityStateMachine,
			boatForceMotionSubActivityStateQueue,
			boatMotionView,
			ref boatForceGivenData,
			ref isBoatMotionSubActivityFinished
			);
	}

	private void Update()
	{
		if (isAppleMotionViewActive && !isAppleMotionSubActivityFinished) appleMotionGameplayDuartion += Time.deltaTime;
		if (isRockMotionViewActive && !isRockMotionSubActivityFinished) rockMotionGameplayDuartion += Time.deltaTime;
		if (isBoatMotionViewActive && !isBoatMotionSubActivityFinished) boatMotionGameplayDuartion += Time.deltaTime;
		if (isAppleMotionSubActivityFinished && isRockMotionSubActivityFinished && isBoatMotionSubActivityFinished) DisplayPerformanceView();
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

	private void SubscribeForceMotionEvents()
	{
		// Apple Force Motion related events
		appleMotionView.OpenViewEvent += () => isAppleMotionViewActive = true;
		appleMotionView.OpenViewEvent += () => UpdateSubActivityStateMachine(
			appleForceMotionSubActivityStateMachine,
			appleForceMotionSubActivityStateQueue,
			appleMotionView,
			ref appleForceGivenData,
			ref isAppleMotionSubActivityFinished
			);
		appleMotionView.QuitViewEvent += () => isAppleMotionViewActive = false;
		appleMotionView.SubmitForceAnswerEvent += (answer) => CheckForceAnswer(
			answer,
			appleForceGivenData,
			ref numIncorrectAppleMotionForceSubmission,
			ref numCorrectAppleMotionForceSubmission,
			appleForceSubmissionStatusDisplay
			);
		appleMotionView.SubmitForceDiagramAnswerEvent += (answer) => CheckForceDiagramAnswer(
			answer,
			appleForceDiagramStateQueue,
			ref numIncorrectAppleMotionForceDiagramSubmission,
			ref numCorrectAppleMotionForceDiagramSubmission,
			appleForceDiagramSubmissionStatusDisplay
			);
		appleForceSubmissionStatusDisplay.ProceedEvent += UpdateAppleSubActivityStateQueue;
		appleForceDiagramSubmissionStatusDisplay.ProceedEvent += UpdateAppleForceDiagramStateQueue;

		// Rock Force Motion related events
		rockMotionView.OpenViewEvent += () => isRockMotionViewActive = true;
		rockMotionView.OpenViewEvent += () => UpdateSubActivityStateMachine(
			rockForceMotionSubActivityStateMachine,
			rockForceMotionSubActivityStateQueue,
			rockMotionView,
			ref rockForceGivenData,
			ref isRockMotionSubActivityFinished
			);
		rockMotionView.QuitViewEvent += () => isRockMotionViewActive = false;
		rockMotionView.SubmitForceAnswerEvent += (answer) => CheckForceAnswer(
			answer,
			rockForceGivenData,
			ref numIncorrectRockMotionForceSubmission,
			ref numCorrectRockMotionForceSubmission,
			rockForceSubmissionStatusDisplay
			);
		rockMotionView.SubmitForceDiagramAnswerEvent += (answer) => CheckForceDiagramAnswer(
			answer,
			rockForceDiagramStateQueue,
			ref numIncorrectRockMotionForceDiagramSubmission,
			ref numCorrectRockMotionForceDiagramSubmission,
			rockForceDiagramSubmissionStatusDisplay
			);
		rockForceSubmissionStatusDisplay.ProceedEvent += UpdateRockSubActivityStateQueue;
		rockForceDiagramSubmissionStatusDisplay.ProceedEvent += UpdateRockForceDiagramStateQueue;

		// Boat Force Motion related events
		boatMotionView.OpenViewEvent += () => isBoatMotionViewActive = true;
		boatMotionView.OpenViewEvent += () => UpdateSubActivityStateMachine(
			boatForceMotionSubActivityStateMachine,
			boatForceMotionSubActivityStateQueue,
			boatMotionView,
			ref boatForceGivenData,
			ref isBoatMotionSubActivityFinished
			);
		boatMotionView.QuitViewEvent += () => isBoatMotionViewActive = false;
		boatMotionView.SubmitForceAnswerEvent += (answer) => CheckForceAnswer(
			answer,
			boatForceGivenData,
			ref numIncorrectBoatMotionForceSubmission,
			ref numCorrectBoatMotionForceSubmission,
			boatForceSubmissionStatusDisplay
			);
		boatMotionView.SubmitForceDiagramAnswerEvent += (answer) => CheckForceDiagramAnswer(
			answer,
			boatForceDiagramStateQueue,
			ref numIncorrectBoatMotionForceDiagramSubmission,
			ref numCorrectBoatMotionForceDiagramSubmission,
			boatForceDiagramSubmissionStatusDisplay
			);
		boatForceSubmissionStatusDisplay.ProceedEvent += UpdateBoatSubActivityStateQueue;
		boatForceDiagramSubmissionStatusDisplay.ProceedEvent += UpdateBoatForceDiagramStateQueue;
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

		boatForceMotionSubActivityStateQueue = new Queue <ActivityFiveSubActivityState>();
		boatForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceDiagram);
		boatForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceDiagram);
		boatForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceCalculation);

		// Enqueue additional force calculations based from difficulty configuration
		switch (difficultyConfiguration)
		{
			case Difficulty.Medium:
				appleForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceCalculation);

				rockForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceCalculation);
				rockForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceCalculation);

				boatForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceDiagram);
				boatForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceCalculation);
				break;
			case Difficulty.Hard:
				appleForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceCalculation);
				appleForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceCalculation);

				rockForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceCalculation);
				rockForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceCalculation);
				rockForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceCalculation);
				rockForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceCalculation);

				boatForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceDiagram);
				boatForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceCalculation);
				boatForceMotionSubActivityStateQueue.Enqueue(ActivityFiveSubActivityState.SolveForceCalculation);
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

		// Initialize queue for boat force
		boatForceDiagramStateQueue = new Queue<ForceObjectMotionType>();

		boatForceDiagramStateQueue.Enqueue(ForceObjectMotionType.Boat_Stationary);
		boatForceDiagramStateQueue.Enqueue(ForceObjectMotionType.Boat_MovingRight);
		// Add additional force diagram if difficulty is medium/hard.
		switch (difficultyConfiguration)
		{
			case Difficulty.Medium:
			case Difficulty.Hard:
				boatForceDiagramStateQueue.Enqueue(ForceObjectMotionType.Boat_MovingLeft);
				break;
		}
	}

	private ForceData GenerateNewForceGivenData(ForceSubActivitySO forceSO)
	{
		ForceData forceData = new ForceData();

		forceData.acceleration = (float) Math.Round(Random.Range(forceSO.accelerationMinVal, forceSO.accelerationMaxVal), 2);
		forceData.mass = (float) Math.Round(Random.Range(forceSO.massMinVal, forceSO.massMaxVal), 2);

		return forceData;
	}

	private void CheckForceAnswer(float? answer, ForceData forceGivenData, ref int numIncorrectForceSubmissionRef, ref int numCorrectForceSubmissionRef, ForceSubmissionStatusDisplay forceSubmissionStatusDisplay)
	{
		bool result = ActivityFiveUtilities.ValidateForceSubmission(answer, forceGivenData);

		// Update force answer gameplay metric
		if (!result)
		{
			numIncorrectForceSubmissionRef++;
		} else
		{
			numCorrectForceSubmissionRef++;
		}

		DisplayForceSubmissionResults(result, forceSubmissionStatusDisplay);
	}

	private void CheckForceDiagramAnswer(ForceDiagramAnswerSubmission answer, Queue<ForceObjectMotionType> forceDiagramStateQueue, ref int numIncorrectForceDiagramSubmissionRef, ref int numCorrectForceDiagramSubmissionRef, ForceDiagramSubmissionStatusDisplay forceDiagramSubmissionStatusDisplay)
	{
		ForceDiagramAnswerSubmissionResults results = ActivityFiveUtilities.ValidateForceDiagramSubmission(forceDiagramStateQueue.Peek(), answer);

		// Update force diagram answer gameplay metric
		if (!results.isAllCorrect()) {
			numIncorrectForceDiagramSubmissionRef++;
		} else
		{
			numCorrectForceDiagramSubmissionRef++;
		}

		DisplayForceDiagramSubmissionResults(answer, results, forceDiagramSubmissionStatusDisplay);
	}

	private void DisplayForceDiagramSubmissionResults(ForceDiagramAnswerSubmission answer, ForceDiagramAnswerSubmissionResults results, ForceDiagramSubmissionStatusDisplay forceDiagramSubmissionStatusDisplay)
	{
		if (results.isAllCorrect())
		{
			forceDiagramSubmissionStatusDisplay.SetSubmissionStatus(true, "The submitted force diagram is correct.");
		}
		else
		{
			forceDiagramSubmissionStatusDisplay.SetSubmissionStatus(false, "There are errors in your submission. Please review and fix it.");
		}

		forceDiagramSubmissionStatusDisplay.UpdateForceDiagramDisplay(answer, results);

		forceDiagramSubmissionStatusDisplay.gameObject.SetActive(true);
	}

	private void DisplayForceSubmissionResults(bool isCorrect, SubmissionStatusDisplay submissionStatusDisplay)
	{
		if (isCorrect)
		{
			submissionStatusDisplay.SetSubmissionStatus(true, "The submitted answer is correct.");
		} else
		{
			submissionStatusDisplay.SetSubmissionStatus(false, "There are errors in your submission. Please review and fix it.");
		}

		submissionStatusDisplay.gameObject.SetActive(true);
	}

	private void UpdateSubActivityStateMachine(
		ForceMotionSubActivityStateMachine forceSubActivityStateMachine,
		Queue<ActivityFiveSubActivityState> subactivityStateQueue,
		ForceMotionView forceMotionView,
		ref ForceData forceGivenData,
		ref bool isSubActivityFinished
		)
	{
		if (subactivityStateQueue.Count == 0)
		{
			forceSubActivityStateMachine.TransitionToState(ActivityFiveSubActivityState.None);
			isSubActivityFinished = true;
		}
		else
		{
			ActivityFiveSubActivityState queueSubActivityHead = subactivityStateQueue.Peek();

			// Do manager handling stuff
			switch (queueSubActivityHead)
			{
				case ActivityFiveSubActivityState.SolveForceDiagram:
					forceMotionView.ResetForceDiagram();
					break;
				case ActivityFiveSubActivityState.SolveForceCalculation:
					forceGivenData = GenerateNewForceGivenData(currentForceLevel);
					forceMotionView.SetupForceCalculationDisplay(forceGivenData);
					break;
			}

			forceSubActivityStateMachine.TransitionToState(queueSubActivityHead);
		}
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
		UpdateSubActivityStateMachine(
			appleForceMotionSubActivityStateMachine,
			appleForceMotionSubActivityStateQueue,
			appleMotionView,
			ref appleForceGivenData,
			ref isAppleMotionSubActivityFinished
			);
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
		UpdateSubActivityStateMachine(
			rockForceMotionSubActivityStateMachine,
			rockForceMotionSubActivityStateQueue,
			rockMotionView,
			ref rockForceGivenData,
			ref isRockMotionSubActivityFinished
			);
	}
	#endregion

	#region Boat Motion
	private void UpdateBoatForceDiagramStateQueue()
	{
		boatForceDiagramStateQueue.Dequeue();
		UpdateBoatSubActivityStateQueue();
	}

	private void UpdateBoatSubActivityStateQueue()
	{
		boatForceMotionSubActivityStateQueue.Dequeue();
		UpdateSubActivityStateMachine(
			boatForceMotionSubActivityStateMachine,
			boatForceMotionSubActivityStateQueue,
			boatMotionView,
			ref boatForceGivenData,
			ref isBoatMotionSubActivityFinished
			);
	}
	#endregion

	public override void DisplayPerformanceView()
	{
		inputReader.SetUI();
		performanceView.gameObject.SetActive(true);

		performanceView.SetTotalTimeDisplay(appleMotionGameplayDuartion + rockMotionGameplayDuartion + boatMotionGameplayDuartion);

		performanceView.SetAppleMotionMetricsDisplay(
			isAccomplished: isAppleMotionSubActivityFinished,
			numIncorrectForceSubmission: numIncorrectAppleMotionForceSubmission,
			numIncorrectForceDiagramSubmission: numIncorrectAppleMotionForceDiagramSubmission,
			duration: appleMotionGameplayDuartion
			);

		performanceView.SetRockMotionMetricsDisplay(
			isAccomplished: isRockMotionSubActivityFinished,
			numIncorrectForceSubmission: numIncorrectRockMotionForceSubmission,
			numIncorrectForceDiagramSubmission: numIncorrectRockMotionForceDiagramSubmission,
			duration: rockMotionGameplayDuartion
			);

		performanceView.SetBoatMotionMetricsDisplay(
			isAccomplished: isBoatMotionSubActivityFinished,
			numIncorrectForceSubmission: numIncorrectBoatMotionForceSubmission,
			numIncorrectForceDiagramSubmission: numIncorrectBoatMotionForceDiagramSubmission,
			duration: boatMotionGameplayDuartion
			);

		// Update its activity feedback display (six args)
		performanceView.UpdateActivityFeedbackDisplay(
			new SubActivityPerformanceMetric(
				subActivityName: "AppleForceDiagram",
				isSubActivityFinished: isAppleMotionSubActivityFinished,
				numIncorrectAnswers: numIncorrectAppleMotionForceDiagramSubmission,
				numCorrectAnswers: numCorrectAppleMotionForceDiagramSubmission,
				badScoreThreshold: 5,
				averageScoreThreshold: 3
				),
			new SubActivityPerformanceMetric(
				subActivityName: "AppleForceCalculation",
				isSubActivityFinished: isAppleMotionSubActivityFinished,
				numIncorrectAnswers: numIncorrectAppleMotionForceSubmission,
				numCorrectAnswers: numCorrectAppleMotionForceSubmission,
				badScoreThreshold: 3,
				averageScoreThreshold: 2
				),
			new SubActivityPerformanceMetric(
				subActivityName: "RockForceDiagram",
				isSubActivityFinished: isRockMotionSubActivityFinished,
				numIncorrectAnswers: numIncorrectRockMotionForceDiagramSubmission,
				numCorrectAnswers: numCorrectRockMotionForceDiagramSubmission,
				badScoreThreshold: 5,
				averageScoreThreshold: 3
				),
			new SubActivityPerformanceMetric(
				subActivityName: "RockForceCalculation",
				isSubActivityFinished: isRockMotionSubActivityFinished,
				numIncorrectAnswers: numIncorrectRockMotionForceSubmission,
				numCorrectAnswers: numCorrectRockMotionForceSubmission,
				badScoreThreshold: 3,
				averageScoreThreshold: 2
				),
			new SubActivityPerformanceMetric(
				subActivityName: "BoatForceDiagram",
				isSubActivityFinished: isBoatMotionSubActivityFinished,
				numIncorrectAnswers: numIncorrectBoatMotionForceDiagramSubmission,
				numCorrectAnswers: numCorrectBoatMotionForceDiagramSubmission,
				badScoreThreshold: 5,
				averageScoreThreshold: 3
				),
			new SubActivityPerformanceMetric(
				subActivityName: "BoatForceCalculation",
				isSubActivityFinished: isBoatMotionSubActivityFinished,
				numIncorrectAnswers: numIncorrectBoatMotionForceSubmission,
				numCorrectAnswers: numCorrectBoatMotionForceSubmission,
				badScoreThreshold: 3,
				averageScoreThreshold: 2
				)
			);
	}

	protected override void HandleGameplayPause()
	{
		base.HandleGameplayPause();
		// Update content of activity pause menu UI
		List<string> taskText = new List<string>();
		if (!isAppleMotionSubActivityFinished)
		{
			taskText.Add("- Find and investigate the apple from the region of Nakalais.");
			taskText.Add("	- Investigate the forces acting on the apple.");
			taskText.Add("	- Determine the value of forces acting on the apple.");
		}
		if (!isRockMotionSubActivityFinished)
		{
			taskText.Add("- Find and investigate the rock from the region of Nakalais.");
			taskText.Add("	- Investigate the forces acting on the rock.");
			taskText.Add("	- Determine the value of forces acting on the rock.");
		}
		if (!isBoatMotionSubActivityFinished)
		{
			taskText.Add("- Find and investigate the boat from the region of Nakalais.");
			taskText.Add("	- Investigate the forces acting on the boat.");
			taskText.Add("	- Determine the value of forces acting on the boat.");
		}

		List<string> objectiveText = new List<string>();
		objectiveText.Add("Conduct research around the environment of Nakalais. " +
			"Gather data about the acting forces and force values of different objects for research.");

		activityPauseMenuUI.UpdateContent("Lesson 5 - Activity 5", taskText, objectiveText);
	}
}