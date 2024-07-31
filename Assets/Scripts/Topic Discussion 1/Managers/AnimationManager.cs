using UnityEngine;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour
{

    private float _indicatorAnimationSpeed = 500f;
    private float _targetWidth = 130f;
    private float _currentWidth;
    private float _currentHeight;
    private SectorIndicator _indicatorRect;
    private bool _animateIndicator = false;

    private float _cirleAnimationSpeed = 3.0f;
    private float _targetCircleAlpha = 1.0f;
    private float _currentCircleAlpha;
    private Color _currentCircleColor;
    private PageCircleButton _circleButton;
    private bool _animatePageCircle = false;

    private float _prevNextButtonAnimationSpeed = 4.0f;
    private float _targetPrevNextButtonAlpha = 1.0f;
    private float _currentPrevNextButtonAlpha;
    private CanvasGroup[] _buttonList;
    private bool _animateButton = false;

    private float _understandMarkerAnimationSpeed = 4.0f;
    private float _targetUnderstandMarkerAlpha = 1.0f;
    private float _currentUnderstandMarkerAlpha;
    private CanvasGroup _understandMarker;
    private bool _animateUnderstandMarker = false;

    private float _progressBarButtonAnimationSpeed = 5.0f;
    private float _targetProgressBarButtonAlpha = 1.0f;
    private float _currentProgressBarButtonAlpha;
    private Image _tempColor;
    private Image _finalColor;
    private Color _oldColor;
    private Color _newColor;
    private bool _animateProgressBar = false;

    private float _pageAnimationSpeed = 4.0f;
    private float _targetPageAlpha = 1.0f;
    private float _currentPageAlpha;
    private CanvasGroup _page;
    private bool _animatePage = false;

    private void OnEnable()
    {
        PageCircleButtonsManager.PageCircleStateUpdate += ActivatePageCircleAnimation;
        ProgressManager.IndicatorRectStateUpdate += ActivateProgressBarButtonAnimation;
        PrevNextButtonsManager.ButtonChangeStateUpdate += ActivatePrevNextButtonAnimation;
        UnderstoodIndicatorsManager.ComprehensionButtonStateChange += ActivateUnderstandMarkerAnimation;
        ProgressManager.ProgressBarButtonStateUpdate += ActivateProgressBarButtonAnimation;
        DiscussionNavigator.PageChangeEvent += ActivatePageAnimation;
    }

    private void OnDisable()
    {
        PageCircleButtonsManager.PageCircleStateUpdate -= ActivatePageCircleAnimation;
        ProgressManager.IndicatorRectStateUpdate -= ActivateProgressBarButtonAnimation;
        PrevNextButtonsManager.ButtonChangeStateUpdate -= ActivatePrevNextButtonAnimation;
        UnderstoodIndicatorsManager.ComprehensionButtonStateChange -= ActivateUnderstandMarkerAnimation;
        ProgressManager.ProgressBarButtonStateUpdate -= ActivateProgressBarButtonAnimation;
        DiscussionNavigator.PageChangeEvent -= ActivatePageAnimation;
    }

    private void Update()
    {
        AnimatePageCircle();
        AnimateIndicatorRect();
        //AnimatePrevNextButton();
        //AnimateUnderstandMarker();
        AnimateProgressBar();
        AnimatePage();
    }

    private void ActivateProgressBarButtonAnimation(ProgressManager manager, int i)
    {
        _currentWidth = 0;
        _currentHeight = manager.sectorIndicatorRectList[i].indicatorRectTransform.rect.height;
        _indicatorRect = manager.sectorIndicatorRectList[i];
        _animateIndicator = true;
    }

    private void ActivatePageCircleAnimation(PageCircleButtonsManager manager, int i)
    {
        _currentCircleAlpha = 0f;
        _currentCircleColor = manager.pageCircleButtonList[i].buttonOutline.color;
        _circleButton = manager.pageCircleButtonList[i];
        _animatePageCircle = true;
    }

    private void ActivatePrevNextButtonAnimation(CanvasGroup[] buttonList)
    {
        _currentPrevNextButtonAlpha = 0f;
        _buttonList = buttonList;
        _animateButton = true;
    }

    private void ActivateUnderstandMarkerAnimation(CanvasGroup understandMarker)
    {
        _currentUnderstandMarkerAlpha = 0f;
        _understandMarker = understandMarker;
        _animateUnderstandMarker = true;
    }

    private void ActivateProgressBarButtonAnimation(ProgressManager manager, int i, Color color)
    {
        _currentProgressBarButtonAlpha = 0f;
        _tempColor = manager.progressBarButtonList[i].progressBarTempColor;
        _finalColor = manager.progressBarButtonList[i].progressBarFinalColor;
        _oldColor = manager.progressBarButtonList[i].progressBarFinalColor.color;
        _newColor = color;
        _animateProgressBar = true;

    }

    private void ActivatePageAnimation(DiscussionNavigator manager)
    {
        _currentPageAlpha = 0f;
        _page = manager.subTopicsList[manager.GetCurrentSectorIndex()].pages[manager.GetCurrentPageIndex()].canvasGroup;
        _animatePage = true;
    }

    private void AnimatePageCircle()
    {
        if (_animatePageCircle)
        {
            if (_currentCircleAlpha < _targetCircleAlpha)
            {
                _currentCircleAlpha += Time.deltaTime * _cirleAnimationSpeed;
                _currentCircleColor.a = _currentCircleAlpha;
                _circleButton.buttonOutline.color = _currentCircleColor;
            }
            else
            {
                _animatePageCircle = false;
            }
            
        }
    }

    private void AnimateIndicatorRect()
    {
        if (_animateIndicator)
        {
            if (_currentWidth < _targetWidth)
            {
                _currentWidth += Time.deltaTime * _indicatorAnimationSpeed;
                _indicatorRect.indicatorRectTransform.sizeDelta = new Vector2(_currentWidth, _currentHeight);
            }
            else
            {
                _animateIndicator = false;
            }
        }
    }

    private void AnimatePrevNextButton()
    {
        if (_animateButton)
        {
            if (_currentPrevNextButtonAlpha < _targetPrevNextButtonAlpha)
            {
                foreach (var button in _buttonList)
                {
                    _currentPrevNextButtonAlpha += Time.deltaTime * _prevNextButtonAnimationSpeed;
                    button.alpha = _currentPrevNextButtonAlpha;
                }
            }
            else
            {
                _animateButton = false;
            }
        }
    }

    private void AnimateUnderstandMarker()
    {
        if (_animateUnderstandMarker)
        {
            if(_currentUnderstandMarkerAlpha < _targetUnderstandMarkerAlpha)
            {
                _currentUnderstandMarkerAlpha += Time.deltaTime * _understandMarkerAnimationSpeed;
                _understandMarker.alpha = _currentUnderstandMarkerAlpha;
            }
            else
            {
                _animateUnderstandMarker = false;
            }
        }
    }

    private void AnimateProgressBar()
    {
        if (_animateProgressBar)
        {
            if (_currentProgressBarButtonAlpha < _targetProgressBarButtonAlpha)
            {
                _currentProgressBarButtonAlpha += Time.deltaTime * _progressBarButtonAnimationSpeed;
                _newColor.a = _currentProgressBarButtonAlpha;
            
                _tempColor.color = _oldColor;
                _finalColor.color = _newColor;
            }
            else
            {
                _tempColor.gameObject.SetActive(false);
                _animateProgressBar = false;
            }
        }
    }

    private void AnimatePage()
    {
        if (_animatePage)
        {
            Debug.Log("ACTIVATE");
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
