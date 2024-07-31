using UnityEngine;
using UnityEngine.UI;

public class SectorIndicator : MonoBehaviour
{
    public Image indicatorRectImage;
    public RectTransform indicatorRectTransform;

    private Vector2 _startPosition;

    public void Initialize()
    {
        indicatorRectImage = GetComponent<Image>();
        indicatorRectTransform = GetComponent<RectTransform>();

        _startPosition = indicatorRectImage.transform.position;
    }
}