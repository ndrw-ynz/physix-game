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
    public DiscussionPrevNextPageButton prevPageButton;
    public DiscussionPrevNextPageButton nextPageButton;
    public DiscussionPrevNextSectorButton prevSectorButton;
    public DiscussionPrevNextSectorButton nextSectorButton;
    public ComprehensionButton markAsUnderstoodButton;
    public ComprehensionButton markAsNotYetUnderstoodButton;

    private int _currentSectorIndex = 0;
    private int _currentPageIndex = 0;

    public void changePage(int direction)
    {
        // Change pages
        if (_currentPageIndex < subTopicsList[_currentSectorIndex].pages.Count) // If the current page index is < current sector's indexed list count
        {
            _currentPageIndex += direction;
            showPage(_currentSectorIndex, _currentPageIndex);
            changeButtonState();
            changeComprehensionButtonState();
        }
    }

    public void JumpToSector(int sectorIndex) 
    {
        GameObject currentSectorAndPage = subTopicsList[_currentSectorIndex].pages[_currentPageIndex].page;
        currentSectorAndPage.SetActive(false);

        _currentSectorIndex = sectorIndex;
        _currentPageIndex = 0;
        showPage(_currentSectorIndex, _currentPageIndex);
        changeButtonState();
        changeComprehensionButtonState();
    }

    public void changeSector(string action)
    {
        // Change sectors
        if (action == "next")
        {
            GameObject currentSectorLastPage = subTopicsList[_currentSectorIndex].pages[_currentPageIndex].page;
            int nextSectorFirstPageIndex = 0;

            currentSectorLastPage.SetActive(false);
            _currentSectorIndex++;
            _currentPageIndex = nextSectorFirstPageIndex;
            showPage(_currentSectorIndex,_currentPageIndex);
            changeButtonState();
            changeComprehensionButtonState();
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
            showPage(_currentSectorIndex, _currentPageIndex);
            changeButtonState();
            changeComprehensionButtonState();
            int currentSector = _currentSectorIndex;
            Debug.Log($"Current Sector: {currentSector++}");
        }
    }

    public void changeComprehensionMark(bool flag)
    {
        subTopicsList[_currentSectorIndex].pages[_currentPageIndex].isMarkedUnderstood = flag;
        changeComprehensionButtonState();
    }

    public void changeComprehensionButtonState()
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

    private void showPage(int currentSector, int currentPage)
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

    public string setPrevSectorText() // To Do: Put change sector text to changeButtonStates
    {
        bool prevSectorButtonIsActive = prevSectorButton.gameObject.activeSelf;

        if (prevSectorButtonIsActive)
        {
            int currentSector = _currentSectorIndex;
            string prevSectorText = subTopicsList[currentSector-1].sectorTitle;
            return prevSectorText;
        }
        else
        {
            return "";
        }
    }

    public string setNextSectorText() // To Do: Put change sector text to changeButtonStates
    {
        bool nextSectorButtonIsActive = nextSectorButton.gameObject.activeSelf;

        if (nextSectorButtonIsActive)
        {
            int currentSector = _currentSectorIndex;
            string nextSectorText = subTopicsList[currentSector+1].sectorTitle;
            return nextSectorText;
        }
        else
        {
            return "";
        }
    }

    private void changeButtonState()
    {
        //Change button states.
        if (_currentSectorIndex == 0 && _currentPageIndex == 0)                             // First Sector, First Page
        {
            // Deactivate previous button functions
            prevPageButton.gameObject.SetActive(false);
            prevSectorButton.gameObject.SetActive(false);
        }
        else if (_currentSectorIndex < subTopicsList.Count - 1
                && subTopicsList[_currentSectorIndex].pages.Count == 1)                     //Only 1 page in a sector
        {
            // To Do: Make the Next and Previous Topic Title Automatically Adjustable
            // Sector - Previous and Sector - Next
            prevPageButton.gameObject.SetActive(false);
            nextPageButton.gameObject.SetActive(false);
            prevSectorButton.gameObject.SetActive(true);
            nextSectorButton.gameObject.SetActive(true);
        }
        else if (_currentSectorIndex == subTopicsList.Count - 1
                && subTopicsList[_currentSectorIndex].pages.Count == 1)                      //Only 1 page in the last sector
        {
            // Sector - Previous
            prevPageButton.gameObject.SetActive(false);
            nextPageButton.gameObject.SetActive(false);
            prevSectorButton.gameObject.SetActive(true);
            nextSectorButton.gameObject.SetActive(false);
        }
        else if (_currentSectorIndex == subTopicsList.Count - 1
                && _currentPageIndex == subTopicsList[_currentSectorIndex].pages.Count - 1) // Last Sector, Last Page
        {
            // Deactivate next button functions
            nextPageButton.gameObject.SetActive(false);
            nextSectorButton.gameObject.SetActive(false);
        }
        else if (_currentSectorIndex > 0 && _currentPageIndex == 0)                         // The first page of any Sector Except First 
        {
            // Page - Next and Sector - Previous
            prevPageButton.gameObject.SetActive(false);
            nextPageButton.gameObject.SetActive(true);
            prevSectorButton.gameObject.SetActive(true);
            nextSectorButton.gameObject.SetActive(false);
        }
        else if (_currentSectorIndex < subTopicsList.Count - 1
                && _currentPageIndex == subTopicsList[_currentSectorIndex].pages.Count - 1) // The last page of any sector except last
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
