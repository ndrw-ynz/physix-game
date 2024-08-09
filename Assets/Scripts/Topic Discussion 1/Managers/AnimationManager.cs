using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private float _pageAnimationSpeed = 4.0f;
    private float _targetPageAlpha = 1.0f;
    private float _currentPageAlpha;
    private CanvasGroup _page;
    private bool _animatePage = false;

    private void OnEnable()
    {
        DiscussionNavigator.PageChangeEvent += ActivatePageAnimation;
    }

    private void OnDisable()
    {
        DiscussionNavigator.PageChangeEvent -= ActivatePageAnimation;
    }

    private void Update()
    {
        AnimatePage();
    }



    private void ActivatePageAnimation(DiscussionNavigator manager)
    {
        _currentPageAlpha = 0f;
        _page = manager.subTopicsList[manager.GetCurrentSectorIndex()].pages[manager.GetCurrentPageIndex()].canvasGroup;
        _animatePage = true;
    }

    private void AnimatePage()
    {
        if (_animatePage)
        {
            if (_currentPageAlpha < _targetPageAlpha)
            {
                _currentPageAlpha += Time.deltaTime * _pageAnimationSpeed;
                _page.alpha = _currentPageAlpha;
            }
            else
            {
                _animatePage = false;
            }
        }
    }
}
