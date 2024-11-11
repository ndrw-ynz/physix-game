using System.Collections.Generic;
using UnityEngine;

public class TutorialOverlayDisplay : MonoBehaviour
{
    [Header("Tutorial Sectors List")]
    [SerializeField] private List<TutorialSector> tutorialSectors;

    [Header("Tutorial Navigation Buttons")]
    [SerializeField] private TutorialNavigationButton previousTutorialPageButton;
    [SerializeField] private TutorialNavigationButton nextTutorialPageButton;

    // Sector and page index values
    private int _currentSectorIndex;
    private int _currentPageIndex;

    private void OnEnable()
    {
        // On start, change the page according to the current sector and page indexes assigned
        ChangeTutorialSector(_currentSectorIndex);

        // Subscribe listeners
        TutorialNavigationButton.TutorialNavigationButtonClick += NavigatePage;
        TutorialSectorJumpButton.SectorJumpButtonClick += ChangeTutorialSector;
    }
    private void OnDisable()
    {
        // Unsubscribe listeners
        TutorialNavigationButton.TutorialNavigationButtonClick -= NavigatePage;
        TutorialSectorJumpButton.SectorJumpButtonClick -= ChangeTutorialSector;
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
            }
            else
            {
                // Ensure that all other sector's game object are closed
                tutorialSectors[i].gameObject.SetActive(false);
            }
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
}
