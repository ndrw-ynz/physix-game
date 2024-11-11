using System.Collections.Generic;
using UnityEngine;

public class TutorialOverlayController : MonoBehaviour
{
    [System.Serializable]
    public class TerminalResultsScreenGroup
    {
        public GameObject terminalUI;
        public GameObject terminalResultScreenUI;
    }

    [SerializeField] private List<TerminalResultsScreenGroup> terminalResultScreenGroup;
    [SerializeField] private List<GameObject> noResultScreenTerminals;
    [SerializeField] private TutorialOverlayDisplay tutorialDisplay;

    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject performanceResultsUI;

    [SerializeField] protected  InputReader inputReader;

    private void OnEnable()
    {
        TutorialOverlayDisplay.TutorialOverlayClose += ResumeGameplay;
    }

    private void OnDisable()
    {
        TutorialOverlayDisplay.TutorialOverlayClose -= ResumeGameplay;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (tutorialDisplay.gameObject.activeSelf)
            {
                tutorialDisplay.gameObject.SetActive(false);
                ResumeGameplay();
                return;
            }

            if (pauseMenuUI.activeSelf)
            {
                return;
            }
            if (performanceResultsUI.activeSelf)
            {
                return;
            }

            if (noResultScreenTerminals.Count > 0)
            {
                for (int i = 0; i < noResultScreenTerminals.Count; i++)
                {
                    if (noResultScreenTerminals[i].gameObject.activeSelf)
                    {
                        tutorialDisplay.gameObject.SetActive(true);
                        inputReader.SetUI();
                    }
                }
            }

            for (int i = 0; i < terminalResultScreenGroup.Count; i++)
            {
                if (terminalResultScreenGroup[i].terminalUI.activeSelf && !terminalResultScreenGroup[i].terminalResultScreenUI.activeSelf)
                {
                    // Load the solving's tutorial first sector page
                    tutorialDisplay._currentSectorIndex = 5;
                    tutorialDisplay.gameObject.SetActive(true);
                    inputReader.SetUI();
                    return;
                }
            }

            tutorialDisplay.gameObject.SetActive(true);
            inputReader.SetUI();
        }
    }

    private void ResumeGameplay()
    {
        if (noResultScreenTerminals.Count > 0)
        {
            for (int i = 0; i < noResultScreenTerminals.Count; i++)
            {
                if (noResultScreenTerminals[i].gameObject.activeSelf)
                {
                    return;
                }
            }
        }

        for (int i = 0; i < terminalResultScreenGroup.Count; i++)
        {
            if (terminalResultScreenGroup[i].terminalUI.activeSelf)
            {
                return;
            }
        }

        inputReader.SetGameplay();
    }
}
