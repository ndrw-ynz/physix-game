using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiscussionBackToActivityButton : MonoBehaviour, IPointerEnterHandler
{
    public static event Action BackToActivityClickEvent;

    [Header("Back To Activity Button")]
    [SerializeField] private Button backToActivityButton;

    private void OnEnable()
    {
        backToActivityButton.onClick.AddListener(() =>
        {
			SceneSoundManager.Instance.PlaySFX("Click_2");
			BackToActivityClickEvent?.Invoke();
        });
    }

    private void OnDisable()
    {
        backToActivityButton.onClick.RemoveAllListeners();
    }

	public void OnPointerEnter(PointerEventData eventData)
	{
		SceneSoundManager.Instance.PlaySFX("UI_Hover_Mono_01");
	}
}
