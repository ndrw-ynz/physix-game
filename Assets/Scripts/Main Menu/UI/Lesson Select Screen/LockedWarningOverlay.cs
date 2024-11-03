using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LockedWarningOverlay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action OkClickEvent;

    [Header("Ok Button")]
    [SerializeField] private Button okButton;
    [SerializeField] private Image buttonOverlay;
    [SerializeField] private TextMeshProUGUI buttonText;

    // Boolean to check if button is being hovered or not
    private bool isHovered;

    private void OnEnable()
    {
        // Subscribe Button Click Listeners
        okButton.onClick.AddListener(() => OkClickEvent?.Invoke());
    }

    private void OnDisable()
    {
        // Unsubscribe Button Click Listeners
        okButton.onClick.RemoveAllListeners();

        // Ensure hover color is removed and set back to normal on disable
        if (isHovered)
        {
            buttonOverlay.gameObject.SetActive(false);
            buttonText.color = Color.white;
            isHovered = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Activate button hover white outline and change text to black
        buttonOverlay.gameObject.SetActive(true);
        buttonText.color = Color.black;
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Deactivate button hover white outline and change text to white on unhover for the current hovered button
        if (isHovered)
        {
            buttonOverlay.gameObject.SetActive(false);
            buttonText.color = Color.white;
            isHovered = false;
        }
    }
}
