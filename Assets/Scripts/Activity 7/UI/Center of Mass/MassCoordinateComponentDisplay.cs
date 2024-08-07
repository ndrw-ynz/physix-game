using TMPro;
using UnityEngine;

public class MassCoordinateComponentDisplay : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI massNumberText;
    [Header("Input Fields")]
    [SerializeField] private TMP_InputField massInputField;
	[SerializeField] private TMP_InputField coordinatesInputField;

    public void SetupInputFields(MassCoordinatePair massCoordinatePair, int massNumber)
    {
        massNumberText.text += massNumber;
        massInputField.text = $"{massCoordinatePair.mass} kg";
        coordinatesInputField.text = $"({massCoordinatePair.coordinate.x},{massCoordinatePair.coordinate.y})";
    }
}