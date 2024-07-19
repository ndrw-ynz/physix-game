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
