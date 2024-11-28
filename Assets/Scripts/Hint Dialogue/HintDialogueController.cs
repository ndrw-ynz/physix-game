using System.Collections.Generic;
using UnityEngine;

public class HintDialogueController : MonoBehaviour
{
    [System.Serializable]
    public class TerminalStatusScreenGroup
    {
        public GameObject terminalUI;
        public GameObject terminalStatusScreenUI;
        public GameObject terminalStatusScreenUI2;
        public List<string> sentences;
    }

    [Header("Game Objects To Be Checked")]
    [SerializeField] private List<TerminalStatusScreenGroup> terminalStatusScreenGroup;
    [SerializeField] private GameObject hintDialogue;
    [SerializeField] private HintDialogue dialogueBox;
    [SerializeField] private GameObject pauseMenuUI;

    [Header("Activity Number")]
    [SerializeField] private int activityNumber;

    private Difficulty currentActivityDifficulty;

    // Start is called before the first frame update
    void OnEnable()
    {
        GetCurrentActivityDifficulty();

        HintDialogue.HintDialogueFinished += CloseHintDialogue;
    }

    private void OnDisable()
    {
        HintDialogue.HintDialogueFinished -= CloseHintDialogue;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            // If the current difficulty is hard for any activity, don't open hint dialogue
            if (currentActivityDifficulty == Difficulty.Hard)
            {
                return;
            }

            // If hint dialouge is active, avoid duplication
            if (hintDialogue.gameObject.activeSelf)
            {
                return;
            }

            // If pause menu UI is active, don't open hint dialogue
            if (pauseMenuUI.activeSelf)
            {
                return;
            }

            // Loop through the terminal results screen group
            for (int i = 0; i < terminalStatusScreenGroup.Count; i++)
            {
                // If a sub activity terminal is active, and its results screen is not active,
                // open the hint dialogue
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
                    dialogueBox.sentences = terminalStatusScreenGroup[i].sentences;
                    hintDialogue.gameObject.SetActive(true);
                    return;
                }
            }

        }
    }

    private void CloseHintDialogue()
    {
        if (hintDialogue.gameObject.activeSelf)
        {
            hintDialogue.gameObject.SetActive(false);
        }
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
