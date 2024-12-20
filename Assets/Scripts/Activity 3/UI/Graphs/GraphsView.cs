using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphsAnswerSubmission
{
	public Graph positionVsTimeGraph { get; private set; }
	public Graph velocityVsTimeGraph {get; private set;}
	public Graph accelerationVsTimeGraph {get; private set;}

	public GraphsAnswerSubmission(
		Graph positionVsTimeGraph,
		Graph velocityVsTimeGraph,
		Graph accelerationVsTimeGraph
		)
	{
		this.positionVsTimeGraph = positionVsTimeGraph;
		this.velocityVsTimeGraph = velocityVsTimeGraph;
		this.accelerationVsTimeGraph = accelerationVsTimeGraph;
	}
}

public class GraphsView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;
	public event Action<GraphsAnswerSubmission> SubmitAnswerEvent;

	[Header("Managers")]
	[SerializeField] private GraphManager graphManager;

	[Header("Text Display")]
	[SerializeField] private TextMeshProUGUI testCountText;

	[Header("Graphs UIs")]
	[SerializeField] private GraphEditorUI graphEditorUI;
	[SerializeField] private GraphViewerUI graphViewerUI;

	[Header("Graph Buttons")]
	[SerializeField] private Button positionGraphButton;
	[SerializeField] private Button velocityGraphButton;
	[SerializeField] private Button accelerationGraphButton;

	private void OnEnable()
	{
		OpenViewEvent?.Invoke();

		positionGraphButton.onClick.AddListener(() => SwitchToGraphViewerUI(graphManager.positionVsTimeGraph));
		velocityGraphButton.onClick.AddListener(() => SwitchToGraphEditorUI(graphManager.velocityVsTimeGraph));
		accelerationGraphButton.onClick.AddListener(() => SwitchToGraphEditorUI(graphManager.accelerationVsTimeGraph));
	}

	public void UpdateTestCountTextDisplay(int currentNumTests, int totalNumTests)
	{
		testCountText.text = $"<color=yellow>Number of Tests Solved: {currentNumTests} / {totalNumTests}</color>";
	}

	private void SwitchToGraphEditorUI(Graph graph)
	{
		SceneSoundManager.Instance.PlaySFX("Click");

		gameObject.SetActive(false);

		graphEditorUI.gameObject.SetActive(true);
		graphViewerUI.gameObject.SetActive(false);

		graphManager.DisplayGraph(graph);
		graphManager.canEditGraph = true;
	}

	private void SwitchToGraphViewerUI(Graph graph)
	{
		SceneSoundManager.Instance.PlaySFX("Click");

		gameObject.SetActive(false);

		graphEditorUI.gameObject.SetActive(false);
		graphViewerUI.gameObject.SetActive(true);

		graphManager.DisplayGraph(graph);
		graphManager.canEditGraph = false;
	}

	public void OnSubmitButtonClick()
	{
		SceneSoundManager.Instance.PlaySFX("Click");

		GraphsAnswerSubmission submission = new GraphsAnswerSubmission(
			positionVsTimeGraph: graphManager.positionVsTimeGraph,
			velocityVsTimeGraph: graphManager.velocityVsTimeGraph,
			accelerationVsTimeGraph: graphManager.accelerationVsTimeGraph
			);

		SubmitAnswerEvent?.Invoke(submission);
	}

	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		positionGraphButton.onClick.RemoveAllListeners();
		velocityGraphButton.onClick.RemoveAllListeners();
		accelerationGraphButton.onClick.RemoveAllListeners();
		QuitViewEvent?.Invoke();
	}
}
