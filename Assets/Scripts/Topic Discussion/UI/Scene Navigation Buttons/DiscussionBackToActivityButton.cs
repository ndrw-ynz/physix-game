using System;
using UnityEngine;
using UnityEngine.UI;

public class DiscussionBackToActivityButton : MonoBehaviour
{
    public static event Action BackToActivityClickEvent;

    [Header("Back To Activity Button")]
    [SerializeField] private Button backToActivityButton;

    private void OnEnable()
    {
        backToActivityButton.onClick.AddListener(() => BackToActivityClickEvent?.Invoke());
    }

    private void OnDisable()
    {
        backToActivityButton.onClick.RemoveAllListeners();
    }
}
