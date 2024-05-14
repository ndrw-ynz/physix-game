using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNewGameButton : MonoBehaviour
{
	public static event Action OnTriggerMenuNewGameButton;
	public void OnClick()
    {
		OnTriggerMenuNewGameButton?.Invoke();
    }
}
