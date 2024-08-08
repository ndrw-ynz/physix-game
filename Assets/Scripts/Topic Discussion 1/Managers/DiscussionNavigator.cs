using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class DiscussionNavigator : MonoBehaviour
{
    [System.Serializable]
    public class Sector
    {
        [System.Serializable]
        public class Page
        {
            public GameObject page;
            public bool isMarkedUnderstood;
            public CanvasGroup canvasGroup;
        }

        public string sectorTitle;
        public List<Page> pages;
    }

    public List<Sector> subTopicsList;
    public static event Action<DiscussionNavigator> PageChangeEvent;
    public static event Action<DiscussionNavigator> SectorChangeEvent;
    public static event Action<DiscussionNavigator> UnderstandMarkerChangeEvent;
    public static event Action<DiscussionNavigator> DiscussionPageStart;

    private int _currentSectorIndex = 0;
    private int _currentPageIndex = 0;

    private void Start()
    {
        PagePrevNextButton.PrevNextPageClickEvent += ChangePage;
        SectorPrevNextButton.PrevNextSectorClickEvent += ChangeSector;
        ProgressBarButton.ProgressBarClickEvent += JumpToSector;
        PageJumpButton.OnPageCircleClick += JumpToPage;
        UnderstoodIndicatorButton.UnderstoodIndicatorClickEvent += ChangeComprehensionMark;

        DiscussionPageStart?.Invoke(this);
    }
    private void OnDisable()
    {
        PagePrevNextButton.PrevNextPageClickEvent -= ChangePage;
        SectorPrevNextButton.PrevNextSectorClickEvent -= ChangeSector;
        ProgressBarButton.ProgressBarClickEvent -= JumpToSector;
        PageJumpButton.OnPageCircleClick -= JumpToPage;
        UnderstoodIndicatorButton.UnderstoodIndicatorClickEvent -= ChangeComprehensionMark;
    }

    // Changing of sector and pages related functions
    public void ChangePage(int direction)
    {
        if (_currentPageIndex < subTopicsList[_currentSectorIndex].pages.Count)
        {
            _currentPageIndex += direction;

            ShowPage(_currentSectorIndex, _currentPageIndex);

            PageChangeEvent?.Invoke(this);
            UnderstandMarkerChangeEvent?.Invoke(this);
        }
    }

    public void JumpToSector(int sectorIndex) 
    {
        if(_currentSectorIndex != sectorIndex)
        {
            CloseCurrentPage();

            _currentSectorIndex = sectorIndex;
            _currentPageIndex = 0;

            ShowPage(_currentSectorIndex, _currentPageIndex);

            PageChangeEvent?.Invoke(this);
            SectorChangeEvent?.Invoke(this);
            UnderstandMarkerChangeEvent?.Invoke(this);
        }
    }

    public void JumpToPage(int pageIndex)
    {
        if (_currentPageIndex != pageIndex)
        {
            _currentPageIndex = pageIndex;
            ShowPage(_currentSectorIndex, _currentPageIndex);

            PageChangeEvent?.Invoke(this);
            UnderstandMarkerChangeEvent?.Invoke(this);
        }
    }

    public void ChangeSector(string action)
    {
        if (action == "next")
        {
            CloseCurrentPage();

            _currentSectorIndex++;
            int nextSectorFirstPageIndex = 0;
            _currentPageIndex = nextSectorFirstPageIndex;

            ShowPage(_currentSectorIndex,_currentPageIndex);

            PageChangeEvent?.Invoke(this);
            SectorChangeEvent?.Invoke(this);
            UnderstandMarkerChangeEvent?.Invoke(this);
        }
        if (action == "previous")
        {
            CloseCurrentPage();

            _currentSectorIndex--;
            int previousSectorLastPageIndex = GetCurrentSectorPagesCount() - 1;
            _currentPageIndex = previousSectorLastPageIndex;

            ShowPage(_currentSectorIndex, _currentPageIndex);

            PageChangeEvent?.Invoke(this);
            SectorChangeEvent?.Invoke(this);
            UnderstandMarkerChangeEvent?.Invoke(this);
        }
    }

    // Counting related functions
    public double CountUnderstoodPages(int sectorIndex)
    {
        double understoodPages = 0;
        for (int i = 0; i < subTopicsList[sectorIndex].pages.Count; i++)
        {
            bool isPageUnderstood = subTopicsList[sectorIndex].pages[i].isMarkedUnderstood;

            if (isPageUnderstood)
            {
                understoodPages++;
            }
        }
        return understoodPages;
    }

    public double CountTotalPages(int sectorIndex)
    {
        return subTopicsList[sectorIndex].pages.Count;
    }

    // Getting DiscussionNavigator's private variables related functions
    public int GetCurrentSectorIndex()
    {
        return _currentSectorIndex;
    }
    public int GetCurrentPageIndex()
    {
        return _currentPageIndex;
    }
    public int GetCurrentSectorPagesCount()
    {
        return subTopicsList[_currentSectorIndex].pages.Count;
    }
    public int GetSubTopicListCount()
    {
        return subTopicsList.Count;
    }
    public string GetSectorTitle(int sectorIndex)
    {
        return subTopicsList[sectorIndex].sectorTitle;
    }
    public string GetPreviousSectorTitle()
    {
        return subTopicsList[_currentSectorIndex - 1].sectorTitle;
    }
    public string GetNextSectorTitle()
    {
        return subTopicsList[_currentSectorIndex + 1].sectorTitle;
    }

    // Checking for condition related functions
    public bool CurrentPageIsMarkedUnderstood()
    {
        return subTopicsList[_currentSectorIndex].pages[_currentPageIndex].isMarkedUnderstood;
    }

    public bool IsPageMarkedUnderstood(int pageIndex)
    {
        return subTopicsList[_currentSectorIndex].pages[pageIndex].isMarkedUnderstood;
    }

    // Private functions, used only inside this class
    private void CloseCurrentPage()
    {
        GameObject currentSectorAndPage = subTopicsList[_currentSectorIndex].pages[_currentPageIndex].page;
        currentSectorAndPage.SetActive(false);
    }
    private void ShowPage(int currentSector, int currentPage)
    {
        for (int i = 0; i < subTopicsList[_currentSectorIndex].pages.Count; i++)
        {
            if (i == currentPage)
            {
                subTopicsList[currentSector].pages[i].page.SetActive(true);
            }
            else
            {
                subTopicsList[currentSector].pages[i].page.SetActive(false);
            }
        }
    }
    private void ChangeComprehensionMark(bool flag)
    {
        subTopicsList[_currentSectorIndex].pages[_currentPageIndex].isMarkedUnderstood = flag;
        UnderstandMarkerChangeEvent?.Invoke(this);
    }

}
