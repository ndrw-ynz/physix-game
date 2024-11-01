using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileMotionCalculationData
{
	public float initialVelocity;
	public float initialHeight;
	public float angleMeasure;
}

public class CircularMotionCalculationData
{
	public float radius;
	public float period;
}

public class ActivityFourManager : ActivityManager
{
	public static Difficulty difficultyConfiguration;

	public event Action ProjectileMotionTerminalClearEvent;

	[Header("Level Data - Projectile Motion")]
    [SerializeField] private ProjectileMotionSubActivitySO projectileMotionLevelOne;
	[SerializeField] private ProjectileMotionSubActivitySO projectileMotionLevelTwo;
	[SerializeField] private ProjectileMotionSubActivitySO projectileMotionLevelThree;
	private ProjectileMotionSubActivitySO currentProjectileMotionLevel;

	[Header("Level Data - Circular Motion")]
	[SerializeField] private CircularMotionSubActivitySO circularMotionLevelOne;
	[SerializeField] private CircularMotionSubActivitySO circularMotionLevelTwo;
	[SerializeField] private CircularMotionSubActivitySO circularMotionLevelThree;
	private CircularMotionSubActivitySO currentCircularMotionLevel;

	[Header("Views")]
	[SerializeField] private ProjectileMotionView projectileMotionView;
	[SerializeField] private CircularMotionView circularMotionView;
	[SerializeField] private ActivityFourPerformanceView performanceView;

	[Header("Submission Status Displays")]
	[SerializeField] private ProjectileMotionSubmissionStatusDisplay projectileMotionSubmissionStatusDisplay;
	[SerializeField] private CircularMotionSubmissionStatusDisplay circularMotionSubmissionStatusDisplay;

	// Variables for keeping track of current number of tests
	private int currentNumProjectileMotionTests;
	private int currentNumCircularMotionTests;

	// Given projectile motion calculation data
	private ProjectileMotionCalculationData givenProjectileMotionData;
	private CircularMotionCalculationData givenCircularMotionData;

	// Variables for tracking which view is currently active
	private bool isProjectileMotionViewActive;
	private bool isCircularMotionViewActive;

	// Gameplay performance metrics variables
	// Projectile Motion Sub Activity
	private float projectileMotionGameplayDuration;
	private bool isProjectileMotionSubActivityFinished;
	private int numIncorrectProjectileMotionSubmission;
	private int numCorrectProjectileMotionSubmission;
	// Circular Motion Sub Activity
	private float circularMotionGameplayDuration;
	private bool isCircularMotionSubActivityFinished;
	private int numIncorrectCircularMotionSubmission;
	private int numCorrectCircularMotionSubmission;

	protected override void Start()
	{
		base.Start();

		ConfigureLevelData(Difficulty.Easy);

		SubscribeViewAndDisplayEvents();

		// Initialize correct given values
		GenerateProjectileGivenData();
		GenerateCircularMotionGivenData();

		// Determine number of tests
		currentNumProjectileMotionTests = currentProjectileMotionLevel.numberOfTests;
		currentNumCircularMotionTests = currentCircularMotionLevel.numberOfTests;

		// Setup views
		// Projectile motion view
		projectileMotionView.UpdateTestCountTextDisplay(currentProjectileMotionLevel.numberOfTests - currentNumProjectileMotionTests, currentProjectileMotionLevel.numberOfTests);
		projectileMotionView.SetupProjectileMotionView(givenProjectileMotionData);
		// Circular motion view
		circularMotionView.UpdateTestCountTextDisplay(currentCircularMotionLevel.numberOfTests - currentNumCircularMotionTests, currentCircularMotionLevel.numberOfTests);
		circularMotionView.SetupCircularMotionView(givenCircularMotionData);

		// Update mission objective display
		missionObjectiveDisplayUI.UpdateMissionObjectiveText(0, $"Calculate the Projectile Motion of the Satellite in the Projectile Motion Terminal ({currentProjectileMotionLevel.numberOfTests - currentNumProjectileMotionTests}/{currentProjectileMotionLevel.numberOfTests})");
		missionObjectiveDisplayUI.UpdateMissionObjectiveText(1, $"Calculate the Centripetal Acceleration of the Satellite in the Circular Motion Terminal ({currentCircularMotionLevel.numberOfTests - currentNumCircularMotionTests}/{currentCircularMotionLevel.numberOfTests})");
	}

	private void Update()
	{
		if (isProjectileMotionViewActive && !isProjectileMotionSubActivityFinished) projectileMotionGameplayDuration += Time.deltaTime;
		if (isCircularMotionViewActive && !isCircularMotionSubActivityFinished) circularMotionGameplayDuration += Time.deltaTime;
		if (isProjectileMotionSubActivityFinished && isCircularMotionSubActivityFinished) DisplayPerformanceView();
	}

	private void ConfigureLevelData(Difficulty difficulty)
	{
		difficultyConfiguration = difficulty;

		switch (difficulty)
		{
			case Difficulty.Easy:
				currentProjectileMotionLevel = projectileMotionLevelOne;
				currentCircularMotionLevel = circularMotionLevelOne;
				break;
			case Difficulty.Medium:
				currentProjectileMotionLevel = projectileMotionLevelTwo;
				currentCircularMotionLevel = circularMotionLevelTwo;
				break;
			case Difficulty.Hard:
				currentProjectileMotionLevel = projectileMotionLevelThree;
				currentCircularMotionLevel = circularMotionLevelThree;
				break;
		}
	}

	private void SubscribeViewAndDisplayEvents()
	{
		// Projectile Motion Sub Activity Related Events
		projectileMotionView.OpenViewEvent += () => isProjectileMotionViewActive = true;
		projectileMotionView.QuitViewEvent += () => isProjectileMotionViewActive = false;
		projectileMotionView.SubmitAnswerEvent += CheckProjectileMotionAnswer;
		projectileMotionSubmissionStatusDisplay.ProceedEvent += UpdateProjectileMotionViewState;

		// Circular Motion Sub Activity Related Events
		circularMotionView.OpenViewEvent += () => isCircularMotionViewActive = true;
		circularMotionView.QuitViewEvent += () => isCircularMotionViewActive = false;
		circularMotionView.SubmitAnswerEvent += CheckCentripetalAccelerationAnswer;
		circularMotionSubmissionStatusDisplay.ProceedEvent += UpdateCircularMotionViewState;
	}

	#region Projectile Motion
	private void GenerateProjectileGivenData()
	{
		ProjectileMotionCalculationData data = new ProjectileMotionCalculationData();
		data.initialVelocity = Random.Range(currentProjectileMotionLevel.minimumVelocityValue, currentProjectileMotionLevel.maximumVelocityValue);
		data.initialHeight = Random.Range(currentProjectileMotionLevel.minimumHeightValue, currentProjectileMotionLevel.maximumHeightValue); 
		switch (currentProjectileMotionLevel.projectileAngleType)
		{
			case ProjectileAngleType.Standard90Angle:
				int[] standard90AngleValues = new int[] { 30, 45, 60, 90 };
				data.angleMeasure = standard90AngleValues[Random.Range(0, standard90AngleValues.Length)];
				break;
			case ProjectileAngleType.Full90Angle:
				data.angleMeasure = Random.Range(1, 90);
				break;
		}
		givenProjectileMotionData = data;
	}

	private void CheckProjectileMotionAnswer(ProjectileMotionAnswerSubmission answer)
	{
		ProjectileMotionSubmissionResults results = ActivityFourUtilities.ValidateProjectileMotionSubmission(answer, givenProjectileMotionData);

		if (results.isAllCorrect())
		{
			numCorrectProjectileMotionSubmission++;
			currentNumProjectileMotionTests--;
			projectileMotionView.UpdateTestCountTextDisplay(currentProjectileMotionLevel.numberOfTests - currentNumProjectileMotionTests, currentProjectileMotionLevel.numberOfTests);
		}
		else
		{
			numIncorrectProjectileMotionSubmission++;
		}

		DisplayProjectileMotionSubmissionResults(results);
	}

	private void DisplayProjectileMotionSubmissionResults(ProjectileMotionSubmissionResults results)
	{
		if (results.isAllCorrect())
		{
			projectileMotionSubmissionStatusDisplay.SetSubmissionStatus(true, "The system has precisely calibrated the satellite's trajectory. Amazing work!");
			missionObjectiveDisplayUI.UpdateMissionObjectiveText(0, $"Calculate the Projectile Motion of the Satellite in the Projectile Motion Terminal ({currentProjectileMotionLevel.numberOfTests - currentNumProjectileMotionTests}/{currentProjectileMotionLevel.numberOfTests})");
		}
		else
		{
			projectileMotionSubmissionStatusDisplay.SetSubmissionStatus(false, "Doctor, it seems there's a misstep in your calculations. Let's take another look!");
		}

		projectileMotionSubmissionStatusDisplay.UpdateStatusBorderDisplaysFromResults(results);

		projectileMotionSubmissionStatusDisplay.gameObject.SetActive(true);
	}

	private void UpdateProjectileMotionViewState()
	{
		if (currentNumProjectileMotionTests > 0)
		{
			GenerateProjectileGivenData();
			projectileMotionView.SetupProjectileMotionView(givenProjectileMotionData);
		}
		else
		{
			isProjectileMotionSubActivityFinished = true;
			projectileMotionView.gameObject.SetActive(false);
			missionObjectiveDisplayUI.ClearMissionObjective(0);
			ProjectileMotionTerminalClearEvent?.Invoke();
		}
	}
	#endregion

	#region Circular Motion
	private void GenerateCircularMotionGivenData()
	{
		CircularMotionCalculationData data = new CircularMotionCalculationData();
		data.radius = Random.Range(currentCircularMotionLevel.minimumRadiusValue, currentCircularMotionLevel.maximumRadiusValue);
		data.period = Random.Range(currentCircularMotionLevel.minimumTimePeriodValue, currentCircularMotionLevel.maximumTimePeriodValue);
		givenCircularMotionData = data;
	}

	private void CheckCentripetalAccelerationAnswer(float? answer)
	{
		bool isCentripetalAccelerationCorrect = ActivityFourUtilities.ValidateCentripetalAccelerationSubmission(answer, givenCircularMotionData);

		if (isCentripetalAccelerationCorrect)
		{
			numCorrectCircularMotionSubmission++;
			currentNumCircularMotionTests--;
			circularMotionView.UpdateTestCountTextDisplay(currentCircularMotionLevel.numberOfTests - currentNumCircularMotionTests, currentCircularMotionLevel.numberOfTests);
		}
		else
		{
			numIncorrectCircularMotionSubmission++;
		}

		DisplayCircularMotionSubmissionResults(isCentripetalAccelerationCorrect);
	}

	private void DisplayCircularMotionSubmissionResults(bool result)
	{
		if (result)
		{
			circularMotionSubmissionStatusDisplay.SetSubmissionStatus(true, "The system has accurately calculated the satellite's trajectory data. Fantastic job!");
			missionObjectiveDisplayUI.UpdateMissionObjectiveText(1, $"Calculate the Centripetal Acceleration of the Satellite in the Circular Motion Terminal ({currentCircularMotionLevel.numberOfTests - currentNumCircularMotionTests}/{currentCircularMotionLevel.numberOfTests})");
		}
		else
		{
			circularMotionSubmissionStatusDisplay.SetSubmissionStatus(false, "Doctor, it seems there's a misstep in your calculations. Let's take another look!");
		}

		circularMotionSubmissionStatusDisplay.UpdateStatusBorderDisplayFromResult(result);

		circularMotionSubmissionStatusDisplay.gameObject.SetActive(true);
	}

	private void UpdateCircularMotionViewState()
	{
		if (currentNumCircularMotionTests > 0)
		{
			GenerateCircularMotionGivenData();
			circularMotionView.SetupCircularMotionView(givenCircularMotionData);
		}
		else
		{
			isCircularMotionSubActivityFinished = true;
			circularMotionView.gameObject.SetActive(false);
			missionObjectiveDisplayUI.ClearMissionObjective(1);
			//ProjectileMotionTerminalClearEvent?.Invoke();
		}
	}
	#endregion

	public override void DisplayPerformanceView()
	{
		inputReader.SetUI();
		performanceView.gameObject.SetActive(true);

		performanceView.SetTotalTimeDisplay(projectileMotionGameplayDuration + circularMotionGameplayDuration);

		performanceView.SetProjectileMotionMetricsDisplay(
			isAccomplished: isProjectileMotionSubActivityFinished,
			numIncorrectSubmission: numIncorrectProjectileMotionSubmission,
			duration: projectileMotionGameplayDuration
			);

		performanceView.SetCircularMotionMetricsDisplay(
			isAccomplished: isCircularMotionSubActivityFinished,
			numIncorrectSubmission: numIncorrectCircularMotionSubmission,
			duration: circularMotionGameplayDuration
			);

		// Update its activity feedback display (two args)
		performanceView.UpdateActivityFeedbackDisplay(
			new SubActivityPerformanceMetric(
				subActivityName: "projectile motion",
				isSubActivityFinished: isProjectileMotionSubActivityFinished,
				numIncorrectAnswers: numIncorrectProjectileMotionSubmission,
				numCorrectAnswers: numCorrectProjectileMotionSubmission,
				badScoreThreshold: 4,
				averageScoreThreshold: 2
				),
			new SubActivityPerformanceMetric(
				subActivityName: "circular motion",
				isSubActivityFinished: isCircularMotionSubActivityFinished,
				numIncorrectAnswers: numIncorrectCircularMotionSubmission,
				numCorrectAnswers: numCorrectCircularMotionSubmission,
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
		if (!isProjectileMotionSubActivityFinished)
		{
			taskText.Add($"-  Interact with the ship’s Projectile Motion Terminal to analyze the satellite’s projectile motion in Nakalai’s orbit and mitigate measurement errors to prevent a crash during launch.");
		}
		if (!isCircularMotionSubActivityFinished)
		{
			taskText.Add("-  Interact with the ship’s Circular Motion Terminal and determine the satellite’s centripetal acceleration to monitor its revolution around Nakalais.");
		}

		List<string> objectiveText = new List<string>();
		objectiveText.Add("Use Orbital 1’s projectile motion terminal and circular motion terminal to launch the satellite into Nakalai’s orbit successfully, ensuring it maintains a stable trajectory.");

		activityPauseMenuUI.UpdateContent("Lesson 4 - Activity 4", taskText, objectiveText);
	}
}