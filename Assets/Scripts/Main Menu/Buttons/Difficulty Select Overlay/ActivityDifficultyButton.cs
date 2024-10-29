using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}

public enum Activity
{
    ActivityOne,
    ActivityTwo,
    ActivityThree,
    ActivityFour,
    ActivityFive,
    ActivitySix,
    ActivitySeven,
    ActivityEight,
    ActivityNine,
}

public class ActivityDifficultyButton : MonoBehaviour
{
    public static event Action<Difficulty> DifficultyHover;
    public static event Action<Activity, Difficulty> DifficultyClick;

    [Header("Difficulty Button Properties")]
    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private Button difficultyButton;

    [Header("Activity and Difficulty Type")]
    [SerializeField] private Activity activityName;
    [SerializeField] private Difficulty difficultyType;

    private void OnEnable()
    {
        // Subscribe Button Click Listeners
        difficultyButton.onClick.AddListener(() => DifficultyClick?.Invoke(activityName, difficultyType));
    }

    private void OnDisable()
    {
        // Unsubscribe Button Click Listeners
        difficultyButton.onClick.RemoveAllListeners();
    }
}
