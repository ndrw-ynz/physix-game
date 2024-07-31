using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarButton : MonoBehaviour
{
    public static event Action<int> ProgressBarClickEvent;

    public Image progressBarTempColor;
    public Image progressBarFinalColor;
    public TextMeshProUGUI sectorTitleText;
    public TextMeshProUGUI progressCountText;

    private Vector2 _startPosition;
    private int _sectorIndex;
    [SerializeField] private Button _progressBarButton;


    public void Initialize(string sectorTitle, string progressCount, int index)
    {
        
        sectorTitleText.text = sectorTitle;
        progressCountText.text = progressCount;

        _startPosition = progressBarFinalColor.transform.position;
        _sectorIndex = index;
        _progressBarButton.onClick.AddListener(() => ProgressBarClickEvent?.Invoke(_sectorIndex));
    }
}