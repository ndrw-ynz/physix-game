using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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

public class ActivityDifficultyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<Activity, Difficulty> DifficultyClick;

    [Header("Availability of Difficulty")]
    public bool isUnlocked;

    [Header("Difficulty Button Properties")]
    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private Button difficultyButton;
    [SerializeField] private TextMeshProUGUI buttonText;

    [Header("Activity and Difficulty Type")]
    [SerializeField] private Activity activityName;
    [SerializeField] private Difficulty difficultyType;

    // Boolean to check if button is being hovered or not
    private bool isHovered;

    private void OnEnable()
    {
        // Subscribe Button Click Listeners
        difficultyButton.onClick.AddListener(() => DifficultyClick?.Invoke(activityName, difficultyType));
        LoadTextColor();
    }

    private void OnDisable()
    {
        // Unsubscribe Button Click Listeners
        difficultyButton.onClick.RemoveAllListeners();

        // Ensure hover color is removed and set back to normal on disable
        if (isHovered)
        {
            buttonText.color = Color.black;
            isHovered = false;
        }
    }

    private void LoadTextColor()
    {
        // Load button text color and button's interactability
        if (isUnlocked)
        {
            buttonText.color = Color.black;
            difficultyButton.interactable = true;
        }
        else
        {
            buttonText.color = Color.gray;
            difficultyButton.interactable = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isUnlocked)
        {
            // Change text to green, orange, or red on hover based on the difficulty type of text button
            switch (difficultyType)
            {
                case Difficulty.Easy:
                    buttonText.color = new Color(0.1292156f, 0.6981132f, 0);
                    break;
                case Difficulty.Medium:
                    buttonText.color = new Color(0.7735849f, 0.7114157f, 0.004378719f);
                    break;
                case Difficulty.Hard:
                    buttonText.color = Color.red;
                    break;
            }
            isHovered = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Change text to black on unhover for current hovered text button
        if (isHovered)
        {
            buttonText.color = Color.black;
            isHovered = false;
        }
    }
}
