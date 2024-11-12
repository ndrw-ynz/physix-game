using System;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCloseButton : MonoBehaviour
{
    public static event Action CloseButtonClicked;

    [SerializeField] private Button closeButton;

    private void OnEnable()
    {
        // Add listeners
        closeButton.onClick.AddListener(() => CloseButtonClicked?.Invoke());
    }

    private void OnDisable()
    {
        // Remove listeners
        closeButton.onClick.RemoveAllListeners();
    }
}
