using System;
using UnityEngine;

public class DotProductView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;

	private void OnEnable()
	{
		OpenViewEvent?.Invoke();
	}

	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitViewEvent?.Invoke();
	}
}