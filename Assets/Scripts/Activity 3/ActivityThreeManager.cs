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

		ConfigureLevelData(Difficulty.Easy);

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
			numCorrectGraphsSubmission++;
			currentNumGraphsTests--;
		}
		else
		{
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
			graphsView.UpdateTestCountTextDisplay(currentGraphsLevel.numberOfTests - currentNumGraphsTests, currentGraphsLevel.numberOfTests);
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
			numCorrectAccelerationSubmission++;
			currentNumAccelerationTests--;
		}
		else
		{
			numIncorrectAccelerationSubmission++;
		}

		DisplayAccelerationSubmissionResults(isAccelerationCorrect);
	}

	private void CheckTotalDepthAnswer(float? answer)
	{
		bool isTotalDepthCorrect = ActivityThreeUtilities.ValidateTotalDepthSubmission(answer, givenTotalDepthData);

		if (isTotalDepthCorrect)
		{
			numCorrectTotalDepthSubmission++;
			currentNumTotalDepthTests--;
		}
		else
		{
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
				kinematics1DView.UpdateTestCountTextDisplay(currentKinematics1DLevel.numberOfAccelerationProblems - currentNumAccelerationTests, currentKinematics1DLevel.numberOfAccelerationProblems);
				kinematics1DView.UpdateAccelerationInfo(givenAccelerationData);
			}
			else
			{
				isAccelerationCalcFinished = true;
				kinematics1DView.DisplayTotalDepthInfo();
				missionObjectiveDisplayUI.ClearMissionObjective(1);
				// INSERT EVENT FOR ENV ANIMATION
			}
		} else
		{
			if (currentNumTotalDepthTests > 0)
			{
				GenerateTotalDepthGivenData();
				kinematics1DView.UpdateTestCountTextDisplay(currentKinematics1DLevel.numberOfTotalDepthProblems - currentNumTotalDepthTests, currentKinematics1DLevel.numberOfTotalDepthProblems);
				kinematics1DView.UpdateTotalDepthInfo(givenTotalDepthData);
			}
			else
			{
				isTotalDepthCalcFinished = true;
				missionObjectiveDisplayUI.ClearMissionObjective(2);
				kinematics1DView.gameObject.SetActive(false);
				// INSERT EVENT FOR ENV ANIMATION
			}
		}
	}
	#endregion

	public override void DisplayPerformanceView()
	{
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

	/*private void Start()
	{
        GraphEditButton.InitiateGraphEditViewSwitch += ChangeViewToGraphEditView;
		ViewGraphEdit.InitiateGraphViewSwitch += ChangeViewToGraphView;
		ViewGraphs.SubmitGraphsAnswerEvent += CheckGraphsAnswer;
		GraphSubmissionModalWindow.RetryGraphSubmission += RestoreGraphViewState;
		GraphSubmissionModalWindow.InitiateNextSubActivity += ChangeViewToCalculatorView;

		View1DKinematics.SubmitAccelerationAnswerEvent += CheckAccelerationAnswer;
		View1DKinematics.SubmitFreeFallAnswerEvent += CheckFreeFallAnswer;
		Kinematics1DSubmissionModalWindow.RetrySubmission += Restore1DKinematicsViewState;
		Kinematics1DSubmissionModalWindow.InitiateNext += Update1DKinematicsView;

		ViewActivityThreePerformance.OpenViewEvent += OnOpenViewActivityThreePerformance;

		currentGraphsLevel = graphsLevelOne; // alter in the future, on changing upon submission in loading of scene.
		currentKinematics1DLevel = kinematics1DLevelOne;

		InitializeCorrectGraphValues();
		graphManager.SetupGraphs(correctPositionValues, correctVelocityValues, correctAccelerationValues);

		Initialize1DKinematicsGiven();
		view1DKinematics.SetupAccelerationGivenText(initialVelocityValue, finalVelocityValue, totalTimeValueOne);
		view1DKinematics.SetupFreeFallGivenText(totalTimeValueTwo);
	}

	#region Graphs Sub Activity
	private void ChangeViewToGraphEditView(Graph graph)
    {
		mainCamera.enabled = false;
		viewGraphEdit.gameObject.SetActive(true);
		viewGraphEdit.interactiveGraphCamera = graph.interactiveGraphCamera;
		viewGraphs.gameObject.SetActive(false);
		graphManager.currentGraph = graph;
    }

	private void ChangeViewToGraphView()
	{
		mainCamera.enabled = true;
		viewGraphEdit.gameObject.SetActive(false);
		viewGraphEdit.interactiveGraphCamera = null;
		viewGraphs.gameObject.SetActive(true);
		graphManager.currentGraph = null;
	}

	private void ChangeViewToCalculatorView()
	{
		viewGraphs.gameObject.SetActive(false);
		graphSubmissionModalWindow.gameObject.SetActive(false);
		view1DKinematics.gameObject.SetActive(true);
	}

	private void InitializeCorrectGraphValues()
	{
		int datasetSize = currentGraphsLevel.datasets[0].dataset.Count;
		int randomDatasetIndex = Random.Range(0, datasetSize);

		foreach (GraphDataset graphDataset in currentGraphsLevel.datasets)
		{
			List<int> graphPointValues = graphDataset.dataset[randomDatasetIndex].Split(',').Select(int.Parse).ToList();
			
			switch (graphDataset.datasetType)
			{
				case DatasetType.Position:
					correctPositionValues = graphPointValues;
					break;
				case DatasetType.Velocity:
					correctVelocityValues = graphPointValues;
					break;
				case DatasetType.Acceleration:
					correctAccelerationValues = graphPointValues;
					break;
			}
		}
	}

	private void CheckGraphsAnswer()
	{
		Graph positionVsTimeGraph = graphManager.positionVsTimeGraph;
		Graph velocityVsTimeGraph = graphManager.velocityVsTimeGraph;
		Graph accelerationVsTimeGraph = graphManager.accelerationVsTimeGraph;

		bool isPositionVsTimeGraphCorrect = ActivityThreeUtilities.ValidateGraphSubmission(correctPositionValues, positionVsTimeGraph);
		bool isVelocityVsTimeGraphCorrect = ActivityThreeUtilities.ValidateGraphSubmission(correctVelocityValues, velocityVsTimeGraph);
		bool isAccelerationVsTimeGraphCorrect = ActivityThreeUtilities.ValidateGraphSubmission(correctAccelerationValues, accelerationVsTimeGraph);

		if (isPositionVsTimeGraphCorrect && isVelocityVsTimeGraphCorrect && isAccelerationVsTimeGraphCorrect)
		{
			isGraphsSubActivityFinished = true;
		}

		if (!isPositionVsTimeGraphCorrect)
		{
			numIncorrectGraphsSubmission++;
		}

		if (!isVelocityVsTimeGraphCorrect)
		{
			numIncorrectGraphsSubmission++;
		}

		if (!isAccelerationVsTimeGraphCorrect)
		{
			numIncorrectGraphsSubmission++;
		}

		Debug.Log(isPositionVsTimeGraphCorrect);
		Debug.Log(isVelocityVsTimeGraphCorrect);
		Debug.Log(isAccelerationVsTimeGraphCorrect);

		// Showing overlay and graph submission modal window.
		viewGraphs.overlay.gameObject.SetActive(true);
		graphSubmissionModalWindow.gameObject.SetActive(true);
		graphSubmissionModalWindow.SetDisplayFromSubmissionResult(isPositionVsTimeGraphCorrect, isVelocityVsTimeGraphCorrect, isAccelerationVsTimeGraphCorrect);
	}

	private void RestoreGraphViewState()
	{
		viewGraphs.overlay.gameObject.SetActive(false);
		graphSubmissionModalWindow.gameObject.SetActive(false);
	}

	#endregion

	#region 1D Kinematics Sub Activity
	private void Initialize1DKinematicsGiven()
	{
		initialVelocityValue = Random.Range(currentKinematics1DLevel.minimumVelocityValue, currentKinematics1DLevel.maximumVelocityValue);
		finalVelocityValue = Random.Range(currentKinematics1DLevel.minimumVelocityValue, currentKinematics1DLevel.maximumVelocityValue);
		totalTimeValueOne = Random.Range(currentKinematics1DLevel.minimumTimeValue, currentKinematics1DLevel.maximumTimeValue);
		totalTimeValueTwo = Random.Range(currentKinematics1DLevel.minimumTimeValue, currentKinematics1DLevel.maximumTimeValue);
	}

	private void CheckAccelerationAnswer(float accelerationAnswer)
	{
		bool isAccelerationCorrect = ActivityThreeUtilities.ValidateAccelerationSubmission(accelerationAnswer, initialVelocityValue, finalVelocityValue, totalTimeValueOne);
		if (isAccelerationCorrect)
		{
			isAccelerationCalculationFinished = true;
		} else
		{
			numIncorrectAccelerationSubmission++;
		}

		view1DKinematics.overlay.gameObject.SetActive(true);
		kinematics1DSubmissionModalWindow.gameObject.SetActive(true);
		kinematics1DSubmissionModalWindow.SetDisplayFromSubmissionResult(isAccelerationCorrect, "Acceleration");
	}

	private void CheckFreeFallAnswer(float freeFallAnswer)
	{
		bool isFreeFallCorrect = ActivityThreeUtilities.ValidateFreeFallSubmission(freeFallAnswer, totalTimeValueTwo);
		if (isFreeFallCorrect)
		{
			isFreeFallCalculationFinished = true;
		} else
		{
			numIncorrectFreeFallSubmission++;
		}

		view1DKinematics.overlay.gameObject.SetActive(true);
		kinematics1DSubmissionModalWindow.gameObject.SetActive(true);
		kinematics1DSubmissionModalWindow.SetDisplayFromSubmissionResult(isFreeFallCorrect, "Free Fall");
	}

	private void Restore1DKinematicsViewState()
	{
		view1DKinematics.overlay.gameObject.SetActive(false);
		kinematics1DSubmissionModalWindow.gameObject.SetActive(false);
	}

	private void Update1DKinematicsView()
	{
		if (isAccelerationCalculationFinished && isFreeFallCalculationFinished)
		{
			viewActivityThreePerformance.gameObject.SetActive(true);
		} else if (isAccelerationCalculationFinished && !isFreeFallCalculationFinished)
		{
			view1DKinematics.SwitchToFreeFallView();
		}

		view1DKinematics.overlay.gameObject.SetActive(false);
		kinematics1DSubmissionModalWindow.gameObject.SetActive(false);
	}

	#endregion

	private void OnOpenViewActivityThreePerformance(ViewActivityThreePerformance view)
	{
		view.GraphsStatusText.text += isGraphsSubActivityFinished ? "Accomplished" : "Not accomplished";
		view.GraphsIncorrectNumText.text = $"Number of Incorrect Submissions: {numIncorrectGraphsSubmission}";

		view.Kinematics1DStatusText.text += isAccelerationCalculationFinished && isFreeFallCalculationFinished ? "Accomplished" : "Not accmplished";

		view.AccelerationProblemStatusText.text += isAccelerationCalculationFinished ? "Accomplished" : "Not accomplished";
		view.AccelerationProblemIncorrectNumText.text = $"Number of Incorrect Submissions: {numIncorrectAccelerationSubmission}";

		view.FreeFallProblemStatusText.text += isFreeFallCalculationFinished ? "Accomplished" : "Not accomplished";
		view.FreeFallProblemIncorrectNumText.text = $"Number of Incorrect Submissions: {numIncorrectFreeFallSubmission}";
	}*/
}