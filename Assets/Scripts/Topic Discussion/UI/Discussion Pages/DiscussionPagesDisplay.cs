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

    #region Sector and Page Changing and Read State Changing Functions
    public void ChangePage(int sectorIndex, int pageIndex)
    {
        // Shows the page of the desired sector and page index
        ShowPage(sectorIndex, pageIndex);
        // Animates the page with a fade in style animation into the discussion page display screen
        ActivatePageAnimation(subTopicsList[sectorIndex].pages[pageIndex]);
    }
    public void CloseCurrentPage(int currentSectorIndex, int currentPageIndex)
    {
        // Closes the current page of the current sector and page index
        GameObject currentSectorAndPage = subTopicsList[currentSectorIndex].pages[currentPageIndex].page;
        currentSectorAndPage.SetActive(false);
    }
    public void ChangeReadState(ReadState readState, int currentSectorIndex, int currentPageIndex)
    {
        switch (readState)
        {
            case ReadState.Read:
                // Set page to have been read
                subTopicsList[currentSectorIndex].pages[currentPageIndex].isMarkedRead = true;
                break;

            case ReadState.NotRead:
                // Set page to not yet read
                subTopicsList[currentSectorIndex].pages[currentPageIndex].isMarkedRead = false;
                break;
        }
    }
    private void ShowPage(int currentSector, int currentPage)
    {
        // Loop through the whole current sector's pages
        for (int i = 0; i < subTopicsList[currentSector].pages.Count; i++)
        {
            if (i == currentPage)
            {
                // Opens the current page if i is the current page
                subTopicsList[currentSector].pages[i].page.SetActive(true);
            }
            else
            {
                // Ensures other pages are close
                subTopicsList[currentSector].pages[i].page.SetActive(false);
            }
        }
    }
    #endregion

    #region Page Animation
    private void ActivatePageAnimation(Page page)
    {
        // Sets the right page, activate the page animation, and records the start time of the page animation
        _page = page;
        _animatePage = true;
        _pageAnimationStartTime = Time.time;
    }
    private void AnimatePage()
    {
        // Animates the desired page when the page animation is activated
        if (_animatePage)
        {
            // Records the elapsed time of the animation
            float elapsedTime = Time.time - _pageAnimationStartTime;

            if (elapsedTime < _pageAnimationDuration)
            {
                // If the elapsed time is less than the set page animation duration, keep animating the fade in effect
                float currentPageAlpha = Mathf.Lerp(0f, 1.0f, elapsedTime / _pageAnimationDuration);
                _page.canvasGroup.alpha = currentPageAlpha;
            }
            else
            {
                // If the elapsed time has reached the set page animation duration, ensure page alpha is 1 and stop animation
                _page.canvasGroup.alpha = 1;
                _animatePage = false;
            }
        }
        
    }
    #endregion

    #region Counting Functions Used Outside Script
    public double CountReadPages(int sectorIndex)
    {
        // Count the read pages of a given sector
        double readPages = 0;

        // Loop through the whole current sector's pages
        for (int i = 0; i < subTopicsList[sectorIndex].pages.Count; i++)
        {
            // Check if the page is marked as read or not
            bool isPageRead = subTopicsList[sectorIndex].pages[i].isMarkedRead;

            if (isPageRead)
            {
                // Increment the readPages count if a page is marked as read
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

    #region Page's Read Checkers Used Outside Script
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

    #region Discussion Navigator Getters Used Outside The Script
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
