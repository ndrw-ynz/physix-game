using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DiscussionMainMenuButton : MonoBehaviour, IPointerEnterHandler
{
    public static event Action BackToMainMenuClickEvent;

    [Header("Back To Main Menu Button")]
    [SerializeField] private Button backToMainMenuButton;

    private void OnEnable()
    {
        backToMainMenuButton.onClick.AddListener(() =>
        {
			SceneSoundManager.Instance.PlaySFX("Click_2");
			BackToMainMenuClickEvent?.Invoke();
        });
    }

    private void OnDisable()
    {
        backToMainMenuButton.onClick.RemoveAllListeners();
    }

	public void OnPointerEnter(PointerEventData eventData)
	{
		SceneSoundManager.Instance.PlaySFX("UI_Hover_Mono_01");
	}
}
