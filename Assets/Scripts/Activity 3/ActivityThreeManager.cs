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

	[Header("Modal Windows")]
	[SerializeField] private GraphSubmissionModalWindow graphSubmissionModalWindow;

	private List<int> correctPositionValues;
	private List<int> correctVelocityValues;
	private List<int> correctAccelerationValues;

	[Header("Metrics for Graphs Subactivity")]
	public bool isGraphsSubActivityFinished;
	private int numIncorrectGraphsSubmission;

	[Header("Metrics for 1D Kinematics Subactivity")]
	public bool isAccelerationCalculationFinished;
	private int numIncorrectAccelerationSubmission;

	[Header("Generated Given Values")]
	private int initialVelocityValue;
	private int finalVelocityValue;
	private int totalTimeValue;

	private void Start()
	{
        GraphEditButton.InitiateGraphEditViewSwitch += ChangeViewToGraphEditView;
		ViewGraphEdit.InitiateGraphViewSwitch += ChangeViewToGraphView;
		ViewGraphs.SubmitGraphsAnswerEvent += CheckGraphsAnswer;
		GraphSubmissionModalWindow.RetryGraphSubmission += RestoreViewGraphState;
		GraphSubmissionModalWindow.InitiateNextSubActivity += ChangeViewToCalculatorView;

		View1DKinematics.SubmitAccelerationAnswerEvent += CheckAccelerationAnswer;

		currentGraphsLevel = graphsLevelOne; // alter in the future, on changing upon submission in loading of scene.
		currentKinematics1DLevel = kinematics1DLevelOne;

		InitializeCorrectGraphValues();
		graphManager.SetupGraphs(correctPositionValues, correctVelocityValues, correctAccelerationValues);

		Initialize1DKinematicsGiven();
		view1DKinematics.SetupGivenText(initialVelocityValue, finalVelocityValue, totalTimeValue);
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

	private void RestoreViewGraphState()
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
		totalTimeValue = Random.Range(currentKinematics1DLevel.minimumTimeValue, currentKinematics1DLevel.maximumTimeValue);
	}

	private void CheckAccelerationAnswer(float accelerationAnswer)
	{
		bool isAccelerationCorrect = ActivityThreeUtilities.ValidateAccelerationSubmission(accelerationAnswer, initialVelocityValue, finalVelocityValue, totalTimeValue);
		if (isAccelerationCorrect)
		{
			isAccelerationCalculationFinished = true;
		} else
		{
			numIncorrectAccelerationSubmission++;
		}
	}

	#endregion
}