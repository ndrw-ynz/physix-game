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
        bool isOnlySinglePageInFirstSector = currentSectorIndex == 0 && discussionNavigator.subTopicsList[currentSectorIndex].pages.Count == 1;
        bool isFirstSectorFirstPage = currentSectorIndex == 0 && currentPageIndex == 0;
        bool isOnlySinglePageInSector = currentSectorIndex < discussionNavigator.subTopicsList.Count - 1 && discussionNavigator.subTopicsList[currentSectorIndex].pages.Count == 1;
        bool isOnlySinglePageInLastSector = currentSectorIndex == discussionNavigator.subTopicsList.Count - 1 && discussionNavigator.subTopicsList[currentSectorIndex].pages.Count == 1;
        bool isLastSectorLastPage = currentSectorIndex == discussionNavigator.subTopicsList.Count - 1 && currentPageIndex == discussionNavigator.subTopicsList[currentSectorIndex].pages.Count - 1;
        bool isNotFirstSectorFirstPage = currentSectorIndex > 0 && currentPageIndex == 0;
        bool isNotLastSectorLastPage = currentSectorIndex < discussionNavigator.subTopicsList.Count - 1 && currentPageIndex == discussionNavigator.subTopicsList[currentSectorIndex].pages.Count - 1;

        //Change button states.
        if (isOnlySinglePageInFirstSector)
        {
            // ACTIVATE ONLY Next Sector Button
            nextSectorButton.gameObject.SetActive(true);

            prevSectorButton.gameObject.SetActive(false);
            prevPageButton.gameObject.SetActive(false);
            nextPageButton.gameObject.SetActive(false);
        }
        else if (isFirstSectorFirstPage)
        {
            // DEACTIVATE Both Previous Buttons
            prevPageButton.gameObject.SetActive(false);
            prevSectorButton.gameObject.SetActive(false);
        }
        else if (isOnlySinglePageInSector)
        {
            // ACTIVATE Both Sector Buttons
            prevSectorButton.gameObject.SetActive(true);
            nextSectorButton.gameObject.SetActive(true);

            prevPageButton.gameObject.SetActive(false);
            nextPageButton.gameObject.SetActive(false);
        }
        else if (isNotFirstSectorFirstPage)
        {
            // ACTIVATE Previous Sector and Next Page Button
            prevSectorButton.gameObject.SetActive(true);
            nextPageButton.gameObject.SetActive(true);

            prevPageButton.gameObject.SetActive(false);
            nextSectorButton.gameObject.SetActive(false);
        }
        else if (isNotLastSectorLastPage)
        {
            // ACTIVATE Next Sector and Previous Page Button
            prevPageButton.gameObject.SetActive(true);
            nextSectorButton.gameObject.SetActive(true);

            nextPageButton.gameObject.SetActive(false);
            prevSectorButton.gameObject.SetActive(false);
        }
        else if (isOnlySinglePageInLastSector)
        {
            // ACTIVATE ONLY Previous Sector Button
            prevSectorButton.gameObject.SetActive(true);

            prevPageButton.gameObject.SetActive(false);
            nextPageButton.gameObject.SetActive(false);
            nextSectorButton.gameObject.SetActive(false);
        }
        else if (isLastSectorLastPage)
        {
            // DEACTIVATE Both Next Buttons
            nextPageButton.gameObject.SetActive(false);
            nextSectorButton.gameObject.SetActive(false);
        }
        else
        {
            // ACTIVATE Both Page Buttons
            prevPageButton.gameObject.SetActive(true);
            nextPageButton.gameObject.SetActive(true);

            nextSectorButton.gameObject.SetActive(false);
            prevSectorButton.gameObject.SetActive(false);
        }
    }


}
