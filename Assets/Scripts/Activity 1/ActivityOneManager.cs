using System;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class ActivityOneManager : ActivityManager
{
	public static Difficulty difficultyConfiguration;

	public event Action SNRoomClearEvent;
	public event Action SNCorrectAnswerEvent;
	public event Action VarianceRoomClearEvent;
	public event Action APRoomClearEvent;
	public event Action errorsRoomClearEvent;

	[Header("Level Data - Scientific Notation (SN)")]
	[SerializeField] private ScientificNotationSubActivitySO SNLevelOne;
	[SerializeField] private ScientificNotationSubActivitySO SNLevelTwo;
	[SerializeField] private ScientificNotationSubActivitySO SNLevelThree;
	private ScientificNotationSubActivitySO currentSNLevel;

    [Header("Managers")]
    [SerializeField] private APGraphManager APGraphManager;

    [Header("Handlers")]
    [SerializeField] private ContainerSelectionHandler containerSelectionHandler;

	[Header("Views")]
	[SerializeField] private ContainerPickerView containerPickerView;
    [SerializeField] private ScientificNotationView scientificNotationView;
    [SerializeField] private VarianceView varianceView;
    [SerializeField] private AccuracyPrecisionView accuracyPrecisionView;
    [SerializeField] private ErrorsView errorsView;
    [SerializeField] private ActivityOnePerformanceView performanceView;

	[Header("Submission Status Displays")]
	[SerializeField] private SNSubmissionStatusDisplay scientificNotationSubmissionStatusDisplay;
    [SerializeField] private VarianceSubmissionStatusDisplay varianceSubmissionStatusDisplay;
    [SerializeField] private APSubmissionStatusDisplay accuracyPrecisionSubmissionStatusDisplay;
    [SerializeField] private ErrorsSubmissionStatusDisplay errorsSubmissionStatusDisplay;

	[Header("New Level Unlocked Screen")]
	[SerializeField] private NewLevelUnlockedScreen newLevelUnlockedScreen;

    // Variables for keeping track of current number of tests
    private int currentNumSNTests;

	private List<float> numericalContainerValues;

	// Variables for tracking which view is currently active
	private bool isSNViewActive;
	private bool isVarianceViewActive;
	private bool isAPViewActive;
	private bool isErrorsViewActive;

	// Gameplay performance metrics variables
	// Scientific Notation Sub Activity
	private float SNGameplayDuration;
	private bool isSNSubActivityFinished;
	private int numIncorrectSNSubmission;
	private int numCorrectSNSubmission;
	// Variance Sub Activity
	private float varianceGameplayDuration;
	private bool isVarianceSubActivityFinished;
	private int numIncorrectVarianceSubmission;
	private int numCorrectVarianceSubmission;
	// Accuracy Precision Sub Activity
	private float APGameplayDuration;
	private bool isAPSubActivityFinished;
	private int numIncorrectAPSubmission;
	private int numCorrectAPSubmission;
	// Errors Sub Activity
	private float errorsGameplayDuration;
	private bool isErrorsSubActivityFinished;
	private int numIncorrectErrorsSubmission;
	private int numCorrectErrorsSubmission;

	protected override void Start()
	{
		base.Start();

		SceneSoundManager.Instance.PlayMusic("With love from Vertex Studio (10)");

		ConfigureLevelData(difficultyConfiguration);

        SubscribeViewAndDisplayEvents();

		// Initialize given values
		containerSelectionHandler.SetupContainerValues(currentSNLevel);
        numericalContainerValues = new List<float>();

		// Setting number of tests
		currentNumSNTests = currentSNLevel.numberOfTests;

        // Setup views
        scientificNotationView.UpdateNumberOfContainersTextDisplay(0, currentNumSNTests);

		inputReader.SetGameplay();
	}

	private void Update()
	{
		if (isSNViewActive && !isSNSubActivityFinished) SNGameplayDuration += Time.deltaTime;
		if (isVarianceViewActive && !isVarianceSubActivityFinished) varianceGameplayDuration += Time.deltaTime;
		if (isAPViewActive && !isAPSubActivityFinished) APGameplayDuration += Time.deltaTime;
		if (isErrorsViewActive && !isErrorsSubActivityFinished) errorsGameplayDuration += Time.deltaTime;
		if (isSNSubActivityFinished && isVarianceSubActivityFinished && isAPSubActivityFinished && isErrorsSubActivityFinished)
		{
			DisplayPerformanceView();
		}
	}

	private void ConfigureLevelData(Difficulty difficulty)
	{
		difficultyConfiguration = difficulty;

		switch (difficulty)
		{
			case Difficulty.Easy:
				currentSNLevel = SNLevelOne;
				break;
			case Difficulty.Medium:
				currentSNLevel = SNLevelTwo;
				break;
			case Difficulty.Hard:
				currentSNLevel = SNLevelThree;
				break;
		}
	}

    private void SubscribeViewAndDisplayEvents()
    {
        // Scientific Notation Sub Activity Related Events
		containerSelectionHandler.UpdateSelectedContainerEvent += (boxContainer) => containerPickerView.UpdateContainerDisplay(boxContainer);
		containerSelectionHandler.UpdateSelectedContainerEvent += (boxContainer) => scientificNotationView.UpdateScientificNotationView(boxContainer);
		scientificNotationView.OpenViewEvent += () => isSNViewActive = true;
		scientificNotationView.QuitViewEvent += () => isSNViewActive = false;
		scientificNotationView.SubmitAnswerEvent += CheckScientificNotationAnswer;
		scientificNotationSubmissionStatusDisplay.ProceedEvent += UpdateSNViewState;

		// Variance Sub Activity Related Events
		varianceView.OpenViewEvent += () => isVarianceViewActive = true;
		varianceView.QuitViewEvent += () => isVarianceViewActive = false;
		varianceView.SubmitAnswerEvent += CheckVarianceAnswer;
		varianceSubmissionStatusDisplay.ProceedEvent += UpdateVarianceViewState;

		// Accuracy Precision Sub Activity Related Events
		accuracyPrecisionView.OpenViewEvent += () => isAPViewActive = true;
		accuracyPrecisionView.QuitViewEvent += () => isAPViewActive = false;
		accuracyPrecisionView.SubmitAnswerEvent += CheckAccuracyPrecisionAnswer;
		accuracyPrecisionSubmissionStatusDisplay.ProceedEvent += UpdateAPViewState;

		// Errors Sub Activity Related Events
		errorsView.OpenViewEvent += () => isErrorsViewActive = true;
		errorsView.QuitViewEvent += () => isErrorsViewActive = false;
		errorsView.SubmitAnswerEvent += CheckErrorsAnswer;
        errorsSubmissionStatusDisplay.ProceedEvent += UpdateErrorsViewState;
	}

	#region Scientific Notation

	private void CheckScientificNotationAnswer(ScientificNotationAnswerSubmission answer)
    {
        BoxContainer selectedContainer = containerSelectionHandler.GetSelectedContainer();
		ScientificNotationAnswerSubmissionResults results = ActivityOneUtilities.ValidateScientificNotationSubmission(answer, selectedContainer.numericalValue, selectedContainer.unitOfMeasurement);

		if (results.isAllCorrect())
		{
			SceneSoundManager.Instance.PlaySFX("UI_Buttonconfirm_Stereo_01");

			numCorrectSNSubmission++;
			currentNumSNTests--;
            numericalContainerValues.Add(selectedContainer.numericalValue);
		}
		else
		{
			SceneSoundManager.Instance.PlaySFX("UI_Forbidden_Stereo_02");

			numIncorrectSNSubmission++;
		}

		DisplaySNSubmissionResults(results);
	}

	private void DisplaySNSubmissionResults(ScientificNotationAnswerSubmissionResults results)
	{
		if (results.isAllCorrect())
		{
			scientificNotationSubmissionStatusDisplay.SetSubmissionStatus(true, "Great job! Your calculations are correct. Making containers for ejection.");
			missionObjectiveDisplayUI.UpdateMissionObjectiveText(0, $"Select storage containers and determine its value in standard scientific notation ({currentNumSNTests}/{currentSNLevel.numberOfTests})");
		}
		else
		{
			scientificNotationSubmissionStatusDisplay.SetSubmissionStatus(false, "Engineer, there seems to be a mistake. Let's try again!");
		}

		scientificNotationSubmissionStatusDisplay.UpdateStatusBorderDisplaysFromResult(results);

		scientificNotationSubmissionStatusDisplay.gameObject.SetActive(true);
	}

	private void UpdateSNViewState()
	{
		SNCorrectAnswerEvent.Invoke();
		if (currentNumSNTests > 0)
		{
            scientificNotationView.UpdateNumberOfContainersTextDisplay(currentNumSNTests, currentSNLevel.numberOfTests);
			scientificNotationView.UpdateScientificNotationView(containerSelectionHandler.GetSelectedContainer());
		}
		else
		{
            isSNSubActivityFinished = true;
            SNRoomClearEvent?.Invoke();
            varianceView.SetupVarianceView(numericalContainerValues);
			missionObjectiveDisplayUI.ClearMissionObjective(0);
		}
		containerPickerView.UpdateContainerDisplay(null);
		scientificNotationView.gameObject.SetActive(false);
	}

	#endregion

	#region Variance
    private void CheckVarianceAnswer(VarianceAnswerSubmission answer)
    {
		VarianceAnswerSubmissionResults results = ActivityOneUtilities.ValidateVarianceSubmission(answer, numericalContainerValues);

		if (results.isAllCorrect())
		{
			SceneSoundManager.Instance.PlaySFX("UI_Buttonconfirm_Stereo_01");

			numCorrectVarianceSubmission++;
		}
		else
		{
			SceneSoundManager.Instance.PlaySFX("UI_Forbidden_Stereo_02");

			numIncorrectVarianceSubmission++;
		}

        DisplayVarianceSubmissionResults(results);
	}

	private void DisplayVarianceSubmissionResults(VarianceAnswerSubmissionResults results)
	{
		if (results.isAllCorrect())
		{
			varianceSubmissionStatusDisplay.SetSubmissionStatus(true, "Amazing work! Container spread verified. Now placing containers on the ejecton site.");
			missionObjectiveDisplayUI.UpdateMissionObjectiveText(1, $"Calculate the variance of the selected storage containers (1/1)");

		}
		else
		{
			varianceSubmissionStatusDisplay.SetSubmissionStatus(false, "Engineer, there seems to be a mistake. Let's try again!");
		}

		varianceSubmissionStatusDisplay.gameObject.SetActive(true);

		varianceSubmissionStatusDisplay.UpdateStatusBorderDisplaysFromResult(results);
	}

	private void UpdateVarianceViewState()
	{
		isVarianceSubActivityFinished = true;
		varianceView.gameObject.SetActive(false);
        VarianceRoomClearEvent?.Invoke();
		missionObjectiveDisplayUI.ClearMissionObjective(1);
	}

	#endregion

	#region Accuracy and Precision
	private void CheckAccuracyPrecisionAnswer(APGraphType? answer) 
    {
        APGraphType correctGraphType = APGraphManager.GetGraphTypeFromGraphs(1);
        bool result = answer == correctGraphType;

		if (result)
		{
			SceneSoundManager.Instance.PlaySFX("UI_Buttonconfirm_Stereo_01");

			numCorrectAPSubmission++;
		}
		else
		{
			SceneSoundManager.Instance.PlaySFX("UI_Forbidden_Stereo_02");

			numIncorrectAPSubmission++;
		}

        DisplayAccuracyPrecisionSubmissionResult(result);
	}

    private void DisplayAccuracyPrecisionSubmissionResult(bool result)
    {
        if (result)
        {
			accuracyPrecisionSubmissionStatusDisplay.SetSubmissionStatus(true, "Great work! Containers are prepared and ready for ejection. Head to the last panel.");
			missionObjectiveDisplayUI.UpdateMissionObjectiveText(2, $"Determine the accuracy & precision of the placement of the selected containers (1/1)");
		}
		else
        {
			accuracyPrecisionSubmissionStatusDisplay.SetSubmissionStatus(false, "Engineer, there seems to be a mistake. Let's try again!");
		}

		accuracyPrecisionSubmissionStatusDisplay.gameObject.SetActive(true);

		accuracyPrecisionSubmissionStatusDisplay.UpdateStatusBorderDisplayFromResult(result);
	}

    private void UpdateAPViewState()
    {
		isAPSubActivityFinished = true;
		accuracyPrecisionView.gameObject.SetActive(false);
		APRoomClearEvent?.Invoke();
		missionObjectiveDisplayUI.ClearMissionObjective(2);
	}

	#endregion

	#region Errors
	private void CheckErrorsAnswer(ErrorType? answer)
    {
        APGraphType[] givenGraphTypes =
        {
            APGraphManager.GetGraphTypeFromGraphs(1),
            APGraphManager.GetGraphTypeFromGraphs(2),
            APGraphManager.GetGraphTypeFromGraphs(3)
        };

		bool result = ActivityOneUtilities.ValidateErrorsSubmission(answer, givenGraphTypes);

		if (result)
		{
			SceneSoundManager.Instance.PlaySFX("UI_Buttonconfirm_Stereo_01");

			numCorrectErrorsSubmission++;
		}
		else
		{
			SceneSoundManager.Instance.PlaySFX("UI_Forbidden_Stereo_02");

			numIncorrectErrorsSubmission++;
		}

		DisplayErrorsSubmissionResult(result);
	}

    private void DisplayErrorsSubmissionResult(bool result)
    {
		if (result)
		{
			errorsSubmissionStatusDisplay.SetSubmissionStatus(true, "Nicely done! You may now eject the containers.");
			missionObjectiveDisplayUI.UpdateMissionObjectiveText(3, $"Determine the type of error from the positions of the selected containers (1/1)");
		}
		else
		{
			errorsSubmissionStatusDisplay.SetSubmissionStatus(false, "Engineer, there seems to be a mistake. Let's try again!");
		}

		errorsSubmissionStatusDisplay.gameObject.SetActive(true);

		errorsSubmissionStatusDisplay.UpdateStatusBorderDisplayFromResult(result);
	}

    private void UpdateErrorsViewState()
    {
		isErrorsSubActivityFinished = true;
		errorsView.gameObject.SetActive(false);
		errorsRoomClearEvent?.Invoke();
		missionObjectiveDisplayUI.ClearMissionObjective(3);
	}
	#endregion

	protected override void AddAttemptRecord()
	{
		Dictionary<string, object> scientificNotationResults = new Dictionary<string, object>
		{
			{ "fields", new Dictionary<string, FirestoreField>
				{
					{ "isAccomplished", new FirestoreField(isSNSubActivityFinished) },
					{ "mistakes", new FirestoreField(numIncorrectSNSubmission) },
					{ "durationInSec", new FirestoreField((int)SNGameplayDuration) }
				}
			}
		};

		Dictionary<string, object> varianceResults = new Dictionary<string, object>
		{
			{ "fields", new Dictionary<string, FirestoreField>
				{
					{ "isAccomplished", new FirestoreField(isVarianceSubActivityFinished) },
					{ "mistakes", new FirestoreField(numIncorrectVarianceSubmission) },
					{ "durationInSec", new FirestoreField((int)varianceGameplayDuration) }
				}
			}
		};

		Dictionary<string, object> accuracyPrecisionResults = new Dictionary<string, object>
		{
			{ "fields", new Dictionary<string, FirestoreField>
				{
					{ "isAccomplished", new FirestoreField(isAPSubActivityFinished) },
					{ "mistakes", new FirestoreField(numIncorrectAPSubmission) },
					{ "durationInSec", new FirestoreField((int)APGameplayDuration) }
				}
			}
		};

		Dictionary<string, object> errorsResults = new Dictionary<string, object>
		{
			{ "fields", new Dictionary<string, FirestoreField>
				{
					{ "isAccomplished", new FirestoreField(isErrorsSubActivityFinished) },
					{ "mistakes", new FirestoreField(numIncorrectErrorsSubmission) },
					{ "durationInSec", new FirestoreField((int)errorsGameplayDuration) }
				}
			}
		};

		Dictionary<string, object> results = new Dictionary<string, object>
		{
			{ "fields", new Dictionary<string, object>
				{
					{ "scientificNotation", new FirestoreField(scientificNotationResults)},
					{ "variance", new FirestoreField(varianceResults)},
					{ "accuracyPrecision", new FirestoreField(accuracyPrecisionResults)},
					{ "errors", new FirestoreField(errorsResults)},
				}
			}
		};

		Dictionary<string, FirestoreField> fields = new Dictionary<string, FirestoreField>
		{
			{ "dateAttempted", new FirestoreField(DateTime.UtcNow) },
			{ "difficulty", new FirestoreField($"{difficultyConfiguration}") },
			{ "results", new FirestoreField(results) },
			{ "isAccomplished", new FirestoreField(isSNSubActivityFinished && isVarianceSubActivityFinished && isAPSubActivityFinished && isErrorsSubActivityFinished) },
			{ "studentId", new FirestoreField(UserManager.Instance.CurrentUser.localId) },
			{ "totalDurationInSec", new FirestoreField((int)(SNGameplayDuration + varianceGameplayDuration + APGameplayDuration + errorsGameplayDuration)) }
		};

		StartCoroutine(UserManager.Instance.CreateAttemptDocument(fields, "activityOneAttempts"));
	}

    protected override void SetNextLevelButtonState()
    {
		bool isAccomplished = isSNSubActivityFinished && isVarianceSubActivityFinished && isAPSubActivityFinished && isErrorsSubActivityFinished;

        int currentUserUnlockedLesson = (int) UserManager.Instance.UserUnlockedLevels.fields["highestUnlockedLesson"].integerValue;
		int currentUserHighestLessonUnlockedDifficulty = (int) UserManager.Instance.UserUnlockedLevels.fields["highestLessonUnlockedDifficulty"].integerValue;

		// If the player accomplished the whole activity in any difficulty, do a switch case
		if (isAccomplished)
		{
            string currentUserLocalID = UserManager.Instance.CurrentUser.localId;

			// If the player accomplished in easy mode, check if the unlocked levels need to progress with respect to the current lesson
            switch (difficultyConfiguration)
            {
                case Difficulty.Easy:
                    if (currentUserUnlockedLesson == 1 && currentUserHighestLessonUnlockedDifficulty == 1)
                    {
						Dictionary<string, FirestoreField> fields = new Dictionary<string, FirestoreField> 
						{
							{"highestUnlockedLesson", new FirestoreField(1) },
							{"highestLessonUnlockedDifficulty", new FirestoreField(2) }
						};

                        StartCoroutine(UserManager.Instance.UpdateUnlockedLevels(fields, currentUserLocalID, (success) =>
                        {
                            if (success)
                            {
                                newLevelUnlockedScreen.gameObject.SetActive(true);
                                StartCoroutine(newLevelUnlockedScreen.SetNewLevelUnlockedScreen("Lesson 1 - <color=#C5B501>Medium"));
                                StartCoroutine(UserManager.Instance.GetUnlockedLevels(currentUserLocalID, HandleUnlockedLevelsChange));
                            }
                            else { Debug.LogError("Failed To Update Unlocked Levels"); }
                        }));
                    }
                    break;

                // If the player accomplished in medium mode, check if the unlocked levels need to progress with respect to the current lesson
                case Difficulty.Medium:
                    if (currentUserUnlockedLesson == 1 && currentUserHighestLessonUnlockedDifficulty == 2)
                    {
                        Dictionary<string, FirestoreField> fields = new Dictionary<string, FirestoreField>
                        {
                            {"highestUnlockedLesson", new FirestoreField(1) },
                            {"highestLessonUnlockedDifficulty", new FirestoreField(3) }
                        };

                        StartCoroutine(UserManager.Instance.UpdateUnlockedLevels(fields, currentUserLocalID, (success) =>
                        {
                            if (success)
                            {
                                newLevelUnlockedScreen.gameObject.SetActive(true);
                                StartCoroutine(newLevelUnlockedScreen.SetNewLevelUnlockedScreen("Lesson 1 - <color=#FF0000>Hard"));
                                StartCoroutine(UserManager.Instance.GetUnlockedLevels(currentUserLocalID, HandleUnlockedLevelsChange));
                            }
                            else { Debug.LogError("Failed To Update Unlocked Levels"); }
                        }));
                    }
                    break;

                // If the player accomplished in hard mode, check if the unlocked levels need to progress with respect to the current lesson
                case Difficulty.Hard:
                    if (currentUserUnlockedLesson == 1 && currentUserHighestLessonUnlockedDifficulty == 3)
                    {
                        Dictionary<string, FirestoreField> fields = new Dictionary<string, FirestoreField>
                        {
                            {"highestUnlockedLesson", new FirestoreField(2) },
                            {"highestLessonUnlockedDifficulty", new FirestoreField(1) }
                        };

                        StartCoroutine(UserManager.Instance.UpdateUnlockedLevels(fields, currentUserLocalID, (success) =>
                        {
							if (success)
							{
                                newLevelUnlockedScreen.gameObject.SetActive(true);
                                StartCoroutine(newLevelUnlockedScreen.SetNewLevelUnlockedScreen("Lesson 2 - <color=#21B200>Easy"));
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

        Debug.Log($"Level Finished: {isSNSubActivityFinished && isVarianceSubActivityFinished && isAPSubActivityFinished && isErrorsSubActivityFinished}");
    }

    public override void DisplayPerformanceView()
	{
		base.DisplayPerformanceView();

		inputReader.SetUI();
		performanceView.gameObject.SetActive(true);

		performanceView.SetTotalTimeDisplay(SNGameplayDuration + varianceGameplayDuration + APGameplayDuration + errorsGameplayDuration);

		performanceView.SetScientificNotationMetricsDisplay(
			isAccomplished: isSNSubActivityFinished,
			numIncorrectSubmission: numIncorrectSNSubmission,
			duration: SNGameplayDuration
			);

		performanceView.SetVarianceMetricsDisplay(
			isAccomplished: isVarianceSubActivityFinished,
			numIncorrectSubmission: numIncorrectVarianceSubmission,
			duration: varianceGameplayDuration
			);

		performanceView.SetAccuracyPrecisionMetricsDisplay(
			isAccomplished: isAPSubActivityFinished,
			numIncorrectSubmission: numIncorrectAPSubmission,
			duration: APGameplayDuration
			);

		performanceView.SetErrorsMetricsDisplay(
			isAccomplished: isErrorsSubActivityFinished,
			numIncorrectSubmission: numIncorrectErrorsSubmission,
			duration: errorsGameplayDuration
			);

		// Update its activity feedback display (four args)
		performanceView.UpdateActivityFeedbackDisplay(
			new SubActivityPerformanceMetric(
				subActivityName: "scientific notation",
				isSubActivityFinished: isSNSubActivityFinished,
				numIncorrectAnswers: numIncorrectSNSubmission,
				numCorrectAnswers: numCorrectSNSubmission,
				badScoreThreshold: 6,
				averageScoreThreshold: 3
				),
			new SubActivityPerformanceMetric(
				subActivityName: "variance",
				isSubActivityFinished: isVarianceSubActivityFinished,
				numIncorrectAnswers: numIncorrectVarianceSubmission,
				numCorrectAnswers: numCorrectVarianceSubmission,
				badScoreThreshold: 5,
				averageScoreThreshold: 2
				),
			new SubActivityPerformanceMetric(
				subActivityName: "accuracy & precision",
				isSubActivityFinished: isAPSubActivityFinished,
				numIncorrectAnswers: numIncorrectAPSubmission,
				numCorrectAnswers: numCorrectAPSubmission,
				badScoreThreshold: 3,
				averageScoreThreshold: 2
				),
			new SubActivityPerformanceMetric(
				subActivityName: "errors",
				isSubActivityFinished: isErrorsSubActivityFinished,
				numIncorrectAnswers: numIncorrectErrorsSubmission,
				numCorrectAnswers: numCorrectErrorsSubmission,
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
		if (!isSNSubActivityFinished)
		{
			taskText.Add($"- Select storage containers and determine its value in standard scientific notation.");
		}
		if (!isVarianceSubActivityFinished)
		{
			taskText.Add("- Calculate the variance of the selected storage containers.");
		}
		if (!isAPSubActivityFinished)
		{
			taskText.Add("- Determine the accuracy & precision of the placement of the selected containers.");
		}
		if (!isErrorsSubActivityFinished)
		{
			taskText.Add("- Determine the type of error from the positions of the selected containers.");
		}

		List<string> objectiveText = new List<string>();
		objectiveText.Add("Eject containers from the space rocket. ");

		activityPauseMenuUI.UpdateContent("Lesson 1 - Activity 1", taskText, objectiveText);
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
		// If the current unlocked lesson is higher than lesson 1, allow user to proceed to next level
        if (currentUnlockedLesson > 1) { performanceView.SetNextLevelButtonState(true); Debug.Log("Next button state is interactable"); return; }

        switch (difficultyConfiguration)
        {
            // If player completed easy, check if current unlocked lesson is greater than or equal to 1
			// and if current unlocked difficulty is greater than 1 or Easy mode, then allow user to proceed to next level
            case Difficulty.Easy:
                if (!(currentUnlockedLesson >= 1)) { performanceView.SetNextLevelButtonState(false); return; }
                if (!(currentHighestLessonUnlockedDifficulty > 1)) { performanceView.SetNextLevelButtonState(false); return; }
                performanceView.SetNextLevelButtonState(true); Debug.Log("Next button state is interactable");
                break;

            // If player completed easy, check if current unlocked lesson is greater than or equal to 1
            // and if current unlocked difficulty is greater than 2 or Medium mode, then allow user to proceed to next level
            case Difficulty.Medium:
                if (!(currentUnlockedLesson >= 1)) { performanceView.SetNextLevelButtonState(false); return; }
                if (!(currentHighestLessonUnlockedDifficulty > 2)) { performanceView.SetNextLevelButtonState(false); return; }
                performanceView.SetNextLevelButtonState(true); Debug.Log("Next button state is interactable");
                break;

			// Don't need case for hard difficulty since the if statement before the switch case already handles it
        }
    }
}
