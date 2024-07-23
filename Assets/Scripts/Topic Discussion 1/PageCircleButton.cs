using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageCircleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image[] images;
    public Image buttonOutline;
    public Image buttonHoverOutline;
    public Image buttonColor;
    public Vector2 startPosition;
    public int pageIndex;
    public static event Action<int> OnPageCircleClick;

    private SpriteRenderer _buttonRenderer;
    private Button _pageCircleButton;

    public void Initialize(int index)
    {
        images = GetComponentsInChildren<Image>();

        buttonOutline = images[0];
        buttonHoverOutline = images[1];
        buttonColor = images[3];
        startPosition = buttonColor.transform.position;
        pageIndex = index;
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
        OnPageCircleClick?.Invoke(pageIndex);
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
