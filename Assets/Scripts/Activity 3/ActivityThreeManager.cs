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

	private List<int> correctPositionValues;
	private List<int> correctVelocityValues;
	private List<int> correctAccelerationValues;

	[Header("Metrics for Graphs Subactivity")]
	public bool isGraphsSubActivityFinished;
	private int numIncorrectGraphsSubmission;

	private void Start()
	{
        GraphEditButton.InitiateGraphEditViewSwitch += ChangeViewToGraphEditView;
		ViewGraphEdit.InitiateGraphViewSwitch += ChangeViewToGraphView;
		ViewGraphs.SubmitGraphsAnswerEvent += CheckGraphsAnswer;

		currentGraphsLevel = graphsLevelOne; // alter in the future, on changing upon submission in loading of scene.

		InitializeCorrectGraphValues();

		graphManager.SetupGraphs(correctPositionValues, correctVelocityValues, correctAccelerationValues);
	}

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
	}
}