using System;
using UnityEngine;
using UnityEngine.UI;

public class PagePrevNextButton : MonoBehaviour
{
    public static event Action<Direction> PagePrevNextClickEvent;

    [Header("Direction of Page")]
    [SerializeField] private Direction direction;
    [Header("Page Button")]
    [SerializeField] private Button _prevNextPageButton;

    private void OnEnable()
    {
        _prevNextPageButton.onClick.AddListener(() => PagePrevNextClickEvent?.Invoke(direction));
    }
    private void OnDisable()
    {
        _prevNextPageButton.onClick.RemoveAllListeners();
    }
}
