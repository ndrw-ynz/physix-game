using UnityEngine;

public class TopicDiscussionManager : MonoBehaviour
{
    [SerializeField] private DiscussionNavigator discussionNavigator;
    [SerializeField] private ProgressDisplay progressDisplay;

    private int _currentSectorIndex = 0;
    private int _currentPageIndex = 0;

    private void Start()
    {
        // Add button click listeners
        discussionNavigator.LoadPage(_currentSectorIndex, _currentPageIndex);
        progressDisplay.LoadProgressBars(discussionNavigator);
        progressDisplay.UpdateIndicatorLine(_currentSectorIndex);

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
                break;

            case Direction.NextPage:
                _currentPageIndex += 1;
                discussionNavigator.ChangePage(_currentSectorIndex, _currentPageIndex);
                break;
            case Direction.PreviousSector:
                discussionNavigator.CloseCurrentPage(_currentSectorIndex, _currentPageIndex);
                _currentSectorIndex -= 1;
                int previousSectorLastPageIndex = discussionNavigator.GetSubTopicListCount() - 1;
                _currentPageIndex = previousSectorLastPageIndex;
                discussionNavigator.ChangePage(_currentSectorIndex, _currentPageIndex);
                break;
            case Direction.NextSector:
                discussionNavigator.CloseCurrentPage(_currentSectorIndex, _currentPageIndex);
                _currentSectorIndex += 1;
                int nextSectorFirstPageIndex = 0;
                _currentPageIndex = nextSectorFirstPageIndex;
                discussionNavigator.ChangePage(_currentSectorIndex, _currentPageIndex);
                break;
        }
    }

    private void HandleProgressBarClick(int sectorIndex)
    {
        discussionNavigator.JumpToSector(sectorIndex);
        progressDisplay.UpdateIndicatorLine(_currentSectorIndex);
    }

    private void HandlePageCircleClick(int pageIndex)
    {
        discussionNavigator.JumpToPage(_currentSectorIndex, pageIndex);
    }

    private void HandleReadIndicatorClick(ReadState readState)
    {
        discussionNavigator.ChangeReadState(readState, _currentSectorIndex, _currentPageIndex);
    }
}
