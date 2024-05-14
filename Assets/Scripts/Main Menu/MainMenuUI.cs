using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Canvas _startCanvas;
	[SerializeField] private Canvas _playSelectionCanvas;

	private void Start()
	{
		MenuPlayButton.OnTriggerMenuPlayButton += SwitchToPlaySelection;
		MenuBackButton.OnTriggerMenuBackButton += SwitchToStart;
		MenuNewGameButton.OnTriggerMenuNewGameButton += InitiateNewGame;
	}

	private void SwitchToPlaySelection()
	{
		_startCanvas.gameObject.SetActive(false);
		_playSelectionCanvas.gameObject.SetActive(true);
	}

	private void SwitchToStart()
	{
		_startCanvas.gameObject.SetActive(true);
		_playSelectionCanvas.gameObject.SetActive(false);
	}

	private void InitiateNewGame()
	{
		SceneManager.LoadScene("Topic Discussion 1");
	}

}
