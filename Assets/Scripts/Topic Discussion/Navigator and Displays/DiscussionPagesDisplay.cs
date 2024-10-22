using System;
using System.Collections.Generic;
using UnityEngine;
public enum Direction
{
    PreviousPage,
    NextPage,
    PreviousSector,
    NextSector
}
public enum ReadState
{
    Read,
    NotRead
}
public class DiscussionPagesDisplay : MonoBehaviour
{
    [Header("Sub Topics Sectors")]
    public List<Sector> subTopicsList;

    // Page animation properties
    private Page _page;
    private float _pageAnimationDuration = 0.2f;
    private bool _animatePage = false;
    private float _pageAnimationStartTime;

    private void Update()
    {
        AnimatePage();
    }

    #region Sector and Page Navigation
    public void LoadPage(int currentSectorIndex, int currentPageIndex)
    {
        // Load Startup Page
        ShowPage(currentSectorIndex, currentPageIndex);
        ActivatePageAnimation(subTopicsList[currentSectorIndex].pages[currentPageIndex]);
    }
    public void ChangePage(int currentSectorIndex, int currentPageIndex)
    {
        ShowPage(currentSectorIndex, currentPageIndex);
        ActivatePageAnimation(subTopicsList[currentSectorIndex].pages[currentPageIndex]);
    }
    public void JumpToSector(int sectorIndex, int pageIndex)
    {
            ShowPage(sectorIndex, pageIndex);
            ActivatePageAnimation(subTopicsList[sectorIndex].pages[pageIndex]);
    }
    public void JumpToPage(int currentSectorIndex, int pageIndex)
    {
            // Set the page to be jumped to
            int jumpedPageIndex = pageIndex;

            ShowPage(currentSectorIndex, jumpedPageIndex);
            ActivatePageAnimation(subTopicsList[currentSectorIndex].pages[jumpedPageIndex]);
    }
    #endregion

    #region Private Classes Used For [Sector and Page Navigation]. Open/Close of Pages and Changing Read Indicator States
    public void CloseCurrentPage(int currentSectorIndex, int currentPageIndex)
    {
        // Closes the current page
        GameObject currentSectorAndPage = subTopicsList[currentSectorIndex].pages[currentPageIndex].page;
        currentSectorAndPage.SetActive(false);
    }
    private void ShowPage(int currentSector, int currentPage)
    {
        for (int i = 0; i < subTopicsList[currentSector].pages.Count; i++)
        {
            if (i == currentPage)
            {
                // Opens the current page
                subTopicsList[currentSector].pages[i].page.SetActive(true);
            }
            else
            {
                // Ensures other pages are close
                subTopicsList[currentSector].pages[i].page.SetActive(false);
            }
        }
    }
    public void ChangeReadState(ReadState readState, int currentSectorIndex, int currentPageIndex)
    {
        switch (readState)
        {
            case ReadState.Read:
                // Set page to read
                subTopicsList[currentSectorIndex].pages[currentPageIndex].isMarkedRead = true;
                break;

            case ReadState.NotRead:
                // Set page to not yet read
                subTopicsList[currentSectorIndex].pages[currentPageIndex].isMarkedRead = false;
                break;
        }
    }
    #endregion

    #region Page Animation
    private void ActivatePageAnimation(Page page)
    {
        _page = page;
        _animatePage = true;
        _pageAnimationStartTime = Time.time;
    }
    private void AnimatePage()
    {
        if (_animatePage)
        {
            float elapsedTime = Time.time - _pageAnimationStartTime;
            if (elapsedTime < _pageAnimationDuration)
            {
                float currentPageAlpha = Mathf.Lerp(0f, 1.0f, elapsedTime / _pageAnimationDuration);
                _page.canvasGroup.alpha = currentPageAlpha;
            }
            else
            {
                _animatePage = false;
                _page.canvasGroup.alpha = 1;
            }
        }
        
    }
    #endregion

    #region Counting Functions Used Outside Script
    public double CountReadPages(int sectorIndex)
    {
        // Count the read pages of a given sector
        double readPages = 0;
        for (int i = 0; i < subTopicsList[sectorIndex].pages.Count; i++)
        {
            bool isPageRead = subTopicsList[sectorIndex].pages[i].isMarkedRead;

            if (isPageRead)
            {
                readPages++;
            }
        }
        return readPages;
    }
    public double CountTotalPages(int sectorIndex)
    {
        // Count the total pages of a given sector
        return subTopicsList[sectorIndex].pages.Count;
    }
    #endregion

    #region Page's Read Checkers for Outside Script
    public bool CurrentPageIsMarkedRead(int currentSectorIndex, int currentPageIndex)
    {
        // Check if the current page is marked as read or not
        return subTopicsList[currentSectorIndex].pages[currentPageIndex].isMarkedRead;
    }
    public bool IsPageMarkedRead(int currentSectorIndex, int pageIndex)
    {
        // Check if the given page is marked as read or not
        return subTopicsList[currentSectorIndex].pages[pageIndex].isMarkedRead;
    }
    #endregion

    #region Discussion Display Getters For Outside Script
    public int GetCurrentSectorPagesCount(int currentSectorIndex)
    {
        // Get the current sector's page's total count
        return subTopicsList[currentSectorIndex].pages.Count;
    }
    public int GetSubTopicListCount()
    {
        // Get the current sub topic list's count
        return subTopicsList.Count;
    }
    public string GetSectorTitle(int sectorIndex)
    {
        // Get the title of a given sector
        return subTopicsList[sectorIndex].sectorTitle;
    }
    public string GetPreviousSectorTitle(int currentSectorIndex)
    {
        // Get the title of the previous sector
        return subTopicsList[currentSectorIndex - 1].sectorTitle;
    }
    public string GetNextSectorTitle(int currentSectorIndex)
    {
        // Get the title of the next sector
        return subTopicsList[currentSectorIndex + 1].sectorTitle;
    }
    #endregion
}
