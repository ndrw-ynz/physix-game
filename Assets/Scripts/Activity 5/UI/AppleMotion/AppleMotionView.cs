using System;
using UnityEngine;

public class AppleMotionView : MonoBehaviour
{
	public static event Action OpenViewEvent;
	public static event Action QuitViewEvent;

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