using System;
using UnityEngine;
using UnityEngine.UI;

public class UnderstoodIndicatorButton : MonoBehaviour
{
    public static event Action<UnderstoodState> UnderstoodIndicatorClickEvent;

    [Header("Flag To Use For Changing States")]
    [SerializeField] private UnderstoodState state;
    [Header("Understood Indicator Button")]
    [SerializeField] private Button _understoodIndicatorButton;

    private void OnEnable()
    {
        _understoodIndicatorButton.onClick.AddListener(() => UnderstoodIndicatorClickEvent?.Invoke(state));
    }

    private void OnDisable()
    {
        _understoodIndicatorButton.onClick.RemoveAllListeners();
    }
}
