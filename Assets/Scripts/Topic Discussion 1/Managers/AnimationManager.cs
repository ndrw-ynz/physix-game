using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{

    private float _indicatorAnimationSpeed = 500f;
    private float _targetWidth = 130f;
    private float _currentWidth;
    private float _currentHeight;
    private SectorIndicatorRect _indicatorRect;
    private bool _animateIndicator = false;

    private float _cirleAnimationSpeed = 3.0f;
    private float _targetCircleAlpha = 1.0f;
    private float _currentCircleAlpha;
    private Color _currentCircleColor;
    private PageCircleButton _circleButton;
    private bool _animatePageCircle = false;

    private float _buttonAnimationSpeed = 2.0f;
    private float _targetButtonAlpha = 1.0f;
    private float _currentButtonAlpha;
    private CanvasGroup[] _buttonList;
    private bool _animateButton = false;

    private void OnEnable()
    {
        PageCircleButtonsManager.PageCircleStateUpdate += ActivatePageCircleAnimation;
        ProgressManager.IndicatorRectStateUpdate += ActivateProgressBarButtonAnimation;
        PrevNextButtonsManager.ButtonChangeStateUpdate += ActivateButtonAnimation;
    }

    private void Update()
    {
        AnimatePageCircle();
        AnimateIndicatorRect();
        AnimateButton();
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

    private void ActivateButtonAnimation(CanvasGroup[] buttonCanvasList)
    {
        _currentButtonAlpha = 0f;
        _buttonList = buttonCanvasList;
        _animateButton = true;
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

    private void AnimateButton()
    {
        if (_animateButton)
        {
            if (_currentButtonAlpha < _targetButtonAlpha)
            {
                foreach (var button in _buttonList)
                {
                    _currentButtonAlpha += Time.deltaTime * _buttonAnimationSpeed;
                    button.alpha = _currentButtonAlpha;
                }
            }
            else
            {
                _animateButton = false;
            }
        }
    }
}
