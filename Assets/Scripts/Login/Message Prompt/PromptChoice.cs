using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ClosePromptChoice
{
    Yes,
    No,
}

public class PromptChoice : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<ClosePromptChoice> ChoiceButtonClick;
    [Header("Prompt Choice Properties")]
    [SerializeField] private Button choiceTextButton;
    [SerializeField] private TextMeshProUGUI buttonText;

    [Header("Prompted Choice Type")]
    [SerializeField] private ClosePromptChoice choice;

    // Boolean to check if button is being hovered or not
    private bool isHovered;

    private void OnEnable()
    {
        // Subscribe button click listeners
        choiceTextButton.onClick.AddListener(() => ChoiceButtonClick?.Invoke(choice));
    }

    private void OnDisable()
    {
        // Unsubscribe button click listeners
        choiceTextButton.onClick.RemoveAllListeners();

        // Ensure text color set back to black on disable
        if (isHovered)
        {
            buttonText.color = Color.black;
            isHovered = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Switch color to green or red based on the text choice on hover
        switch (choice)
        {
            case ClosePromptChoice.Yes:
                buttonText.color = new Color(0f, 0.6431373f, 0.1019608f);
                break;
            case ClosePromptChoice.No:
                buttonText.color = new Color(0.9254902f, 0.0627451f, 0.0627451f);
                break;
        }
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Set the text color to black on unhover
        if (isHovered)
        {
            buttonText.color = Color.black;
            isHovered = false;
        }
    }
}
