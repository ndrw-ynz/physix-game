using System;
using UnityEngine;
using UnityEngine.UI;

public class UnderstoodIndicator : MonoBehaviour
{
    public static event Action<bool> UnderstoodIndicatorClickEvent;

    [Header("Object Group for Animation")]
    public CanvasGroup understoodIndicatorCanvasGroup;

    [Header("Flag State To Be Used")]
    [SerializeField] private bool flag;

    // Button Component of Understood Indicator
    private Button _understoodIndicatorButton;

    private void OnEnable()
    {
        understoodIndicatorCanvasGroup = GetComponent<CanvasGroup>();

        _understoodIndicatorButton = GetComponent<Button>();
        _understoodIndicatorButton.onClick.AddListener(() => UnderstoodIndicatorClickEvent?.Invoke(flag));
    }

    private void OnDisable()
    {
        _understoodIndicatorButton.onClick.RemoveAllListeners();
    }
}
