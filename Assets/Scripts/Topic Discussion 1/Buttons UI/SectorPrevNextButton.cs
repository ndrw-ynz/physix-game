using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SectorPrevNextButton : MonoBehaviour
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
        _prevNextSectorButton.onClick.AddListener(() => SectorPrevNextClickEvent.Invoke(direction));
    }

    private void OnDisable()
    {
        _prevNextSectorButton.onClick.RemoveAllListeners();
    }
}
