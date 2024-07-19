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
        }

        public string sectorTitle;
        public List<Page> pages;
    }

    public List<Sector> subTopicsList;
    public UnderstoodNotUnderstoodButton markAsUnderstoodButton;
    public UnderstoodNotUnderstoodButton markAsNotYetUnderstoodButton;

    public static event Action<int, int, int, int> PageChangeEvent;
    public static event Action<DiscussionNavigator> DiscussionPageStart;
    private int _currentSectorIndex = 0;
    private int _currentPageIndex = 0;


    private void Start()
    {
            DiscussionPrevNextPageButton.PrevNextPageClickEvent += ChangePage;
            DiscussionPrevNextSectorButton.PrevNextSectorClickEvent += ChangeSector;
            ProgressBarButton.ProgressBarClickEvent += JumpToSector;
            UnderstoodNotUnderstoodButton.UnderstoodNotUnderstoodClickEvent += ChangeComprehensionMark;

            PrevNextButtonsManager.PreviousSectorButtonActiveEvent += OnPreviousSectorButtonActive;
            PrevNextButtonsManager.NextSectorButtonActiveEvent += OnNextSectorButtonActive;

            DiscussionPageStart?.Invoke(this);
    }

    public void ChangePage(int direction)
    {
        // Change pages
        if (_currentPageIndex < subTopicsList[_currentSectorIndex].pages.Count) // If the current page index is < current sector's indexed list count
        {
            _currentPageIndex += direction;
            ShowPage(_currentSectorIndex, _currentPageIndex);
            PageChangeEvent?.Invoke(_currentSectorIndex, _currentPageIndex, subTopicsList.Count, 
                subTopicsList[_currentSectorIndex].pages.Count);
            ChangeComprehensionButtonState();
        }
    }

    public void JumpToSector(int sectorIndex) 
    {
        CloseCurrentPage();

        _currentSectorIndex = sectorIndex;
        _currentPageIndex = 0;
        ShowPage(_currentSectorIndex, _currentPageIndex);
        PageChangeEvent?.Invoke(_currentSectorIndex, _currentPageIndex, subTopicsList.Count,
                subTopicsList[_currentSectorIndex].pages.Count);
        ChangeComprehensionButtonState();
    }

    public void ChangeSector(string action)
    {
        // Change sectors
        if (action == "next")
        {
            CloseCurrentPage();

            _currentSectorIndex++;
            int nextSectorFirstPageIndex = 0;
            _currentPageIndex = nextSectorFirstPageIndex;

            ShowPage(_currentSectorIndex,_currentPageIndex);
            ChangeComprehensionButtonState();

            PageChangeEvent?.Invoke(_currentSectorIndex, _currentPageIndex, subTopicsList.Count,
                subTopicsList[_currentSectorIndex].pages.Count);
        }
        if (action == "previous")
        {
            CloseCurrentPage();

            _currentSectorIndex--;
            int previousSectorLastPageIndex = subTopicsList[_currentSectorIndex].pages.Count - 1;
            _currentPageIndex = previousSectorLastPageIndex;

            ShowPage(_currentSectorIndex, _currentPageIndex);

            ChangeComprehensionButtonState();

            PageChangeEvent?.Invoke(_currentSectorIndex, _currentPageIndex, subTopicsList.Count,
                subTopicsList[_currentSectorIndex].pages.Count);
        }
    }

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

    public string GetSectorTitle(int sectorIndex) 
    { 
        return subTopicsList[sectorIndex].sectorTitle;
    }

    public int GetTotalSubTopicListCount()
    {
        return subTopicsList.Count;
    }

    public void ChangeComprehensionMark(bool flag)
    {
        subTopicsList[_currentSectorIndex].pages[_currentPageIndex].isMarkedUnderstood = flag;
        //ChangeComprehensionButtonState();
    }

    public void ChangeComprehensionButtonState()
    {
        if (!subTopicsList[_currentSectorIndex].pages[_currentPageIndex].isMarkedUnderstood)
        {
            markAsUnderstoodButton.gameObject.SetActive(true);
            markAsNotYetUnderstoodButton.gameObject.SetActive(false);
        }
        else
        {
            markAsUnderstoodButton.gameObject.SetActive(false);
            markAsNotYetUnderstoodButton.gameObject.SetActive(true);
        }
    }

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

    private void OnPreviousSectorButtonActive(PrevNextButtonsManager prevNextButtonsManager)
    {
        prevNextButtonsManager.SetPrevSectorText(subTopicsList[_currentSectorIndex - 1].sectorTitle);
    }
    
    private void OnNextSectorButtonActive(PrevNextButtonsManager prevNextButtonsManager)
    {
        prevNextButtonsManager.SetNextSectorText(subTopicsList[_currentSectorIndex + 1].sectorTitle);
    }
}
