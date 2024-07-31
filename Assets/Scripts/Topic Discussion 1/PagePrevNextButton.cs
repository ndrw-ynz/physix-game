using System;
using UnityEngine;
using UnityEngine.UI;

public class PagePrevNextButton : MonoBehaviour
{
    public static event Action<int> PrevNextPageClickEvent;

    public int step;
    public CanvasGroup canvasGroup;

    private Button _prevNextPageButton;

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
