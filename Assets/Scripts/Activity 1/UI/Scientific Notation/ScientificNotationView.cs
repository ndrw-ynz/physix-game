using System;
using System.Xml.Linq;
using TMPro;
using UnityEngine;

public class ScientificNotationView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;

	[Header("Display Text")]
	[SerializeField] private TextMeshProUGUI numberOfContainersText;

	[Header("Given Fields")]
	[SerializeField] private TMP_InputField givenContainerMass;

	[Header("Input Fields")]
	[SerializeField] private TMP_InputField coefficientInputField;
	[SerializeField] private TMP_InputField exponentInputField;

	[Header("Prompts")]
	[SerializeField] private GameObject selectContainerPrompt;

	private void OnEnable()
	{
		OpenViewEvent?.Invoke();
	}

	public void UpdateNumberOfContainersTextDisplay(int numOfContainers, int totalContainers)
	{
		numberOfContainersText.text = $"Converted Containers: {numOfContainers} / {totalContainers}";
	}

	public void UpdateScientificNotationView(BoxContainer selectedBoxContainer)
	{
		if (selectedBoxContainer == null)
		{
			selectContainerPrompt.gameObject.SetActive(true);
		}
		else
		{
			givenContainerMass.text = selectedBoxContainer.measurementText.text;
			selectContainerPrompt.gameObject.SetActive(false);
		}
		ClearAllFields();
	}

	private void ClearAllFields()
	{
		coefficientInputField.text = "0";
		exponentInputField.text = "0";
	}

	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitViewEvent?.Invoke();
	}

}