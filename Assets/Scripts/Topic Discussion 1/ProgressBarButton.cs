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
    private Button _progressBarButton;
    public Image progressBarImage;
    public Vector2 startPosition;
    public TextMeshProUGUI sectorTitleText;
    public TextMeshProUGUI progressCountText;
    
    public int sectorIndex;

    public void Initialize(string sectorTitle, string progressCount, int index, UnityEngine.Events.UnityAction<int> onClickAction)
    {
        discussionNavigator = GameObject.Find("MANAGERS").transform.Find("Discussion Navigator").GetComponent<DiscussionNavigator>();

        progressBarImage = GetComponent<Image>();
        startPosition = progressBarImage.transform.position;
        sectorTitleText = GetComponentInChildren<TextMeshProUGUI>();
        sectorTitleText.text = sectorTitle;
        progressCountText = GetComponentInChildren<TextMeshProUGUI>();
        progressCountText.text = progressCount;
        sectorIndex = index;
        _progressBarButton = this.GetComponent<Button>();
        _progressBarButton.onClick.AddListener(() => JumpSectors(sectorIndex));
    }

    private void JumpSectors(int index)
    {
        discussionNavigator.JumpToSector(index);
        Debug.Log("Jumped to Sector Index: " + index);
    }


}