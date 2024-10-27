using System;
using UnityEngine;

public class GraphEditorUI : MonoBehaviour
{
	public event Action QuitGraphEditorEvent;

	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitGraphEditorEvent?.Invoke();
	}
}
