using System;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSectorJumpButton : MonoBehaviour
{
    public static event Action<int> SectorJumpButtonClick;

    [Header("Sector Jump Button Properties")]
    [SerializeField] private Button sectorJumpButton;
    [SerializeField] private int index;

    private void OnEnable()
    {
        // Add listeners
        sectorJumpButton.onClick.AddListener(() => SectorJumpButtonClick?.Invoke(index));
    }

    private void OnDisable()
    {
        // Remove listeners
        sectorJumpButton.onClick.RemoveAllListeners();
    }
}
