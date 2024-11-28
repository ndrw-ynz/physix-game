using System.Collections.Generic;
using UnityEngine;

public class TutorialOverlayController : MonoBehaviour
{
    [System.Serializable]
    public class TerminalStatusScreenGroup
    {
        public GameObject terminalUI;
        public GameObject terminalStatusScreenUI;
        public GameObject terminalStatusScreenUI2;
    }

    [Header ("Game Objects To Be Checked")]
    [SerializeField] private List<TerminalStatusScreenGroup> terminalStatusScreenGroup;
    [SerializeField] private List<GameObject> noStatusScreenTerminals;
    [SerializeField] private TutorialOverlayDisplay tutorialDisplay;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject performanceResultsUI;

    [Header("Input Reader")]
    [SerializeField] protected InputReader inputReader;

    [Header("Activity Number")]
    [SerializeField] private int activityNumber;

    private Difficulty currentActivityDifficulty;

    private void OnEnable()
    {
        GetCurrentActivityDifficulty();

        // Add subscriber for resuming gameplay with tutorial overlay is closed
        TutorialOverlayDisplay.TutorialOverlayClose += ResumeGameplay;
    }

    private void OnDisable()
    {
        // Remove subscribers
        TutorialOverlayDisplay.TutorialOverlayClose -= ResumeGameplay;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            // If the current difficulty is hard for any activity, don't open tutorial overlay
            if (currentActivityDifficulty == Difficulty.Hard)
            {
                return;
            }

            // If tutorial overlay is active, close tutorial overlay
            if (tutorialDisplay.gameObject.activeSelf)
            {
                tutorialDisplay.gameObject.SetActive(false);
                ResumeGameplay();
                return;
            }

            // If pause menu UI is active, don't open tutorial overlay
            if (pauseMenuUI.activeSelf)
            {
                return;
            }

            // If performance results UI is active, dont open tutorial overlay
            if (performanceResultsUI.activeSelf)
            {
                return;
            }

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

            // Loop through the terminal results screen group
            for (int i = 0; i < terminalStatusScreenGroup.Count; i++)
            {
                // If a sub activity terminal is active, and its results screen is not active,
                // open the tutorial overlay to the solvings section's first page
                if (terminalStatusScreenGroup[i].terminalStatusScreenUI.activeSelf)
                {
                    return;
                }

                if (terminalStatusScreenGroup[i].terminalStatusScreenUI2 != null && terminalStatusScreenGroup[i].terminalStatusScreenUI2.activeSelf)
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

            // Open the tutorial display normally at the first page of the gameplay section.
            // Will not open directly to the solvings area since no terminals are opened
            tutorialDisplay.gameObject.SetActive(true);
            inputReader.SetUI();
        }
    }

    private void ResumeGameplay()
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

        // Go back to gameplay mode if there is no active terminals
        inputReader.SetGameplay();
    }

    private void GetCurrentActivityDifficulty()
    {
        // Get the current difficulty for the specific activity
        switch (activityNumber)
        {
            case 1:
                currentActivityDifficulty = ActivityOneManager.difficultyConfiguration;
                break;

            case 2:
                currentActivityDifficulty = ActivityTwoManager.difficultyConfiguration;
                break;

            case 3:
                currentActivityDifficulty = ActivityThreeManager.difficultyConfiguration;
                break;

            case 4:
                currentActivityDifficulty = ActivityFourManager.difficultyConfiguration;
                break;

            case 5:
                currentActivityDifficulty = ActivityFiveManager.difficultyConfiguration;
                break;

            case 6:
                currentActivityDifficulty = ActivitySixManager.difficultyConfiguration;
                break;

            case 7:
                currentActivityDifficulty = ActivitySevenManager.difficultyConfiguration;
                break;

            case 8:
                currentActivityDifficulty = ActivityEightManager.difficultyConfiguration;
                break;

            case 9:
                currentActivityDifficulty = ActivityNineManager.difficultyConfiguration;
                break;
        }
    }
}
