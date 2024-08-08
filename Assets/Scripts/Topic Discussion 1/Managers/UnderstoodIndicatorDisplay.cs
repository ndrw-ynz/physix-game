using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class UnderstoodIndicatorDisplay : MonoBehaviour
//{
//    public UnderstoodIndicator markAsUnderstoodButton;
//    public UnderstoodIndicator markAsNotYetUnderstoodButton;
//
//    private void OnEnable()
//    {
//        DiscussionNavigator.UnderstandMarkerChangeEvent += ChangeComprehensionButtonState;
//    }
//    private void OnDisable()
//    {
//        DiscussionNavigator.UnderstandMarkerChangeEvent -= ChangeComprehensionButtonState;
//    }
//
//    public void ChangeComprehensionButtonState(DiscussionNavigator discNav)
//    {
//        if (!discNav.CurrentPageIsMarkedUnderstood())
//        {
//            markAsUnderstoodButton.gameObject.SetActive(true);
//            markAsNotYetUnderstoodButton.gameObject.SetActive(false);
//        }
//        else
//        {
//            markAsUnderstoodButton.gameObject.SetActive(false);
//            markAsNotYetUnderstoodButton.gameObject.SetActive(true);
//        }
//    }
//}
