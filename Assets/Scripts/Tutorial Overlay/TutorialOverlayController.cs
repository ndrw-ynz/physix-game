using System.Collections.Generic;
using UnityEngine;

public class TutorialOverlayController : MonoBehaviour
{
    [System.Serializable]
    public class TerminalStatusScreenGroup
    {
        public GameObject terminalUI;
        public GameObject terminalStatusScreenUI;
    }

    [Header("Game Objects To Be Checked")]
    [SerializeField] private List<TerminalStatusScreenGroup> terminalStatusScreenGroup;
    [SerializeField] private List<GameObject> noStatusScreenTerminals;
    [SerializeField] private TutorialOverlayDisplay tutorialDisplay;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject performanceResultsUI;

    [Header("Input Reader")]
    [SerializeField] protected  InputReader inputReader;

    private void OnEnable()
    {
        // Add subscriber for resuming gameplay upon closing tutorial oerlay
        TutorialOverlayDisplay.TutorialOverlayClose += ResumeGameplay;
    }

    private void OnDisable()
    {
        // Remove subscriber
        TutorialOverlayDisplay.TutorialOverlayClose -= ResumeGameplay;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            // Checker for closing tutorial overlay upon pressing 'T' again
            CheckForActiveTutorialDisplay();

            // Checkers for avoiding tutorial overlay opening if specific game objects are active
            CheckForActivePauseMenu();
            CheckForActivePerformanceResult();

            // Checkers for opening directly to the calculations section of the tutorial overlay
            CheckForActiveNoStatusScreenTerminals();
            CheckForActiveOnlySubActivityTerminals();

            // Open the tutorial display normally at the first page of the gameplay section.
            // Will not open directly to the solvings area since no terminals are opened
            tutorialDisplay.gameObject.SetActive(true);
            inputReader.SetUI();
        }
    }

    private void ResumeGameplay()
    {
        CheckForActiveSubActivityRelatedScreen();

        // Go back to gameplay mode if there is no active terminals
        inputReader.SetGameplay();
    }

    #region On Tutorial Overlay Open Game Object Checkers
    private void CheckForActiveTutorialDisplay()
    {
        // If tutorial overlay is active, close tutorial overlay
        if (tutorialDisplay.gameObject.activeSelf)
        {
            tutorialDisplay.gameObject.SetActive(false);
            ResumeGameplay();
            return;
        }
    }
    private void CheckForActivePauseMenu()
    {
        // If pause menu UI is active, don't open tutorial overlay
        if (pauseMenuUI.activeSelf)
        {
            return;
        }
    }
    private void CheckForActivePerformanceResult()
    {
        // If performance results UI is active, dont open tutorial overlay
        if (performanceResultsUI.activeSelf)
        {
            return;
        }
    }
    private void CheckForActiveNoStatusScreenTerminals()
    {
        // If no results screen terminals list count is more than 0, loop through list
        if (noStatusScreenTerminals.Count > 0)
        {
            // Check if there's an active no results screen terminal, if yes, open tutorial overlay
            // I might have only done this for activity one since it has a container picker terminal
            for (int i = 0; i < noStatusScreenTerminals.Count; i++)
            {
                if (noStatusScreenTerminals[i].gameObject.activeSelf)
                {
                    tutorialDisplay._currentSectorIndex = 5;
                    tutorialDisplay.gameObject.SetActive(true);
                    inputReader.SetUI();
                }
            }
        }
    }
    private void CheckForActiveOnlySubActivityTerminals()
    {
        // Loop through the terminal results screen group
        for (int i = 0; i < terminalStatusScreenGroup.Count; i++)
        {
            // If a sub activity terminal is active, and its results screen is not active,
            // open the tutorial overlay to the solvings section's first page
            if (terminalStatusScreenGroup[i].terminalStatusScreenUI.activeSelf)
            {
                return;
            }

            if (terminalStatusScreenGroup[i].terminalUI.activeSelf)
            {
                // Load the solving's tutorial first sector page
                tutorialDisplay._currentSectorIndex = 5;
                tutorialDisplay.gameObject.SetActive(true);
                inputReader.SetUI();
                return;
            }
        }
    }
    #endregion

    #region On Tutorial Overlay Close Game Object Checkers
    private void CheckForActiveSubActivityRelatedScreen()
    {
        // If no results screen terminals list count is more than 0, loop through list
        if (noStatusScreenTerminals.Count > 0)
        {
            // Loop through list
            for (int i = 0; i < noStatusScreenTerminals.Count; i++)
            {
                // If there is an active terminal that does not have a result screen, do not go back to gameplay mode
                if (noStatusScreenTerminals[i].gameObject.activeSelf)
                {
                    return;
                }
            }
        }

        // Loop through list
        for (int i = 0; i < terminalStatusScreenGroup.Count; i++)
        {
            // If there is an active sub activity terminal, do not go back to gameplay mode
            if (terminalStatusScreenGroup[i].terminalUI.activeSelf)
            {
                return;
            }
        }
    }
    #endregion
}
