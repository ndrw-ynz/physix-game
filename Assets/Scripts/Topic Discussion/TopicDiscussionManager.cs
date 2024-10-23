using UnityEngine;

public class TopicDiscussionManager : MonoBehaviour
{
    [SerializeField] private DiscussionPagesDisplay discussionNavigator;
    [SerializeField] private ProgressBarsDisplay progressDisplay;
    [SerializeField] private PageJumpButtonsDisplay pageJumpDisplay;
    [SerializeField] private PreviousNextButtonsDisplay previousNextButtonsDisplay;
    [SerializeField] private ReadIndicatorsDisplay readIndicatorsDisplay;

    private int _currentSectorIndex = 0;
    private int _currentPageIndex = 0;

    private void Start()
    {
        // Add button click listeners
        discussionNavigator.ChangePage(_currentSectorIndex, _currentPageIndex);

        ChangePreviousAndNextButtonState();

        ChangeReadIndicatorButtonState();

        LoadProgressBarButtons();
        UpdateProgressBarButtonTextAndColor();
        progressDisplay.UpdateIndicatorLinePosition(_currentSectorIndex);

        LoadPageJumpButtons();
        pageJumpDisplay.UpdatePageJumpButtonsOutline(_currentPageIndex);
        UpdatePageJumpButtonColors();

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
        switch (direction) 
        {
            case Direction.PreviousPage:
                _currentPageIndex -= 1;

                discussionNavigator.ChangePage(_currentSectorIndex, _currentPageIndex);
                ChangePreviousAndNextButtonState();

                ChangeReadIndicatorButtonState();

                UpdateProgressBarButtonTextAndColor();

                pageJumpDisplay.UpdatePageJumpButtonsOutline(_currentPageIndex);
                UpdatePageJumpButtonColors();
                break;

            case Direction.NextPage:
                _currentPageIndex += 1;

                discussionNavigator.ChangePage(_currentSectorIndex, _currentPageIndex);

                ChangePreviousAndNextButtonState();

                ChangeReadIndicatorButtonState();

                UpdateProgressBarButtonTextAndColor();

                pageJumpDisplay.UpdatePageJumpButtonsOutline(_currentPageIndex);
                UpdatePageJumpButtonColors();
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

                UpdateProgressBarButtonTextAndColor();
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

                UpdateProgressBarButtonTextAndColor();
                progressDisplay.UpdateIndicatorLinePosition(_currentSectorIndex);

                LoadPageJumpButtons();
                pageJumpDisplay.UpdatePageJumpButtonsOutline(_currentPageIndex);
                UpdatePageJumpButtonColors();
                break;
        }
    }

    private void HandleProgressBarClick(int sectorIndex)
    {
        if (_currentSectorIndex != sectorIndex)
        {
            discussionNavigator.CloseCurrentPage(_currentSectorIndex,_currentPageIndex);

            _currentSectorIndex = sectorIndex;
            _currentPageIndex = 0;

            discussionNavigator.ChangePage(_currentSectorIndex, _currentPageIndex);

            ChangePreviousAndNextButtonState();

            ChangeReadIndicatorButtonState();

            UpdateProgressBarButtonTextAndColor();
            progressDisplay.UpdateIndicatorLinePosition(_currentSectorIndex);
            LoadPageJumpButtons();

            pageJumpDisplay.UpdatePageJumpButtonsOutline(_currentPageIndex);
            UpdatePageJumpButtonColors();
        }
    }

    private void HandlePageCircleClick(int pageIndex)
    {
        if (_currentPageIndex != pageIndex)
        {
            _currentPageIndex = pageIndex;
            discussionNavigator.ChangePage(_currentSectorIndex, pageIndex);

            ChangePreviousAndNextButtonState();

            ChangeReadIndicatorButtonState();

            UpdateProgressBarButtonTextAndColor();

            pageJumpDisplay.UpdatePageJumpButtonsOutline(_currentPageIndex);
            UpdatePageJumpButtonColors();
        }
    }

    private void HandleReadIndicatorClick(ReadState readState)
    {
        discussionNavigator.ChangeReadState(readState, _currentSectorIndex, _currentPageIndex);

        ChangeReadIndicatorButtonState();

        UpdateProgressBarButtonTextAndColor();

        UpdatePageJumpButtonColors();
    }

    private void ChangePreviousAndNextButtonState()
    {
        int subTopicsListCount = discussionNavigator.GetSubTopicListCount();
        int currentSectorPagesCount = discussionNavigator.GetCurrentSectorPagesCount(_currentSectorIndex);
        string previousSectorTitle = _currentSectorIndex>0 ? discussionNavigator.GetPreviousSectorTitle(_currentSectorIndex): null;
        string nextSectorTitle = _currentSectorIndex<discussionNavigator.GetSubTopicListCount()-1 ? discussionNavigator.GetNextSectorTitle(_currentSectorIndex): null;
        previousNextButtonsDisplay.ChangePrevNextButtonsState(_currentSectorIndex, _currentPageIndex, subTopicsListCount, currentSectorPagesCount, previousSectorTitle, nextSectorTitle);
    }

    private void ChangeReadIndicatorButtonState()
    {
        bool isPageMarkedRead = discussionNavigator.CurrentPageIsMarkedRead(_currentSectorIndex, _currentPageIndex);
        readIndicatorsDisplay.ChangeReadIndicatorButtonsState(_currentSectorIndex, _currentPageIndex, isPageMarkedRead);
    }

    private void LoadProgressBarButtons()
    {
        // Get the amount of buttons to be created
        int subtopicsCount = discussionNavigator.GetSubTopicListCount();

        // Create buttons with corresponding titles and progress count
        for (int i = 0; i < subtopicsCount; i++)
        {
            string sectorTitle = discussionNavigator.GetSectorTitle(i);
            string progressCount = $"{discussionNavigator.CountReadPages(i).ToString()}/{discussionNavigator.CountTotalPages(i).ToString()}";
            progressDisplay.GenerateProgressBarButton(i, sectorTitle, progressCount);
        }

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
        double currReadPagesCount = discussionNavigator.CountReadPages(_currentSectorIndex);
        double currSectorPagesCount = discussionNavigator.CountTotalPages(_currentSectorIndex);
        progressDisplay.UpdateProgressBarButtonTextAndColor(_currentSectorIndex, currReadPagesCount, currSectorPagesCount);
    }

    private void LoadPageJumpButtons()
    {
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
        int pageJumpButtonsLength = pageJumpDisplay.GetPageJumpButtonsLength();
        for (int i = 0; i < pageJumpButtonsLength; i++)
        {
            bool isPageMarkedRead = discussionNavigator.IsPageMarkedRead(_currentSectorIndex, i);
            pageJumpDisplay.UpdatePageJumpButtonColor(_currentSectorIndex, isPageMarkedRead, i);
        }
    }
}
