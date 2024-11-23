using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TopicDiscussionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<int> TopicDiscussionClick;

    [Header("Topic Discussion Button Properties")]
    [SerializeField] private Image topicDiscussionPanel;
    [SerializeField] private TextMeshProUGUI panelText;
    [SerializeField] private Button topicDiscussionButton;

    [Header("Topic Discussion Button Number")]
    [SerializeField] private int topicDiscussionNumber;

    // Boolean to check if button is being hovered or not
    private bool isHovered;

    private void OnEnable()
    {
        // Subscribe Button Click Listeners
        topicDiscussionButton.onClick.AddListener(() =>
        {
			SceneSoundManager.Instance.PlaySFX("Click_2");
			TopicDiscussionClick?.Invoke(topicDiscussionNumber);
        });
    }

    private void OnDisable()
    {
        // Unsubscribe Button Click Listeners
        topicDiscussionButton.onClick.RemoveAllListeners();

        // Ensure hover color is removed and set back to normal on disable
        if (isHovered)
        {
            topicDiscussionPanel.color = Color.white;
            panelText.color = Color.black;
            isHovered = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
		SceneSoundManager.Instance.PlaySFX("UI_Hover_Mono_01");
		// Change panel color to black and text to white on hover for topic discussion button
		topicDiscussionPanel.color = Color.black;
        panelText.color = Color.white;
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Change panel color to white and text to black on unhover for current hovered button
        if (isHovered)
        {
            topicDiscussionPanel.color = Color.white;
            panelText.color = Color.black;
            isHovered = false;
        }
    }
}
