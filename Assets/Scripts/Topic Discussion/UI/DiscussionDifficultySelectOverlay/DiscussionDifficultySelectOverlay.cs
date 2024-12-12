using UnityEngine;

public class DiscussionDifficultySelectOverlay : MonoBehaviour
{
    [Header("Lesson Difficulty Button Group")]
    [SerializeField] private LessonDifficultyButtonGroup lessonDifficultyButtonGroup;

    // Highest unlocked lesson of the current student
    private int highestUnlockedLesson;
    private int highestLessonUnlockedDifficulties;
    public void ActivateDifficultyButtons(int discussionNumber)
    {
        // Unlock available difficulty buttons based on user unlocked levels

        highestUnlockedLesson = (int)UserManager.Instance.UserUnlockedLevels.fields["highestUnlockedLesson"].integerValue; // [1-9] Lessons
        highestLessonUnlockedDifficulties = (int)UserManager.Instance.UserUnlockedLevels.fields["highestLessonUnlockedDifficulty"].integerValue; ; // [1-3] Difficulty Levels
        if (highestUnlockedLesson > discussionNumber)
        {
            lessonDifficultyButtonGroup.difficultyButtons[0].isUnlocked = true;
            lessonDifficultyButtonGroup.difficultyButtons[1].isUnlocked = true;
            lessonDifficultyButtonGroup.difficultyButtons[2].isUnlocked = true;
        }
        else
        {
            for (int i = 0; i < highestLessonUnlockedDifficulties; i++)
            {
                if (i < lessonDifficultyButtonGroup.difficultyButtons.Count)
                {
                    lessonDifficultyButtonGroup.difficultyButtons[i].isUnlocked = true;
                }
                else
                {
                    Debug.Log($"Highest unlocked lesson's unlocked difficulty value {i + 1} surpassed the list count.");
                }
            }
        }
    }

    public void LockDifficultyButtons()
    {
        // Lock all difficulty buttons
        lessonDifficultyButtonGroup.difficultyButtons[0].isUnlocked = false;
        lessonDifficultyButtonGroup.difficultyButtons[1].isUnlocked = false;
        lessonDifficultyButtonGroup.difficultyButtons[2].isUnlocked = false;
    }
}
