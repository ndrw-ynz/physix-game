using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialOverlayDisplay : MonoBehaviour
{
    public static event Action TutorialOverlayClose;

    [Header("Tutorial Sectors List")]
    [SerializeField] private List<TutorialSector> tutorialSectors;

    [Header("Tutorial Navigation Buttons")]
    [SerializeField] private TutorialNavigationButton previousTutorialPageButton;
    [SerializeField] private TutorialNavigationButton nextTutorialPageButton;

    [Header("Tutorial Page Text Indicator")]
    [SerializeField] private TextMeshProUGUI pageNumberText;

    // Sector and page index values
    public int _currentSectorIndex;
    private int _currentPageIndex;

    private void OnEnable()
    {
        // On start, change the page according to the current sector and page indexes assigned
        SetupSpecificTutorialSector(_currentSectorIndex);
        UpdatePageNumberText();

        // Subscribe listeners
        TutorialNavigationButton.TutorialNavigationButtonClick += NavigatePage;
        TutorialSectorJumpButton.SectorJumpButtonClick += ChangeTutorialSector;
        TutorialCloseButton.CloseButtonClicked += CloseTutorialOverlay;
    }
    private void OnDisable()
    {
        // Reset index values
        _currentSectorIndex = 0;
        _currentPageIndex = 0;

        DeactivateAllPages();

        // Unsubscribe listeners
        TutorialNavigationButton.TutorialNavigationButtonClick -= NavigatePage;
        TutorialSectorJumpButton.SectorJumpButtonClick -= ChangeTutorialSector;
        TutorialCloseButton.CloseButtonClicked -= CloseTutorialOverlay;
    }

    private void NavigatePage(TutorialNavigation direction)
    {
        // Navigate either previous or next page
        switch (direction)
        {
            case TutorialNavigation.PreviousPage:
                _currentPageIndex--;
                break;

            case TutorialNavigation.NextPage:
                _currentPageIndex++;
                break;
        }

        // Change the page according to the current sector and page indexes
        ChangeTutorialPage(_currentSectorIndex, _currentPageIndex);
        UpdatePageNumberText();
        UpdateNavigationButtons();
    }

    private void ChangeTutorialPage(int currentSector, int currentPage)
    {
        // Loop through the whole current sector's pages
        for (int i = 0; i < tutorialSectors[currentSector].pages.Count; i++)
        {
            if (i == currentPage)
            {
                // Opens the current page if i is the current page
                tutorialSectors[currentSector].pages[i].gameObject.SetActive(true);
                UpdatePageNumberText();
            }
            else
            {
                // Ensures other pages are close
                tutorialSectors[currentSector].pages[i].gameObject.SetActive(false);
            }
        }
    }

    private void ChangeTutorialSector(int newSector)
    {
        if (_currentSectorIndex == newSector)
        {
            // Ignore code below if current sector is the sector passed as an argument
            return;
        }
        // Loop through the whole current sector
        for (int i = 0; i < tutorialSectors.Count; i++)
        {
            if (i == newSector)
            {
                // Opens the current sector and its first page if i is the current sector
                tutorialSectors[i].gameObject.SetActive(true);
                tutorialSectors[i].pages[0].gameObject.SetActive(true);

                // Reset the current page index to the first page index
                _currentSectorIndex = i;
                _currentPageIndex = 0;

                // Update navigation button states
                UpdateNavigationButtons();
                UpdatePageNumberText();
            }
            else
            {
                // Ensure that all other sector's game object are closed
                tutorialSectors[i].gameObject.SetActive(false);
                for (int j = 0; j < tutorialSectors[i].pages.Count; j++)
                {
                    tutorialSectors[i].pages[j].gameObject.SetActive(false);
                }
            }
        }
    }

    private void SetupSpecificTutorialSector(int sectorToSetup)
    {
        for (int i = 0; i < tutorialSectors.Count; i++)
        {
            if (i == sectorToSetup)
            {
                // Opens the current sector and its first page if i is the current sector
                tutorialSectors[i].gameObject.SetActive(true);
                tutorialSectors[i].pages[0].gameObject.SetActive(true);

                // Reset the current page index to the first page index
                _currentSectorIndex = i;
                _currentPageIndex = 0;

                // Update navigation button states
                UpdateNavigationButtons();
                UpdatePageNumberText();
            }
            else
            {
                // Ensure that all other sector's game object are closed
                tutorialSectors[i].gameObject.SetActive(false);
                for (int j = 0; j < tutorialSectors[i].pages.Count; j++)
                {
                    tutorialSectors[i].pages[j].gameObject.SetActive(false);
                }
            }
        }
    }

    private void CloseTutorialOverlay()
    {
        // Close tutorial overlay
        if (this.gameObject.activeSelf)
        {
            TutorialOverlayClose?.Invoke();

            this.gameObject.SetActive(false);
        }
    }

    private void UpdateNavigationButtons()
    {
        // Booleans for checking the current page index if first page or last page
        bool isFirstPage = _currentPageIndex == 0;
        bool isLastPage = _currentPageIndex == tutorialSectors[_currentSectorIndex].pages.Count - 1;

        // Change the active state of previous and next page based on the current page's index
        if (isFirstPage)
        {
            previousTutorialPageButton.gameObject.SetActive(false);
            nextTutorialPageButton.gameObject.gameObject.SetActive(true);
        }
        else if (isLastPage)
        {
            previousTutorialPageButton.gameObject.SetActive(true);
            nextTutorialPageButton.gameObject.gameObject.SetActive(false);
        }
        else
        {
            previousTutorialPageButton.gameObject.SetActive(true);
            nextTutorialPageButton.gameObject.gameObject.SetActive(true);
        }
    }

    private void UpdatePageNumberText()
    {
        // Get the current page number and sector's total pages
        int currentPageNumber = ((_currentPageIndex) +1);
        int currentSectorTotalPages = tutorialSectors[_currentSectorIndex].pages.Count;

        // Update the page number indicator text
        pageNumberText.text = $"Page {currentPageNumber}/{currentSectorTotalPages}";
    }

    private void DeactivateAllPages()
    {
        for (int i = 0; i < tutorialSectors.Count; i++)
        {
            for (int j = 0; j < tutorialSectors[i].pages.Count; j++)
            {
                tutorialSectors[i].pages[j].gameObject.SetActive(false);
            }
        }
    }
}
