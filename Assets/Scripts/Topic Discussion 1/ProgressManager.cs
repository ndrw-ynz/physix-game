using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.VolumeComponent;

public class ProgressManager : MonoBehaviour
{
    public ProgressBarButton progressBarButtonPrefab;
    public SectorIndicator sectorCircleIndicatorPrefab;

    private List<ProgressBarButton> progressBarButtonList = new List<ProgressBarButton>();
    private List<SectorIndicator> sectorCircleIndicatorList = new List<SectorIndicator>();
    private RectTransform progressAreaParent;
    private int _numButtons;
    private float _buttonSpacing = 300.0f;

    private float _indicatorAnimationSpeed = 500f;
    private int _currentIndicatorIndex = 0;
    private float _targetWidth = 130f;
    private float _currentWidth;
    private float _currentHeight;
    private bool _animateIndicator = false;


    private void OnEnable()
    {
        DiscussionNavigator.DiscussionPageStart += LoadProgressBar;
        DiscussionNavigator.DiscussionPageStart += LoadCircleIndicators;
        DiscussionNavigator.DiscussionPageStart += UpdateProgressBar;
        DiscussionNavigator.SectorChangeEvent += UpdateCircleIndicators;
        DiscussionNavigator.UnderstandMarkerChangeEvent += UpdateProgressBar;
    }

    private void OnDisable()
    {
        DiscussionNavigator.DiscussionPageStart -= LoadProgressBar;
        DiscussionNavigator.DiscussionPageStart -= UpdateProgressBar;
        DiscussionNavigator.DiscussionPageStart -= UpdateCircleIndicators;
        DiscussionNavigator.SectorChangeEvent -= UpdateCircleIndicators;
        DiscussionNavigator.UnderstandMarkerChangeEvent -= UpdateProgressBar;
    }

    private void Update()
    {
        if (_animateIndicator)
        {
            if (_currentWidth < _targetWidth) {
                _currentWidth += Time.deltaTime * _indicatorAnimationSpeed;
                sectorCircleIndicatorList[_currentIndicatorIndex].circleIndicatorRectTransform.sizeDelta = new Vector2(_currentWidth, _currentHeight);
            }
            else
            {
                _animateIndicator = false;
            }
        }
    }

    private void LoadProgressBar(DiscussionNavigator discNavig)
    {
        progressAreaParent = GameObject.Find("BUTTONS").transform.Find("Progress Bar Buttons").GetComponent<RectTransform>();
        _numButtons = discNavig.GetSubTopicListCount();

        float totalWidth = (_numButtons - 1) * _buttonSpacing;
        float startX = -totalWidth / 2f;

        for (int i = 0; i < _numButtons; i++)
        {
            Vector2 buttonPosition = new Vector2(startX + i * _buttonSpacing, 0f);
            string sectorTitle = discNavig.GetSectorTitle(i);
            string progressCount = $"{discNavig.CountUnderstoodPages(i).ToString()}/{discNavig.CountTotalPages(i).ToString()}";
            GenerateProgressBarButton(buttonPosition, i, sectorTitle, progressCount);

            Vector2 circlePosition = new Vector2(buttonPosition.x, buttonPosition.y - 46);
            GenerateSectorCircleIndicator(circlePosition, i);
        }
    }

    private void LoadCircleIndicators(DiscussionNavigator discNav)
    {
        int currentSectorIndex = discNav.GetCurrentSectorIndex();
        for (int i = 0; i < sectorCircleIndicatorList.Count; i++)
        {
            if (i == currentSectorIndex)
            {
                sectorCircleIndicatorList[i].gameObject.SetActive(true);
            }
            else
            {
                sectorCircleIndicatorList[i].gameObject.SetActive(false);
            }
        }
    }

    private void GenerateProgressBarButton(Vector2 buttonPosition, int i, string sectorTitle, string progressCount)
    {
        ProgressBarButton newButton = Instantiate(progressBarButtonPrefab);
        newButton.transform.SetParent(progressAreaParent, false);
        newButton.name = $"Progress Button {i + 1}";
        newButton.transform.localPosition = buttonPosition;
        newButton.Initialize(sectorTitle, progressCount, i);
        progressBarButtonList.Add(newButton);

    }

    private void GenerateSectorCircleIndicator(Vector2 circlePosition, int i)
    {
        SectorIndicator newIndicator = Instantiate(sectorCircleIndicatorPrefab);
        newIndicator.transform.SetParent(progressAreaParent, false);
        newIndicator.name = $"Circle Button {i + 1}";
        newIndicator.transform.localPosition = circlePosition;
        newIndicator.Initialize();
        sectorCircleIndicatorList.Add(newIndicator);
    }

    private void UpdateProgressBar(DiscussionNavigator discNavig)
    {
        for (int i = 0; i < progressBarButtonList.Count; i++)
        {
            double currUnderstoodPagesCount = discNavig.CountUnderstoodPages(i);
            double currSectorPagesCount = discNavig.CountTotalPages(i);
            progressBarButtonList[i].progressCountText.text = $"{currUnderstoodPagesCount}/{currSectorPagesCount}";

            double currProgressBarPercentage = currUnderstoodPagesCount / currSectorPagesCount * 100;
            if (currProgressBarPercentage == 100)
            {
                progressBarButtonList[i].progressBarImage.color = new Color(0.5890471f, 1f, 0.5264151f);
            }
            else if (currProgressBarPercentage > 50)
            {
                progressBarButtonList[i].progressBarImage.color = new Color(0.9546386f, 1f, 0.5254902f);
            }
            else if (currUnderstoodPagesCount > 0)
            {
                progressBarButtonList[i].progressBarImage.color = new Color(0.8339623f, 0.8339623f, 0.8339623f);
            }
            else
            {
                progressBarButtonList[i].progressBarImage.color = Color.gray;
            }
        }
    }

    private void UpdateCircleIndicators(DiscussionNavigator discNav)
    {
        int currentSectorIndex = discNav.GetCurrentSectorIndex();
        for (int i = 0; i < sectorCircleIndicatorList.Count; i++)
        {
            if (i == currentSectorIndex)
            {
                sectorCircleIndicatorList[i].gameObject.SetActive(true);
                _currentIndicatorIndex = i;
                _currentWidth = 0;
                _currentHeight = sectorCircleIndicatorList[i].circleIndicatorRectTransform.rect.height;
                _animateIndicator = true;
            }
            else 
            {
                sectorCircleIndicatorList[i].gameObject.SetActive(false);
            }
        }
    }
}
