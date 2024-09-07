using TMPro;
using UnityEngine;

public class GivenVariableDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI variableDescriptorText;
	[SerializeField] private TMP_InputField valueInputField;

	public void SetupGivenVariableDisplay(string descriptor, string value)
	{
		variableDescriptorText.text = descriptor;
		valueInputField.text = value;
	}
}