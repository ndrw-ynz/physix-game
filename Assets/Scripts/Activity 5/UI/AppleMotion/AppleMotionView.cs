using System;
using UnityEngine;

public class AppleMotionView : MonoBehaviour
{
	public static event Action QuitViewEvent;

	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitViewEvent?.Invoke();
	}
}