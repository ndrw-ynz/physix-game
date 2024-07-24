using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiscussionPrevNextSectorButton : MonoBehaviour
{
    public DiscussionNavigator discussionNavigator;
    public string action;
    public TextMeshProUGUI sectorButtonText;
    public CanvasGroup canvasGroup;

    public static event Action<string> PrevNextSectorClickEvent;
    private Button _prevNextSectorButton;

    private void OnEnable()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();

        _prevNextSectorButton = this.GetComponent<Button>();
        _prevNextSectorButton.onClick.AddListener(() => PrevNextSectorClickEvent.Invoke(action));
    }

    private void OnDisable()
    {
        _prevNextSectorButton.onClick.RemoveAllListeners();
    }
}
