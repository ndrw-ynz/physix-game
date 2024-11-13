using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MechanicSectorButton : MonoBehaviour
{
    public static event Action<int> SectorJumpButtonClick;

    [Header("Button Image and Text Properties")]
    public Image buttonImage;
    public TextMeshProUGUI buttonText;

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
