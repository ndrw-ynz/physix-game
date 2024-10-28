using System;
using UnityEngine;
using UnityEngine.UI;

public class TopicDiscussionButton : MonoBehaviour
{
    public static event Action<int> TopicDiscussionClick;

    [Header("Topic Discussion Button Properties")]
    [SerializeField] private GameObject topicDiscussionPanel;
    [SerializeField] private Button topicDiscussionButton;

    [Header("Topic Discussion Button Number")]
    [SerializeField] private int topicDiscussionNumber;

    private void OnEnable()
    {
        // Subscribe Button Click Listeners
        topicDiscussionButton.onClick.AddListener(() => TopicDiscussionClick?.Invoke(topicDiscussionNumber));
    }

    private void OnDisable()
    {
        // Unsubscribe Button Click Listeners
        topicDiscussionButton.onClick.RemoveAllListeners();
    }
}
