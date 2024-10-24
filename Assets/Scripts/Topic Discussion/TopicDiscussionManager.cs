using UnityEngine;

public class TopicDiscussionManager : MonoBehaviour
{
    [Header("Game Objects with Attached Display Scripts")]
    [SerializeField] private DiscussionPagesDisplay discussionNavigator;
    [SerializeField] private ProgressBarsDisplay progressDisplay;
    [SerializeField] private PageJumpButtonsDisplay pageJumpDisplay;
    [SerializeField] private PreviousNextButtonsDisplay previousNextButtonsDisplay;
    [SerializeField] private ReadIndicatorsDisplay readIndicatorsDisplay;

    // Current sector and page index values
    private static int _currentSectorIndex;
    private static int _currentPageIndex;

    private int _subTopicsListCount;

    private void Start()
    {
        /* Currently on the first page of the first sector. Can be modified when loading from the result screen's reccommended actions
         * Thou must be careful to load proper index values for different topic discussion scenes or else it will throw an "INDEX OUT OF BOUNDS" error
         * I might add a conditional statement for this that will load the first page instead if the error does happen */
        _currentSectorIndex = 0;
        _currentPageIndex = 0;

        // Get the sub topics' list count
        _subTopicsListCount = discussionNavigator.GetSubTopicListCount();

        // Load the current sector and page of the topic discussion
        discussionNavigator.ChangePage(_currentSectorIndex, _currentPageIndex);

        // Activate the proper previous and next button(s) based from the current sector and page of the topic discussion
        ChangePreviousAndNextButtonState();

        // Activate the proper read indicator button based on the current sector and page read state
        ChangeReadIndicatorButtonState();

        // Load all progress bar buttons' texts and colors to the left of the screen based on the amount of sectors of the topic discussion
        LoadProgressBarButtons();

        // Set the indicator's line position to the bottom of the current sector's progress bar button
        progressDisplay.UpdateIndicatorLinePosition(_currentSectorIndex);

        // Load all page jump buttons based on the current sector's page count
        LoadPageJumpButtons();
        // Set the page jump button outline to the current page's page jump button
        pageJumpDisplay.UpdatePageJumpButtonsOutline(_currentPageIndex);
        // Update the page jump button colors of the current sector based on their read states
        UpdatePageJumpButtonColors();

        // Add button click listeners
        PagePrevNextButton.PagePrevNextClickEvent += HandlePrevNextClick;
        SectorPrevNextButton.SectorPrevNextClickEvent += HandlePrevNextClick;
        ProgressBarButton.ProgressBarClickEvent += HandleProgressBarClick;
        PageJumpButton.PageCircleClick += HandlePageCircleClick;
        ReadIndicatorButton.ReadIndicatorClickEvent += HandleReadIndicatorClick;
    }

    private void OnDisable()
    {
        // Remove button click listeners
        PagePrevNextButton.PagePrevNextClickEvent -= HandlePrevNextClick;
        SectorPrevNextButton.SectorPrevNextClickEvent -= HandlePrevNextClick;
        ProgressBarButton.ProgressBarClickEvent -= HandleProgressBarClick;
        PageJumpButton.PageCircleClick -= HandlePageCircleClick;
        ReadIndicatorButton.ReadIndicatorClickEvent -= HandleReadIndicatorClick;
    }

    private void HandlePrevNextClick(Direction direction)
    {
        // Change current sector and page index values based on direction
        switch (direction) 
        {
            case Direction.PreviousPage:
                _currentPageIndex -= 1;

                discussionNavigator.ChangePage(_currentSectorIndex, _currentPageIndex);
                ChangePreviousAndNextButtonState();

                ChangeReadIndicatorButtonState();

                pageJumpDisplay.UpdatePageJumpButtonsOutline(_currentPageIndex);
                break;

            case Direction.NextPage:
                _currentPageIndex += 1;

                discussionNavigator.ChangePage(_currentSectorIndex, _currentPageIndex);

                ChangePreviousAndNextButtonState();

                ChangeReadIndicatorButtonState();

                pageJumpDisplay.UpdatePageJumpButtonsOutline(_currentPageIndex);
                break;

            case Direction.PreviousSector:
                discussionNavigator.CloseCurrentPage(_currentSectorIndex, _currentPageIndex);

                int previousSectorLastPageIndex = discussionNavigator.GetCurrentSectorPagesCount((_currentSectorIndex)-1) - 1;
                Debug.Log(previousSectorLastPageIndex);
                _currentSectorIndex -= 1;
                _currentPageIndex = previousSectorLastPageIndex;

                discussionNavigator.ChangePage(_currentSectorIndex, _currentPageIndex);

                ChangePreviousAndNextButtonState();

                ChangeReadIndicatorButtonState();

                progressDisplay.UpdateIndicatorLinePosition(_currentSectorIndex);

                LoadPageJumpButtons();
                pageJumpDisplay.UpdatePageJumpButtonsOutline(_currentPageIndex);
                UpdatePageJumpButtonColors();
                break;

            case Direction.NextSector:
                discussionNavigator.CloseCurrentPage(_currentSectorIndex, _currentPageIndex);

                int nextSectorFirstPageIndex = 0;
                _currentSectorIndex += 1;
                _currentPageIndex = nextSectorFirstPageIndex;

                discussionNavigator.ChangePage(_currentSectorIndex, _currentPageIndex);

                ChangePreviousAndNextButtonState();

                ChangeReadIndicatorButtonState();

                progressDisplay.UpdateIndicatorLinePosition(_currentSectorIndex);

                LoadPageJumpButtons();
                pageJumpDisplay.UpdatePageJumpButtonsOutline(_currentPageIndex);
                UpdatePageJumpButtonColors();
                break;
        }
    }
    private void HandleProgressBarClick(int sectorIndex)
    {
        // Close current page and jump to the specified sector index if not sectorIndex is not the current sector
        if (_currentSectorIndex != sectorIndex)
        {
            discussionNavigator.CloseCurrentPage(_currentSectorIndex,_currentPageIndex);

            _currentSectorIndex = sectorIndex;
            _currentPageIndex = 0;

            discussionNavigator.ChangePage(_currentSectorIndex, _currentPageIndex);

            ChangePreviousAndNextButtonState();

            ChangeReadIndicatorButtonState();

            progressDisplay.UpdateIndicatorLinePosition(_currentSectorIndex);
            LoadPageJumpButtons();

            pageJumpDisplay.UpdatePageJumpButtonsOutline(_currentPageIndex);
            UpdatePageJumpButtonColors();
        }
    }
    private void HandlePageCircleClick(int pageIndex)
    {
        // Jump to a specified page if not pageIndex is not the current page
        if (_currentPageIndex != pageIndex)
        {
            _currentPageIndex = pageIndex;
            discussionNavigator.ChangePage(_currentSectorIndex, pageIndex);

            ChangePreviousAndNextButtonState();

            ChangeReadIndicatorButtonState();

            pageJumpDisplay.UpdatePageJumpButtonsOutline(_currentPageIndex);
        }
    }
    private void HandleReadIndicatorClick(ReadState readState)
    {
        /* Change read state, read indicator button state. 
         * Then update progress bar text and color and page jump button colors */
        discussionNavigator.ChangeReadState(readState, _currentSectorIndex, _currentPageIndex);

        ChangeReadIndicatorButtonState();

        UpdateProgressBarButtonTextAndColor();

        UpdatePageJumpButtonColors();
    }

    private void ChangePreviousAndNextButtonState()
    {
        /* Set the current sector's pages count, previous sector title,next sector title, and update
           and update previous and next button state*/
        int currentSectorPagesCount = discussionNavigator.GetCurrentSectorPagesCount(_currentSectorIndex);
        string previousSectorTitle = _currentSectorIndex>0 ? discussionNavigator.GetPreviousSectorTitle(_currentSectorIndex): null;
        string nextSectorTitle = _currentSectorIndex< _subTopicsListCount - 1 ? discussionNavigator.GetNextSectorTitle(_currentSectorIndex): null;
        previousNextButtonsDisplay.ChangePrevNextButtonsState(_currentSectorIndex, _currentPageIndex, _subTopicsListCount, currentSectorPagesCount, previousSectorTitle, nextSectorTitle);
    }
    private void ChangeReadIndicatorButtonState()
    {
        // Check if page is marked as read and update read indicator state
        bool isPageMarkedRead = discussionNavigator.CurrentPageIsMarkedRead(_currentSectorIndex, _currentPageIndex);
        readIndicatorsDisplay.ChangeReadIndicatorButtonsState(_currentSectorIndex, _currentPageIndex, isPageMarkedRead);
    }
    private void LoadProgressBarButtons()
    {
        // Create progress bar buttons with corresponding titles and progress count
        for (int i = 0; i < _subTopicsListCount; i++)
        {
            string sectorTitle = discussionNavigator.GetSectorTitle(i);
            string progressCount = $"{discussionNavigator.CountReadPages(i).ToString()}/{discussionNavigator.CountTotalPages(i).ToString()}";
            progressDisplay.GenerateProgressBarButton(i, sectorTitle, progressCount);
        }

        // Load progress bar button colors
        int progressDisplayLength = progressDisplay.GetProgressBarButtonsLength();
        for (int i = 0; i < progressDisplayLength; i++)
        {
            double currentReadPagesCount = discussionNavigator.CountReadPages(_currentSectorIndex);
            double currentTotalPagesCount = discussionNavigator.CountTotalPages(_currentSectorIndex);
            progressDisplay.LoadProgressBarButtonsColors(currentReadPagesCount, currentTotalPagesCount, i);
        }
    }
    private void UpdateProgressBarButtonTextAndColor()
    {
        // Update the current sector's progress bar button color
        double currReadPagesCount = discussionNavigator.CountReadPages(_currentSectorIndex);
        double currSectorPagesCount = discussionNavigator.CountTotalPages(_currentSectorIndex);
        progressDisplay.UpdateProgressBarButtonTextAndColor(_currentSectorIndex, currReadPagesCount, currSectorPagesCount);
    }
    private void LoadPageJumpButtons()
    {
        // Load page jump buttons for the current sector
        int pageJumpButtonsLength = pageJumpDisplay.GetPageJumpButtonsLength();
        if (pageJumpButtonsLength > 0)
        {
            pageJumpDisplay.DestroyImmediateAllPageJumpButtons();
        }

        int currentSectorPagesCount = discussionNavigator.GetCurrentSectorPagesCount(_currentSectorIndex);
        // Create buttons for the new sector
        for (int i = 0; i < currentSectorPagesCount; i++)
        {
            pageJumpDisplay.GeneratePageJumpButton(i);
        }
    }
    private void UpdatePageJumpButtonColors()
    {
        // Update page jump button colors
        int pageJumpButtonsLength = pageJumpDisplay.GetPageJumpButtonsLength();
        for (int i = 0; i < pageJumpButtonsLength; i++)
        {
            bool isPageMarkedRead = discussionNavigator.IsPageMarkedRead(_currentSectorIndex, i);
            pageJumpDisplay.UpdatePageJumpButtonColor(_currentSectorIndex, isPageMarkedRead, i);
        }
    }
}
