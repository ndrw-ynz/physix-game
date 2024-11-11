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
            if (noResultScreenTerminals.Count > 0)
            {
                // Check if there's an active no results screen terminal, if yes, open tutorial overlay
                // I might have only done this for activity one since it has a container picker terminal
                for (int i = 0; i < noResultScreenTerminals.Count; i++)
                {
                    if (noResultScreenTerminals[i].gameObject.activeSelf)
                    {
                        tutorialDisplay._currentSectorIndex = 5;
                        tutorialDisplay.gameObject.SetActive(true);
                        inputReader.SetUI();
                    }
                }
            }

            // Loop through the terminaal results screen group
            for (int i = 0; i < terminalResultScreenGroup.Count; i++)
            {
                // If a sub activity terminal is active, and its results screen is not active,
                // open the tutorial overlay to the solvings section's first page
                if (terminalResultScreenGroup[i].terminalUI.activeSelf && !terminalResultScreenGroup[i].terminalResultScreenUI.activeSelf)
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
        if (noResultScreenTerminals.Count > 0)
        {
            // Loop through list
            for (int i = 0; i < noResultScreenTerminals.Count; i++)
            {
                // If there is an active terminal that does not have a result screen, do not go back to gameplay mode
                if (noResultScreenTerminals[i].gameObject.activeSelf)
                {
                    return;
                }
            }
        }

        // Loop through list
        for (int i = 0; i < terminalResultScreenGroup.Count; i++)
        {
            // If there is an active sub activity terminal, do not go back to gameplay mode
            if (terminalResultScreenGroup[i].terminalUI.activeSelf)
            {
                return;
            }
        }

        // Go back to gameplay mode if there is no active terminals
        inputReader.SetGameplay();
    }
}
