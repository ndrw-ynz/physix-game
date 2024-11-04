using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BackButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Screen Game Objects")]
    public GameObject screenToActivate;
    public GameObject screenToDeactivate;

    [Header("Back Button Properties")]
    [SerializeField] private Image buttonHoverOutline;
    [SerializeField] private Button backButton;

    // Boolean to check if button is being hovered or not
    private bool isHovered;

    private void OnEnable()
    {
        // Subscribe Button Click Listeners
        backButton.onClick.AddListener(() => CloseScreen(screenToActivate, screenToDeactivate));
    }

    private void OnDisable()
    {
        // Unsubscribe Button Click Listeners
        backButton.onClick.RemoveAllListeners();

        // Ensure hover color is removed and set back to normal on disable
        if (isHovered)
        {
            buttonHoverOutline.gameObject.SetActive(false);
            isHovered = false;
        }
    }

    private void CloseScreen(GameObject screenToActivate, GameObject screenToDeactivate)
    {
        // Close current screen and open specified screen
        if (screenToDeactivate.activeSelf && screenToActivate != null)
        {
            screenToDeactivate.gameObject.SetActive(false);
            screenToActivate.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Current screen is already deactivated. Maybe you referenced something wrong?");
        }
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
