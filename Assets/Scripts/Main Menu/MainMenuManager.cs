using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
	private void Start()
	{
		MenuNewGameButton.OnTriggerMenuNewGameButton += InitiateNewGame;
	}

	private void InitiateNewGame()
	{
		SceneManager.LoadScene("Topic Discussion 1");
	}
}
