using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SectorIndicatorRect : MonoBehaviour
{
    public Image indicatorRectImage;
    public RectTransform indicatorRectTransform;
    public Vector2 startPosition;

    public void Initialize()
    {
        indicatorRectImage = GetComponent<Image>();
        indicatorRectTransform = GetComponent<RectTransform>();
        startPosition = indicatorRectImage.transform.position;
    }
}