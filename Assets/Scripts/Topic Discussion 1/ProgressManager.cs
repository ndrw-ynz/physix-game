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

    private DiscussionNavigator discussionNavigator;
    private List<ProgressBarButton> progressBarButtonList;
    private RectTransform progressAreaParent;
    private int numButtons;
    private float buttonSpacing = 300.0f;

    private void OnEnable()
    {
        CreateProgressBarButtons();
    }

    private void CreateProgressBarButtons()
    {
        progressBarButtonList = new List<ProgressBarButton>();
        progressAreaParent = GameObject.Find("BUTTONS").transform.Find("Progress Bar Buttons").GetComponent<RectTransform>();
        discussionNavigator = GameObject.Find("MANAGERS").transform.Find("Discussion Navigator").GetComponent<DiscussionNavigator>();
        numButtons = discussionNavigator.subTopicsList.Count;

        float totalWidth = (numButtons - 1) * buttonSpacing;
        float startX = -totalWidth / 2f;


        for (int i = 0; i < numButtons; i++)
        {
            Vector2 buttonPosition = new Vector2(startX + i * buttonSpacing, 0f);
            ProgressBarButton newButton = Instantiate(progressBarButtonPrefab);


            newButton.transform.SetParent(progressAreaParent, false);
            newButton.name = $"Progress Button {i + 1}";
            newButton.transform.localPosition = buttonPosition;

            string sectorTitle = discussionNavigator.subTopicsList[i].sectorTitle;
            string progressCount = $"{CountUnderstoodPages(i).ToString()}/{CountTotalPages(i).ToString()}";

            newButton.Initialize(sectorTitle, progressCount, i);
            progressBarButtonList.Add(newButton);
        }
    }

    private double CountUnderstoodPages(int sectorIndex)
    {
        double understoodPages = 0;
        for (int i = 0; i < discussionNavigator.subTopicsList[sectorIndex].pages.Count; i++)
        {
            bool isPageUnderstood = discussionNavigator.subTopicsList[sectorIndex].pages[i].isMarkedUnderstood;

            if (isPageUnderstood)
            {
                understoodPages++;
            }
        }
        return understoodPages;
    }

    private double CountTotalPages(int sectorIndex)
    {
        return discussionNavigator.subTopicsList[sectorIndex].pages.Count;
    }
    //public void UpdateProgressBar(int sectorIndex)
    //{
    //    for (int i = 0; i < discussionNavigator.subTopicsList.Count; i++)
    //    {
    //        double understoodPages = 0;
    //        double currentSectorTotalPages = 0;
    //
    //        for (int j = 0; j < discussionNavigator.subTopicsList[i].pages.Count; j++)
    //        {
    //            Debug.Log("Current Sector Index:" + i);
    //            Debug.Log("Current Page Index:" + j);
    //            if (currentPageIsMarkedUnderstood(i, j))
    //            {
    //                understoodPages++;
    //            }
    //        }
    //
    //        double progressPercentage = (understoodPages / currentSectorTotalPages) * 100;
    //
    //        if (progressPercentage == 100)
    //        {
    //            progressBarButtonList[i].progressBarImage.color = Color.green;
    //        }
    //        else if (progressPercentage > 50)
    //        {
    //            progressBarButtonList[i].progressBarImage.color = Color.yellow;
    //        }
    //        else
    //        {
    //            progressBarButtonList[i].progressBarImage.color = Color.gray;
    //        }
    //
    //        progressBarButtonList[i].progressCountText.SetText($"{understoodPages}/{currentSectorTotalPages}");
    //        
    //    }
    //}
    //
    //private bool currentPageIsMarkedUnderstood(int currentSectorIndex, int currentPageIndex)
    //{
    //    return discussionNavigator.subTopicsList[currentSectorIndex].pages[currentPageIndex].isMarkedUnderstood;
    //}
}
