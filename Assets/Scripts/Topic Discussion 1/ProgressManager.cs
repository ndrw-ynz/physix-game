using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ProgressManager : MonoBehaviour
{
    public ProgressBarButton progressBarButtonPrefab;

    private List<ProgressBarButton> progressBarButtonList = new List<ProgressBarButton>();
    private RectTransform progressAreaParent;
    private int numButtons;
    private float buttonSpacing = 300.0f;

    private void OnEnable()
    {
        DiscussionNavigator.DiscussionPageStart += CreateProgressBarButtons;
        DiscussionNavigator.DiscussionPageStart += UpdateProgressBar;
        DiscussionNavigator.UnderstandMarkerChangeEvent += UpdateProgressBar;
    }

    private void CreateProgressBarButtons(DiscussionNavigator discNavig)
    {
        progressAreaParent = GameObject.Find("BUTTONS").transform.Find("Progress Bar Buttons").GetComponent<RectTransform>();
        numButtons = discNavig.GetSubTopicListCount();

        float totalWidth = (numButtons - 1) * buttonSpacing;
        float startX = -totalWidth / 2f;

        for (int i = 0; i < numButtons; i++)
    {
            Vector2 buttonPosition = new Vector2(startX + i * buttonSpacing, 0f);
            ProgressBarButton newButton = Instantiate(progressBarButtonPrefab);
            newButton.transform.SetParent(progressAreaParent, false);
            newButton.name = $"Progress Button {i + 1}";
            newButton.transform.localPosition = buttonPosition;

            string sectorTitle = discNavig.GetSectorTitle(i);
            string progressCount = $"{discNavig.CountUnderstoodPages(i).ToString()}/{discNavig.CountTotalPages(i).ToString()}";
            newButton.Initialize(sectorTitle, progressCount, i);
            progressBarButtonList.Add(newButton);
    }

    private void UpdateProgressBar(DiscussionNavigator discNavig)
        {
            string currentSectorTitle = discussionNavigator.subTopicsList[i].sectorTitle;
            double understoodPages = 0;
            double currentSectorTotalPages = discussionNavigator.subTopicsList[i].pages.Count;

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
            else if(currUnderstoodPagesCount > 0)
            {
                progressBarButtonList[i].progressBarImage.color = new Color(0.8339623f, 0.8339623f, 0.8339623f);
            }
            else
            {
                progressBarButtonList[i].progressBarImage.color = Color.gray;
        }
    }

    private bool currentPageIsMarkedUnderstood(int currentSectorIndex, int currentPageIndex)
    {
        return discussionNavigator.subTopicsList[currentSectorIndex].pages[currentPageIndex].isMarkedUnderstood;
    }
}
