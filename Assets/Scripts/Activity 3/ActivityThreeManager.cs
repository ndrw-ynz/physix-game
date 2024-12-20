using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GraphsSubActivitySO;
using Random = UnityEngine.Random;

public class AccelerationCalculationData
{
	public float initialVelocity;
	public float finalVelocity;
	public float totalTime;
}

public class TotalDepthCalculationData
{
	public float initialVelocity;
	public float totalTime;
}

public class ActivityThreeManager : ActivityManager
{
	public static Difficulty difficultyConfiguration;

	public event Action GraphsAreaClearEvent;
	public event Action AccelerationProblemClearEvent;
	public event Action TotalDepthProblemClearEvent;

	[Header("Level Data - Graphs")]
	[SerializeField] private GraphsSubActivitySO graphsLevelOne;
	[SerializeField] private GraphsSubActivitySO graphsLevelTwo;
	[SerializeField] private GraphsSubActivitySO graphsLevelThree;
	private GraphsSubActivitySO currentGraphsLevel;

	[Header("Level Data - 1D Kinematics")]
	[SerializeField] private Kinematics1DSubActivitySO kinematics1DLevelOne;
	[SerializeField] private Kinematics1DSubActivitySO kinematics1DLevelTwo;
	[SerializeField] private Kinematics1DSubActivitySO kinematics1DLevelThree;
	private Kinematics1DSubActivitySO currentKinematics1DLevel;

	[Header("Managers")]
	[SerializeField] private GraphManager graphManager;

	[Header("Views")]
	[SerializeField] private GraphsView graphsView;
	[SerializeField] private GraphEditorUI graphEditorUI;
	[SerializeField] private GraphViewerUI graphViewerUI;
	[SerializeField] private Kinematics1DView kinematics1DView;
	[SerializeField] private ActivityThreePerformanceView performanceView;

	[Header("Submission Status Displays")]
	[SerializeField] private GraphsSubmissionStatusDisplay graphsSubmissionStatusDisplay;
	[SerializeField] private Kinematics1DSubmissionStatusDisplay kinematics1DSubmissionStatusDisplay;

	// Variables for keeping track of current number of tests
	private int currentNumGraphsTests;
	private int currentNumAccelerationTests;
	private int currentNumTotalDepthTests;

	// Given graph values
	private List<List<int>> correctPositionValues;
	private List<List<int>> correctVelocityValues;
	private List<List<int>> correctAccelerationValues;

	// Given 1D Kinematics values
	private AccelerationCalculationData givenAccelerationData;
	private TotalDepthCalculationData givenTotalDepthData;

	// Variables for tracking which view is currently active
	private bool isGraphsViewActive;
	private bool isKinematics1DViewActive;

	// Gameplay performance metrics variables
	// Graphs Sub Activity
	private float graphsGameplayDuration;
	private bool isGraphsSubActivityFinished;
	private int numIncorrectGraphsSubmission;
	private int numCorrectGraphsSubmission;
	// Kinematics 1D Sub Activity
	private float kinematics1DGameplayDuration;
	// Acceleration Solving
	private bool isAccelerationCalcFinished;
	private int numIncorrectAccelerationSubmission;
	private int numCorrectAccelerationSubmission;
	// Total Depth Solving
	private bool isTotalDepthCalcFinished;
	private int numIncorrectTotalDepthSubmission;
	private int numCorrectTotalDepthSubmission;

	protected override void Start()
	{
		base.Start();

		SceneSoundManager.Instance.PlayMusic("With love from Vertex Studio (3)");

		ConfigureLevelData(difficultyConfiguration);

		SubscribeViewAndDisplayEvents();

		// Initialize correct given values
		InitializeCorrectGraphValues();
		GenerateAccelerationGivenData();
		GenerateTotalDepthGivenData();

		// Determine number of tests
		currentNumGraphsTests = currentGraphsLevel.numberOfTests;
		currentNumAccelerationTests = currentKinematics1DLevel.numberOfAccelerationProblems;
		currentNumTotalDepthTests = currentKinematics1DLevel.numberOfTotalDepthProblems;

		// Setup graph manager
		graphManager.SetupGraphs(correctPositionValues[currentGraphsLevel.numberOfTests - currentNumGraphsTests]);

		// Setup views
		graphsView.UpdateTestCountTextDisplay(currentGraphsLevel.numberOfTests - currentNumGraphsTests, currentGraphsLevel.numberOfTests);
		kinematics1DView.UpdateTestCountTextDisplay(currentKinematics1DLevel.numberOfAccelerationProblems - currentNumAccelerationTests, currentKinematics1DLevel.numberOfAccelerationProblems);
		kinematics1DView.UpdateAccelerationInfo(givenAccelerationData);
		kinematics1DView.UpdateTotalDepthInfo(givenTotalDepthData);

		// Update mission objective display
		missionObjectiveDisplayUI.UpdateMissionObjectiveText(0, $"Re-calibrate the ship's navigation system in the Graphs terminal ({currentGraphsLevel.numberOfTests - currentNumGraphsTests}/{currentGraphsLevel.numberOfTests})");
		missionObjectiveDisplayUI.UpdateMissionObjectiveText(1, $"Calculate the ship's acceleration on its journey on the 1D Kinematics terminal ({currentKinematics1DLevel.numberOfAccelerationProblems - currentNumAccelerationTests}/{currentKinematics1DLevel.numberOfAccelerationProblems})");
		missionObjectiveDisplayUI.UpdateMissionObjectiveText(2, $"Calculate the ship's total depth in arriving on planet Nakalais in the 1D Kinematics terminal ({currentKinematics1DLevel.numberOfTotalDepthProblems - currentNumTotalDepthTests}/{currentKinematics1DLevel.numberOfTotalDepthProblems})");

		inputReader.SetGameplay();
	}

	private void Update()
	{
		if (isGraphsViewActive && !isGraphsSubActivityFinished) graphsGameplayDuration += Time.deltaTime;
		if (isKinematics1DViewActive && !isAccelerationCalcFinished && !isTotalDepthCalcFinished) kinematics1DGameplayDuration += Time.deltaTime;
		if (isGraphsSubActivityFinished && isAccelerationCalcFinished && isTotalDepthCalcFinished) DisplayPerformanceView();
	}

	private void ConfigureLevelData(Difficulty difficulty)
	{
		difficultyConfiguration = difficulty;

		switch (difficulty)
		{
			case Difficulty.Easy:
				currentGraphsLevel = graphsLevelOne;
				currentKinematics1DLevel = kinematics1DLevelOne;
				break;
			case Difficulty.Medium:
				currentGraphsLevel = graphsLevelTwo;
				currentKinematics1DLevel = kinematics1DLevelTwo;
				break;
			case Difficulty.Hard:
				currentGraphsLevel = graphsLevelThree;
				currentKinematics1DLevel = kinematics1DLevelThree;
				break;
		}
	}

	private void SubscribeViewAndDisplayEvents()
	{
		// Graphs Sub Activity Related Events
		graphEditorUI.QuitGraphEditorEvent += () => graphsView.gameObject.SetActive(true);
		graphViewerUI.QuitGraphViewerEvent += () => graphsView.gameObject.SetActive(true);
		graphsView.OpenViewEvent += () => isGraphsViewActive = true;
		graphsView.QuitViewEvent += () => isGraphsViewActive = false;
		graphsView.SubmitAnswerEvent += CheckGraphsAnswer;
		graphsSubmissionStatusDisplay.ProceedEvent += UpdateGraphsViewState;

		// 1D Kinematics Sub Activity Related Events
		kinematics1DView.OpenViewEvent += () => isKinematics1DViewActive = true;
		kinematics1DView.QuitViewEvent += () => isKinematics1DViewActive = false;
		kinematics1DView.SubmitAccelerationAnswerEvent += CheckAccelerationAnswer;
		kinematics1DView.SubmitTotalDepthAnswerEvent += CheckTotalDepthAnswer;
		kinematics1DSubmissionStatusDisplay.ProceedEvent += UpdateKinematics1DViewState;
	}

	#region Graphs
	private void InitializeCorrectGraphValues()
	{
		correctPositionValues = new List<List<int>>();
		correctVelocityValues = new List<List<int>>();
		correctAccelerationValues = new List<List<int>>();

		int datasetSize = currentGraphsLevel.datasets[0].dataset.Count;

		for (int i = 0; i < currentGraphsLevel.numberOfTests; i++)
		{
			int randomDatasetIndex = Random.Range(0, datasetSize);

			foreach (GraphDataset graphDataset in currentGraphsLevel.datasets)
			{
				List<int> graphPointValues = graphDataset.dataset[randomDatasetIndex].Split(',').Select(int.Parse).ToList();

				switch (graphDataset.datasetType)
				{
					case DatasetType.Position:
						correctPositionValues.Add(graphPointValues);
						break;
					case DatasetType.Velocity:
						correctVelocityValues.Add(graphPointValues);
						break;
					case DatasetType.Acceleration:
						correctAccelerationValues.Add(graphPointValues);
						break;
				}
			}
		}
	}

	private void CheckGraphsAnswer(GraphsAnswerSubmission answer)
	{
		GraphsAnswerSubmissionResults results = ActivityThreeUtilities.ValidateGraphSubmission(
			answer, 
			correctPositionValues[currentGraphsLevel.numberOfTests - currentNumGraphsTests], 
			correctVelocityValues[currentGraphsLevel.numberOfTests - currentNumGraphsTests], 
			correctAccelerationValues[currentGraphsLevel.numberOfTests - currentNumGraphsTests]
			);

		if (results.isAllCorrect())
		{
			SceneSoundManager.Instance.PlaySFX("UI_Buttonconfirm_Stereo_01");

			numCorrectGraphsSubmission++;
			currentNumGraphsTests--;
			graphsView.UpdateTestCountTextDisplay(currentGraphsLevel.numberOfTests - currentNumGraphsTests, currentGraphsLevel.numberOfTests);
		}
		else
		{
			SceneSoundManager.Instance.PlaySFX("UI_Forbidden_Stereo_02");

			numIncorrectGraphsSubmission++;
		}

		DisplayGraphsSubmissionResults(results);
	}

	private void DisplayGraphsSubmissionResults(GraphsAnswerSubmissionResults results)
	{
		if (results.isAllCorrect())
		{
			graphsSubmissionStatusDisplay.SetSubmissionStatus(true, "Orbital 1's movement is optimal. Nice job!");
			missionObjectiveDisplayUI.UpdateMissionObjectiveText(0, $"Re-calibrate the ship's navigation system in the Graphs terminal ({currentGraphsLevel.numberOfTests - currentNumGraphsTests}/{currentGraphsLevel.numberOfTests})");
		}
		else
		{
			graphsSubmissionStatusDisplay.SetSubmissionStatus(false, "Engineer, there seems to be a mistake. Let's try again!");
		}

		graphsSubmissionStatusDisplay.UpdateStatusBorderDisplayFromResults(results);

		graphsSubmissionStatusDisplay.gameObject.SetActive(true);
	}

	private void UpdateGraphsViewState()
	{
		if (currentNumGraphsTests > 0)
		{
			graphManager.UpdateGraphs(correctPositionValues[currentGraphsLevel.numberOfTests - currentNumGraphsTests]);
		}
		else
		{
			isGraphsSubActivityFinished = true;
			missionObjectiveDisplayUI.ClearMissionObjective(0);
			graphsView.gameObject.SetActive(false);
			GraphsAreaClearEvent?.Invoke();
		}
	}
	#endregion

	#region 1D Kinematics
	private void GenerateAccelerationGivenData()
	{
		AccelerationCalculationData data = new AccelerationCalculationData();
		data.initialVelocity = Random.Range(currentKinematics1DLevel.minimumVelocityValue, currentKinematics1DLevel.maximumVelocityValue);
		data.finalVelocity = Random.Range(currentKinematics1DLevel.minimumVelocityValue, currentKinematics1DLevel.maximumVelocityValue);
		data.totalTime = Random.Range(currentKinematics1DLevel.minimumTimeValue, currentKinematics1DLevel.maximumTimeValue);
		givenAccelerationData = data;
	}

	private void GenerateTotalDepthGivenData()
	{
		TotalDepthCalculationData data = new TotalDepthCalculationData();
		data.initialVelocity = Random.Range(currentKinematics1DLevel.minimumVelocityValue, currentKinematics1DLevel.maximumVelocityValue);
		data.totalTime = Random.Range(currentKinematics1DLevel.minimumTimeValue, currentKinematics1DLevel.maximumTimeValue);
		givenTotalDepthData = data;
	}

	private void CheckAccelerationAnswer(float? answer)
	{
		bool isAccelerationCorrect = ActivityThreeUtilities.ValidateAccelerationSubmission(answer, givenAccelerationData);

		if (isAccelerationCorrect)
		{
			SceneSoundManager.Instance.PlaySFX("UI_Buttonconfirm_Stereo_01");

			numCorrectAccelerationSubmission++;
			currentNumAccelerationTests--;
			kinematics1DView.UpdateTestCountTextDisplay(currentKinematics1DLevel.numberOfAccelerationProblems - currentNumAccelerationTests, currentKinematics1DLevel.numberOfAccelerationProblems);
		}
		else
		{
			SceneSoundManager.Instance.PlaySFX("UI_Forbidden_Stereo_02");

			numIncorrectAccelerationSubmission++;
		}

		DisplayAccelerationSubmissionResults(isAccelerationCorrect);
	}

	private void CheckTotalDepthAnswer(float? answer)
	{
		bool isTotalDepthCorrect = ActivityThreeUtilities.ValidateTotalDepthSubmission(answer, givenTotalDepthData);

		if (isTotalDepthCorrect)
		{
			SceneSoundManager.Instance.PlaySFX("UI_Buttonconfirm_Stereo_01");

			numCorrectTotalDepthSubmission++;
			currentNumTotalDepthTests--;
			kinematics1DView.UpdateTestCountTextDisplay(currentKinematics1DLevel.numberOfTotalDepthProblems - currentNumTotalDepthTests, currentKinematics1DLevel.numberOfTotalDepthProblems);
		}
		else
		{
			SceneSoundManager.Instance.PlaySFX("UI_Forbidden_Stereo_02");

			numIncorrectTotalDepthSubmission++;
		}

		DisplayTotalDepthSubmissionResults(isTotalDepthCorrect);
	}

	private void DisplayAccelerationSubmissionResults(bool result)
	{
		if (result)
		{
			kinematics1DSubmissionStatusDisplay.SetSubmissionStatus(true, "Orbital 1's acceleration is stable. Great work!");
			missionObjectiveDisplayUI.UpdateMissionObjectiveText(1, $"Calculate the ship's acceleration on its journey on the 1D Kinematics terminal ({currentKinematics1DLevel.numberOfAccelerationProblems - currentNumAccelerationTests}/{currentKinematics1DLevel.numberOfAccelerationProblems})");
		}
		else
		{
			kinematics1DSubmissionStatusDisplay.SetSubmissionStatus(false, "Engineer, there seems to be a mistake. Let's try again!");
		}

		kinematics1DSubmissionStatusDisplay.UpdateStatusBorderDisplayFromResult(result);

		kinematics1DSubmissionStatusDisplay.gameObject.SetActive(true);
	}

	private void DisplayTotalDepthSubmissionResults(bool result)
	{
		if (result)
		{
			kinematics1DSubmissionStatusDisplay.SetSubmissionStatus(true, "Calculations complete. Calculated total depth is correct. Great job!");
			missionObjectiveDisplayUI.UpdateMissionObjectiveText(2, $"Calculate the ship's total depth in arriving on planet Nakalais in the 1D Kinematics terminal ({currentKinematics1DLevel.numberOfTotalDepthProblems - currentNumTotalDepthTests}/{currentKinematics1DLevel.numberOfTotalDepthProblems})");
		}
		else
		{
			kinematics1DSubmissionStatusDisplay.SetSubmissionStatus(false, "Engineer, there seems to be a mistake. Let's try again!");
		}

		kinematics1DSubmissionStatusDisplay.UpdateStatusBorderDisplayFromResult(result);

		kinematics1DSubmissionStatusDisplay.gameObject.SetActive(true);
	}

	private void UpdateKinematics1DViewState()
	{
		if (!isAccelerationCalcFinished)
		{
			if (currentNumAccelerationTests > 0)
			{
				GenerateAccelerationGivenData();
				kinematics1DView.UpdateAccelerationInfo(givenAccelerationData);
			}
			else
			{
				isAccelerationCalcFinished = true;
				kinematics1DView.DisplayTotalDepthInfo();
				kinematics1DView.UpdateTestCountTextDisplay(currentKinematics1DLevel.numberOfTotalDepthProblems - currentNumTotalDepthTests, currentKinematics1DLevel.numberOfTotalDepthProblems);
				missionObjectiveDisplayUI.ClearMissionObjective(1);
				AccelerationProblemClearEvent?.Invoke();
			}
		} else
		{
			if (currentNumTotalDepthTests > 0)
			{
				GenerateTotalDepthGivenData();
				kinematics1DView.UpdateTotalDepthInfo(givenTotalDepthData);
			}
			else
			{
				isTotalDepthCalcFinished = true;
				missionObjectiveDisplayUI.ClearMissionObjective(2);
				kinematics1DView.gameObject.SetActive(false);
				TotalDepthProblemClearEvent?.Invoke();
			}
		}
	}
	#endregion

	protected override void AddAttemptRecord()
	{
		Dictionary<string, object> graphsResults = new Dictionary<string, object>
		{
			{ "fields", new Dictionary<string, FirestoreField>
				{
					{ "isAccomplished", new FirestoreField(isGraphsSubActivityFinished) },
					{ "mistakes", new FirestoreField(numIncorrectGraphsSubmission) },
					{ "durationInSec", new FirestoreField((int)graphsGameplayDuration) }
				}
			}
		};

		Dictionary<string, object> kinematics1DResults = new Dictionary<string, object>
		{
			{ "fields", new Dictionary<string, FirestoreField>
				{
					{ "isAccelerationAccomplished", new FirestoreField(isAccelerationCalcFinished) },
					{ "accelerationMistakes", new FirestoreField(numIncorrectAccelerationSubmission) },
					{ "isTotalDepthAccomplished", new FirestoreField(isTotalDepthCalcFinished) },
					{ "totalDepthMistakes", new FirestoreField(numIncorrectTotalDepthSubmission) },
					{ "durationInSec", new FirestoreField((int)kinematics1DGameplayDuration) }
				}
			}
		};

		Dictionary<string, object> results = new Dictionary<string, object>
		{
			{ "fields", new Dictionary<string, object>
				{
					{ "graphs", new FirestoreField(graphsResults)},
					{ "1DKinematics", new FirestoreField(kinematics1DResults)},
				}
			}
		};

		Dictionary<string, FirestoreField> fields = new Dictionary<string, FirestoreField>
		{
			{ "dateAttempted", new FirestoreField(DateTime.UtcNow) },
			{ "difficulty", new FirestoreField($"{difficultyConfiguration}") },
			{ "results", new FirestoreField(results) },
			{ "isAccomplished", new FirestoreField(isGraphsSubActivityFinished && isAccelerationCalcFinished && isTotalDepthCalcFinished) },
			{ "studentId", new FirestoreField(UserManager.Instance.CurrentUser.localId) },
			{ "totalDurationInSec", new FirestoreField((int)(graphsGameplayDuration + kinematics1DGameplayDuration)) }
		};

		StartCoroutine(UserManager.Instance.CreateAttemptDocument(fields, "activityThreeAttempts"));
	}

    protected override void SetNextLevelButtonState()
    {
        bool isAccomplished = isGraphsSubActivityFinished && isAccelerationCalcFinished && isTotalDepthCalcFinished;

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
                    if (currentUserUnlockedLesson == 3 && currentUserHighestLessonUnlockedDifficulty == 1)
                    {
                        Dictionary<string, FirestoreField> fields = new Dictionary<string, FirestoreField>
                        {
                            {"highestUnlockedLesson", new FirestoreField(3) },
                            {"highestLessonUnlockedDifficulty", new FirestoreField(2) }
                        };

                        StartCoroutine(UserManager.Instance.UpdateUnlockedLevels(fields, currentUserLocalID, (success) =>
                        {
                            if (success)
                            {
                                newLevelUnlockedScreen.gameObject.SetActive(true);
                                StartCoroutine(newLevelUnlockedScreen.SetNewLevelUnlockedScreen("Lesson 3 - <color=#C5B501>Medium"));
                                StartCoroutine(UserManager.Instance.GetUnlockedLevels(currentUserLocalID, HandleUnlockedLevelsChange));
                            }
                            else { Debug.LogError("Failed To Update Unlocked Levels"); }
                        }));
                    }
                    break;

                // If the player accomplished in medium mode, check if the unlocked levels need to progress with respect to the current lesson
                case Difficulty.Medium:
                    if (currentUserUnlockedLesson == 3 && currentUserHighestLessonUnlockedDifficulty == 2)
                    {
                        Dictionary<string, FirestoreField> fields = new Dictionary<string, FirestoreField>
                        {
                            {"highestUnlockedLesson", new FirestoreField(3) },
                            {"highestLessonUnlockedDifficulty", new FirestoreField(3) }
                        };

                        StartCoroutine(UserManager.Instance.UpdateUnlockedLevels(fields, currentUserLocalID, (success) =>
                        {
                            if (success)
                            {
                                newLevelUnlockedScreen.gameObject.SetActive(true);
                                StartCoroutine(newLevelUnlockedScreen.SetNewLevelUnlockedScreen("Lesson 3 - <color=#FF0000>Hard"));
                                StartCoroutine(UserManager.Instance.GetUnlockedLevels(currentUserLocalID, HandleUnlockedLevelsChange));
                            }
                            else { Debug.LogError("Failed To Update Unlocked Levels"); }
                        }));
                    }
                    break;

                // If the player accomplished in hard mode, check if the unlocked levels need to progress with respect to the current lesson
                case Difficulty.Hard:
                    if (currentUserUnlockedLesson == 3 && currentUserHighestLessonUnlockedDifficulty == 3)
                    {
                        Dictionary<string, FirestoreField> fields = new Dictionary<string, FirestoreField>
                        {
                            {"highestUnlockedLesson", new FirestoreField(4) },
                            {"highestLessonUnlockedDifficulty", new FirestoreField(1) }
                        };

                        StartCoroutine(UserManager.Instance.UpdateUnlockedLevels(fields, currentUserLocalID, (success) =>
                        {
                            if (success)
                            {
                                newLevelUnlockedScreen.gameObject.SetActive(true);
                                StartCoroutine(newLevelUnlockedScreen.SetNewLevelUnlockedScreen("Lesson 4 - <color=#21B200>Easy"));
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

        Debug.Log($"Level Finished: {isGraphsSubActivityFinished && isAccelerationCalcFinished && isTotalDepthCalcFinished}");
    }

    public override void DisplayPerformanceView()
	{
		base.DisplayPerformanceView();

		inputReader.SetUI();
		performanceView.gameObject.SetActive(true);

		performanceView.SetTotalTimeDisplay(graphsGameplayDuration + kinematics1DGameplayDuration);

		performanceView.SetGraphsMetricsDisplay(
			isAccomplished: isGraphsSubActivityFinished,
			numIncorrectSubmission: numIncorrectGraphsSubmission,
			duration: graphsGameplayDuration
			);

		performanceView.SetKinematics1DMetricsDisplay(
			isAccelerationAccomplished: isAccelerationCalcFinished,
			isTotalDepthAccomplished: isTotalDepthCalcFinished,
			numIncorrectAcceleration: numIncorrectAccelerationSubmission,
			numIncorrectTotalDepth: numIncorrectTotalDepthSubmission,
			duration: kinematics1DGameplayDuration
			);

		// Update its activity feedback display (three args)
		performanceView.UpdateActivityFeedbackDisplay(
			new SubActivityPerformanceMetric(
				subActivityName: "graphs",
				isSubActivityFinished: isGraphsSubActivityFinished,
				numIncorrectAnswers: numIncorrectGraphsSubmission,
				numCorrectAnswers: numCorrectGraphsSubmission,
				badScoreThreshold: 5,
				averageScoreThreshold: 3
				),
			new SubActivityPerformanceMetric(
				subActivityName: "1D Kinematics - acceleration",
				isSubActivityFinished: isAccelerationCalcFinished,
				numIncorrectAnswers: numIncorrectAccelerationSubmission,
				numCorrectAnswers: numCorrectAccelerationSubmission,
				badScoreThreshold: 3,
				averageScoreThreshold: 2
				),
			new SubActivityPerformanceMetric(
				subActivityName: "1D Kinematics - total depth",
				isSubActivityFinished: isTotalDepthCalcFinished,
				numIncorrectAnswers: numIncorrectTotalDepthSubmission,
				numCorrectAnswers: numCorrectTotalDepthSubmission,
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
		if (!isGraphsSubActivityFinished)
		{
			taskText.Add($"-  Interact with the ship�s Graphs Terminal and plot data from position vs. time graphs to velocity and acceleration vs. time graphs to re-calibrate the ship's navigation system.");
		}
		if (!isAccelerationCalcFinished)
		{
			taskText.Add("-  Interact with the ship�s 1D Kinematics Terminal and adjust the Orbital 1�s acceleration traveling in space.");
		}
		if (!isTotalDepthCalcFinished)
		{
			taskText.Add("-  Interact with the ship�s 1D Kinematics Terminal and determine the total depth of the ship from the Nakalais surface to ensure a safe landing.");
		}

		List<string> objectiveText = new List<string>();
		objectiveText.Add("Safely navigate Orbital 1 through the vastness of space while adjusting its acceleration and determining the total depth from the Nakalais surface to ensure a safe landing.");

		activityPauseMenuUI.UpdateContent("Lesson 3 - Activity 3", taskText, objectiveText);
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

        // If the current unlocked lesson is higher than lesson 3, allow user to proceed to next level
        if (currentUnlockedLesson > 3) { performanceView.SetNextLevelButtonState(true); Debug.Log("Next button state is interactable"); return; }

        switch (difficultyConfiguration)
        {
            // If player completed easy, check if current unlocked lesson is greater than or equal to 3
            // and if current unlocked difficulty is greater than 1 or Easy mode, then allow user to proceed to next level
            case Difficulty.Easy:
                if (!(currentUnlockedLesson >= 3)) { performanceView.SetNextLevelButtonState(false); return; }
                if (!(currentHighestLessonUnlockedDifficulty > 1)) { performanceView.SetNextLevelButtonState(false); return; }
                performanceView.SetNextLevelButtonState(true); Debug.Log("Next button state is interactable");
                break;

            // If player completed easy, check if current unlocked lesson is greater than or equal to 3
            // and if current unlocked difficulty is greater than 2 or Medium mode, then allow user to proceed to next level
            case Difficulty.Medium:
                if (!(currentUnlockedLesson >= 3)) { performanceView.SetNextLevelButtonState(false); return; }
                if (!(currentHighestLessonUnlockedDifficulty > 2)) { performanceView.SetNextLevelButtonState(false); return; }
                performanceView.SetNextLevelButtonState(true); Debug.Log("Next button state is interactable");
                break;

                // Don't need case for hard difficulty since the if statement before the switch case already handles it
        }
    }
}