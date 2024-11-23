using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiscussionActivityButton : MonoBehaviour
{
    public static event Action StartActivityClickEvent;

    [Header("Start Activity Button")]
    [SerializeField] private Button startActivityButton;

    private void OnEnable()
    {
        startActivityButton.onClick.AddListener(() =>
        {
			SceneSoundManager.Instance.PlaySFX("Click_2");
			StartActivityClickEvent?.Invoke();
        });
    }

    private void OnDisable()
    {
        startActivityButton.onClick.RemoveAllListeners();
    }

	public void OnPointerEnter(PointerEventData eventData)
	{
		SceneSoundManager.Instance.PlaySFX("UI_Hover_Mono_01");
	}
}
