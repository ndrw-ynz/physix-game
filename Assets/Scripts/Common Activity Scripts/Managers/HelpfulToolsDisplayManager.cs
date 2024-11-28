using System.Collections.Generic;
using UnityEngine;

public class HelpfulToolsDisplayManager : MonoBehaviour
{
    [SerializeField] private List<HelpfulToolsDisplay> helpfulToolsDisplays;

    [Header("Activity Number")]
    [SerializeField] private int activityNumber;

    private Difficulty currentActivityDifficulty;

    private void Start()
    {
        GetCurrentActivityDifficulty();

        foreach (HelpfulToolsDisplay helpfulToolDisplay in helpfulToolsDisplays)
        {
            helpfulToolDisplay.LockHelpfulTools(currentActivityDifficulty);
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
