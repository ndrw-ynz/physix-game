using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;

public class UnderstoodNotUnderstoodButton : MonoBehaviour
{
    public DiscussionNavigator discussionNavigator;
    public ProgressManager progressManager;
    public bool flag;

    public static event Action<bool> UnderstoodNotUnderstoodClickEvent;

    private Button _understoodNotUnderstoodButton;

    private void OnEnable()
    {
        _understoodNotUnderstoodButton = this.GetComponent<Button>();
        _understoodNotUnderstoodButton.onClick.AddListener(() => UnderstoodNotUnderstoodClickEvent?.Invoke(flag));
    }
}
