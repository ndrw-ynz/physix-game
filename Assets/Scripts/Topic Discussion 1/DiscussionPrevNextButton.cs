using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscussionPrevNextButton : MonoBehaviour
{
    public DiscussionNavigator discussionNavigator;
    public int step;

    public void OnClick()
    {
        discussionNavigator.changePage(step);
    }

}
