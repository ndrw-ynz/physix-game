using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageJumpButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<int> OnPageCircleClick;

    public Image buttonOutline;
    public Image buttonHoverOutline;
    public Image buttonColor;

    private int _pageIndex;
    private Vector2 _startPosition;
    private SpriteRenderer _buttonRenderer;
    private Button _pageCircleButton;

    public void Initialize(int index)
    {
        _pageIndex = index;

        _startPosition = buttonColor.transform.position;
        _buttonRenderer = GetComponentInChildren<SpriteRenderer>();
        _pageCircleButton = GetComponentInChildren<Button>();
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
