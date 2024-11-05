using System;
using UnityEngine;
using UnityEngine.UI;

public class LogoutButton : MonoBehaviour
{
    public static event Action LogoutButtonClick;

    [Header("Logout Button")]
    [SerializeField] Button logoutButton;

    private void OnEnable()
    {
        // Add Listener
        logoutButton.onClick.AddListener(() => LogoutButtonClick?.Invoke());
    }

    private void OnDisable()
    {
        // Remove Listener
        logoutButton.onClick.RemoveAllListeners();
    }
}
