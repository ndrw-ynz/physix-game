using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlayButton : MonoBehaviour
{

	public static event Action OnTriggerMenuPlayButton;

	public void OnClick()
    {
		OnTriggerMenuPlayButton?.Invoke();
    }
}
