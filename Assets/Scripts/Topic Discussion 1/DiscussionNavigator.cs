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

    public static event Action<int, int> PageChangeEvent;
    private int _currentSectorIndex = 0;
    private int _currentPageIndex = 0;


    private void Start()
    {
            DiscussionPrevNextPageButton.PrevNextPageClickEvent += ChangePage;
            DiscussionPrevNextSectorButton.PrevNextSectorClickEvent += ChangeSector;
            ProgressBarButton.ProgressBarClickEvent += JumpToSector;
            UnderstoodNotUnderstoodButton.UnderstoodNotUnderstoodClickEvent += ChangeComprehensionMark;
    }

    public void ChangePage(int direction)
    {
        // Change pages
        if (_currentPageIndex < subTopicsList[_currentSectorIndex].pages.Count) // If the current page index is < current sector's indexed list count
        {
            _currentPageIndex += direction;
            ShowPage(_currentSectorIndex, _currentPageIndex);
            PageChangeEvent?.Invoke(_currentSectorIndex, _currentPageIndex);
            ChangeComprehensionButtonState();
        }
    }

    public void JumpToSector(int sectorIndex) 
    {
        GameObject currentSectorAndPage = subTopicsList[_currentSectorIndex].pages[_currentPageIndex].page;
        currentSectorAndPage.SetActive(false);

        _currentSectorIndex = sectorIndex;
        _currentPageIndex = 0;
        ShowPage(_currentSectorIndex, _currentPageIndex);
        PageChangeEvent?.Invoke(_currentSectorIndex, _currentPageIndex);
        ChangeComprehensionButtonState();
    }

    public void ChangeSector(string action)
    {
        // Change sectors
        if (action == "next")
        {
            GameObject currentSectorLastPage = subTopicsList[_currentSectorIndex].pages[_currentPageIndex].page;
            int nextSectorFirstPageIndex = 0;

            currentSectorLastPage.SetActive(false);
            _currentSectorIndex++;
            _currentPageIndex = nextSectorFirstPageIndex;
            ShowPage(_currentSectorIndex,_currentPageIndex);
            PageChangeEvent?.Invoke(_currentSectorIndex, _currentPageIndex);
            ChangeComprehensionButtonState();
            int currentSector = _currentSectorIndex;
            Debug.Log($"Current Sector: {currentSector++}");
            

        }
        if (action == "previous")
        {
            GameObject currentSectorFirstPage = subTopicsList[_currentSectorIndex].pages[_currentPageIndex].page;
            
            currentSectorFirstPage.SetActive(false);
            _currentSectorIndex--;

            int previousSectorLastPageIndex = subTopicsList[_currentSectorIndex].pages.Count - 1;
            _currentPageIndex = previousSectorLastPageIndex;
            ShowPage(_currentSectorIndex, _currentPageIndex);
            PageChangeEvent?.Invoke(_currentSectorIndex, _currentPageIndex);
            ChangeComprehensionButtonState();
            int currentSector = _currentSectorIndex;
            Debug.Log($"Current Sector: {currentSector++}");
        }
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

    //public string SetPrevSectorText() // To Do: Put change sector text to changeButtonStates
    //{
    //    bool prevSectorButtonIsActive = prevSectorButton.gameObject.activeSelf;
    //
    //    if (prevSectorButtonIsActive)
    //    {
    //        int currentSector = _currentSectorIndex;
    //        string prevSectorText = subTopicsList[currentSector-1].sectorTitle;
    //        return prevSectorText;
    //    }
    //    else
    //    {
    //        return "";
    //    }
    //}
    //
    //public string SetNextSectorText() // To Do: Put change sector text to changeButtonStates
    //{
    //    bool nextSectorButtonIsActive = nextSectorButton.gameObject.activeSelf;
    //
    //    if (nextSectorButtonIsActive)
    //    {
    //        int currentSector = _currentSectorIndex;
    //        string nextSectorText = subTopicsList[currentSector+1].sectorTitle;
    //        return nextSectorText;
    //    }
    //    else
    //    {
    //        return "";
    //    }
    //}
}
