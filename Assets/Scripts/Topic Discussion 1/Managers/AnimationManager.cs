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

    private float _cirleAnimationSpeed = 3f;
    private float _targetAlpha = 1.0f;
    private float _currentAlpha;
    private Color _currentColor;
    private PageCircleButton _circleButton;
    private bool _animatePageCircle = false;

    private void OnEnable()
    {
        PageCircleButtonsManager.PageCircleStateUpdate += ActivatePageCircleAnimation;
        ProgressManager.IndicatorRectStateUpdate += ActivateProgressBarButtonAnimation;
    }

    private void Update()
    {
        AnimatePageCircle();
        AnimateIndicatorRect();
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
        _currentAlpha = 0f;
        _currentColor = manager.pageCircleButtonList[i].buttonOutline.color;
        _circleButton = manager.pageCircleButtonList[i];
        _animatePageCircle = true;
    }

    private void AnimatePageCircle()
    {
        if (_animatePageCircle)
        {
            if (_currentAlpha < _targetAlpha)
            {
                _currentAlpha += Time.deltaTime * _cirleAnimationSpeed;
                _currentColor.a = _currentAlpha;
                _circleButton.buttonOutline.color = _currentColor;
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
}
