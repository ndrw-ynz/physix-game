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
        public string sectorTitle;
        public List<GameObject> pages;
    }

    public List<Sector> subTopicsList;
    public DiscussionPrevNextPageButton prevPageButton;
    public DiscussionPrevNextPageButton nextPageButton;
    public DiscussionPrevNextSectorButton prevSectorButton;
    public DiscussionPrevNextSectorButton nextSectorButton;

    private int _currentSectorIndex = 0;
    private int _currentPageIndex = 0;

    public void changePage(int direction)
    {
        // Change pages
        if (_currentPageIndex < subTopicsList[_currentSectorIndex].pages.Count) // If the current page index is < current sector's indexed list count
        {
            _currentPageIndex += direction;
            showPage(_currentPageIndex);
            changeButtonState();
            //Debug.Log($"Current Sector: {_currentSectorIndex}");
            //Debug.Log($"Current Sector: {_currentSectorIndex--}");
            Debug.Log($"Is Prev Sector Active: {prevSectorButton.gameObject.activeSelf}");
            Debug.Log($"Is Next Sector Active: {nextSectorButton.gameObject.activeSelf}");
            int currentSector = _currentSectorIndex;
            Debug.Log($"Current Sector: {currentSector}");
            Debug.Log($"Next Sector: {currentSector+1}");

            //Debug.Log($"Next Sector Title: {subTopicsList[_currentSectorIndex++].sectorTitle}");


        }
    }

    public void changeSector(string action)
    {
        // Change sectors
        if (action == "next")
        {
            GameObject currentSectorLastPage = subTopicsList[_currentSectorIndex].pages[_currentPageIndex];
            int nextSectorFirstPageIndex = 0;

            currentSectorLastPage.SetActive(false);
            _currentSectorIndex++;
            _currentPageIndex = nextSectorFirstPageIndex;
            showPage(_currentPageIndex);
            changeButtonState();
            int currentSector = _currentSectorIndex;
            Debug.Log($"Current Sector: {currentSector++}");
            

        }
        if (action == "previous")
        {
            GameObject currentSectorFirstPage = subTopicsList[_currentSectorIndex].pages[_currentPageIndex];
            
            currentSectorFirstPage.SetActive(false);
            _currentSectorIndex--;

            int previousSectorLastPageIndex = subTopicsList[_currentSectorIndex].pages.Count - 1;
            _currentPageIndex = previousSectorLastPageIndex;
            showPage(_currentPageIndex);
            changeButtonState();
            int currentSector = _currentSectorIndex;
            Debug.Log($"Current Sector: {currentSector++}");


        }
    }

    private void showPage(int currentPage)
    {
        for (int i = 0; i < subTopicsList[_currentSectorIndex].pages.Count; i++)
        {
            if (i == currentPage)
            {
                subTopicsList[_currentSectorIndex].pages[i].SetActive(true);
            }
            else
            {
                subTopicsList[_currentSectorIndex].pages[i].SetActive(false);
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
