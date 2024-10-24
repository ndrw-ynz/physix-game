using System;
using UnityEngine;
using UnityEngine.UI;

public class DiscussionActivityButton : MonoBehaviour
{
    public static event Action StartActivityClickEvent;

    [Header("Start Activity Button")]
    [SerializeField] private Button startActivityButton;

    private void OnEnable()
    {
        startActivityButton.onClick.AddListener(() => StartActivityClickEvent?.Invoke());
    }

    private void OnDisable()
    {
        startActivityButton.onClick.RemoveAllListeners();
    }
}
