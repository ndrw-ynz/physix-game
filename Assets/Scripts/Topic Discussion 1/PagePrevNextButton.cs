using System;
using UnityEngine;
using UnityEngine.UI;

public class PagePrevNextButton : MonoBehaviour
{
    public static event Action<Direction> PagePrevNextClickEvent;

    public CanvasGroup canvasGroup;

    [Header("Direction of Page")]
    [SerializeField] private Direction direction;
    [Header("Page Button")]
    [SerializeField] private Button _prevNextPageButton;

    private void OnEnable()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        _prevNextPageButton = GetComponent<Button>();
        _prevNextPageButton.onClick.AddListener(() => PagePrevNextClickEvent?.Invoke(direction));
    }
    private void OnDisable()
    {
        _prevNextPageButton.onClick.RemoveAllListeners();
    }
}
