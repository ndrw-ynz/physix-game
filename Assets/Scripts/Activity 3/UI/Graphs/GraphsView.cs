using System;
using UnityEngine;
using UnityEngine.UI;

public class GraphsView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;

	[Header("Managers")]
	[SerializeField] private GraphManager graphManager;

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

	private void SwitchToGraphEditorUI(Graph graph)
	{
		gameObject.SetActive(false);

		graphEditorUI.gameObject.SetActive(true);
		graphViewerUI.gameObject.SetActive(false);

		graphManager.DisplayGraph(graph);
	}

	private void SwitchToGraphViewerUI(Graph graph)
	{
		gameObject.SetActive(false);

		graphEditorUI.gameObject.SetActive(false);
		graphViewerUI.gameObject.SetActive(true);

		graphManager.DisplayGraph(graph);
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
