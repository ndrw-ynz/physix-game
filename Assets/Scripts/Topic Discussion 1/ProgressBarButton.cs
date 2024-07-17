using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarButton : MonoBehaviour
{
    DiscussionNavigator discussionNavigator;
    TextMeshProUGUI[] textComponents;
    private Button _progressBarButton;
    public Image progressBarImage;
    public Vector2 startPosition;
    public TextMeshProUGUI sectorTitleText;
    public TextMeshProUGUI progressCountText;

    public int sectorIndex;

    public static event Action<int> ProgressBarClickEvent;

    public void Initialize(int index, UnityEngine.Events.UnityAction<int> onClickAction)
    {
        discussionNavigator = GameObject.Find("MANAGERS").transform.Find("Discussion Navigator").GetComponent<DiscussionNavigator>();
        textComponents = GetComponentsInChildren<TextMeshProUGUI>();
        string sectorTitle = discussionNavigator.subTopicsList[index].sectorTitle;
        string progressCount = $"{CountUnderstoodPages(index).ToString()}/{ CountTotalPages(index).ToString()}";

        progressBarImage = GetComponent<Image>();
        startPosition = progressBarImage.transform.position;
        sectorTitleText = textComponents[0];
        sectorTitleText.text = sectorTitle;
        progressCountText = textComponents[1];
        progressCountText.text = progressCount;
        sectorIndex = index;
        _progressBarButton = this.GetComponent<Button>();
        _progressBarButton.onClick.AddListener(() => ProgressBarClickEvent?.Invoke(sectorIndex));
    }

    public double CountUnderstoodPages(int sectorIndex)
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

    public double CountTotalPages(int sectorIndex)
    {
        return discussionNavigator.subTopicsList[sectorIndex].pages.Count;
    }
}