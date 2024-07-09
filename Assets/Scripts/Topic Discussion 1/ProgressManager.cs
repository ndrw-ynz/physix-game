using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;

public class ProgressManager : MonoBehaviour
{
    public ProgressBarButton progressBarButtonPrefab;
    public RectTransform progressAreaParent;
    public DiscussionNavigator discussionNavigator;
    public List<ProgressBarButton> sectorProgressBars;

    public int numButtons;
    private float buttonSpacing = 300.0f;

    private void Start()
    {

        CreateProgressBarButtons();
        Debug.Log((numButtons - 1) * buttonSpacing);
        //loadProgressBars();
    }

    private void CreateProgressBarButtons()
    {
        float totalWidth = (numButtons - 1) * buttonSpacing;
        float startX = -totalWidth / 2f;
    
        for (int i = 0; i < numButtons; i++)
        {
            progressAreaParent = GetComponent<RectTransform>();
            Vector2 buttonPosition = new Vector2(startX + i * buttonSpacing, 0f);
            ProgressBarButton newButton = Instantiate(progressBarButtonPrefab);
            newButton.transform.SetParent(progressAreaParent, false);
            newButton.name = $"Progress Button {i + 1}";
            newButton.transform.localPosition = buttonPosition;
        }
    }

    //public void loadProgressBars()
    //{
    //    for (int i = 0; i < discussionNavigator.subTopicsList.Count; i++)
    //    {
    //        string currentSectorTitle = discussionNavigator.subTopicsList[i].sectorTitle;
    //        double understoodPages = 0;
    //        double currentSectorTotalPages = discussionNavigator.subTopicsList[i].pages.Count;
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
    //            sectorProgressBars[i].progressBarRect.color = Color.green;
    //        }
    //        else if (progressPercentage > 50)
    //        {
    //            sectorProgressBars[i].progressBarRect.color = Color.yellow;
    //        }
    //        else
    //        {
    //            sectorProgressBars[i].progressBarRect.color = Color.gray;
    //        }
    //
    //        sectorProgressBars[i].sectorTitle.SetText(currentSectorTitle);
    //        sectorProgressBars[i].progressCounter.SetText($"{understoodPages}/{currentSectorTotalPages}");
    //        
    //    }
    //}
    //
    //private bool currentPageIsMarkedUnderstood(int currentSectorIndex, int currentPageIndex)
    //{
    //    return discussionNavigator.subTopicsList[currentSectorIndex].pages[currentPageIndex].isMarkedUnderstood;
    //}
}
