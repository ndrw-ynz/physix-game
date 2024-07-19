using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrevNextButtonsManager : MonoBehaviour
{
    public DiscussionPrevNextPageButton prevPageButton;
    public DiscussionPrevNextPageButton nextPageButton;
    public DiscussionPrevNextSectorButton prevSectorButton;
    public DiscussionPrevNextSectorButton nextSectorButton;

    private void OnEnable()
    {
        DiscussionNavigator.PageChangeEvent += ChangeButtonState;
    }

    private void ChangeButtonState(DiscussionNavigator discNav)
    {
        bool isOnlySinglePageInFirstSector = discNav.GetCurrentSectorIndex() == 0 && discNav.GetCurrentSectorPagesCount() == 1;
        bool isFirstSectorFirstPage = discNav.GetCurrentSectorIndex() == 0 && discNav.GetCurrentPageIndex() == 0;
        bool isOnlySinglePageInSector = discNav.GetCurrentSectorIndex() < discNav.GetSubTopicListCount() - 1 && discNav.GetCurrentSectorPagesCount() == 1;
        bool isOnlySinglePageInLastSector = discNav.GetCurrentSectorIndex() == discNav.GetSubTopicListCount() - 1 && discNav.GetCurrentSectorPagesCount() == 1;
        bool isLastSectorLastPage = discNav.GetCurrentSectorIndex() == discNav.GetSubTopicListCount() - 1 && discNav.GetCurrentPageIndex() == discNav.GetCurrentSectorPagesCount() - 1;
        bool isNotFirstSectorFirstPage = discNav.GetCurrentSectorIndex() > 0 && discNav.GetCurrentPageIndex() == 0;
        bool isNotLastSectorLastPage = discNav.GetCurrentSectorIndex() < discNav.GetSubTopicListCount() - 1 && discNav.GetCurrentPageIndex() == discNav.GetCurrentSectorPagesCount() - 1;

        //Change button states.
        if (isOnlySinglePageInFirstSector)
        {
            // ACTIVATE ONLY Next Sector Button
            nextSectorButton.gameObject.SetActive(true);

            SetNextSectorText(discNav.GetNextSectorTitle());

            prevSectorButton.gameObject.SetActive(false);
            prevPageButton.gameObject.SetActive(false);
            nextPageButton.gameObject.SetActive(false);
        }
        else if (isFirstSectorFirstPage)
        {
            // DEACTIVATE Both Previous Buttons
            prevPageButton.gameObject.SetActive(false);
            prevSectorButton.gameObject.SetActive(false);

            SetNextSectorText(discNav.GetNextSectorTitle());
        }
        else if (isOnlySinglePageInSector)
        {
            // ACTIVATE Both Sector Buttons
            prevSectorButton.gameObject.SetActive(true);
            nextSectorButton.gameObject.SetActive(true);

            SetPrevSectorText(discNav.GetPreviousSectorTitle());
            SetNextSectorText(discNav.GetNextSectorTitle());

            prevPageButton.gameObject.SetActive(false);
            nextPageButton.gameObject.SetActive(false);
        }
        else if (isNotFirstSectorFirstPage)
        {
            // ACTIVATE Previous Sector and Next Page Button
            prevSectorButton.gameObject.SetActive(true);
            nextPageButton.gameObject.SetActive(true);

            SetPrevSectorText(discNav.GetPreviousSectorTitle());

            prevPageButton.gameObject.SetActive(false);
            nextSectorButton.gameObject.SetActive(false);
        }
        else if (isNotLastSectorLastPage)
        {
            // ACTIVATE Next Sector and Previous Page Button
            prevPageButton.gameObject.SetActive(true);
            nextSectorButton.gameObject.SetActive(true);

            SetNextSectorText(discNav.GetNextSectorTitle());

            nextPageButton.gameObject.SetActive(false);
            prevSectorButton.gameObject.SetActive(false);
        }
        else if (isOnlySinglePageInLastSector)
        {
            // ACTIVATE ONLY Previous Sector Button
            prevSectorButton.gameObject.SetActive(true);

            SetPrevSectorText(discNav.GetPreviousSectorTitle());

            prevPageButton.gameObject.SetActive(false);
            nextPageButton.gameObject.SetActive(false);
            nextSectorButton.gameObject.SetActive(false);
        }
        else if (isLastSectorLastPage)
        {
            // DEACTIVATE Both Next Buttons
            nextPageButton.gameObject.SetActive(false);
            nextSectorButton.gameObject.SetActive(false);

            SetPrevSectorText(discNav.GetPreviousSectorTitle());
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

    private void SetPrevSectorText(string previousSectorTitle)
    {
        prevSectorButton.sectorButtonText.text = previousSectorTitle;
    }

    private void SetNextSectorText(string nextSectorTitle)
    {
        nextSectorButton.sectorButtonText.text = nextSectorTitle;
    }
}
