using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ReadIndicatorButton : MonoBehaviour, IPointerEnterHandler
{
    public static event Action<ReadState> ReadIndicatorClickEvent;

    [Header("Flag To Use For Changing States")]
    [SerializeField] private ReadState state;

    [Header("Read Indicator Button")]
    [SerializeField] private Button _readIndicatorButton;

    private void OnEnable()
    {
        // Add click listener for a read indicator button
        _readIndicatorButton.onClick.AddListener(() =>
        {
			SceneSoundManager.Instance.PlaySFX("Click_2");
			ReadIndicatorClickEvent?.Invoke(state);
        });
    }

    private void OnDisable()
    {
        // Remove listeners of the button when the game object has been disabled
        _readIndicatorButton.onClick.RemoveAllListeners();
    }

	public void OnPointerEnter(PointerEventData eventData)
	{
		SceneSoundManager.Instance.PlaySFX("UI_Hover_Mono_01");
	}
}
