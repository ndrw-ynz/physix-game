using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SectorPrevNextButton : MonoBehaviour
{
    public static event Action<string> PrevNextSectorClickEvent;

    [Header("Subtopic Name Text Holder")]
    public TextMeshProUGUI sectorButtonText;
    public CanvasGroup canvasGroup;
    [Header("Direction of Sector Button")]
    public string action;

    // Button Component for Previous and Next Sector Button
    private Button _prevNextSectorButton;

    private void OnEnable()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        _prevNextSectorButton = GetComponent<Button>();
        _prevNextSectorButton.onClick.AddListener(() => PrevNextSectorClickEvent.Invoke(action));
    }

    private void OnDisable()
    {
        _prevNextSectorButton.onClick.RemoveAllListeners();
    }
}
