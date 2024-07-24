using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderstandMarkersManager : MonoBehaviour
{
    public UnderstoodNotUnderstoodButton markAsUnderstoodButton;
    public UnderstoodNotUnderstoodButton markAsNotYetUnderstoodButton;

    public static event Action<CanvasGroup> ComprehensionButtonStateChange;
    private void OnEnable()
    {
        DiscussionNavigator.UnderstandMarkerChangeEvent += ChangeComprehensionButtonState;
    }
    private void OnDisable()
    {
        DiscussionNavigator.UnderstandMarkerChangeEvent -= ChangeComprehensionButtonState;
    }

    public void ChangeComprehensionButtonState(DiscussionNavigator discNav)
    {
        if (!discNav.CurrentPageIsMarkedUnderstood())
        {
            markAsUnderstoodButton.gameObject.SetActive(true);
            markAsNotYetUnderstoodButton.gameObject.SetActive(false);

            ComprehensionButtonStateChange?.Invoke(markAsUnderstoodButton.canvasGroup);
        }
        else
        {
            markAsUnderstoodButton.gameObject.SetActive(false);
            markAsNotYetUnderstoodButton.gameObject.SetActive(true);

            ComprehensionButtonStateChange?.Invoke(markAsNotYetUnderstoodButton.canvasGroup);
        }
    }
}
