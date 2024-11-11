using System;
using UnityEngine;
using UnityEngine.UI;

public enum TutorialNavigation
{
    PreviousPage,
    NextPage,
}

public class TutorialNavigationButton : MonoBehaviour
{
    public static event Action<TutorialNavigation> TutorialNavigationButtonClick;

    [Header("Tutorial Navigation Button Properties")]
    [SerializeField] private Button navigationButton;
    [SerializeField] private TutorialNavigation direction;
    private void OnEnable()
    {
        // Add button listener
        navigationButton.onClick.AddListener(() => TutorialNavigationButtonClick?.Invoke(direction));
    }

    private void OnDisable()
    {
        // Remove button listener
        navigationButton.onClick.RemoveAllListeners();
    }
}
