using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiscussionPrevNextPageButton : MonoBehaviour
{
    public DiscussionNavigator discussionNavigator;
    public int step;
    

    public void OnClick()
    {
        discussionNavigator.changePage(step);
    }
}
