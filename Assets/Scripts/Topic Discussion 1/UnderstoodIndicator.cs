using System;
using UnityEngine;
using UnityEngine.UI;

public class UnderstoodIndicator : MonoBehaviour
{
    public static event Action<bool> UnderstoodIndicatorClickEvent;

    [Header("Object Group for Animation")]
    public CanvasGroup understoodIndicatorCanvasGroup;
    [Header("Flag To Use For Changing States")]
    [SerializeField] private bool flag;
    [Header("Understood Indicator Button")]
    [SerializeField] private Button _understoodIndicatorButton;

    private void OnEnable()
    {
        understoodIndicatorCanvasGroup = GetComponent<CanvasGroup>();
        _understoodIndicatorButton.onClick.AddListener(() => UnderstoodIndicatorClickEvent?.Invoke(flag));
    }

    private void OnDisable()
    {
        _understoodIndicatorButton.onClick.RemoveAllListeners();
    }
}
