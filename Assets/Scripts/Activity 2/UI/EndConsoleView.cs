using System;
using UnityEngine;

public class EndConsoleView : MonoBehaviour
{
    public event Action OpenViewEvent;

	private void OnEnable()
	{
		OpenViewEvent?.Invoke();
	}
}
