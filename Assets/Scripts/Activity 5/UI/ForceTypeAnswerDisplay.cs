using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ForceTypeAnswerDisplay : MonoBehaviour
{
	[SerializeField] private Image forceTypeStatusBorderDisplay;
	[SerializeField] private TextMeshProUGUI forceSymbolText;
    [SerializeField] private TextMeshProUGUI subscriptText;

	public void UpdateStatusBorderDisplay(bool isCorrect)
	{
		forceTypeStatusBorderDisplay.color = (
			isCorrect
			) ?
			new Color32(175, 255, 155, 255) :
			new Color32(200, 75, 55, 255);
	}

	public void UpdateTextDisplay(ForceType? forceType)
	{
		if (forceType == null)
		{
			forceSymbolText.text = "N/A";
			subscriptText.text = "";
			return;
		}

		forceSymbolText.text = "F";
		switch (forceType)
		{
			case ForceType.FrictionalForce:
				subscriptText.text = "f";
				break;
			case ForceType.TensionForce:
				subscriptText.text = "T";
				break;
			case ForceType.SpringForce:
				subscriptText.text = "s";
				break;
			case ForceType.AppliedForce:
				subscriptText.text = "a";
				break;
			case ForceType.ThrustForce:
				subscriptText.text = "t";
				break;
			case ForceType.DragForce:
				subscriptText.text = "d";
				break;
			case ForceType.GravitationalForce:
				subscriptText.text = "g";
				break;
			case ForceType.NormalForce:
				subscriptText.text = "N";
				break;
			case ForceType.BuoyantForce:
				subscriptText.text = "B";
				break;
		}
	}
}