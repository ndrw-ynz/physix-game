using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GraphsSubActivitySO;

public class ActivityThreeManager : MonoBehaviour
{
	public static Difficulty difficultyConfiguration;

	[Header("Level Data - Graphs")]
	[SerializeField] private GraphsSubActivitySO graphsLevelOne;
	[SerializeField] private GraphsSubActivitySO graphsLevelTwo;
	[SerializeField] private GraphsSubActivitySO graphsLevelThree;
	private GraphsSubActivitySO currentGraphsLevel;

	[Header("Level Data - 1D Kinematics")]
	[SerializeField] private Kinematics1DSubActivitySO kinematics1DLevelOne;
	private Kinematics1DSubActivitySO currentKinematics1DLevel;

	[Header("Managers")]
	[SerializeField] private GraphManager graphManager;

	[Header("Views")]
	[SerializeField] private GraphsView graphsView;
	[SerializeField] private GraphEditorUI graphEditorUI;
	[SerializeField] private GraphViewerUI graphViewerUI;
	
	[Header("Submission Status Displays")]
	[SerializeField] private GraphsSubmissionStatusDisplay graphsSubmissionStatusDisplay;

	[Header("Modal Windows")]
	[SerializeField] private GraphSubmissionModalWindow graphSubmissionModalWindow;
	[SerializeField] private Kinematics1DSubmissionModalWindow kinematics1DSubmissionModalWindow;

	// Given graph values
	private List<int> correctPositionValues;
	private List<int> correctVelocityValues;
	private List<int> correctAccelerationValues;

	// Gameplay performance metrics variables
	// Graphs Sub Activity
	private float graphsGameplayDuration;
	private bool isGraphsSubActivityFinished;
	private int numIncorrectGraphsSubmission;
	private int numCorrectGraphsSubmission;

	[Header("Metrics for 1D Kinematics Subactivity")]
	public bool isAccelerationCalculationFinished;
	public bool isFreeFallCalculationFinished;
	private int numIncorrectAccelerationSubmission;
	private int numIncorrectFreeFallSubmission;

	[Header("Generated Given Values")]
	private int initialVelocityValue;
	private int finalVelocityValue;
	private int totalTimeValueOne;
	private int totalTimeValueTwo;

	private void Start()
	{
		ConfigureLevelData(Difficulty.Easy);

		SubscribeViewAndDisplayEvents();

		InitializeCorrectGraphValues();
		graphManager.SetupGraphs(correctPositionValues);
	}

	private void ConfigureLevelData(Difficulty difficulty)
	{
		difficultyConfiguration = difficulty;

		switch (difficulty)
		{
			case Difficulty.Easy:
				currentGraphsLevel = graphsLevelOne;
				// currentKinematics1DLevel = kinematics1DLevelOne;
				break;
			case Difficulty.Medium:
				currentGraphsLevel = graphsLevelTwo;
				// currentKinematics1DLevel = kinematics1DLevelTwo;
				break;
			case Difficulty.Hard:
				currentGraphsLevel = graphsLevelThree;
				// currentKinematics1DLevel = kinematics1DLevelThree;
				break;
		}
	}

	private void SubscribeViewAndDisplayEvents()
	{
		// Graphs Sub Activity Related Events
		graphEditorUI.QuitGraphEditorEvent += () => graphsView.gameObject.SetActive(true);
		graphViewerUI.QuitGraphViewerEvent += () => graphsView.gameObject.SetActive(true);
		graphsView.SubmitAnswerEvent += CheckGraphsAnswer;
	}

	#region Graphs
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

	private void CheckGraphsAnswer(GraphsAnswerSubmission answer)
	{
		GraphsAnswerSubmissionResults results = ActivityThreeUtilities.ValidateGraphSubmission(answer, correctPositionValues, correctVelocityValues, correctAccelerationValues);

		if (results.isAllCorrect())
		{
			numCorrectGraphsSubmission++;
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
			//missionObjectiveDisplayUI.UpdateMissionObjectiveText(0, $"");
		}
		else
		{
			graphsSubmissionStatusDisplay.SetSubmissionStatus(false, "Engineer, there seems to be a mistake. Let's try again!");
		}

		graphsSubmissionStatusDisplay.UpdateStatusBorderDisplayFromResults(results);

		graphsSubmissionStatusDisplay.gameObject.SetActive(true);
	}
	#endregion


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