using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackButton : MonoBehaviour
{
	public static event Action OnTriggerMenuBackButton;

	public void OnClick()
	{
		OnTriggerMenuBackButton?.Invoke();
	}

}
