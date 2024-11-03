using System.Collections.Generic;
using UnityEngine;

public class LessonSelectScreen : MonoBehaviour
{
    [Header("Lesson Select Buttons List")]
    [SerializeField] List<LessonSelectButton> lessonSelectButtons;

    [Header("Lesson Locked Message Warning Overlay")]
    [SerializeField] GameObject lessonLockedWarningOverlay;

    public void LoadUnlockedLessons(int highestUnlockedLesson)
    {
        // Load all unlocked lessons based until the highest unlocked lesson only
        for (int i = 0; i<highestUnlockedLesson; i++)
        {
            if (i < lessonSelectButtons.Count) 
            {
                lessonSelectButtons[i].isUnlocked = true;
            }
            else
            {
                Debug.Log($"Highest unlocked lesson value {i+1} surpassed the list count.");
            }
            
        }
    }

    public void LockAllLessons()
    {
        // Lock all lessons at start
        for (int i = 0; i < lessonSelectButtons.Count; i++)
        {
            lessonSelectButtons[i].isUnlocked = false;
        }
    }

    public void ActivateWarningOverlay()
    {
        lessonLockedWarningOverlay.SetActive(true);
    }

    public void DeactivateWarningOverlay()
    {
        lessonLockedWarningOverlay.SetActive(false);
    }
}
