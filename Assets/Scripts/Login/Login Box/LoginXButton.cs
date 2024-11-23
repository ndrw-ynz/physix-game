using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoginXButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action LoginXButtonClicked;

    [Header("Login X Button Properties")]
    [SerializeField] private Button loginXButton;
    [SerializeField] private Image loginXHoverBackground;

    // Boolean to check if button is being hovered or not
    private bool isHovered;

    private void OnEnable()
    {
        // Subscribe Button Click Listeners
        loginXButton.onClick.AddListener(() =>
        {
			SceneSoundManager.Instance.PlaySFX("Click_2");
			LoginXButtonClicked?.Invoke();
        });
    }

    private void OnDisable()
    {
        // Unsubscribe Button Click Listeners
        loginXButton.onClick.RemoveAllListeners();

        // Ensure hover color is removed and set back to normal on disable
        if (isHovered)
        {
            loginXHoverBackground.gameObject.SetActive(false);
            isHovered = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
		SceneSoundManager.Instance.PlaySFX("UI_Hover_Mono_01");
		// Activate button hover red outline
		loginXHoverBackground.gameObject.SetActive(true);
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Deactivate button hover red outline on unhover for the current hovered button
        if (isHovered)
        {
            loginXHoverBackground.gameObject.SetActive(false);
            isHovered = false;
        }
    }
}
