using System;
using UnityEngine;

public class GraphViewerUI : MonoBehaviour
{
	public event Action QuitGraphViewerEvent;

	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitGraphViewerEvent?.Invoke();
	}
}
