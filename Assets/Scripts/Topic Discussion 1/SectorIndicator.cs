using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SectorIndicator : MonoBehaviour
{
    public Image circleIndicatorImage;
    public RectTransform circleIndicatorRectTransform;
    public Vector2 startPosition;

    public void Initialize()
    {
        circleIndicatorImage = GetComponent<Image>();
        circleIndicatorRectTransform = GetComponent<RectTransform>();
        startPosition = circleIndicatorImage.transform.position;
    }
}