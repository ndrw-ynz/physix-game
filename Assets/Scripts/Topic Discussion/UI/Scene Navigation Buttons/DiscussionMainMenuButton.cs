using System;
using UnityEngine;
using UnityEngine.UI;


public class DiscussionMainMenuButton : MonoBehaviour
{
    public static event Action BackToMainMenuClickEvent;

    [Header("Back To Main Menu Button")]
    [SerializeField] private Button backToMainMenuButton;

    private void OnEnable()
    {
        backToMainMenuButton.onClick.AddListener(() => BackToMainMenuClickEvent?.Invoke());
    }

    private void OnDisable()
    {
        backToMainMenuButton.onClick.RemoveAllListeners();
    }
}
