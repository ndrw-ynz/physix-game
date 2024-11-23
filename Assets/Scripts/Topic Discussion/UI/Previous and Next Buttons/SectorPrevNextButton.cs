using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SectorPrevNextButton : MonoBehaviour, IPointerEnterHandler
{
    public static event Action<Direction> SectorPrevNextClickEvent;

    [Header("Subtopic Name Text Holder")]
    public TextMeshProUGUI sectorButtonText;

    [Header("Direction of Sector")]
    [SerializeField] private Direction direction;

    [Header("Sector Button")]
    [SerializeField] private Button _prevNextSectorButton;

    private void OnEnable()
    {
        // Add click listener for a previous and next sector button
        _prevNextSectorButton.onClick.AddListener(() =>
        {
			SceneSoundManager.Instance.PlaySFX("Click_2");
			SectorPrevNextClickEvent.Invoke(direction);
        });
    }

    private void OnDisable()
    {
        // Remove listeners of the button when the game object has been disabled
        _prevNextSectorButton.onClick.RemoveAllListeners();
    }

	public void OnPointerEnter(PointerEventData eventData)
	{
		SceneSoundManager.Instance.PlaySFX("UI_Hover_Mono_01");
	}
}
