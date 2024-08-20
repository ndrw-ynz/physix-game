using TMPro;
using UnityEngine;

public enum TorqueDirection
{
    Upward,
    Downward
}

public class TorqueDirectionButton : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI displayText;
	[SerializeField] private TorqueDirectionButton buttonCounterpart;
	public bool isClicked { get; private set; }
	public TorqueDirection torqueDirection;

	public void ResetState()
	{
		isClicked = false;
		displayText.color = new Color32(200, 75, 55, 255);
	}

	public void OnClick()
	{
		// Reset state of button counterpart.
		buttonCounterpart.ResetState();

		// Afterwards, this portion is only implemented on clicked instance.
		isClicked = true;
		displayText.color = new Color32(175, 255, 155, 255);
	}
}