using System;
using UnityEngine;
using UnityEngine.UI;

public class LessonSelectButton : MonoBehaviour
{
    public static event Action<int> LessonSelectClick;

    [Header("Lesson Select Button Properties")]
    [SerializeField] private GameObject lessonPanel;
    [SerializeField] private Button lessonSelectButton;

    [Header("Lesson Select Button Number")]
    [SerializeField] private int lessonNumber;

    private void OnEnable()
    {
        // Subscribe Button Click Listeners
        lessonSelectButton.onClick.AddListener(() => LessonSelectClick?.Invoke(lessonNumber));
    }

    private void OnDisable()
    {
        // Unsubscribe Button Click Listeners
        lessonSelectButton.onClick.RemoveAllListeners();
    }


}
