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
    private Button _progressBarButton;
    public Image progressBarImage;
    public Vector2 startPosition;
    public TextMeshProUGUI sectorTitleText;
    public TextMeshProUGUI progressCountText;

    public int sectorIndex;

    public static event Action<int> ProgressBarClickEvent;

    public void Initialize(string sectorTitle, string progressCount, int index)
    {
        textComponents = GetComponentsInChildren<TextMeshProUGUI>();

        progressBarImage = GetComponent<Image>();
        startPosition = progressBarImage.transform.position;
        sectorTitleText = textComponents[0];
        sectorTitleText.text = sectorTitle;
        progressCountText = textComponents[1];
        progressCountText.text = progressCount;
        sectorIndex = index;
        _progressBarButton = this.GetComponent<Button>();
        _progressBarButton.onClick.AddListener(() => ProgressBarClickEvent?.Invoke(sectorIndex));
    }
}