using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiscussionPrevNextPageButton : MonoBehaviour
{
    public DiscussionNavigator discussionNavigator;
    public int step;
    public TextMeshProUGUI prevSectorTitle;
    public TextMeshProUGUI nextSectorTitle;
    
    public static event Action<int> PrevNextPageClickEvent;
    private Button _prevNextPageButton;

    private void OnEnable()
    {
        _prevNextPageButton = this.GetComponent<Button>();
        _prevNextPageButton.onClick.AddListener(() => PrevNextPageClickEvent?.Invoke(step));
    }
    private void OnDisable()
    {
        _prevNextPageButton.onClick.RemoveAllListeners();
    }
}
