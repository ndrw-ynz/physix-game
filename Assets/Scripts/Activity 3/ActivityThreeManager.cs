using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GraphsSubActivitySO;

public class ActivityThreeManager : MonoBehaviour
{
	[Header("Level Data - Graphs")]
	[SerializeField] private GraphsSubActivitySO graphsLevelOne;
	[SerializeField] private GraphsSubActivitySO graphsLevelTwo;
	[SerializeField] private GraphsSubActivitySO graphsLevelThree;
	public GraphsSubActivitySO currentGraphsLevel;

	[Header("Level Data - 1D Kinematics")]
	[SerializeField] private Kinematics1DSubActivitySO kinematics1DLevelOne;
	public Kinematics1DSubActivitySO currentKinematics1DLevel;

	[Header("Managers")]
	[SerializeField] private GraphManager graphManager;

    [Header("Cameras")]
	[SerializeField] private Camera mainCamera;
	[SerializeField] private Camera positionVsTimeGraphCamera;
	[SerializeField] private Camera velocityVsTimeGraphCamera;
	[SerializeField] private Camera accelerationVsTimeGraphCamera;

	[Header("Views")]
    [SerializeField] private ViewGraphs viewGraphs;
    [SerializeField] private ViewGraphEdit viewGraphEdit;
	[SerializeField] private View1DKinematics view1DKinematics;
	[SerializeField] private ViewActivityThreePerformance viewActivityThreePerformance;

	[Header("Modal Windows")]
	[SerializeField] private GraphSubmissionModalWindow graphSubmissionModalWindow;
	[SerializeField] private Kinematics1DSubmissionModalWindow kinematics1DSubmissionModalWindow;

	private List<int> correctPositionValues;
	private List<int> correctVelocityValues;
	private List<int> correctAccelerationValues;

	[Header("Metrics for Graphs Subactivity")]
	public bool isGraphsSubActivityFinished;
	private int numIncorrectGraphsSubmission;

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
		foreach (GraphDataset graphDataset in currentGraphsLevel.datasets)
		{
			string rawStringDataValues = graphDataset.dataset[Random.Range(0, graphDataset.dataset.Count)];
			List<int> graphPointValues = rawStringDataValues.Split(',').Select(int.Parse).ToList();
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
	}
}