using System;
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

    public static event Action<PrevNextButtonsManager> PreviousSectorButtonActiveEvent;
    public static event Action<PrevNextButtonsManager> NextSectorButtonActiveEvent;

    private void OnEnable()
    {
        DiscussionNavigator.PageChangeEvent += ChangeButtonState;
    }

    public void SetPrevSectorText(string previousSectorTitle)
    {
        prevSectorButton.sectorButtonText.text = previousSectorTitle;
    }

    public void SetNextSectorText(string nextSectorTitle)
    {
        nextSectorButton.sectorButtonText.text = nextSectorTitle;
    }

    private void ChangeButtonState(int currentSectorIndex, int currentPageIndex, int subTopicsListCount, int currentSectorPagesCount)
    {
        bool isOnlySinglePageInFirstSector = currentSectorIndex == 0 && currentSectorPagesCount == 1;
        bool isFirstSectorFirstPage = currentSectorIndex == 0 && currentPageIndex == 0;
        bool isOnlySinglePageInSector = currentSectorIndex < subTopicsListCount - 1 && currentSectorPagesCount == 1;
        bool isOnlySinglePageInLastSector = currentSectorIndex == subTopicsListCount - 1 && currentSectorPagesCount == 1;
        bool isLastSectorLastPage = currentSectorIndex == subTopicsListCount - 1 && currentPageIndex == currentSectorPagesCount - 1;
        bool isNotFirstSectorFirstPage = currentSectorIndex > 0 && currentPageIndex == 0;
        bool isNotLastSectorLastPage = currentSectorIndex < subTopicsListCount - 1 && currentPageIndex == currentSectorPagesCount - 1;

        //Change button states.
        if (isOnlySinglePageInFirstSector)
        {
            // ACTIVATE ONLY Next Sector Button
            nextSectorButton.gameObject.SetActive(true);

            NextSectorButtonActiveEvent?.Invoke(this);

            prevSectorButton.gameObject.SetActive(false);
            prevPageButton.gameObject.SetActive(false);
            nextPageButton.gameObject.SetActive(false);
        }
        else if (isFirstSectorFirstPage)
        {
            // DEACTIVATE Both Previous Buttons
            prevPageButton.gameObject.SetActive(false);
            prevSectorButton.gameObject.SetActive(false);

            NextSectorButtonActiveEvent?.Invoke(this);
        }
        else if (isOnlySinglePageInSector)
        {
            // ACTIVATE Both Sector Buttons
            prevSectorButton.gameObject.SetActive(true);
            nextSectorButton.gameObject.SetActive(true);

            PreviousSectorButtonActiveEvent?.Invoke(this);
            NextSectorButtonActiveEvent?.Invoke(this);

            prevPageButton.gameObject.SetActive(false);
            nextPageButton.gameObject.SetActive(false);
        }
        else if (isNotFirstSectorFirstPage)
        {
            // ACTIVATE Previous Sector and Next Page Button
            prevSectorButton.gameObject.SetActive(true);
            nextPageButton.gameObject.SetActive(true);

            PreviousSectorButtonActiveEvent?.Invoke(this);

            prevPageButton.gameObject.SetActive(false);
            nextSectorButton.gameObject.SetActive(false);
        }
        else if (isNotLastSectorLastPage)
        {
            // ACTIVATE Next Sector and Previous Page Button
            prevPageButton.gameObject.SetActive(true);
            nextSectorButton.gameObject.SetActive(true);

            NextSectorButtonActiveEvent?.Invoke(this);

            nextPageButton.gameObject.SetActive(false);
            prevSectorButton.gameObject.SetActive(false);
        }
        else if (isOnlySinglePageInLastSector)
        {
            // ACTIVATE ONLY Previous Sector Button
            prevSectorButton.gameObject.SetActive(true);

            PreviousSectorButtonActiveEvent?.Invoke(this);

            prevPageButton.gameObject.SetActive(false);
            nextPageButton.gameObject.SetActive(false);
            nextSectorButton.gameObject.SetActive(false);
        }
        else if (isLastSectorLastPage)
        {
            // DEACTIVATE Both Next Buttons
            nextPageButton.gameObject.SetActive(false);
            nextSectorButton.gameObject.SetActive(false);

            PreviousSectorButtonActiveEvent?.Invoke(this);
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
