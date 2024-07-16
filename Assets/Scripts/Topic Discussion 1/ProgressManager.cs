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
    public RectTransform progressAreaParent;
    public DiscussionNavigator discussionNavigator;
    private List<ProgressBarButton> progressBarButtonList;
    public int numButtons;

    private float buttonSpacing = 300.0f;

    private void OnEnable()
    {
        CreateProgressBarButtons();
    }

    private void CreateProgressBarButtons()
    {
        float totalWidth = (numButtons - 1) * buttonSpacing;
        float startX = -totalWidth / 2f;
        progressBarButtonList = new List<ProgressBarButton>();
        progressAreaParent = GameObject.Find("BUTTONS").transform.Find("Progress Bar Buttons").GetComponent<RectTransform>();


        for (int i = 0; i < numButtons; i++)
        {
            Vector2 buttonPosition = new Vector2(startX + i * buttonSpacing, 0f);
            ProgressBarButton newButton = Instantiate(progressBarButtonPrefab);

            newButton.transform.SetParent(progressAreaParent, false);
            newButton.name = $"Progress Button {i + 1}";
            newButton.transform.localPosition = buttonPosition;
            newButton.Initialize("Test", "Test", i, OnClick);
            progressBarButtonList.Add(newButton);
        }
    }

    private void OnClick(int i)
    {
        Debug.Log("Hello" + i);
    }

    public void loadProgressBars()
    {
        for (int i = 0; i < discussionNavigator.subTopicsList.Count; i++)
        {
            string currentSectorTitle = discussionNavigator.subTopicsList[i].sectorTitle;
            double understoodPages = 0;
            double currentSectorTotalPages = discussionNavigator.subTopicsList[i].pages.Count;
    
            for (int j = 0; j < discussionNavigator.subTopicsList[i].pages.Count; j++)
            {
                Debug.Log("Current Sector Index:" + i);
                Debug.Log("Current Page Index:" + j);
                if (currentPageIsMarkedUnderstood(i, j))
                {
                    understoodPages++;
                }
            }
    
            double progressPercentage = (understoodPages / currentSectorTotalPages) * 100;
    
            if (progressPercentage == 100)
            {
                progressBarButtonList[i].progressBarImage.color = Color.green;
            }
            else if (progressPercentage > 50)
            {
                progressBarButtonList[i].progressBarImage.color = Color.yellow;
            }
            else
            {
                progressBarButtonList[i].progressBarImage.color = Color.gray;
            }

            progressBarButtonList[i].progressCountText.SetText($"{understoodPages}/{currentSectorTotalPages}");
            
        }
    }
    
    private bool currentPageIsMarkedUnderstood(int currentSectorIndex, int currentPageIndex)
    {
        return discussionNavigator.subTopicsList[currentSectorIndex].pages[currentPageIndex].isMarkedUnderstood;
    }
}
