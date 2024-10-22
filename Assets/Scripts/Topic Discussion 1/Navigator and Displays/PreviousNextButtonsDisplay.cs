using UnityEngine;

public class PreviousNextButtonsDisplay : MonoBehaviour
{
    [Header("Previous and Next Buttons")]
    [SerializeField] private PagePrevNextButton prevPageButton;
    [SerializeField] private PagePrevNextButton nextPageButton;
    [SerializeField] private SectorPrevNextButton prevSectorButton;
    [SerializeField] private SectorPrevNextButton nextSectorButton;

    private void OnEnable()
    {
        // Add listeners
        //DiscussionNavigator.PageChangeEvent += ChangePrevNextButtonsState;
        //DiscussionNavigator.ReadMarkerChangeEvent += ChangeReadIndicatorButtonsState;
    }
    private void OnDisable()
    {
        // Remove listeners
        //DiscussionNavigator.PageChangeEvent -= ChangePrevNextButtonsState;
        //DiscussionNavigator.ReadMarkerChangeEvent -= ChangeReadIndicatorButtonsState;
    }

    #region Previous and Next Buttons
    public void ChangePrevNextButtonsState(int currentSectorIndex, int currentPageIndex, DiscussionNavigator discNav)
    {
        // All cases of page indexes
        // There's a lot considering there's also previous and next sector button activate cases
        bool isOnlySinglePageInFirstSector = currentSectorIndex == 0 && discNav.GetCurrentSectorPagesCount(currentSectorIndex) == 1;
        bool isFirstSectorFirstPage = currentSectorIndex == 0 && currentPageIndex == 0;
        bool isOnlySinglePageInSector = currentSectorIndex < discNav.GetSubTopicListCount() - 1 && discNav.GetCurrentSectorPagesCount(currentSectorIndex) == 1;
        bool isOnlySinglePageInLastSector = currentSectorIndex == discNav.GetSubTopicListCount() - 1 && discNav.GetCurrentSectorPagesCount(currentSectorIndex) == 1;
        bool isLastSectorLastPage = currentSectorIndex == discNav.GetSubTopicListCount() - 1 && currentPageIndex == discNav.GetCurrentSectorPagesCount(currentSectorIndex) - 1;
        bool isNotFirstSectorFirstPage = currentSectorIndex > 0 && currentPageIndex == 0;
        bool isNotLastSectorLastPage = currentSectorIndex < discNav.GetSubTopicListCount() - 1 && currentPageIndex == discNav.GetCurrentSectorPagesCount(currentSectorIndex) - 1;

        // Change button states depending on the current sector and page index from DiscussionNavigator.cs
        if (isOnlySinglePageInFirstSector)
        {
            // Activate only the next sector button and attach next subtopic title to the button
            nextSectorButton.gameObject.SetActive(true);

            SetNextSectorText(discNav.GetNextSectorTitle(currentSectorIndex));

            prevSectorButton.gameObject.SetActive(false);
            prevPageButton.gameObject.SetActive(false);
            nextPageButton.gameObject.SetActive(false);
        }
        else if (isFirstSectorFirstPage)
        {
            // Activate only the next page button
            nextPageButton.gameObject.SetActive(true);

            prevPageButton.gameObject.SetActive(false);
            prevSectorButton.gameObject.SetActive(false);
            nextSectorButton.gameObject.SetActive(false);
        }
        else if (isOnlySinglePageInSector)
        {
            // Activate both sector buttons and attach previous and next subtopics to each corresponding button
            prevSectorButton.gameObject.SetActive(true);
            nextSectorButton.gameObject.SetActive(true);

            SetPrevSectorText(discNav.GetPreviousSectorTitle(currentSectorIndex));
            SetNextSectorText(discNav.GetNextSectorTitle(currentSectorIndex));

            prevPageButton.gameObject.SetActive(false);
            nextPageButton.gameObject.SetActive(false);
        }
        else if (isOnlySinglePageInLastSector)
        {
            // Activate only previous sector button and attach previous subtopic title to the button
            prevSectorButton.gameObject.SetActive(true);

            SetPrevSectorText(discNav.GetPreviousSectorTitle(currentSectorIndex));

            prevPageButton.gameObject.SetActive(false);
            nextPageButton.gameObject.SetActive(false);
            nextSectorButton.gameObject.SetActive(false);
        }
        else if (isNotFirstSectorFirstPage)
        {
            // Activate previous sector and next page button and attach previous subtopic title to the button
            prevSectorButton.gameObject.SetActive(true);
            nextPageButton.gameObject.SetActive(true);

            SetPrevSectorText(discNav.GetPreviousSectorTitle(currentSectorIndex));

            prevPageButton.gameObject.SetActive(false);
            nextSectorButton.gameObject.SetActive(false);
        }
        else if (isNotLastSectorLastPage)
        {
            // Activate next sector and previous page button and attach next subtopic title to the button
            prevPageButton.gameObject.SetActive(true);
            nextSectorButton.gameObject.SetActive(true);

            SetNextSectorText(discNav.GetNextSectorTitle(currentSectorIndex));

            nextPageButton.gameObject.SetActive(false);
            prevSectorButton.gameObject.SetActive(false);
        }
        else if (isLastSectorLastPage)
        {
            // Activate only previous page button
            prevPageButton.gameObject.SetActive(true);

            prevSectorButton.gameObject.SetActive(false);
            nextPageButton.gameObject.SetActive(false);
            nextSectorButton.gameObject.SetActive(false);
        }
        else
        {
            // Activate both page buttons
            prevPageButton.gameObject.SetActive(true);
            nextPageButton.gameObject.SetActive(true);

            nextSectorButton.gameObject.SetActive(false);
            prevSectorButton.gameObject.SetActive(false);
        }
    }

    private void SetPrevSectorText(string previousSectorTitle)
    {
        // Sets the previous sector's button title into the previous sector's title
        prevSectorButton.sectorButtonText.text = previousSectorTitle;
    }

    private void SetNextSectorText(string nextSectorTitle)
    {
        // Sets the previous sector's button title into the next sector's title
        nextSectorButton.sectorButtonText.text = nextSectorTitle;
    }
    #endregion
}
