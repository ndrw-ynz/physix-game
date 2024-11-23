using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActivityButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<int> ActivityClick;

    [Header("Activity Button Properties")]
    [SerializeField] private Image activityPanel;
    [SerializeField] private TextMeshProUGUI panelText;
    [SerializeField] private Button activityButton;

    [Header("Activity Button Number")]
    [SerializeField] private int activityNumber;

    // Boolean to check if button is being hovered or not
    private bool isHovered;

    private void OnEnable()
    {
        // Subscribe Button Click Listeners
        activityButton.onClick.AddListener(() =>
        {
			SceneSoundManager.Instance.PlaySFX("Click_2");
			ActivityClick?.Invoke(activityNumber);
        });
    }

    private void OnDisable()
    {
        // Unsubscribe Button Click Listeners
        activityButton.onClick.RemoveAllListeners();

        // Ensure hover color is removed and set back to normal on disable
        if (isHovered)
        {
            activityPanel.color = Color.white;
            panelText.color = Color.black;
            isHovered = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
		SceneSoundManager.Instance.PlaySFX("UI_Hover_Mono_01");
		// Change panel color to black and text to white on hover for activity button
		activityPanel.color = Color.black;
        panelText.color = Color.white;
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Change panel color to white and text to black on unhover for current hovered button
        if (isHovered)
        {
            activityPanel.color = Color.white;
            panelText.color = Color.black;
            isHovered = false;
        }
    }
}
