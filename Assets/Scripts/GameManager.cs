using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputReader _input;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private ScientificNotationSO level1;
    [SerializeField] private List<BoxContainer> boxContainers;
    private BoxContainer _currentBoxContainer;

    void Start()
    {
        _input.PauseEvent += HandlePause;
        _input.ResumeEvent += HandleResume;
        BoxContainer.BoxInteractEvent += OnSelectBox;
        ViewScientificNotation.OpenViewEvent += OnOpenViewSN;

        // TODO: handle event for randomizing contents of box containers
        RandomlyGenerateBoxValues();
    }

    private void HandlePause()
    {
        //pauseMenu.SetActive(true);
    }

    private void HandleResume()
    {
        //pauseMenu.SetActive(false);
    }

    private void RandomlyGenerateBoxValues()
    {
        foreach (BoxContainer box in boxContainers) {
            box.SetValues(level1);
        }
    }
    private void OnSelectBox(BoxContainer container)
    {
        _currentBoxContainer = container;
    }

    private void OnOpenViewSN(ViewScientificNotation view)
    {
        if (_currentBoxContainer != null)
        {
            view.measurementText.text = _currentBoxContainer.measurementText.text;
        }
    }
}
