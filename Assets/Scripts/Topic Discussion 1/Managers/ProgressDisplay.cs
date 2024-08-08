using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.VolumeComponent;

public class ProgressDisplay : MonoBehaviour
{
    public ProgressBarButton progressBarButtonPrefab;
    public SectorIndicator sectorIndicatorRectPrefab;

    public List<ProgressBarButton> progressBarButtonList = new List<ProgressBarButton>();
    public List<SectorIndicator> sectorIndicatorRectList = new List<SectorIndicator>();
    private RectTransform progressAreaParent;
    private int _numButtons;
    private float _buttonSpacing = 300.0f;

    public static event Action<ProgressDisplay, int, Color> ProgressBarButtonStateUpdate;
    public static event Action<ProgressDisplay, int> IndicatorRectStateUpdate;
    private void OnEnable()
    {
        DiscussionNavigator.DiscussionPageStart += LoadProgressBar;
        DiscussionNavigator.DiscussionPageStart += ActivateIndicatorRect;
        DiscussionNavigator.DiscussionPageStart += UpdateProgressBarStates;
        DiscussionNavigator.SectorChangeEvent += UpdateIndicatorRects;
        DiscussionNavigator.UnderstandMarkerChangeEvent += UpdateProgressBar;
    }

    private void OnDisable()
    {
        DiscussionNavigator.DiscussionPageStart -= LoadProgressBar;
        DiscussionNavigator.DiscussionPageStart -= ActivateIndicatorRect;
        DiscussionNavigator.DiscussionPageStart -= UpdateProgressBarStates;
        DiscussionNavigator.SectorChangeEvent -= UpdateIndicatorRects;
        DiscussionNavigator.UnderstandMarkerChangeEvent -= UpdateProgressBar;
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

            Vector2 rectPosition = new Vector2(buttonPosition.x, buttonPosition.y - 46);
            GenerateSectorIndicatorRect(rectPosition, i);
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

    private void GenerateSectorIndicatorRect(Vector2 rectPosition, int i)
    {
        SectorIndicator newIndicatorRect = Instantiate(sectorIndicatorRectPrefab);
        newIndicatorRect.transform.SetParent(progressAreaParent, false);
        newIndicatorRect.name = $"Indicator Rect {i + 1}";
        newIndicatorRect.transform.localPosition = rectPosition;
        newIndicatorRect.Initialize();
        sectorIndicatorRectList.Add(newIndicatorRect);
    }

    private void ActivateIndicatorRect(DiscussionNavigator discNav)
    {
        int currentSectorIndex = discNav.GetCurrentSectorIndex();
        for (int i = 0; i < sectorIndicatorRectList.Count; i++)
        {
            if (i == currentSectorIndex)
            {
                sectorIndicatorRectList[i].gameObject.SetActive(true);
            }
            else
            {
                sectorIndicatorRectList[i].gameObject.SetActive(false);
            }
        }
    }

    private void UpdateProgressBarStates(DiscussionNavigator discNavig)
    {
        for (int i = 0; i < progressBarButtonList.Count; i++)
        {
            progressBarButtonList[i].progressBarTempColor.gameObject.SetActive(false);

            double currUnderstoodPagesCount = discNavig.CountUnderstoodPages(i);
            double currSectorPagesCount = discNavig.CountTotalPages(i);
            progressBarButtonList[i].progressCountText.text = $"{currUnderstoodPagesCount}/{currSectorPagesCount}";

            double currProgressBarPercentage = currUnderstoodPagesCount / currSectorPagesCount * 100;
            if (currProgressBarPercentage == 100)
            {
                progressBarButtonList[i].progressBarFinalColor.color = new Color(0.5890471f, 1f, 0.5264151f);
            }
            else if (currProgressBarPercentage > 50)
            {
                progressBarButtonList[i].progressBarFinalColor.color = new Color(0.9546386f, 1f, 0.5254902f);
            }
            else if (currUnderstoodPagesCount > 0)
            {
                progressBarButtonList[i].progressBarFinalColor.color = new Color(0.8339623f, 0.8339623f, 0.8339623f);
            }
            else
            {
                progressBarButtonList[i].progressBarFinalColor.color = Color.gray;
            }
        }
    }

    private void UpdateProgressBar(DiscussionNavigator discNavig)
    {
        for (int i = 0; i < progressBarButtonList.Count; i++)
        {
            if(i == discNavig.GetCurrentSectorIndex())
            {
                progressBarButtonList[i].progressBarTempColor.gameObject.SetActive(true);
                double currUnderstoodPagesCount = discNavig.CountUnderstoodPages(i);
                double currSectorPagesCount = discNavig.CountTotalPages(i);
                progressBarButtonList[i].progressCountText.text = $"{currUnderstoodPagesCount}/{currSectorPagesCount}";

                double currProgressBarPercentage = currUnderstoodPagesCount / currSectorPagesCount * 100;
                if (currProgressBarPercentage == 100)
                {
                    Color color = new Color(0.5890471f, 1f, 0.5264151f);
                    ProgressBarButtonStateUpdate?.Invoke(this, i, color);
                }
                else if (currProgressBarPercentage > 50)
                {
                    Color color = new Color(0.9546386f, 1f, 0.5254902f);
                    ProgressBarButtonStateUpdate?.Invoke(this, i, color);
                }
                else if (currUnderstoodPagesCount > 0)
                {
                    Color color = new Color(0.8339623f, 0.8339623f, 0.8339623f);
                    ProgressBarButtonStateUpdate?.Invoke(this, i, color);
                }
                else
                {
                    Color color = Color.gray;
                    ProgressBarButtonStateUpdate?.Invoke(this, i, color);
                }
            }
        }
    }

    private void UpdateIndicatorRects(DiscussionNavigator discNav)
    {
        int currentSectorIndex = discNav.GetCurrentSectorIndex();
        for (int i = 0; i < sectorIndicatorRectList.Count; i++)
        {
            if (i == currentSectorIndex)
            {
                sectorIndicatorRectList[i].gameObject.SetActive(true);
                IndicatorRectStateUpdate?.Invoke(this, i);
            }
            else 
            {
                sectorIndicatorRectList[i].gameObject.SetActive(false);
            }
        }
    }
}
