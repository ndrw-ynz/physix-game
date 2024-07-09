using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;

public class ComprehensionButton : MonoBehaviour
{
    public DiscussionNavigator discussionNavigator;
    public ProgressManager progressManager;
    public bool flag;

    public void OnClick()
    {
        discussionNavigator.changeComprehensionMark(flag);
        //progressManager.loadProgressBars();
        Debug.Log("Loaded Progress Bars");
    }

    
}
