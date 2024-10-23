using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageJumpButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<int> PageCircleClick;

    [Header("Page Jump Image Properties")]
    public Image buttonOutline;
    public Image buttonHoverOutline;
    public Image buttonColor;

    [Header("Page Circle Button")]
    [SerializeField] private Button _pageCircleButton;

    [Header("Button Renderer")]
    [SerializeField] private SpriteRenderer _buttonRenderer;

    // Page index to be jumped after button press
    private int _pageIndex;
    // Start Position of page jump button
    private Vector2 _startPosition;

    public void Initialize(int index)
    {
        // Initialize the proper page index, starting position, and on click listener for a page jump button
        _pageIndex = index;
        _startPosition = buttonColor.transform.position;
        _pageCircleButton.onClick.AddListener(() => OnPageCircleButtonCLick());
    }

    private void OnDisable()
    {
        // Remove listeners of the button when the game object has been disabled
        _pageCircleButton.onClick.RemoveAllListeners();
    }

    private void OnPageCircleButtonCLick()
    {
        // Disable the button's on hover outline and invoke the page circle click event
        buttonHoverOutline.gameObject.SetActive(false);
        PageCircleClick?.Invoke(_pageIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!buttonOutline.gameObject.activeSelf)
        {
            // Activate the button's hover outline upon mouse hover only if the button outline is not active
            buttonHoverOutline.gameObject.SetActive(true);
            buttonHoverOutline.color = new Color(1f, 1f, 1f, 0.3f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Deactivate the button's hover outline
        buttonHoverOutline.gameObject.SetActive(false);
    }
}
