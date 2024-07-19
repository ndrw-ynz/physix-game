using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderstandMarkersManager : MonoBehaviour
{
    public UnderstoodNotUnderstoodButton markAsUnderstoodButton;
    public UnderstoodNotUnderstoodButton markAsNotYetUnderstoodButton;

    private void OnEnable()
    {
        DiscussionNavigator.UnderstandMarkerChangeEvent += ChangeComprehensionButtonState;
    }

    public void ChangeComprehensionButtonState(DiscussionNavigator discNav)
    {
        if (!discNav.CurrentPageIsMarkedUnderstood())
        {
            markAsUnderstoodButton.gameObject.SetActive(true);
            markAsNotYetUnderstoodButton.gameObject.SetActive(false);
        }
        else
        {
            markAsUnderstoodButton.gameObject.SetActive(false);
            markAsNotYetUnderstoodButton.gameObject.SetActive(true);
        }
    }
}
