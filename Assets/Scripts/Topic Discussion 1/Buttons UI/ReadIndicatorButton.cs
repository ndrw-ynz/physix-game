using System;
using UnityEngine;
using UnityEngine.UI;

public class ReadIndicatorButton : MonoBehaviour
{
    public static event Action<ReadState> ReadIndicatorClickEvent;

    [Header("Flag To Use For Changing States")]
    [SerializeField] private ReadState state;
    [Header("Read Indicator Button")]
    [SerializeField] private Button _readIndicatorButton;

    private void OnEnable()
    {
        _readIndicatorButton.onClick.AddListener(() => ReadIndicatorClickEvent?.Invoke(state));
    }

    private void OnDisable()
    {
        _readIndicatorButton.onClick.RemoveAllListeners();
    }
}
