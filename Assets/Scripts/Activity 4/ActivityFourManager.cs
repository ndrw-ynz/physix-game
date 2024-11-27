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

		SceneSoundManager.Instance.PlayMusic("With love from Vertex Studio (16)");

		ConfigureLevelData(difficultyConfiguration);

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

		inputReader.SetGameplay();
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
			SceneSoundManager.Instance.PlaySFX("UI_Buttonconfirm_Stereo_01");

			numCorrectProjectileMotionSubmission++;
			currentNumProjectileMotionTests--;
			projectileMotionView.UpdateTestCountTextDisplay(currentProjectileMotionLevel.numberOfTests - currentNumProjectileMotionTests, currentProjectileMotionLevel.numberOfTests);
		}
		else
		{
			SceneSoundManager.Instance.PlaySFX("UI_Forbidden_Stereo_02");

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
			SceneSoundManager.Instance.PlaySFX("UI_Buttonconfirm_Stereo_01");

			numCorrectCircularMotionSubmission++;
			currentNumCircularMotionTests--;
			circularMotionView.UpdateTestCountTextDisplay(currentCircularMotionLevel.numberOfTests - currentNumCircularMotionTests, currentCircularMotionLevel.numberOfTests);
		}
		else
		{
			SceneSoundManager.Instance.PlaySFX("UI_Forbidden_Stereo_02");

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

	protected override void AddAttemptRecord()
	{
		Dictionary<string, object> projectileMotionResults = new Dictionary<string, object>
		{
			{ "fields", new Dictionary<string, FirestoreField>
				{
					{ "isAccomplished", new FirestoreField(isProjectileMotionSubActivityFinished) },
					{ "mistakes", new FirestoreField(numIncorrectProjectileMotionSubmission) },
					{ "durationInSec", new FirestoreField((int)projectileMotionGameplayDuration) }
				}
			}
		};

		Dictionary<string, object> circularMotionResults = new Dictionary<string, object>
		{
			{ "fields", new Dictionary<string, FirestoreField>
				{
					{ "isAccomplished", new FirestoreField(isCircularMotionSubActivityFinished) },
					{ "mistakes", new FirestoreField(numIncorrectCircularMotionSubmission) },
					{ "durationInSec", new FirestoreField((int)circularMotionGameplayDuration) }
				}
			}
		};

		Dictionary<string, object> results = new Dictionary<string, object>
		{
			{ "fields", new Dictionary<string, object>
				{
					{ "projectileMotion", new FirestoreField(projectileMotionResults)},
					{ "circularMotion", new FirestoreField(circularMotionResults)},
				}
			}
		};

		Dictionary<string, FirestoreField> fields = new Dictionary<string, FirestoreField>
		{
			{ "dateAttempted", new FirestoreField(DateTime.UtcNow) },
			{ "difficulty", new FirestoreField($"{difficultyConfiguration}") },
			{ "results", new FirestoreField(results) },
			{ "isAccomplished", new FirestoreField(isProjectileMotionSubActivityFinished && isCircularMotionSubActivityFinished) },
			{ "studentId", new FirestoreField(UserManager.Instance.CurrentUser.localId) },
			{ "totalDurationInSec", new FirestoreField((int)(projectileMotionGameplayDuration + circularMotionGameplayDuration)) }
		};

		StartCoroutine(UserManager.Instance.CreateAttemptDocument(fields, "activityFourAttempts"));
	}

    protected override void SetNextLevelButtonState()
    {
        bool isAccomplished = isProjectileMotionSubActivityFinished && isCircularMotionSubActivityFinished;

        int currentUserUnlockedLesson = (int)UserManager.Instance.UserUnlockedLevels.fields["highestUnlockedLesson"].integerValue;
        int currentUserHighestLessonUnlockedDifficulty = (int)UserManager.Instance.UserUnlockedLevels.fields["highestLessonUnlockedDifficulty"].integerValue;

        // If the player accomplished the whole activity in any difficulty, do a switch case
        if (isAccomplished)
        {
            string currentUserLocalID = UserManager.Instance.CurrentUser.localId;

            // If the player accomplished in easy mode, check if the unlocked levels need to progress with respect to the current lesson
            switch (difficultyConfiguration)
            {
                case Difficulty.Easy:
                    if (currentUserUnlockedLesson == 4 && currentUserHighestLessonUnlockedDifficulty == 1)
                    {
                        Dictionary<string, FirestoreField> fields = new Dictionary<string, FirestoreField>
                        {
                            {"highestUnlockedLesson", new FirestoreField(4) },
                            {"highestLessonUnlockedDifficulty", new FirestoreField(2) }
                        };

                        StartCoroutine(UserManager.Instance.UpdateUnlockedLevels(fields, currentUserLocalID, (success) =>
                        {
                            if (success)
                            {
                                newLevelUnlockedScreen.gameObject.SetActive(true);
                                StartCoroutine(newLevelUnlockedScreen.SetNewLevelUnlockedScreen("Lesson 4 - <color=#C5B501>Medium"));
                                StartCoroutine(UserManager.Instance.GetUnlockedLevels(currentUserLocalID, HandleUnlockedLevelsChange));
                            }
                            else { Debug.LogError("Failed To Update Unlocked Levels"); }
                        }));
                    }
                    break;

                // If the player accomplished in medium mode, check if the unlocked levels need to progress with respect to the current lesson
                case Difficulty.Medium:
                    if (currentUserUnlockedLesson == 4 && currentUserHighestLessonUnlockedDifficulty == 2)
                    {
                        Dictionary<string, FirestoreField> fields = new Dictionary<string, FirestoreField>
                        {
                            {"highestUnlockedLesson", new FirestoreField(4) },
                            {"highestLessonUnlockedDifficulty", new FirestoreField(3) }
                        };

                        StartCoroutine(UserManager.Instance.UpdateUnlockedLevels(fields, currentUserLocalID, (success) =>
                        {
                            if (success)
                            {
                                newLevelUnlockedScreen.gameObject.SetActive(true);
                                StartCoroutine(newLevelUnlockedScreen.SetNewLevelUnlockedScreen("Lesson 4 - <color=#FF0000>Hard"));
                                StartCoroutine(UserManager.Instance.GetUnlockedLevels(currentUserLocalID, HandleUnlockedLevelsChange));
                            }
                            else { Debug.LogError("Failed To Update Unlocked Levels"); }
                        }));
                    }
                    break;

                // If the player accomplished in hard mode, check if the unlocked levels need to progress with respect to the current lesson
                case Difficulty.Hard:
                    if (currentUserUnlockedLesson == 4 && currentUserHighestLessonUnlockedDifficulty == 3)
                    {
                        Dictionary<string, FirestoreField> fields = new Dictionary<string, FirestoreField>
                        {
                            {"highestUnlockedLesson", new FirestoreField(5) },
                            {"highestLessonUnlockedDifficulty", new FirestoreField(1) }
                        };

                        StartCoroutine(UserManager.Instance.UpdateUnlockedLevels(fields, currentUserLocalID, (success) =>
                        {
                            if (success)
                            {
                                newLevelUnlockedScreen.gameObject.SetActive(true);
                                StartCoroutine(newLevelUnlockedScreen.SetNewLevelUnlockedScreen("Lesson 5 - <color=#21B200>Easy"));
                                StartCoroutine(UserManager.Instance.GetUnlockedLevels(currentUserLocalID, HandleUnlockedLevelsChange));
                            }
                            else { Debug.LogError("Failed To Update Unlocked Levels"); }
                        }));
                    }
                    break;
            }
        }

        // Accomplished or not, check and change the next level button's state
        // Useful for checking if the level was already finished before but student just reattempted the activity on any difficulty
        ProcessNextLevelButtonStateChange(currentUserUnlockedLesson, currentUserHighestLessonUnlockedDifficulty);

        Debug.Log($"Level Finished: {isProjectileMotionSubActivityFinished && isCircularMotionSubActivityFinished}");
    }

    public override void DisplayPerformanceView()
	{
		base.DisplayPerformanceView();

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

    private void HandleUnlockedLevelsChange(bool success)
    {
        if (success)
        {
            Debug.Log("Incremented Student's Level Progress by 1");
            int currentUserUnlockedLesson = (int)UserManager.Instance.UserUnlockedLevels.fields["highestUnlockedLesson"].integerValue;
            int currentUserHighestLessonUnlockedDifficulty = (int)UserManager.Instance.UserUnlockedLevels.fields["highestLessonUnlockedDifficulty"].integerValue;
            ProcessNextLevelButtonStateChange(currentUserUnlockedLesson, currentUserHighestLessonUnlockedDifficulty);
        }
        else
        {
            Debug.LogError("Failed To Increment Student's Progress");
        }
    }

    private void ProcessNextLevelButtonStateChange(int currentUnlockedLesson, int currentHighestLessonUnlockedDifficulty)
    {
        // By default, set next level button state to not interactable
        performanceView.SetNextLevelButtonState(false);

        // If the current unlocked lesson is higher than lesson 4, allow user to proceed to next level
        if (currentUnlockedLesson > 4) { performanceView.SetNextLevelButtonState(true); Debug.Log("Next button state is interactable"); return; }

        switch (difficultyConfiguration)
        {
            // If player completed easy, check if current unlocked lesson is greater than or equal to 4
            // and if current unlocked difficulty is greater than 1 or Easy mode, then allow user to proceed to next level
            case Difficulty.Easy:
                if (!(currentUnlockedLesson >= 4)) { performanceView.SetNextLevelButtonState(false); return; }
                if (!(currentHighestLessonUnlockedDifficulty > 1)) { performanceView.SetNextLevelButtonState(false); return; }
                performanceView.SetNextLevelButtonState(true); Debug.Log("Next button state is interactable");
                break;

            // If player completed easy, check if current unlocked lesson is greater than or equal to 4
            // and if current unlocked difficulty is greater than 2 or Medium mode, then allow user to proceed to next level
            case Difficulty.Medium:
                if (!(currentUnlockedLesson >= 4)) { performanceView.SetNextLevelButtonState(false); return; }
                if (!(currentHighestLessonUnlockedDifficulty > 2)) { performanceView.SetNextLevelButtonState(false); return; }
                performanceView.SetNextLevelButtonState(true); Debug.Log("Next button state is interactable");
                break;

                // Don't need case for hard difficulty since the if statement before the switch case already handles it
        }
    }
}