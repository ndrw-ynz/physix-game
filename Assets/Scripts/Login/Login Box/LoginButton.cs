using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoginButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action LoginButtonClick;

    [Header("Login Button Properties")]
    [SerializeField] private Button loginButton;
    [SerializeField] private Image buttonImage;

    // Boolean to check if button is being hovered or not
    private bool isHovered;

    private void OnEnable()
    {
        // Subscribe Button Click Listeners
        loginButton.onClick.AddListener(() => LoginButtonClick?.Invoke());
    }

    private void OnDisable()
    {
        // Unsubscribe Button Click Listeners
        loginButton.onClick.RemoveAllListeners();

        // Ensure hover color is removed and set back to normal on disable
        if (isHovered)
        {
            buttonImage.color = new Color(0.4117647f, 0.6078432f, 0.9058824f);
            isHovered = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Set color to darker blue when hovered
        buttonImage.color = new Color(0.3393556f, 0.5116689f, 0.7735849f);
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isHovered)
        {
            // Set back to lighter blue when unhovered
            buttonImage.color = new Color(0.4117647f, 0.6078432f, 0.9058824f);
            isHovered = false;
        }
    }
}
