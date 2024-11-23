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
    [SerializeField] private TextMeshProUGUI difficultyDescription;

    [Header("Activity and Difficulty Type")]
    [SerializeField] private Activity activityName;
    [SerializeField] private Difficulty difficultyType;

    private string easyDescription = "<b>Basic problem set</b>\nIn this level, you'll encounter less problems designed to help you grasp the foundational concepts of the lesson.";
    private string mediumDescription = "<b>Increased number of problems</b>\nAt this level, you'll tackle additional problems, offering more opportunities for practice and occasionally requiring unit conversions or extra steps to solve.";
    private string hardDescription = "<b>Even more problem sets</b>\nHere, you'll face even more problem sets, and you will not have access to helpful tools, encouraging you to rely on your current understanding of the lesson.";

    // Boolean to check if button is being hovered or not
    private bool isHovered;

    private void OnEnable()
    {
        // Subscribe Button Click Listeners
        difficultyButton.onClick.AddListener(() =>
        {
			SceneSoundManager.Instance.PlaySFX("Click_2");
			DifficultyClick?.Invoke(activityName, difficultyType);
        });
        LoadTextColor();
    }

    private void OnDisable()
    {
        // Unsubscribe Button Click Listeners
        difficultyButton.onClick.RemoveAllListeners();

        // Ensure hover color is removed, description text is blank, and set back to normal on disable
        if (isHovered)
        {
            buttonText.color = Color.black;
            difficultyDescription.text = "";
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
		SceneSoundManager.Instance.PlaySFX("UI_Hover_Mono_01");
		if (isUnlocked)
        {
            // Change text to green, orange, or red on hover based on the difficulty type of text button
            switch (difficultyType)
            {
                case Difficulty.Easy:
                    buttonText.color = new Color(0.1292156f, 0.6981132f, 0);
                    difficultyDescription.text = easyDescription;
                    break;
                case Difficulty.Medium:
                    buttonText.color = new Color(0.7735849f, 0.7114157f, 0.004378719f);
                    difficultyDescription.text = mediumDescription;
                    break;
                case Difficulty.Hard:
                    buttonText.color = Color.red;
                    difficultyDescription.text = hardDescription;
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
            difficultyDescription.text = "";
            isHovered = false;
        }
    }
}
