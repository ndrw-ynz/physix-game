using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleScreenButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Screen Game Objects")]
    [SerializeField] private GameObject screenToActivate;
    [SerializeField] private GameObject screenToDeactivate;

    [Header("Title Screen Button Properties")]
    [SerializeField] private Button titleScreenButton;
    [SerializeField] private Image titleScreenButtonImage;
    [SerializeField] private TextMeshProUGUI titleScreenButtonText;

    // Boolean to check if button is being hovered or not
    private bool isHovered;

    private void OnEnable()
    {
        // Subscribe Button Click Listeners
        titleScreenButton.onClick.AddListener(() => ChangeScreen(screenToActivate, screenToDeactivate));
    }

    private void OnDisable()
    {
        // Unsubscribe Button Click Listeners
        titleScreenButton.onClick.RemoveAllListeners();

        // Ensure hover color is removed and set back to normal on disable
        if (isHovered)
        {
            titleScreenButtonImage.gameObject.SetActive(false);
            titleScreenButtonText.color = Color.white;
            isHovered = false;
        }
    }

    private void ChangeScreen(GameObject screenToActivate, GameObject screenToDeactivate)
    {
        // Close current screen and open specified screen
        if (screenToDeactivate.activeSelf && screenToActivate != null)
        {
            screenToDeactivate.gameObject.SetActive(false);
            screenToActivate.gameObject.SetActive(true);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Activate button hover white outline and change text to black
        titleScreenButtonImage.gameObject.SetActive(true);
        titleScreenButtonText.color = Color.black;
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Deactivate button hover white outline and change text to white on unhover for the current hovered button
        if (isHovered)
        {
            titleScreenButtonImage.gameObject.SetActive(false);
            titleScreenButtonText.color = Color.white;
            isHovered = false;
        }
    }
}
