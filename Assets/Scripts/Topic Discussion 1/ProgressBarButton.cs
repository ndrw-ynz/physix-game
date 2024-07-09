using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarButton : MonoBehaviour
{
    private Image _progressBarImage;
    private Button _progressBarButton;
    public Vector2 startPosition;
    public TextMeshProUGUI sectorTitleText;
    public TextMeshProUGUI progressCountText;
    

    public int sectorIndex;

    public void Initialize(string sectorTitle, string progressCount, int index, UnityEngine.Events.UnityAction<int> onClickAction)
    {
        _progressBarImage = GetComponent<Image>();
        startPosition = _progressBarImage.transform.position;
        sectorTitleText = GetComponentInChildren<TextMeshProUGUI>();
        sectorTitleText.text = sectorTitle;
        progressCountText = GetComponentInChildren<TextMeshProUGUI>();
        progressCountText.text = progressCount;
        _progressBarButton = this.GetComponent<Button>();
        _progressBarButton.onClick.AddListener(() => onClickAction.Invoke(index));
    }
}