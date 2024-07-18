using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrevNextButtonsManager : MonoBehaviour
{
    public DiscussionNavigator discussionNavigator;
    public DiscussionPrevNextPageButton prevPageButton;
    public DiscussionPrevNextPageButton nextPageButton;
    public DiscussionPrevNextSectorButton prevSectorButton;
    public DiscussionPrevNextSectorButton nextSectorButton;

    private void OnEnable()
    {
        discussionNavigator = discussionNavigator = GameObject.Find("MANAGERS").transform.Find("Discussion Navigator").GetComponent<DiscussionNavigator>();
        DiscussionNavigator.PageChangeEvent += ChangeButtonState;
    }


    private void ChangeButtonState(int currentSectorIndex, int currentPageIndex)
    {
        //Change button states.
        if (currentSectorIndex == 0 && currentPageIndex == 0)                             // First Sector, First Page
        {
            // Deactivate previous button functions
            prevPageButton.gameObject.SetActive(false);
            prevSectorButton.gameObject.SetActive(false);
        }
        else if (currentSectorIndex < discussionNavigator.subTopicsList.Count - 1
                && discussionNavigator.subTopicsList[currentSectorIndex].pages.Count == 1)                     //Only 1 page in a sector
        {
            // To Do: Make the Next and Previous Topic Title Automatically Adjustable
            // Sector - Previous and Sector - Next
            prevPageButton.gameObject.SetActive(false);
            nextPageButton.gameObject.SetActive(false);
            prevSectorButton.gameObject.SetActive(true);
            nextSectorButton.gameObject.SetActive(true);
        }
        else if (currentSectorIndex == discussionNavigator.subTopicsList.Count - 1
                && discussionNavigator.subTopicsList[currentSectorIndex].pages.Count == 1)                      //Only 1 page in the last sector
        {
            // Sector - Previous
            prevPageButton.gameObject.SetActive(false);
            nextPageButton.gameObject.SetActive(false);
            prevSectorButton.gameObject.SetActive(true);
            nextSectorButton.gameObject.SetActive(false);
        }
        else if (currentSectorIndex == discussionNavigator.subTopicsList.Count - 1
                && currentPageIndex == discussionNavigator.subTopicsList[currentSectorIndex].pages.Count - 1) // Last Sector, Last Page
        {
            // Deactivate next button functions
            nextPageButton.gameObject.SetActive(false);
            nextSectorButton.gameObject.SetActive(false);
        }
        else if (currentSectorIndex > 0 && currentPageIndex == 0)                         // The first page of any Sector Except First 
        {
            // Page - Next and Sector - Previous
            prevPageButton.gameObject.SetActive(false);
            nextPageButton.gameObject.SetActive(true);
            prevSectorButton.gameObject.SetActive(true);
            nextSectorButton.gameObject.SetActive(false);
        }
        else if (currentSectorIndex < discussionNavigator.subTopicsList.Count - 1
                && currentPageIndex == discussionNavigator.subTopicsList[currentSectorIndex].pages.Count - 1) // The last page of any sector except last
        {
            // Page - Previous and Sector - Next
            prevPageButton.gameObject.SetActive(true);
            nextPageButton.gameObject.SetActive(false);
            prevSectorButton.gameObject.SetActive(false);
            nextSectorButton.gameObject.SetActive(true);
        }
        else
        {
            // Activate both previous and next page buttons
            prevPageButton.gameObject.SetActive(true);
            nextPageButton.gameObject.SetActive(true);
            nextSectorButton.gameObject.SetActive(false);
            prevSectorButton.gameObject.SetActive(false);
        }
    }
}
