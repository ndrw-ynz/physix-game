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
        // Add click listener for a previous and next page button
        _prevNextPageButton.onClick.AddListener(() => PagePrevNextClickEvent?.Invoke(direction));
    }
    private void OnDisable()
    {
        // Remove listeners of the button when the game object has been disabled
        _prevNextPageButton.onClick.RemoveAllListeners();
    }
}
