using UnityEngine;
using UnityEngine.UI;

public class SectorIndicator : MonoBehaviour
{
    [Header("Indicator Image Properties")]
    public Image indicatorRectImage;
    public RectTransform indicatorRectTransform;

    // Start position of sector indicator
    private Vector2 _startPosition;

    public void Initialize()
    {
        _startPosition = indicatorRectImage.transform.position;
    }
}