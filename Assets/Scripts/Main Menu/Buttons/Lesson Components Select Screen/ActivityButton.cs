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
        activityButton.onClick.AddListener(() => ActivityClick?.Invoke(activityNumber));
    }

    private void OnDisable()
    {
        // Unsubscribe Button Click Listeners
        activityButton.onClick.RemoveAllListeners();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        activityPanel.color = Color.black;
        panelText.color = Color.white;
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        activityPanel.color = Color.white;
        panelText.color = Color.black;
        isHovered = false;
    }
}
