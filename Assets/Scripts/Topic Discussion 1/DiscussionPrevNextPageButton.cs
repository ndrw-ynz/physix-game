using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiscussionPrevNextPageButton : MonoBehaviour
{
    public DiscussionNavigator discussionNavigator;
    public int step;
    public TextMeshProUGUI prevSectorTitle;
    public TextMeshProUGUI nextSectorTitle;
    

    public void OnClick()
    {
        discussionNavigator.changePage(step);
        prevSectorTitle.SetText(discussionNavigator.setPrevSectorText());
        nextSectorTitle.SetText(discussionNavigator.setNextSectorText());
    }
}
