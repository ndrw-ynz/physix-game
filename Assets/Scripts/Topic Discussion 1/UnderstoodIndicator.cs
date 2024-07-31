using System;
using UnityEngine;
using UnityEngine.UI;

public class UnderstoodIndicator : MonoBehaviour
{
    public static event Action<bool> UnderstoodIndicatorClickEvent;
    public CanvasGroup canvasGroup;

    [SerializeField] private bool flag;
    private Button _understoodIndicatorButton;

    private void OnEnable()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        _understoodIndicatorButton = GetComponent<Button>();
        _understoodIndicatorButton.onClick.AddListener(() => UnderstoodIndicatorClickEvent?.Invoke(flag));
    }

    private void OnDisable()
    {
        _understoodIndicatorButton.onClick.RemoveAllListeners();
    }
}
