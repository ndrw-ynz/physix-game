using System;
using UnityEngine;
using UnityEngine.UI;

public class PagePrevNextButton : MonoBehaviour
{
    public static event Action<int> PrevNextPageClickEvent;

    public CanvasGroup canvasGroup;

    [Header("Direction of Page")]
    [SerializeField] private int step;
    [Header("Page Button")]
    [SerializeField] private Button _prevNextPageButton;

    private void OnEnable()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        _prevNextPageButton = GetComponent<Button>();
        _prevNextPageButton.onClick.AddListener(() => PrevNextPageClickEvent?.Invoke(step));
    }
    private void OnDisable()
    {
        _prevNextPageButton.onClick.RemoveAllListeners();
    }
}
