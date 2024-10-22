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

        previousNextButtonsDisplay.ChangePrevNextButtonsState(_currentSectorIndex, _currentPageIndex, discussionNavigator);

        readIndicatorsDisplay.ChangeReadIndicatorButtonsState(_currentSectorIndex,_currentPageIndex, discussionNavigator);

        progressDisplay.LoadProgressBars(discussionNavigator);
        progressDisplay.UpdateProgressBar(_currentSectorIndex, discussionNavigator);
        progressDisplay.UpdateIndicatorLine(_currentSectorIndex);

        pageJumpDisplay.LoadPageJumpButtons(_currentSectorIndex, discussionNavigator);
        pageJumpDisplay.UpdatePageJumpButtonOutline(_currentPageIndex);
        pageJumpDisplay.UpdatePageJumpButtonColors(_currentSectorIndex,_currentPageIndex, discussionNavigator);

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

    private void Update()
    {
        //Debug.Log("Current Sector Index is: "+ _currentSectorIndex);
        //Debug.Log("Current Page Index is: "+ _currentPageIndex);
    }

    private void HandlePrevNextClick(Direction direction)
    {
        switch (direction) 
        {
            case Direction.PreviousPage:
                _currentPageIndex -= 1;
                discussionNavigator.ChangePage(_currentSectorIndex, _currentPageIndex);
                previousNextButtonsDisplay.ChangePrevNextButtonsState(_currentSectorIndex,_currentPageIndex,discussionNavigator);
                progressDisplay.UpdateProgressBar(_currentSectorIndex, discussionNavigator);
                readIndicatorsDisplay.ChangeReadIndicatorButtonsState(_currentSectorIndex, _currentPageIndex, discussionNavigator);
                pageJumpDisplay.UpdatePageJumpButtonOutline(_currentPageIndex);
                pageJumpDisplay.UpdatePageJumpButtonColors(_currentSectorIndex, _currentPageIndex, discussionNavigator);
                break;

            case Direction.NextPage:
                _currentPageIndex += 1;
                discussionNavigator.ChangePage(_currentSectorIndex, _currentPageIndex);
                previousNextButtonsDisplay.ChangePrevNextButtonsState(_currentSectorIndex, _currentPageIndex, discussionNavigator);
                progressDisplay.UpdateProgressBar(_currentSectorIndex, discussionNavigator);
                readIndicatorsDisplay.ChangeReadIndicatorButtonsState(_currentSectorIndex, _currentPageIndex, discussionNavigator);
                pageJumpDisplay.UpdatePageJumpButtonOutline(_currentPageIndex);
                pageJumpDisplay.UpdatePageJumpButtonColors(_currentSectorIndex, _currentPageIndex, discussionNavigator);
                break;
            case Direction.PreviousSector:
                discussionNavigator.CloseCurrentPage(_currentSectorIndex, _currentPageIndex);
                _currentSectorIndex -= 1;
                int previousSectorLastPageIndex = discussionNavigator.GetSubTopicListCount() - 1;
                _currentPageIndex = previousSectorLastPageIndex;
                discussionNavigator.ChangePage(_currentSectorIndex, _currentPageIndex);
                previousNextButtonsDisplay.ChangePrevNextButtonsState(_currentSectorIndex, _currentPageIndex, discussionNavigator);
                progressDisplay.UpdateProgressBar(_currentSectorIndex, discussionNavigator);
                progressDisplay.UpdateIndicatorLine(_currentSectorIndex);
                readIndicatorsDisplay.ChangeReadIndicatorButtonsState(_currentSectorIndex, _currentPageIndex, discussionNavigator);

                pageJumpDisplay.LoadPageJumpButtons(_currentSectorIndex, discussionNavigator);
                pageJumpDisplay.UpdatePageJumpButtonOutline(_currentPageIndex);
                pageJumpDisplay.UpdatePageJumpButtonColors(_currentSectorIndex, _currentPageIndex, discussionNavigator);
                break;
            case Direction.NextSector:
                discussionNavigator.CloseCurrentPage(_currentSectorIndex, _currentPageIndex);
                _currentSectorIndex += 1;
                int nextSectorFirstPageIndex = 0;
                _currentPageIndex = nextSectorFirstPageIndex;
                discussionNavigator.ChangePage(_currentSectorIndex, _currentPageIndex);
                previousNextButtonsDisplay.ChangePrevNextButtonsState(_currentSectorIndex, _currentPageIndex, discussionNavigator);
                progressDisplay.UpdateProgressBar(_currentSectorIndex, discussionNavigator);
                progressDisplay.UpdateIndicatorLine(_currentSectorIndex);
                readIndicatorsDisplay.ChangeReadIndicatorButtonsState(_currentSectorIndex, _currentPageIndex, discussionNavigator);

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

            previousNextButtonsDisplay.ChangePrevNextButtonsState(_currentSectorIndex, _currentPageIndex, discussionNavigator);

            readIndicatorsDisplay.ChangeReadIndicatorButtonsState(_currentSectorIndex, _currentPageIndex, discussionNavigator);

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
            Debug.Log("Current Page in Topic Discussion: " + _currentPageIndex);
            Debug.Log("Page to be jumped: " + pageIndex);

            _currentPageIndex = pageIndex;
            discussionNavigator.JumpToPage(_currentSectorIndex, pageIndex);

            previousNextButtonsDisplay.ChangePrevNextButtonsState(_currentSectorIndex, _currentPageIndex, discussionNavigator);
            readIndicatorsDisplay.ChangeReadIndicatorButtonsState(_currentSectorIndex, _currentPageIndex, discussionNavigator);

            progressDisplay.UpdateProgressBar(_currentSectorIndex, discussionNavigator);

            pageJumpDisplay.UpdatePageJumpButtonOutline(_currentPageIndex);
            pageJumpDisplay.UpdatePageJumpButtonColors(_currentSectorIndex, _currentPageIndex, discussionNavigator);
        }
    }

    private void HandleReadIndicatorClick(ReadState readState)
    {
        discussionNavigator.ChangeReadState(readState, _currentSectorIndex, _currentPageIndex);
        progressDisplay.UpdateProgressBar(_currentSectorIndex, discussionNavigator);
        pageJumpDisplay.UpdatePageJumpButtonColors(_currentSectorIndex, _currentPageIndex, discussionNavigator);
        readIndicatorsDisplay.ChangeReadIndicatorButtonsState(_currentSectorIndex, _currentPageIndex, discussionNavigator);
    }
}
