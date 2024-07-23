using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageCircleButton : MonoBehaviour
{
    Image[] images;
    public Image buttonOutline;
    public Image buttonColor;
    public Vector2 startPosition;
    public int pageIndex;
    public static event Action<int> OnPageCircleClick;

    private Button pageCircleButton;

    public void Initialize(int index)
    {
        images = GetComponentsInChildren<Image>();

        buttonOutline = images[0];
        buttonColor = images[2];
        startPosition = buttonColor.transform.position;
        pageIndex = index;

        pageCircleButton = GetComponentInChildren<Button>();
        pageCircleButton.onClick.AddListener(() => OnPageCircleClick?.Invoke(pageIndex));
    }

    private void OnDisable()
    {
        pageCircleButton.onClick.RemoveAllListeners();
    }
}
