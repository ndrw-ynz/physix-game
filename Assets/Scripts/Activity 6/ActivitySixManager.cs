using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DotProductData
{
	public Vector3 satelliteDishVector;
	public Vector3 targetObjectVector;
}

public class WorkSubActivityData
{
	public WorkSubActivityState workSubActivityState;
	public float acceleration;
	public float mass;
	public float displacement;
	public int angleMeasure;
}

public enum ForceDisplacementCurveType
{
	ConstantForceGraph,
	LinearlyIncreasingForceGraph,
	LinearlyDecreasingForceGraph
}

public class ActivitySixManager : ActivityManager
{
	public static Difficulty difficultyConfiguration;

	public event Action MainSatelliteAreaClearEvent;

	[Header("Level Data - Dot Product")]
	[SerializeField] private DotProductSubActivitySO dotProductLevelOne;
	[SerializeField] private DotProductSubActivitySO dotProductLevelTwo;
	[SerializeField] private DotProductSubActivitySO dotProductLevelThree;
	private DotProductSubActivitySO currentDotProductLevel;

	[Header("Level Data - Work")]
	[SerializeField] private WorkSubActivitySO workLevelOne;
	[SerializeField] private WorkSubActivitySO workLevelTwo;
	[SerializeField] private WorkSubActivitySO workLevelThree;
	private WorkSubActivitySO currentWorkLevel;

	[Header("Views")]
	[SerializeField] private DotProductView dotProductView;
	[SerializeField] private WorkView workView;
	[SerializeField] private WorkGraphInterpretationView workGraphInterpretationView;
	[SerializeField] private ActivitySixPerformanceView performanceView;

	[Header("Submission Status Displays")]
	[SerializeField] private DotProductSubmissionStatusDisplay dotProductSubmissionStatusDisplay;
	[SerializeField] private WorkSubmissionStatusDisplay workSubmissionStatusDisplay;
	[SerializeField] private WorkGraphSubmissionStatusDisplay workGraphSubmissionStatusDisplay;

	// queue for work sub actvity
	private WorkSubActivityStateMachine workSubActivityStateMachine;
	private Queue<WorkSubActivityState> workSubActivityStateQueue;

	// Given data
	private DotProductData dotProductGivenData;
	private WorkSubActivityData workSubActivityGivenData;
	private Dictionary<ForceDisplacementCurveType, List<Vector2>> forceDisplacementGraphData;

	// Track current graph type displayed
	private ForceDisplacementCurveType currentGraphTypeDisplayed;

	// Variables for keeping track of current number of tests
	private int currentNumDotProductTests;
	private int currentNumWorkGraphTests;
	private int totalWorkGraphTests;

	// Variables for tracking which view is currently active
	private bool isDotProductViewActive;
	private bool isWorkViewActive;
	private bool isWorkGraphInterpretationViewActive;

	// Gameplay performance metrics variables
	// Dot Product Sub Activity
	private float dotProductGameplayDuration;
	private bool isDotProductSubActivityFinished;
	private int numIncorrectDotProductSubmission;
	private int numCorrectDotProductSubmission;
	// Work Sub Activity
	private float workSubActivityGameplayDuration;
	private bool isWorkSubActivityFinished;
	private int numIncorrectWorkSubmission;
	private int numCorrectWorkSubmission;
	// Work Graph Interaction Sub Activity
	private float workGraphSubActivityDuration;
	private bool isWorkGraphSubActivityFinished;
	private int numIncorrectWorkGraphSubmission;
	private int numCorrectWorkGraphSubmission;

	protected override void Start()
	{
		base.Start();

		SceneSoundManager.Instance.PlayMusic("With love from Vertex Studio (37)");

		ConfigureLevelData(difficultyConfiguration);

		SubscribeViewAndDisplayEvents();

		// Initialize values for state machines
		workSubActivityStateMachine = new WorkSubActivityStateMachine(workView);
		workSubActivityStateMachine.Initialize(WorkSubActivityState.None);
		InitializeWorkSubActivityStateQueues();


		// Initialize given values
		GenerateDotProductGivenData(currentDotProductLevel);
		GenerateWorkSubActivityGivenData(currentWorkLevel, workSubActivityStateQueue.Peek());
		GenerateForceDisplacementGraphData();
		UpdateCurrentGraphTypeRandomly();

		// Setting number of tests
		currentNumDotProductTests = currentDotProductLevel.numberOfTests;
		currentNumWorkGraphTests = difficultyConfiguration switch
		{
			Difficulty.Easy => 1,
			Difficulty.Medium => 2,
			Difficulty.Hard => 3,
			_ => 0
		};
		totalWorkGraphTests = currentNumWorkGraphTests;

		// Setting up views
		dotProductView.SetupDotProductView(dotProductGivenData);
		workView.SetupWorkView(workSubActivityGivenData, workSubActivityStateQueue.Peek());
		workGraphInterpretationView.SetupWorkGraphInterpretationView(forceDisplacementGraphData, currentGraphTypeDisplayed);

		// Update mission objective display
		missionObjectiveDisplayUI.UpdateMissionObjectiveText(0, $"Monitor the Satellite's Direction by Computing for the Dot Product on the Satellite Control Panel ({currentDotProductLevel.numberOfTests - currentNumDotProductTests}/{currentDotProductLevel.numberOfTests})");
		missionObjectiveDisplayUI.UpdateMissionObjectiveText(1, $"Embark on the Satellite Truck and Monitor the Work Exerted by the Vehicle ({currentWorkLevel.numberOfRepetitions * 3 - workSubActivityStateQueue.Count}/{currentWorkLevel.numberOfRepetitions * 3})");
		missionObjectiveDisplayUI.UpdateMissionObjectiveText(2, $"Find the Crashed Drone and Determine the Net Work Exerted from its Force vs. Displacement Graph Data ({totalWorkGraphTests - currentNumWorkGraphTests}/{totalWorkGraphTests})");
	}

	private void Update()
	{
		if (isDotProductViewActive && !isDotProductSubActivityFinished) dotProductGameplayDuration += Time.deltaTime;
		if (isWorkViewActive && !isWorkSubActivityFinished) workSubActivityGameplayDuration += Time.deltaTime;
		if (isWorkGraphInterpretationViewActive && !isWorkGraphSubActivityFinished) workGraphSubActivityDuration += Time.deltaTime;
		if (isDotProductSubActivityFinished && isWorkSubActivityFinished && isWorkGraphSubActivityFinished) DisplayPerformanceView();
	}

	private void ConfigureLevelData(Difficulty difficulty)
	{
		difficultyConfiguration = difficulty;

		switch (difficulty)
		{
			case Difficulty.Easy:
				currentDotProductLevel = dotProductLevelOne;
				currentWorkLevel = workLevelOne;
				break;
			case Difficulty.Medium:
				currentDotProductLevel = dotProductLevelTwo;
				currentWorkLevel = workLevelTwo;
				break;
			case Difficulty.Hard:
				currentDotProductLevel = dotProductLevelThree;
				currentWorkLevel = workLevelThree;
				break;
		}
	}

	private void SubscribeViewAndDisplayEvents()
	{
		dotProductView.OpenViewEvent += () => isDotProductViewActive = true;
		dotProductView.QuitViewEvent += () => isDotProductViewActive = false;
		dotProductView.SubmitAnswerEvent += CheckDotProductAnswers;
		dotProductSubmissionStatusDisplay.ProceedEvent += UpdateDotProductViewState;

		workView.OpenViewEvent += UpdateWorkSubActivityStateMachine;
		workView.OpenViewEvent += () =>
		{
			isWorkViewActive = true;
			SetMissionObjectiveDisplay(false);
		};
		workView.QuitViewEvent += () =>
		{
			isWorkViewActive = false;
			SetMissionObjectiveDisplay(true);
		};
		workView.SubmitAnswerEvent += CheckWorkSubActivityAnswers;
		workSubmissionStatusDisplay.ProceedEvent += UpdateWorkViewState;

		workGraphInterpretationView.OpenViewEvent += () => isWorkGraphInterpretationViewActive = true;
		workGraphInterpretationView.QuitViewEvent += () => isWorkGraphInterpretationViewActive = false;
		workGraphInterpretationView.SubmitAnswerEvent += CheckWorkGraphInterpretationAnswer;
		workGraphSubmissionStatusDisplay.ProceedEvent += UpdateWorkGraphInterpretationViewState;
	}

	#region Dot Product
	private void GenerateDotProductGivenData(DotProductSubActivitySO dotProductSO)
	{
		DotProductData data = new DotProductData();
		data.satelliteDishVector = GenerateRandomVector(dotProductSO.satelliteDishVectorMin, dotProductSO.satelliteDishVectorMax);
		data.targetObjectVector = GenerateRandomVector(dotProductSO.targetObjectVectorMin, dotProductSO.targetObjectVectorMax);
		dotProductGivenData = data;
	}

	private Vector3 GenerateRandomVector(Vector3 vectorMinThreshold, Vector3 vectorMaxThreshold)
	{
		Vector3 randomVector = new Vector3();
		randomVector.x = (float) Math.Round(Random.Range(vectorMinThreshold.x, vectorMaxThreshold.x), 1);
		randomVector.y = (float) Math.Round(Random.Range(vectorMinThreshold.y, vectorMaxThreshold.y), 1);
		randomVector.z = (float) Math.Round(Random.Range(vectorMinThreshold.z, vectorMaxThreshold.z), 1);
		return randomVector;
	}

	private void CheckDotProductAnswers(DotProductAnswerSubmission answer)
	{
		DotProductAnswerSubmissionResults results = ActivitySixUtilities.ValidateDotProductSubmission(answer, dotProductGivenData);

		if (results.isAllCorrect()) {
			SceneSoundManager.Instance.PlaySFX("UI_Buttonconfirm_Stereo_01");

			numCorrectDotProductSubmission++;
			currentNumDotProductTests--;
		} else
		{
			SceneSoundManager.Instance.PlaySFX("UI_Forbidden_Stereo_02");

			numIncorrectDotProductSubmission++;
		}

		DisplayDotProductSubmissionResults(results);
	}

	private void DisplayDotProductSubmissionResults(DotProductAnswerSubmissionResults results)
	{
		if (results.isAllCorrect())
		{
			dotProductSubmissionStatusDisplay.SetSubmissionStatus(true, "The submitted calculations are correct.");
			missionObjectiveDisplayUI.UpdateMissionObjectiveText(0, $"Monitor the Satellite's Direction by Computing for the Dot Product on the Satellite Control Panel ({currentDotProductLevel.numberOfTests - currentNumDotProductTests}/{currentDotProductLevel.numberOfTests})");
		}
		else
		{
			dotProductSubmissionStatusDisplay.SetSubmissionStatus(false, "There are errors in your submission. Please review and fix it.");
		}

		dotProductSubmissionStatusDisplay.UpdateStatusBorderDisplaysFromResult(results);

		dotProductSubmissionStatusDisplay.gameObject.SetActive(true);
	}

	private void UpdateDotProductViewState()
	{
		if (currentNumDotProductTests > 0)
		{
			GenerateDotProductGivenData(currentDotProductLevel);
			dotProductView.SetupDotProductView(dotProductGivenData);
		} else
		{
			isDotProductSubActivityFinished = true;
			dotProductView.gameObject.SetActive(false);
			MainSatelliteAreaClearEvent?.Invoke();
		}
	}
	#endregion

	#region Work
	private void GenerateWorkSubActivityGivenData(WorkSubActivitySO workSubActivitySO, WorkSubActivityState workSubActivityState)
	{
		WorkSubActivityData data = new WorkSubActivityData();
		data.workSubActivityState = workSubActivityState;
		data.acceleration = (float) Math.Round(Random.Range(workSubActivitySO.accelerationMinVal, workSubActivitySO.accelerationMaxVal), 2);
		data.mass = (float) Math.Round(Random.Range(workSubActivitySO.massMinVal, workSubActivitySO.massMinVal), 2);
		data.displacement = (float)Math.Round(Random.Range(workSubActivitySO.displacementMinVal, workSubActivitySO.displacementMaxVal), 2);
		data.angleMeasure = Random.Range(workSubActivitySO.degreeMinVal, workSubActivitySO.degreeMaxVal);
		workSubActivityGivenData = data;
	}

	private void UpdateWorkSubActivityStateMachine()
	{
		if (workSubActivityStateQueue.Count == 0)
		{
			workSubActivityStateMachine.TransitionToState(WorkSubActivityState.None);
		}
		else
		{
			WorkSubActivityState queueSubActivityHead = workSubActivityStateQueue.Peek();
			workSubActivityStateMachine.TransitionToState(queueSubActivityHead);
		}
	}

	private void InitializeWorkSubActivityStateQueues()
	{
		workSubActivityStateQueue = new Queue<WorkSubActivityState>();
		for (int i = 0; i < currentWorkLevel.numberOfRepetitions; i++)
		{
			workSubActivityStateQueue.Enqueue(WorkSubActivityState.LinearWork);
			workSubActivityStateQueue.Enqueue(WorkSubActivityState.AngularWork);
			workSubActivityStateQueue.Enqueue(WorkSubActivityState.LinearWork);
		}
	}

	private void CheckWorkSubActivityAnswers(WorkSubActivityAnswerSubmission answer)
	{
		WorkSubActivityAnswerSubmissionResults results = ActivitySixUtilities.ValidateWorkSubActivitySubmission(answer, workSubActivityGivenData);
		if (results.isAllCorrect())
		{
			SceneSoundManager.Instance.PlaySFX("UI_Buttonconfirm_Stereo_01");

			numCorrectWorkSubmission++;
		} else
		{
			SceneSoundManager.Instance.PlaySFX("UI_Forbidden_Stereo_02");

			numIncorrectWorkSubmission++;
		}

		DisplayWorkSubActivitySubmissionResults(results);
	}

	private void DisplayWorkSubActivitySubmissionResults(WorkSubActivityAnswerSubmissionResults results)
	{
		if (results.isAllCorrect())
		{
			workSubmissionStatusDisplay.SetSubmissionStatus(true, "The submitted calculations are correct.");
		}
		else
		{
			workSubmissionStatusDisplay.SetSubmissionStatus(false, "There are errors in your submission. Please review and fix it.");
		}

		workSubmissionStatusDisplay.UpdateStatusBorderDisplaysFromResult(results);

		workSubmissionStatusDisplay.gameObject.SetActive(true);
	}

	private void UpdateWorkViewState()
	{
		workSubActivityStateQueue.Dequeue();
		missionObjectiveDisplayUI.UpdateMissionObjectiveText(1, $"Embark on the Satellite Truck and Monitor the Work Exerted by the Vehicle ({currentWorkLevel.numberOfRepetitions * 3 - workSubActivityStateQueue.Count}/{currentWorkLevel.numberOfRepetitions * 3})");
		if (workSubActivityStateQueue.Count > 0)
		{
			UpdateWorkSubActivityStateMachine();
			GenerateWorkSubActivityGivenData(currentWorkLevel, workSubActivityStateQueue.Peek());
			workView.SetupWorkView(workSubActivityGivenData, workSubActivityStateQueue.Peek());
		}
		else
		{
			isWorkSubActivityFinished = true;
			workView.gameObject.SetActive(false);
		}
	}
	#endregion

	#region Work Graph Interpretation

	private void GenerateForceDisplacementGraphData()
	{
		Dictionary<ForceDisplacementCurveType, List<Vector2>> data = new Dictionary<ForceDisplacementCurveType, List<Vector2>>();

		// Constant
		List<Vector2> constantForceGraphPoints = new List<Vector2>();
		int constantVal = Random.Range(0, 10);
		for (int i = 0; i < 11; i++)
		{
			constantForceGraphPoints.Add(new Vector2(i, constantVal));
		}
		data.Add(ForceDisplacementCurveType.ConstantForceGraph, constantForceGraphPoints);

		// Linearly ascending
		List<Vector2> linearlyIncreasingForceGraphPoints = new List<Vector2>();
		for (int i = 0; i < 11; i++)
		{
			linearlyIncreasingForceGraphPoints.Add(new Vector2(i, i));
		}
		data.Add(ForceDisplacementCurveType.LinearlyIncreasingForceGraph, linearlyIncreasingForceGraphPoints);

		// Linearly descending
		List<Vector2> linearlyDecreasingForceGraphPoints = new List<Vector2>();
		for (int i = 0; i < 11; i++)
		{
			linearlyDecreasingForceGraphPoints.Add(new Vector2(i, 10-i));
		}
		data.Add(ForceDisplacementCurveType.LinearlyDecreasingForceGraph, linearlyDecreasingForceGraphPoints);

		forceDisplacementGraphData = data;
	}

	private void UpdateCurrentGraphTypeRandomly()
	{
		List<ForceDisplacementCurveType> keys = forceDisplacementGraphData.Keys.ToList();

		// Randomly select one key
		int randomIndex = Random.Range(0, keys.Count);
		currentGraphTypeDisplayed = keys[randomIndex];
	}

	private void CheckWorkGraphInterpretationAnswer(float? answer)
	{
		bool result = ActivitySixUtilities.ValidateWorkGraphInterpretationSubmission(answer, forceDisplacementGraphData, currentGraphTypeDisplayed);
		if (result)
		{
			SceneSoundManager.Instance.PlaySFX("UI_Buttonconfirm_Stereo_01");

			currentNumWorkGraphTests--;
			numCorrectWorkGraphSubmission++;
			forceDisplacementGraphData.Remove(currentGraphTypeDisplayed);
		}
		else
		{
			SceneSoundManager.Instance.PlaySFX("UI_Forbidden_Stereo_02");

			numIncorrectWorkGraphSubmission++;
		}
		DisplayWorkGraphInterpretationSubmissionResults(result);
	}

	private void DisplayWorkGraphInterpretationSubmissionResults(bool result)
	{
		if (result)
		{
			workGraphSubmissionStatusDisplay.SetSubmissionStatus(true, "The submitted calculations are correct.");
			missionObjectiveDisplayUI.UpdateMissionObjectiveText(2, $"Find the Crashed Drone and Determine the Net Work Exerted from its Force vs. Displacement Graph Data ({totalWorkGraphTests - currentNumWorkGraphTests}/{totalWorkGraphTests})");
		}
		else
		{
			workGraphSubmissionStatusDisplay.SetSubmissionStatus(false, "There are errors in your submission. Please review and fix it.");
		}

		workGraphSubmissionStatusDisplay.UpdateStatusBorderDisplaysFromResult(result);

		workGraphSubmissionStatusDisplay.gameObject.SetActive(true);
	}
	 private void UpdateWorkGraphInterpretationViewState()
	{
		if (currentNumWorkGraphTests > 0)
		{
			UpdateCurrentGraphTypeRandomly();
			workGraphInterpretationView.SetupWorkGraphInterpretationView(forceDisplacementGraphData, currentGraphTypeDisplayed);
		}
		else
		{
			isWorkGraphSubActivityFinished = true;
			workGraphInterpretationView.gameObject.SetActive(false);
		}
	}
	#endregion

	protected override void AddAttemptRecord()
	{
		Dictionary<string, object> dotProductResults = new Dictionary<string, object>
		{
			{ "fields", new Dictionary<string, FirestoreField>
				{
					{ "isAccomplished", new FirestoreField(isDotProductSubActivityFinished) },
					{ "mistakes", new FirestoreField(numIncorrectDotProductSubmission) },
					{ "durationInSec", new FirestoreField((int)dotProductGameplayDuration) }
				}
			}
		};

		Dictionary<string, object> workResults = new Dictionary<string, object>
		{
			{ "fields", new Dictionary<string, FirestoreField>
				{
					{ "isAccomplished", new FirestoreField(isWorkSubActivityFinished) },
					{ "mistakes", new FirestoreField(numIncorrectWorkSubmission) },
					{ "durationInSec", new FirestoreField((int)workSubActivityGameplayDuration) }
				}
			}
		};

		Dictionary<string, object> workGraphInterpretationResults = new Dictionary<string, object>
		{
			{ "fields", new Dictionary<string, FirestoreField>
				{
					{ "isAccomplished", new FirestoreField(isWorkGraphSubActivityFinished) },
					{ "mistakes", new FirestoreField(numIncorrectWorkGraphSubmission) },
					{ "durationInSec", new FirestoreField((int)workGraphSubActivityDuration) }
				}
			}
		};

		Dictionary<string, object> results = new Dictionary<string, object>
		{
			{ "fields", new Dictionary<string, object>
				{
					{ "dotProduct", new FirestoreField(dotProductResults)},
					{ "work", new FirestoreField(workResults)},
					{ "workGraphInterpretation", new FirestoreField(workGraphInterpretationResults)},
				}
			}
		};

		Dictionary<string, FirestoreField> fields = new Dictionary<string, FirestoreField>
		{
			{ "dateAttempted", new FirestoreField(DateTime.UtcNow) },
			{ "difficulty", new FirestoreField($"{difficultyConfiguration}") },
			{ "results", new FirestoreField(results) },
			{ "isAccomplished", new FirestoreField(isDotProductSubActivityFinished && isWorkSubActivityFinished && isWorkGraphSubActivityFinished) },
			{ "studentId", new FirestoreField(UserManager.Instance.CurrentUser.localId) },
			{ "totalDurationInSec", new FirestoreField((int)(dotProductGameplayDuration + workSubActivityGameplayDuration + workGraphSubActivityDuration)) }
		};

		StartCoroutine(UserManager.Instance.CreateAttemptDocument(fields, "activitySixAttempts"));
	}

    protected override void SetNextLevelButtonState()
    {
        bool isAccomplished = isDotProductSubActivityFinished && isWorkSubActivityFinished && isWorkGraphSubActivityFinished;

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
                    if (currentUserUnlockedLesson == 6 && currentUserHighestLessonUnlockedDifficulty == 1)
                    {
                        Dictionary<string, FirestoreField> fields = new Dictionary<string, FirestoreField>
                        {
                            {"highestUnlockedLesson", new FirestoreField(6) },
                            {"highestLessonUnlockedDifficulty", new FirestoreField(2) }
                        };

                        StartCoroutine(UserManager.Instance.UpdateUnlockedLevels(fields, currentUserLocalID, (success) =>
                        {
                            if (success)
                            {
                                newLevelUnlockedScreen.gameObject.SetActive(true);
                                StartCoroutine(newLevelUnlockedScreen.SetNewLevelUnlockedScreen("Lesson 6 - <color=#C5B501>Medium"));
                                StartCoroutine(UserManager.Instance.GetUnlockedLevels(currentUserLocalID, HandleUnlockedLevelsChange));
                            }
                            else { Debug.LogError("Failed To Update Unlocked Levels"); }
                        }));
                    }
                    break;

                // If the player accomplished in medium mode, check if the unlocked levels need to progress with respect to the current lesson
                case Difficulty.Medium:
                    if (currentUserUnlockedLesson == 6 && currentUserHighestLessonUnlockedDifficulty == 2)
                    {
                        Dictionary<string, FirestoreField> fields = new Dictionary<string, FirestoreField>
                        {
                            {"highestUnlockedLesson", new FirestoreField(6) },
                            {"highestLessonUnlockedDifficulty", new FirestoreField(3) }
                        };

                        StartCoroutine(UserManager.Instance.UpdateUnlockedLevels(fields, currentUserLocalID, (success) =>
                        {
                            if (success)
                            {
                                newLevelUnlockedScreen.gameObject.SetActive(true);
                                StartCoroutine(newLevelUnlockedScreen.SetNewLevelUnlockedScreen("Lesson 6 - <color=#FF0000>Hard"));
                                StartCoroutine(UserManager.Instance.GetUnlockedLevels(currentUserLocalID, HandleUnlockedLevelsChange));
                            }
                            else { Debug.LogError("Failed To Update Unlocked Levels"); }
                        }));
                    }
                    break;

                // If the player accomplished in hard mode, check if the unlocked levels need to progress with respect to the current lesson
                case Difficulty.Hard:
                    if (currentUserUnlockedLesson == 6 && currentUserHighestLessonUnlockedDifficulty == 3)
                    {
                        Dictionary<string, FirestoreField> fields = new Dictionary<string, FirestoreField>
                        {
                            {"highestUnlockedLesson", new FirestoreField(7) },
                            {"highestLessonUnlockedDifficulty", new FirestoreField(1) }
                        };

                        StartCoroutine(UserManager.Instance.UpdateUnlockedLevels(fields, currentUserLocalID, (success) =>
                        {
                            if (success)
                            {
                                newLevelUnlockedScreen.gameObject.SetActive(true);
                                StartCoroutine(newLevelUnlockedScreen.SetNewLevelUnlockedScreen("Lesson 7 - <color=#21B200>Easy"));
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

        Debug.Log($"Level Finished: {isDotProductSubActivityFinished && isWorkSubActivityFinished && isWorkGraphSubActivityFinished}");
    }

    public override void DisplayPerformanceView()
	{
		base.DisplayPerformanceView();

		inputReader.SetUI();
		performanceView.gameObject.SetActive(true);

		performanceView.SetTotalTimeDisplay(dotProductGameplayDuration + workSubActivityGameplayDuration + workGraphSubActivityDuration);

		performanceView.SetDotProductMetricsDisplay(
			isAccomplished: isDotProductSubActivityFinished,
			numIncorrectSubmission: numIncorrectDotProductSubmission,
			duration: dotProductGameplayDuration
			);

		performanceView.SetWorkSubActivityMetricsDisplay(
			isAccomplished: isWorkSubActivityFinished,
			numIncorrectSubmission: numIncorrectWorkSubmission,
			duration: workSubActivityGameplayDuration
			);

		performanceView.SetWorkGraphInterpretationMetricsDisplay(
			isAccomplished: isWorkGraphSubActivityFinished,
			numIncorrectSubmission: numIncorrectWorkGraphSubmission,
			duration: workGraphSubActivityDuration
			);

		// Update its activity feedback display (three args)
		performanceView.UpdateActivityFeedbackDisplay(
			new SubActivityPerformanceMetric(
				subActivityName: "dot product",
				isSubActivityFinished: isDotProductSubActivityFinished,
				numIncorrectAnswers: numIncorrectDotProductSubmission,
				numCorrectAnswers: numCorrectDotProductSubmission,
				badScoreThreshold: 3,
				averageScoreThreshold: 2
				),
			new SubActivityPerformanceMetric(
				subActivityName: "work calculation",
				isSubActivityFinished: isWorkSubActivityFinished,
				numIncorrectAnswers: numIncorrectWorkSubmission,
				numCorrectAnswers: numCorrectWorkSubmission,
				badScoreThreshold: 5,
				averageScoreThreshold: 3
				),
			new SubActivityPerformanceMetric(
				subActivityName: "work graph interpretation",
				isSubActivityFinished: isWorkGraphSubActivityFinished,
				numIncorrectAnswers: numIncorrectWorkGraphSubmission,
				numCorrectAnswers: numCorrectWorkGraphSubmission,
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
		if (!isDotProductSubActivityFinished)
		{
			taskText.Add("- Find and open the satellite control panel. Monitor the satellite antenna's direction to the target object by computing the dot product.");
		}
		if (!isWorkSubActivityFinished)
		{
			taskText.Add("- Embark on the satellite truck. Monitor the work done by the vehicle while travelling on the land of Nakalais.");
		}
		if (!isWorkGraphSubActivityFinished)
		{
			taskText.Add("- Find and interact with the drone at the crash site. Determine the net work exerted from the Force vs. Displacement graph data.");
		}

		List<string> objectiveText = new List<string>();
		objectiveText.Add("Properly adjust the satellite’s direction, observe the work done by the vehicle, " +
			"and investigate the crash site of the drone from the previous team.");

		activityPauseMenuUI.UpdateContent("Lesson 6 - Activity 6", taskText, objectiveText);
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
        // If the current unlocked lesson is higher than lesson 6, allow user to proceed to next level
        if (currentUnlockedLesson > 6) { performanceView.SetNextLevelButtonState(true); Debug.Log("Next button state is interactable"); return; }

        switch (difficultyConfiguration)
        {
            // If player completed easy, check if current unlocked lesson is greater than or equal to 6
            // and if current unlocked difficulty is greater than 1 or Easy mode, then allow user to proceed to next level
            case Difficulty.Easy:
                if (!(currentUnlockedLesson >= 6)) { performanceView.SetNextLevelButtonState(false); return; }
                if (!(currentHighestLessonUnlockedDifficulty > 1)) { performanceView.SetNextLevelButtonState(false); return; }
                performanceView.SetNextLevelButtonState(true); Debug.Log("Next button state is interactable");
                break;

            // If player completed easy, check if current unlocked lesson is greater than or equal to 6
            // and if current unlocked difficulty is greater than 2 or Medium mode, then allow user to proceed to next level
            case Difficulty.Medium:
                if (!(currentUnlockedLesson >= 6)) { performanceView.SetNextLevelButtonState(false); return; }
                if (!(currentHighestLessonUnlockedDifficulty > 2)) { performanceView.SetNextLevelButtonState(false); return; }
                performanceView.SetNextLevelButtonState(true); Debug.Log("Next button state is interactable");
                break;

                // Don't need case for hard difficulty since the if statement before the switch case already handles it
        }
    }
}