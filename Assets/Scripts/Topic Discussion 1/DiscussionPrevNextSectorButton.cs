using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiscussionPrevNextSectorButton : MonoBehaviour
{
    public DiscussionNavigator discussionNavigator;
    public string action;
    public TextMeshProUGUI prevSectorTitle;
    public TextMeshProUGUI nextSectorTitle;

    public void OnClick()
    {
        discussionNavigator.changeSector(action);
        prevSectorTitle.SetText(discussionNavigator.setPrevSectorText());
        nextSectorTitle.SetText(discussionNavigator.setNextSectorText());
    }
}
