using UnityEngine;

public class ButtonsDisplay : MonoBehaviour
{
    [Header("Previous and Next Buttons")]
    [SerializeField] private PagePrevNextButton prevPageButton;
    [SerializeField] private PagePrevNextButton nextPageButton;
    [SerializeField] private SectorPrevNextButton prevSectorButton;
    [SerializeField] private SectorPrevNextButton nextSectorButton;
    [Header("Read Indicator Buttons")]
    [SerializeField] private ReadIndicatorButton markAsReadButton;
    [SerializeField] private ReadIndicatorButton markAsNotReadButton;

    private void OnEnable()
    {
        // Add listeners
        DiscussionNavigator.PageChangeEvent += ChangePrevNextButtonsState;
        DiscussionNavigator.ReadMarkerChangeEvent += ChangeReadIndicatorButtonsState;
    }
    private void OnDisable()
    {
        // Remove listeners
        DiscussionNavigator.PageChangeEvent -= ChangePrevNextButtonsState;
        DiscussionNavigator.ReadMarkerChangeEvent -= ChangeReadIndicatorButtonsState;
    }

    #region Previous and Next Buttons
    private void ChangePrevNextButtonsState(DiscussionNavigator discNav)
    {
        // All cases of page indexes
        // There's a lot considering there's also previous and next sector button activate cases
        bool isOnlySinglePageInFirstSector = discNav.GetCurrentSectorIndex() == 0 && discNav.GetCurrentSectorPagesCount() == 1;
        bool isFirstSectorFirstPage = discNav.GetCurrentSectorIndex() == 0 && discNav.GetCurrentPageIndex() == 0;
        bool isOnlySinglePageInSector = discNav.GetCurrentSectorIndex() < discNav.GetSubTopicListCount() - 1 && discNav.GetCurrentSectorPagesCount() == 1;
        bool isOnlySinglePageInLastSector = discNav.GetCurrentSectorIndex() == discNav.GetSubTopicListCount() - 1 && discNav.GetCurrentSectorPagesCount() == 1;
        bool isLastSectorLastPage = discNav.GetCurrentSectorIndex() == discNav.GetSubTopicListCount() - 1 && discNav.GetCurrentPageIndex() == discNav.GetCurrentSectorPagesCount() - 1;
        bool isNotFirstSectorFirstPage = discNav.GetCurrentSectorIndex() > 0 && discNav.GetCurrentPageIndex() == 0;
        bool isNotLastSectorLastPage = discNav.GetCurrentSectorIndex() < discNav.GetSubTopicListCount() - 1 && discNav.GetCurrentPageIndex() == discNav.GetCurrentSectorPagesCount() - 1;

        // Change button states depending on the current sector and page index from DiscussionNavigator.cs
        if (isOnlySinglePageInFirstSector)
        {
            // Activate only the next sector button and attach next subtopic title to the button
            nextSectorButton.gameObject.SetActive(true);

            SetNextSectorText(discNav.GetNextSectorTitle());

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

            SetPrevSectorText(discNav.GetPreviousSectorTitle());
            SetNextSectorText(discNav.GetNextSectorTitle());

            prevPageButton.gameObject.SetActive(false);
            nextPageButton.gameObject.SetActive(false);
        }
        else if (isOnlySinglePageInLastSector)
        {
            // Activate only previous sector button and attach previous subtopic title to the button
            prevSectorButton.gameObject.SetActive(true);

            SetPrevSectorText(discNav.GetPreviousSectorTitle());

            prevPageButton.gameObject.SetActive(false);
            nextPageButton.gameObject.SetActive(false);
            nextSectorButton.gameObject.SetActive(false);
        }
        else if (isNotFirstSectorFirstPage)
        {
            // Activate previous sector and next page button and attach previous subtopic title to the button
            prevSectorButton.gameObject.SetActive(true);
            nextPageButton.gameObject.SetActive(true);

            SetPrevSectorText(discNav.GetPreviousSectorTitle());

            prevPageButton.gameObject.SetActive(false);
            nextSectorButton.gameObject.SetActive(false);
        }
        else if (isNotLastSectorLastPage)
        {
            // Activate next sector and previous page button and attach next subtopic title to the button
            prevPageButton.gameObject.SetActive(true);
            nextSectorButton.gameObject.SetActive(true);

            SetNextSectorText(discNav.GetNextSectorTitle());

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

    #region Read Indicator Buttons
    public void ChangeReadIndicatorButtonsState(DiscussionNavigator discNav)
    {
        if (!discNav.CurrentPageIsMarkedRead())
        {
            // Activate the mark as read button
            markAsReadButton.gameObject.SetActive(true);
            markAsNotReadButton.gameObject.SetActive(false);
        }
        else
        {
            // Activate the mark as not yet read button
            markAsReadButton.gameObject.SetActive(false);
            markAsNotReadButton.gameObject.SetActive(true);
        }
    }
    #endregion
}
