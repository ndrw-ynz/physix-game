using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopicDiscussionManager : MonoBehaviour
{
    [Header("Game Objects with Attached Display Scripts")]
    [SerializeField] private DiscussionPagesDisplay discussionPagesDisplay;
    [SerializeField] private ProgressBarsDisplay progressDisplay;
    [SerializeField] private PageJumpButtonsDisplay pageJumpDisplay;
    [SerializeField] private PreviousNextButtonsDisplay previousNextButtonsDisplay;
    [SerializeField] private ReadIndicatorsDisplay readIndicatorsDisplay;
    [SerializeField] private SceneNavigationButtons sceneNavigationDisplay;

    [Header("Current Topic Discussion Number")]
    [SerializeField] private int _topicDiscussionNumber;

    // Current sector and page index values
    private static int _currentSectorIndex;
    private static int _currentPageIndex;

    // Current topic discussion's sub topics list count
    private int _subTopicsListCount;

    // Read pages map data to be loaded from cloud firestore database
    private Dictionary<string, List<int>> _readPagesMapData;
    private void Start()
    {
        // TO DO: add a function that checks if an activity scene is currently active
        bool isActivitySceneActive = false;

        // Activate only the back to game button when an activity scene is active
        if (isActivitySceneActive)
        {
            sceneNavigationDisplay.ActivateBackToGameButtonOnly();
        }


        /* Currently on the first page of the first sector. Can be modified when loading from the result screen's reccommended actions
         * Thou must be careful to load proper index values for different topic discussion scenes or else it will throw an "INDEX OUT OF BOUNDS" error
         * I might add a conditional statement for this that will load the first page instead if the error does happen */
        _currentSectorIndex = 0;
        _currentPageIndex = 0;

        // Get the sub topics' list count
        _subTopicsListCount = discussionPagesDisplay.GetSubTopicListCount();

        // Test data for topic discussion 9
        _readPagesMapData = new Dictionary<string, List<int>>()
        {
            { "sector1", new List<int> { 0, 1, 2, 3, 4} },
            { "sector2", new List<int> { 0 } },
        };

        // Load read pages data into the sub topics list of discussion pages display
        discussionPagesDisplay.LoadReadPagesData(_readPagesMapData);

        // Load the current sector and page of the topic discussion
        discussionPagesDisplay.ChangePage(_currentSectorIndex, _currentPageIndex);

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
        DiscussionMainMenuButton.BackToMainMenuClickEvent += HandleBackToMainMenuClick;
        DiscussionActivityButton.StartActivityClickEvent += HandleStartActivityClick;
        DiscussionBackToActivityButton.BackToActivityClickEvent += HandleBackToGameClick;
    }

    private void OnDisable()
    {
        // Remove button click listeners
        PagePrevNextButton.PagePrevNextClickEvent -= HandlePrevNextClick;
        SectorPrevNextButton.SectorPrevNextClickEvent -= HandlePrevNextClick;
        ProgressBarButton.ProgressBarClickEvent -= HandleProgressBarClick;
        PageJumpButton.PageCircleClick -= HandlePageCircleClick;
        ReadIndicatorButton.ReadIndicatorClickEvent -= HandleReadIndicatorClick;
        DiscussionMainMenuButton.BackToMainMenuClickEvent -= HandleBackToMainMenuClick;
        DiscussionActivityButton.StartActivityClickEvent -= HandleStartActivityClick;
        DiscussionBackToActivityButton.BackToActivityClickEvent -= HandleBackToGameClick;
    }

    private void HandlePrevNextClick(Direction direction)
    {
        // Change current sector and page index values based on direction
        switch (direction) 
        {
            case Direction.PreviousPage:
                _currentPageIndex -= 1;

                discussionPagesDisplay.ChangePage(_currentSectorIndex, _currentPageIndex);
                ChangePreviousAndNextButtonState();

                ChangeReadIndicatorButtonState();

                pageJumpDisplay.UpdatePageJumpButtonsOutline(_currentPageIndex);
                break;

            case Direction.NextPage:
                _currentPageIndex += 1;

                discussionPagesDisplay.ChangePage(_currentSectorIndex, _currentPageIndex);

                ChangePreviousAndNextButtonState();

                ChangeReadIndicatorButtonState();

                pageJumpDisplay.UpdatePageJumpButtonsOutline(_currentPageIndex);
                break;

            case Direction.PreviousSector:
                discussionPagesDisplay.CloseCurrentPage(_currentSectorIndex, _currentPageIndex);

                int previousSectorLastPageIndex = discussionPagesDisplay.GetCurrentSectorPagesCount((_currentSectorIndex)-1) - 1;
                Debug.Log(previousSectorLastPageIndex);
                _currentSectorIndex -= 1;
                _currentPageIndex = previousSectorLastPageIndex;

                discussionPagesDisplay.ChangePage(_currentSectorIndex, _currentPageIndex);

                ChangePreviousAndNextButtonState();

                ChangeReadIndicatorButtonState();

                progressDisplay.UpdateIndicatorLinePosition(_currentSectorIndex);

                LoadPageJumpButtons();
                pageJumpDisplay.UpdatePageJumpButtonsOutline(_currentPageIndex);
                UpdatePageJumpButtonColors();
                break;

            case Direction.NextSector:
                discussionPagesDisplay.CloseCurrentPage(_currentSectorIndex, _currentPageIndex);

                int nextSectorFirstPageIndex = 0;
                _currentSectorIndex += 1;
                _currentPageIndex = nextSectorFirstPageIndex;

                discussionPagesDisplay.ChangePage(_currentSectorIndex, _currentPageIndex);

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
            discussionPagesDisplay.CloseCurrentPage(_currentSectorIndex,_currentPageIndex);

            _currentSectorIndex = sectorIndex;
            _currentPageIndex = 0;

            discussionPagesDisplay.ChangePage(_currentSectorIndex, _currentPageIndex);

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
            discussionPagesDisplay.ChangePage(_currentSectorIndex, pageIndex);

            ChangePreviousAndNextButtonState();

            ChangeReadIndicatorButtonState();

            pageJumpDisplay.UpdatePageJumpButtonsOutline(_currentPageIndex);
        }
    }
    private void HandleReadIndicatorClick(ReadState readState)
    {
        /* Change read state, read indicator button state. 
         * Then update progress bar text and color and page jump button colors */
        discussionPagesDisplay.ChangeReadState(readState, _currentSectorIndex, _currentPageIndex);

        ChangeReadIndicatorButtonState();

        UpdateProgressBarButtonTextAndColor();

        UpdatePageJumpButtonColors();
    }

    private void HandleBackToMainMenuClick()
    {
        // Save read pages data and load main menu scene
        discussionPagesDisplay.SaveReadPagesData();
        sceneNavigationDisplay.LoadMainMenu();
    }

    private void HandleStartActivityClick()
    {
        // Save read pages data and load specified activity scene
        discussionPagesDisplay.SaveReadPagesData();
        sceneNavigationDisplay.LoadActivity(_topicDiscussionNumber);
    }

    private void HandleBackToGameClick()
    {
        // Save read pages data and close current discussion scene
        discussionPagesDisplay.SaveReadPagesData();
        sceneNavigationDisplay.CloseCurrentDiscussion(_topicDiscussionNumber);

    }

    private void ChangePreviousAndNextButtonState()
    {
        /* Set the current sector's pages count, previous sector title,next sector title, and update
           and update previous and next button state*/
        int currentSectorPagesCount = discussionPagesDisplay.GetCurrentSectorPagesCount(_currentSectorIndex);
        string previousSectorTitle = _currentSectorIndex>0 ? discussionPagesDisplay.GetPreviousSectorTitle(_currentSectorIndex): null;
        string nextSectorTitle = _currentSectorIndex< _subTopicsListCount - 1 ? discussionPagesDisplay.GetNextSectorTitle(_currentSectorIndex): null;
        previousNextButtonsDisplay.ChangePrevNextButtonsState(_currentSectorIndex, _currentPageIndex, _subTopicsListCount, currentSectorPagesCount, previousSectorTitle, nextSectorTitle);
    }
    private void ChangeReadIndicatorButtonState()
    {
        // Check if page is marked as read and update read indicator state
        bool isPageMarkedRead = discussionPagesDisplay.CurrentPageIsMarkedRead(_currentSectorIndex, _currentPageIndex);
        readIndicatorsDisplay.ChangeReadIndicatorButtonsState(_currentSectorIndex, _currentPageIndex, isPageMarkedRead);
    }
    private void LoadProgressBarButtons()
    {
        // Create progress bar buttons with corresponding titles and progress count
        for (int i = 0; i < _subTopicsListCount; i++)
        {
            string sectorTitle = discussionPagesDisplay.GetSectorTitle(i);
            string progressCount = $"{discussionPagesDisplay.CountReadPages(i).ToString()}/{discussionPagesDisplay.CountTotalPages(i).ToString()}";
            progressDisplay.GenerateProgressBarButton(i, sectorTitle, progressCount);
        }

        // Load progress bar button colors
        int progressDisplayLength = progressDisplay.GetProgressBarButtonsLength();
        for (int i = 0; i < progressDisplayLength; i++)
        {
            double currentReadPagesCount = discussionPagesDisplay.CountReadPages(i);
            double currentTotalPagesCount = discussionPagesDisplay.CountTotalPages(i);
            progressDisplay.LoadProgressBarButtonsColors(currentReadPagesCount, currentTotalPagesCount, i);
        }
    }
    private void UpdateProgressBarButtonTextAndColor()
    {
        // Update the current sector's progress bar button color
        double currReadPagesCount = discussionPagesDisplay.CountReadPages(_currentSectorIndex);
        double currSectorPagesCount = discussionPagesDisplay.CountTotalPages(_currentSectorIndex);
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

        int currentSectorPagesCount = discussionPagesDisplay.GetCurrentSectorPagesCount(_currentSectorIndex);
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
            bool isPageMarkedRead = discussionPagesDisplay.IsPageMarkedRead(_currentSectorIndex, i);
            pageJumpDisplay.UpdatePageJumpButtonColor(_currentSectorIndex, isPageMarkedRead, i);
        }
    }
}
