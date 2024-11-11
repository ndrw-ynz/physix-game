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
        // Subscribe listeners
        TutorialNavigationButton.TutorialNavigationButtonClick += NavigatePage;
    }
    private void OnDisable()
    {
        // Unsubscribe listeners
        TutorialNavigationButton.TutorialNavigationButtonClick -= NavigatePage;
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
}
