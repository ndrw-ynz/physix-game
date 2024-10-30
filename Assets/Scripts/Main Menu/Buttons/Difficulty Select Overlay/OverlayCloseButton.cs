using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OverlayCloseButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<int> OverlayCloseClicked;

    [Header("Close Overlay Button Properties")]
    [SerializeField] private Image buttonHoverOutline;
    [SerializeField] private Button closeOverlayButton;

    [Header("Difficulty Select Lesson Number")]
    [SerializeField] private int lessonNumber;

    // Boolean to check if button is being hovered or not
    private bool isHovered;

    private void OnEnable()
    {
        // Subscribe Button Click Listeners
        closeOverlayButton.onClick.AddListener(() => OverlayCloseClicked?.Invoke(lessonNumber));
    }

    private void OnDisable()
    {
        // Unsubscribe Button Click Listeners
        closeOverlayButton.onClick.RemoveAllListeners();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Activate button hover red outline
        buttonHoverOutline.gameObject.SetActive(true);
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Deactivate button hover red outline on unhover for the current hovered button
        if (isHovered)
        {
            buttonHoverOutline.gameObject.SetActive(false);
            isHovered = false;
        }
    }
}
