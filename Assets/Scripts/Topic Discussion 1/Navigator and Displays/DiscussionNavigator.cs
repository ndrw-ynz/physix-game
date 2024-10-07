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
public class DiscussionNavigator : MonoBehaviour
{
    public static event Action<DiscussionNavigator> PageChangeEvent;
    public static event Action<DiscussionNavigator> SectorChangeEvent;
    public static event Action<DiscussionNavigator> ReadMarkerChangeEvent;
    public static event Action<DiscussionNavigator> DiscussionPageStart;

    [Header("Sub Topics Sectors")]
    public List<Sector> subTopicsList;

    // Current sector and page indexes
    private int _currentSectorIndex = 0;
    private int _currentPageIndex = 0;

    // Page animation properties
    private Page _page;
    private float _pageAnimationDuration = 0.2f;
    private bool _animatePage = false;
    private float _pageAnimationStartTime;

    private void Start()
    {
        // Add button click listeners
        PagePrevNextButton.PagePrevNextClickEvent += ChangePage;
        SectorPrevNextButton.SectorPrevNextClickEvent += ChangePage;
        ProgressBarButton.ProgressBarClickEvent += JumpToSector;
        PageJumpButton.PageCircleClick += JumpToPage;
        ReadIndicatorButton.ReadIndicatorClickEvent += ChangeReadState;

        // Setup display scripts
        DiscussionPageStart?.Invoke(this);

        // Load the proper page when scene is loaded
        // Might be useful when rule based algorithm creates a suggestion to review a certain sector
        LoadPage();
    }
    private void OnDisable()
    {
        // Remove button click listeners
        PagePrevNextButton.PagePrevNextClickEvent -= ChangePage;
        SectorPrevNextButton.SectorPrevNextClickEvent -= ChangePage;
        ProgressBarButton.ProgressBarClickEvent -= JumpToSector;
        PageJumpButton.PageCircleClick -= JumpToPage;
        ReadIndicatorButton.ReadIndicatorClickEvent -= ChangeReadState;
    }

    private void Update()
    {
        AnimatePage();
    }

    #region Sector and Page Navigation
    private void LoadPage()
    {
        // Load Startup Page
        ShowPage(_currentSectorIndex, _currentPageIndex);
        ActivatePageAnimation(subTopicsList[_currentSectorIndex].pages[_currentPageIndex]);

        PageChangeEvent?.Invoke(this);
        SectorChangeEvent?.Invoke(this);
        ReadMarkerChangeEvent?.Invoke(this);
    }
    private void ChangePage(Direction direction)
    {
        // Change pages based on directions of previous page or sector and next page or sector
        switch (direction) 
        {
            case Direction.PreviousPage:
                // Change to previous page
                _currentPageIndex -= 1;
                ShowPage(_currentSectorIndex, _currentPageIndex);
                ActivatePageAnimation(subTopicsList[_currentSectorIndex].pages[_currentPageIndex]);

                PageChangeEvent?.Invoke(this);
                ReadMarkerChangeEvent?.Invoke(this);
                break;

            case Direction.NextPage:
                // Change to next page
                _currentPageIndex += 1;
                ShowPage(_currentSectorIndex, _currentPageIndex);
                ActivatePageAnimation(subTopicsList[_currentSectorIndex].pages[_currentPageIndex]);

                PageChangeEvent?.Invoke(this);
                ReadMarkerChangeEvent?.Invoke(this);
                break;

            case Direction.PreviousSector:
                // Change to previous sector
                CloseCurrentPage();

                _currentSectorIndex--;
                int previousSectorLastPageIndex = GetCurrentSectorPagesCount() - 1;
                _currentPageIndex = previousSectorLastPageIndex;

                ShowPage(_currentSectorIndex, _currentPageIndex);
                ActivatePageAnimation(subTopicsList[_currentSectorIndex].pages[_currentPageIndex]);

                PageChangeEvent?.Invoke(this);
                SectorChangeEvent?.Invoke(this);
                ReadMarkerChangeEvent?.Invoke(this);
                break;

            case Direction.NextSector:
                // Change to next sector
                CloseCurrentPage();

                _currentSectorIndex += 1;
                int nextSectorFirstPageIndex = 0;
                _currentPageIndex = nextSectorFirstPageIndex;

                ShowPage(_currentSectorIndex, _currentPageIndex);
                ActivatePageAnimation(subTopicsList[_currentSectorIndex].pages[_currentPageIndex]);

                PageChangeEvent?.Invoke(this);
                SectorChangeEvent?.Invoke(this);
                ReadMarkerChangeEvent?.Invoke(this);
                break;
        }
    }
    public void JumpToSector(int sectorIndex) 
    {
        // Jumps to a sector's first page if button is pressed and currently not on that same sector
        if(_currentSectorIndex != sectorIndex)
        {
            // Close the page first
            CloseCurrentPage();

            // Set the sector to be jumped to
            _currentSectorIndex = sectorIndex;
            _currentPageIndex = 0;

            ShowPage(_currentSectorIndex, _currentPageIndex);
            ActivatePageAnimation(subTopicsList[_currentSectorIndex].pages[_currentPageIndex]);

            PageChangeEvent?.Invoke(this);
            SectorChangeEvent?.Invoke(this);
            ReadMarkerChangeEvent?.Invoke(this);
        }
    }
    public void JumpToPage(int pageIndex)
    {
        // Jumps to a page of the current sector if button is pressed and currently not on that same page
        if (_currentPageIndex != pageIndex)
        {
            // Set the page to be jumped to
            _currentPageIndex = pageIndex;

            ShowPage(_currentSectorIndex, _currentPageIndex);
            ActivatePageAnimation(subTopicsList[_currentSectorIndex].pages[_currentPageIndex]);

            PageChangeEvent?.Invoke(this);
            ReadMarkerChangeEvent?.Invoke(this);
        }
    }
    #endregion

    #region Private Classes Used For [Sector and Page Navigation]. Open/Close of Pages and Changing Read Indicator States
    private void CloseCurrentPage()
    {
        // Closes the current page
        GameObject currentSectorAndPage = subTopicsList[_currentSectorIndex].pages[_currentPageIndex].page;
        currentSectorAndPage.SetActive(false);
    }
    private void ShowPage(int currentSector, int currentPage)
    {
        for (int i = 0; i < subTopicsList[_currentSectorIndex].pages.Count; i++)
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
    private void ChangeReadState(ReadState readState)
    {
        switch (readState)
        {
            case ReadState.Read:
                // Set page to read
                subTopicsList[_currentSectorIndex].pages[_currentPageIndex].isMarkedRead = true;
                ReadMarkerChangeEvent?.Invoke(this);
                break;

            case ReadState.NotRead:
                // Set page to not yet read
                subTopicsList[_currentSectorIndex].pages[_currentPageIndex].isMarkedRead = false;
                ReadMarkerChangeEvent?.Invoke(this);
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
    public bool CurrentPageIsMarkedRead()
    {
        // Check if the current page is marked as read or not
        return subTopicsList[_currentSectorIndex].pages[_currentPageIndex].isMarkedRead;
    }
    public bool IsPageMarkedRead(int pageIndex)
    {
        // Check if the given page is marked as read or not
        return subTopicsList[_currentSectorIndex].pages[pageIndex].isMarkedRead;
    }
    #endregion

    #region Discussion Navigator Getters For Outside Script
    public int GetCurrentSectorIndex()
    {
        // Get the current sector's index
        return _currentSectorIndex;
    }
    public int GetCurrentPageIndex()
    {
        // Get the current page's index
        return _currentPageIndex;
    }
    public int GetCurrentSectorPagesCount()
    {
        // Get the current sector's page's total count
        return subTopicsList[_currentSectorIndex].pages.Count;
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
    public string GetPreviousSectorTitle()
    {
        // Get the title of the previous sector
        return subTopicsList[_currentSectorIndex - 1].sectorTitle;
    }
    public string GetNextSectorTitle()
    {
        // Get the title of the next sector
        return subTopicsList[_currentSectorIndex + 1].sectorTitle;
    }
    #endregion
}
