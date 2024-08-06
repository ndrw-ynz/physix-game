using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageJumpButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<int> OnPageCircleClick;

    [Header("Images")]
    public Image buttonOutline;
    public Image buttonHoverOutline;
    public Image buttonColor;
    [Header("Button")]
    [SerializeField] private Button _pageCircleButton;
    [Header("Renderer")]
    [SerializeField] private SpriteRenderer _buttonRenderer;

    // Page index to be jumped after button press
    private int _pageIndex;
    // Start Position of page jump button
    private Vector2 _startPosition;

    public void Initialize(int index)
    {
        _pageIndex = index;
        _startPosition = buttonColor.transform.position;
        _pageCircleButton.onClick.AddListener(() => OnPageCircleButtonCLick());
    }

    private void OnDisable()
    {
        _pageCircleButton.onClick.RemoveAllListeners();
    }

    private void OnPageCircleButtonCLick()
    {
        buttonHoverOutline.gameObject.SetActive(false);
        OnPageCircleClick?.Invoke(_pageIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!buttonOutline.gameObject.activeSelf)
        {
            buttonHoverOutline.gameObject.SetActive(true);
            buttonHoverOutline.color = new Color(1f, 1f, 1f, 0.3f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonHoverOutline.gameObject.SetActive(false);
    }
}
