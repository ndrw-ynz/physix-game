using System.Collections.Generic;
using UnityEngine;
public class DifficultySelectOverlays : MonoBehaviour
{
    [Header("Lesson Difficulty Button Groups List")]
    [SerializeField] public List<LessonDifficultyButtonGroup> lessonDifficultyButtonGroups;

    [Header("Lesson Difficulty Select Overlays")]
    [SerializeField] private GameObject lessonOneDifficultySelectOverlay;
    [SerializeField] private GameObject lessonTwoDifficultySelectOverlay;
    [SerializeField] private GameObject lessonThreeDifficultySelectOverlay;
    [SerializeField] private GameObject lessonFourDifficultySelectOverlay;
    [SerializeField] private GameObject lessonFiveDifficultySelectOverlay;
    [SerializeField] private GameObject lessonSixDifficultySelectOverlay;
    [SerializeField] private GameObject lessonSevenDifficultySelectOverlay;
    [SerializeField] private GameObject lessonEightDifficultySelectOverlay;
    [SerializeField] private GameObject lessonNineDifficultySelectOverlay;

    // Lesson Difficulty Select Overlays Dictionary
    private Dictionary<int, GameObject> lessonDifficultyKeyValuePairs;

    private void Start()
    {
        // Initialize Lesson Difficulty Select Screens Key Value Pairs
        lessonDifficultyKeyValuePairs = new Dictionary<int, GameObject>()
        {
            {1, lessonOneDifficultySelectOverlay },
            {2, lessonTwoDifficultySelectOverlay },
            {3, lessonThreeDifficultySelectOverlay },
            {4, lessonFourDifficultySelectOverlay },
            {5, lessonFiveDifficultySelectOverlay},
            {6, lessonSixDifficultySelectOverlay},
            {7, lessonSevenDifficultySelectOverlay},
            {8, lessonEightDifficultySelectOverlay},
            {9, lessonNineDifficultySelectOverlay}
        };
    }
    public void LockAllDifficulty()
    {
        // Lock all lesson's difficulty text buttons
        for (int i = 0; i < lessonDifficultyButtonGroups.Count; i++)
        {
            LessonDifficultyButtonGroup currentLessonButtonGroup = lessonDifficultyButtonGroups[i];
            lessonDifficultyButtonGroups[i].difficultyButtons[0].isUnlocked = false;
            lessonDifficultyButtonGroups[i].difficultyButtons[1].isUnlocked = false;
            lessonDifficultyButtonGroups[i].difficultyButtons[2].isUnlocked = false;
        }
    }
    public void LoadLessonDifficultyButtons(int studHighestUnlockedLesson, int studHighestLessonUnlockedDifficulty)
    {
        // Load all lesson's difficulty buttons based on the user's highest unlocked lessons and difficulties
        for (int i = 0; i < studHighestUnlockedLesson; i++)
        {
            // If current lesson is not the last unlocked lesson of the user, unlock all difficulties
            LessonDifficultyButtonGroup currentLessonButtonGroup = lessonDifficultyButtonGroups[i];
            if (i != (studHighestUnlockedLesson - 1))
            {
                UnlockCurrentLessonAllDifficulties(currentLessonButtonGroup);
            }
            else
            {
                // If the current lesson is the last unlocked level of user, unlock difficulties
                // according to the user's highest lesson's unlocked difficulties
                for (int j = 0; j < studHighestLessonUnlockedDifficulty; j++)
                {
                    // Error check if the difficulty unlocked is only 1-3(Easy, Medium, Hard)
                    // If it surpasses this limit, ignore and throw a message that it went over the limit
                    if (j < currentLessonButtonGroup.difficultyButtons.Count)
                    {
                        currentLessonButtonGroup.difficultyButtons[j].isUnlocked = true;
                    }
                    else
                    {
                        Debug.Log($"Highest unlocked lesson's unlocked difficulty value {j+1} surpassed the list count.");
                    }
                }
            }
        }
    }
    public void LoadActivityDifficultyOverlay(int keyValue)
    {
        // Open the specified lesson components screen
        lessonDifficultyKeyValuePairs[keyValue].SetActive(true);
    }
    public void CloseDifficultySelectOverlay(int lessonNumber)
    {
        // Close the current difficulty select overlay
        if (lessonDifficultyKeyValuePairs[lessonNumber].gameObject.activeSelf)
        {
            lessonDifficultyKeyValuePairs[lessonNumber].gameObject.SetActive(false);
        }
        else
        {
            Debug.Log(lessonDifficultyKeyValuePairs[lessonNumber].gameObject + "is already not active. Maybe there's something wrong with the indexing?");
        }
    }
    private void UnlockCurrentLessonAllDifficulties(LessonDifficultyButtonGroup currentLessonButtonGroup)
    {
        // Unlock all difficulties for the current lesson
        for (int i = 0; i < currentLessonButtonGroup.difficultyButtons.Count; i++)
        {
            currentLessonButtonGroup.difficultyButtons[i].isUnlocked = true;
        }
    }
}
