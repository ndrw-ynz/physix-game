using System;
using UnityEngine;
using UnityEngine.UI;

public class ViewGraphs : MonoBehaviour
{
	public static event Action SubmitGraphsAnswerEvent;

	[Header("Buttons")]
	public Button submitGraphsButton;
	[Header("Overlay")]
	public Image overlay;

	private void OnEnable()
	{
		submitGraphsButton.onClick.AddListener(() => SubmitGraphsAnswerEvent?.Invoke());
	}

	private void OnDisable()
	{
		submitGraphsButton.onClick.RemoveAllListeners();
	}
}
