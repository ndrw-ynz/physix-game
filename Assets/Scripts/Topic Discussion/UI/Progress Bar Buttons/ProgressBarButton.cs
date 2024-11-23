using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProgressBarButton : MonoBehaviour, IPointerEnterHandler
{
    public static event Action<int> ProgressBarClickEvent;

    [Header("Image for Colors")]
    public Image progressBarTempColor;
    public Image progressBarFinalColor;
    [Header("Image for Indicator")]
    public Image progressBarIndicator;
    public RectTransform progressBarRectTransform;
    [Header("Text Holders")]
    public TextMeshProUGUI sectorTitleText;
    public TextMeshProUGUI progressCountText;
    [Header("Progress Bar Button")]
    [SerializeField] private Button _progressBarButton;

    // Index to be jumped when button is pressed
    private int _sectorIndex;

    public void Initialize(string sectorTitle, string progressCount, int index)
    {
        // Initialize the proper sector title text, progress count text, sector index,and on click listener for a progress bar button button
        sectorTitleText.text = sectorTitle;
        progressCountText.text = progressCount;
        _sectorIndex = index;
        _progressBarButton.onClick.AddListener(() =>
        {
			SceneSoundManager.Instance.PlaySFX("Click_2");
			ProgressBarClickEvent?.Invoke(_sectorIndex);
        });
    }

	public void OnPointerEnter(PointerEventData eventData)
	{
		SceneSoundManager.Instance.PlaySFX("UI_Hover_Mono_01");
	}
}