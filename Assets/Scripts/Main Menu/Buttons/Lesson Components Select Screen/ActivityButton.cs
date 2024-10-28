using System;
using UnityEngine;
using UnityEngine.UI;

public class ActivityButton : MonoBehaviour
{
    public static event Action<int> ActivityClick;

    [Header("Activity Button Properties")]
    [SerializeField] private GameObject activityPanel;
    [SerializeField] private Button activityButton;

    [Header("Activity Button Number")]
    [SerializeField] private int activityNumber;

    private void OnEnable()
    {
        // Subscribe Button Click Listeners
        activityButton.onClick.AddListener(() => ActivityClick?.Invoke(activityNumber));
    }

    private void OnDisable()
    {
        // Unsubscribe Button Click Listeners
        activityButton.onClick.RemoveAllListeners();
    }
}
