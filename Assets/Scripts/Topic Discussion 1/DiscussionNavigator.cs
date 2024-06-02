using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DiscussionNavigator : MonoBehaviour
{
    public List<GameObject> discussionPagesList;
    public DiscussionPrevNextButton prevButton;
    public DiscussionPrevNextButton nextButton;
    private int _currentIndex = 0;

    public void changePage(int direction)
    {
        // Change pages.
        if (_currentIndex != 0 || _currentIndex < discussionPagesList.Count)
        {
            _currentIndex += direction;
            for (int i = 0; i < discussionPagesList.Count; i++)
            {
                if (i == _currentIndex)
                {
                    discussionPagesList[i].SetActive(true);
                } else
                {
                    discussionPagesList[i].SetActive(false);
                }
            }
        }
        // Change button states.
        if (_currentIndex == 0)
        {
            prevButton.gameObject.SetActive(false);
        }
        else if (_currentIndex == discussionPagesList.Count-1)
        {
            nextButton.gameObject.SetActive(false);
        }
        else
        {
            prevButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);
        }
    }
    
}
