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
    [Header("Button")]
    [SerializeField] private Button _prevNextSectorButton;

    private void OnEnable()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        _prevNextSectorButton.onClick.AddListener(() => PrevNextSectorClickEvent.Invoke(action));
    }

    private void OnDisable()
    {
        _prevNextSectorButton.onClick.RemoveAllListeners();
    }
}
