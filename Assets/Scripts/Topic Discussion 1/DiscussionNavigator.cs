using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
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
    public TextMeshProUGUI prevSectorText;
    public TextMeshProUGUI nextSectorText;

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
        }
    }

    public void changeSector(string action)
    {
        // Change sectors
        if (action == "next")
        {
            subTopicsList[_currentSectorIndex].pages[_currentPageIndex].SetActive(false);
            _currentSectorIndex++;
            _currentPageIndex = 0;
            showPage(_currentPageIndex);
            changeButtonState();

        }
        if (action == "previous")
        {
            subTopicsList[_currentSectorIndex].pages[_currentPageIndex].SetActive(false);
            _currentSectorIndex--;
            _currentPageIndex = subTopicsList[_currentSectorIndex].pages.Count - 1;
            showPage(_currentPageIndex);
            changeButtonState();

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

    public void setSectorText(string sectorText)
    {

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
            prevSectorText.SetText("Previous Topic" + subTopicsList[_currentSectorIndex--].sectorTitle);
            nextSectorText.SetText("Next Topic" + subTopicsList[_currentSectorIndex++].sectorTitle);
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
