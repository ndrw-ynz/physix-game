using System;
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

	/*[SerializeField] private ViewProjectileMotion viewProjectileMotion;
	[SerializeField] private ViewCircularMotion viewCircularMotion;
	[SerializeField] private ViewActivityFourPerformance viewActivityFourPerformance;*/

	/*[Header("Problem Display Content")]
	[SerializeField] private TextMeshProUGUI problemText;*/

	/*[Header("Modal Window")]
	[SerializeField] private CalcSubmissionModalWindow submissionModalWindow;*/

	/*[Header("Given Values - Projectile Motion")]
	private int initialProjectileVelocityValue;
	private int projectileHeightValue;
	private int projectileAngleValue;

	[Header("Given Values - Circular Motion")]
	private int satelliteRadiusValue;
	private int satelliteTimePeriodValue;

	[Header("Metrics - Projectile Motion")]
	private bool isMaximumHeightCalculationFinished;
	private bool isHorizontalRangeCalculationFinished;
	private bool isTimeOfFlightCalculationFinished;
	private int numIncorrectMaximumHeightSubmission;
	private int numIncorrectHorizontalRangeSubmission;
	private int numIncorrectTimeOfFlightSubmission;

	[Header("Metrics - Circular Motion")]
	private bool isCentripetalAccelerationCalculationFinished;
	private int numIncorrectCentripetalAccelerationSubmission;*/

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
		missionObjectiveDisplayUI.UpdateMissionObjectiveText(0, $"Calculate the Projectile Motion of the Satellite ({currentProjectileMotionLevel.numberOfTests - currentNumProjectileMotionTests}/{currentProjectileMotionLevel.numberOfTests})");
		missionObjectiveDisplayUI.UpdateMissionObjectiveText(1, $"Calculate the Centripetal Acceleration of the Satellite ({currentCircularMotionLevel.numberOfTests - currentNumCircularMotionTests}/{currentCircularMotionLevel.numberOfTests})");
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
			missionObjectiveDisplayUI.UpdateMissionObjectiveText(0, $"Calculate the Projectile Motion of the Satellite ({currentProjectileMotionLevel.numberOfTests - currentNumProjectileMotionTests}/{currentProjectileMotionLevel.numberOfTests})");
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
			missionObjectiveDisplayUI.UpdateMissionObjectiveText(1, $"Calculate the Centripetal Acceleration of the Satellite ({currentCircularMotionLevel.numberOfTests - currentNumCircularMotionTests}/{currentCircularMotionLevel.numberOfTests})");
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
	#endregion

	/*#region Projectile Motion

	private void InitializeProjectileMotionGiven(ProjectileMotionSubActivitySO projectileMotionSO)
	{
		initialProjectileVelocityValue = Random.Range(projectileMotionSO.minimumVelocityValue, projectileMotionSO.maximumVelocityValue);
		projectileHeightValue = Random.Range(projectileMotionSO.minimumHeightValue, projectileMotionSO.maximumHeightValue);

		switch (projectileMotionSO.projectileAngleType)
		{
			case ProjectileAngleType.Standard90Angle:
				int[] standard90AngleValues = new int[] { 30, 45, 60, 90};
				projectileAngleValue = standard90AngleValues[Random.Range(0, standard90AngleValues.Length)];
				break;
			case ProjectileAngleType.Full90Angle:
				projectileAngleValue = Random.Range(1, 90);
				break;
		}
	}

	private void CheckMaximumHeightAnswer(float maximumHeightAnswer)
	{
		bool isMaximumHeightCorrect = ActivityFourUtilities.ValidateMaximumHeightSubmission(maximumHeightAnswer, initialProjectileVelocityValue, projectileAngleValue);
		if (isMaximumHeightCorrect)
		{
			isMaximumHeightCalculationFinished = true;

			problemText.text = "What is the horizontal range of the projectile?";
		}
		else
		{
			numIncorrectMaximumHeightSubmission++;
		}

		viewProjectileMotion.SetOverlays(true);
		viewProjectileMotion.ResetState();
		submissionModalWindow.gameObject.SetActive(true);
		submissionModalWindow.SetDisplayFromSubmissionResult(isMaximumHeightCalculationFinished, "Maximum Height");
	}

	private void CheckHorizontalRangeAnswer(float horizontalRangeAnswer)
	{
		bool isHorizontalRangeCorrect = ActivityFourUtilities.ValidateHorizontalRangeSubmission(horizontalRangeAnswer, initialProjectileVelocityValue);
		if (isHorizontalRangeCorrect)
		{
			isHorizontalRangeCalculationFinished = true;

			problemText.text = "What is the time of flight of the projectile?";
		}
		else
		{
			numIncorrectHorizontalRangeSubmission++;
		}

		viewProjectileMotion.SetOverlays(true);
		viewProjectileMotion.ResetState();
		submissionModalWindow.gameObject.SetActive(true);
		submissionModalWindow.SetDisplayFromSubmissionResult(isHorizontalRangeCalculationFinished, "Horizontal Range");
	}

	private void CheckTimeOfFlightAnswer(float timeOfFlightAnswer)
	{
		bool isTimeOfFlightCorrect = ActivityFourUtilities.ValidateTimeOfFlightSubmission(timeOfFlightAnswer, initialProjectileVelocityValue, projectileAngleValue);
		if (isTimeOfFlightCorrect)
		{
			isTimeOfFlightCalculationFinished = true;

			viewProjectileMotion.gameObject.SetActive(false);
			viewCircularMotion.gameObject.SetActive(true);

			viewCircularMotion.SetupCentripetalAccelerationProblemDisplay(satelliteRadiusValue, satelliteTimePeriodValue);
		}
		else
		{
			numIncorrectTimeOfFlightSubmission++;
		}

		viewProjectileMotion.SetOverlays(true);
		viewProjectileMotion.ResetState();
		submissionModalWindow.gameObject.SetActive(true);
		submissionModalWindow.SetDisplayFromSubmissionResult(isTimeOfFlightCalculationFinished, "Time of Flight");
	}

	#endregion

	#region Circular Motion

	private void InitializeCircularMotionGiven(CircularMotionSubActivitySO circularMotionSO)
	{
		satelliteRadiusValue = Random.Range(circularMotionSO.minimumRadiusValue, circularMotionSO.maximumRadiusValue);
		satelliteTimePeriodValue = Random.Range(circularMotionSO.minimumTimePeriodValue, circularMotionSO.maximumTimePeriodValue);
	}

	private void CheckCentripetalAccelerationAnswer(float centripetalAccelerationAnswer)
	{
		bool isCentripetalAccelerationCorrect = ActivityFourUtilities.ValidateCentripetalAccelerationSubmission(centripetalAccelerationAnswer, satelliteRadiusValue, satelliteTimePeriodValue);
		if (isCentripetalAccelerationCorrect)
		{
			isCentripetalAccelerationCalculationFinished = true;
		} else
		{
			numIncorrectCentripetalAccelerationSubmission++;
		}

		viewCircularMotion.SetOverlays(true);
		viewCircularMotion.ResetState();
		submissionModalWindow.gameObject.SetActive(true);
		submissionModalWindow.SetDisplayFromSubmissionResult(isCentripetalAccelerationCalculationFinished, "Centripetal Acceleration");
	}

	#endregion

	private void OnOpenViewActivityFourerformance(ViewActivityFourPerformance view)
	{
		view.MaximumHeightProblemStatusText.text += isMaximumHeightCalculationFinished ? "Accomplished" : "Not accomplished";
		view.MaximumHeightProblemIncorrectNumText.text += numIncorrectMaximumHeightSubmission;

		view.HorizontalRangeProblemStatusText.text += isHorizontalRangeCalculationFinished ? "Accomplished" : "Not accomplished";
		view.HorizontalRangeProblemIncorrectNumText.text += numIncorrectHorizontalRangeSubmission;

		view.TimeOfFlightProblemStatusText.text += isTimeOfFlightCalculationFinished ? "Accomplished" : "Not accomplished";
		view.TimeOfFlightProblemIncorrectNumText.text += numIncorrectTimeOfFlightSubmission;

		view.CentripetalAccelerationProblemStatusText.text += isCentripetalAccelerationCalculationFinished ? "Accomplished" : "Not accomplished";
		view.CentripetalAccelerationProblemIncorrectNumText.text += numIncorrectCentripetalAccelerationSubmission;
	}

	private void RestoreViewState()
	{
		submissionModalWindow.gameObject.SetActive(false);
		viewProjectileMotion.SetOverlays(false);
		viewCircularMotion.SetOverlays(false);
	}

	private void UpdateViewState()
	{
		if (isCentripetalAccelerationCalculationFinished)
		{
			viewActivityFourPerformance.gameObject.SetActive(true);
		} else if (isMaximumHeightCalculationFinished && isHorizontalRangeCalculationFinished && !isTimeOfFlightCalculationFinished)
		{
			// All solved except time of flight
			viewProjectileMotion.SetSubmissionButtonStates(false, false, true);
		} else if (isMaximumHeightCalculationFinished && !isHorizontalRangeCalculationFinished)
		{
			// Maximum height solved but not horizontal range
			viewProjectileMotion.SetSubmissionButtonStates(false, true, false);
		}

		RestoreViewState();
	}*/
}