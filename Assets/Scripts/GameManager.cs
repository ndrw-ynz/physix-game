using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputReader _input;
    [SerializeField] private GameObject pauseMenu;
    void Start()
    {
        _input.PauseEvent += HandlePause;
        _input.ResumeEvent += HandleResume;
    }

    private void HandlePause()
    {
        pauseMenu.SetActive(true);
    }

    private void HandleResume()
    {
        pauseMenu.SetActive(false);
    }
}
