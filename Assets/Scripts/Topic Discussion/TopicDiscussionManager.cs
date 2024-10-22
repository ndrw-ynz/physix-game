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
        discussionNavigator.LoadPage(_currentSectorIndex, _currentPageIndex);

        ChangePreviousAndNextButtonState();

        ChangeReadIndicatorButtonState();

        progressDisplay.LoadProgressBars(discussionNavigator);
        progressDisplay.UpdateProgressBar(_currentSectorIndex, discussionNavigator);
        progressDisplay.UpdateIndicatorLine(_currentSectorIndex);

        pageJumpDisplay.LoadPageJumpButtons(_currentSectorIndex, discussionNavigator);
        pageJumpDisplay.UpdatePageJumpButtonOutline(_currentPageIndex);
        pageJumpDisplay.UpdatePageJumpButtonColors(_currentSectorIndex, _currentPageIndex, discussionNavigator);

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

                progressDisplay.UpdateProgressBar(_currentSectorIndex, discussionNavigator);

                pageJumpDisplay.UpdatePageJumpButtonOutline(_currentPageIndex);
                pageJumpDisplay.UpdatePageJumpButtonColors(_currentSectorIndex, _currentPageIndex, discussionNavigator);
                break;

            case Direction.NextPage:
                _currentPageIndex += 1;

                discussionNavigator.ChangePage(_currentSectorIndex, _currentPageIndex);

                ChangePreviousAndNextButtonState();

                ChangeReadIndicatorButtonState();

                progressDisplay.UpdateProgressBar(_currentSectorIndex, discussionNavigator);
                
                pageJumpDisplay.UpdatePageJumpButtonOutline(_currentPageIndex);
                pageJumpDisplay.UpdatePageJumpButtonColors(_currentSectorIndex, _currentPageIndex, discussionNavigator);
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

                progressDisplay.UpdateProgressBar(_currentSectorIndex, discussionNavigator);
                progressDisplay.UpdateIndicatorLine(_currentSectorIndex);

                pageJumpDisplay.LoadPageJumpButtons(_currentSectorIndex, discussionNavigator);
                pageJumpDisplay.UpdatePageJumpButtonOutline(_currentPageIndex);
                pageJumpDisplay.UpdatePageJumpButtonColors(_currentSectorIndex, _currentPageIndex, discussionNavigator);
                break;
            case Direction.NextSector:
                discussionNavigator.CloseCurrentPage(_currentSectorIndex, _currentPageIndex);

                int nextSectorFirstPageIndex = 0;
                _currentSectorIndex += 1;
                _currentPageIndex = nextSectorFirstPageIndex;

                discussionNavigator.ChangePage(_currentSectorIndex, _currentPageIndex);

                ChangePreviousAndNextButtonState();

                ChangeReadIndicatorButtonState();

                progressDisplay.UpdateProgressBar(_currentSectorIndex, discussionNavigator);
                progressDisplay.UpdateIndicatorLine(_currentSectorIndex);

                pageJumpDisplay.LoadPageJumpButtons(_currentSectorIndex, discussionNavigator);
                pageJumpDisplay.UpdatePageJumpButtonOutline(_currentPageIndex);
                pageJumpDisplay.UpdatePageJumpButtonColors(_currentSectorIndex, _currentPageIndex, discussionNavigator);
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

            discussionNavigator.JumpToSector(_currentSectorIndex, _currentPageIndex);

            ChangePreviousAndNextButtonState();

            ChangeReadIndicatorButtonState();

            progressDisplay.UpdateProgressBar(_currentSectorIndex, discussionNavigator);
            progressDisplay.UpdateIndicatorLine(_currentSectorIndex);
            pageJumpDisplay.LoadPageJumpButtons(_currentSectorIndex, discussionNavigator);

            pageJumpDisplay.UpdatePageJumpButtonOutline(_currentPageIndex);
            pageJumpDisplay.UpdatePageJumpButtonColors(_currentSectorIndex, _currentPageIndex, discussionNavigator);
        }
    }

    private void HandlePageCircleClick(int pageIndex)
    {
        if (_currentPageIndex != pageIndex)
        {
            _currentPageIndex = pageIndex;
            discussionNavigator.JumpToPage(_currentSectorIndex, pageIndex);

            ChangePreviousAndNextButtonState();

            ChangeReadIndicatorButtonState();

            progressDisplay.UpdateProgressBar(_currentSectorIndex, discussionNavigator);

            pageJumpDisplay.UpdatePageJumpButtonOutline(_currentPageIndex);
            pageJumpDisplay.UpdatePageJumpButtonColors(_currentSectorIndex, _currentPageIndex, discussionNavigator);
        }
    }

    private void HandleReadIndicatorClick(ReadState readState)
    {
        discussionNavigator.ChangeReadState(readState, _currentSectorIndex, _currentPageIndex);

        ChangeReadIndicatorButtonState();

        progressDisplay.UpdateProgressBar(_currentSectorIndex, discussionNavigator);

        pageJumpDisplay.UpdatePageJumpButtonColors(_currentSectorIndex, _currentPageIndex, discussionNavigator);
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
}
