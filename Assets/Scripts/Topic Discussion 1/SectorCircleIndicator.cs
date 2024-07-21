using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SectorCircleIndicator : MonoBehaviour
{
    public Image circleIndicatorImage;
    public Vector2 startPosition;

    public void Initialize()
    {
        circleIndicatorImage = GetComponent<Image>();
        startPosition = circleIndicatorImage.transform.position;
    }
}