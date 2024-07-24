using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarButton : MonoBehaviour
{
    TextMeshProUGUI[] textComponents;
    Image[] imageComponents;

    public Image progressBarTempColor;
    public Image progressBarFinalColor;
    public Vector2 startPosition;
    public TextMeshProUGUI sectorTitleText;
    public TextMeshProUGUI progressCountText;

    public static event Action<int> ProgressBarClickEvent;

    private int _sectorIndex;
    private Button _progressBarButton;


    public void Initialize(string sectorTitle, string progressCount, int index)
    {
        textComponents = GetComponentsInChildren<TextMeshProUGUI>();
        imageComponents = GetComponentsInChildren<Image>();

        progressBarTempColor = imageComponents[0];
        progressBarFinalColor = imageComponents[1];
        startPosition = progressBarFinalColor.transform.position;
        
        sectorTitleText = textComponents[0];
        sectorTitleText.text = sectorTitle;
        progressCountText = textComponents[1];
        progressCountText.text = progressCount;
        _sectorIndex = index;
        _progressBarButton = this.GetComponent<Button>();
        _progressBarButton.onClick.AddListener(() => ProgressBarClickEvent?.Invoke(_sectorIndex));
    }
}