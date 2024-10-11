using System;
using TMPro;
using UnityEngine;

public class ContainerPickerView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;

	[Header("Container Display Components")]
	[SerializeField] private TextMeshProUGUI containerValueText;
	[SerializeField] private GameObject displayedContainerObject;

	public void UpdateContainerDisplay(BoxContainer boxContainer)
	{
		if (boxContainer == null)
		{
			containerValueText.text = "";
			displayedContainerObject.gameObject.SetActive(false);
		} else
		{
			containerValueText.text = $"{boxContainer.measurementText.text}";
			displayedContainerObject.gameObject.SetActive(true);
		}
	}

	private void OnEnable()
	{
		OpenViewEvent?.Invoke();
	}

	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitViewEvent?.Invoke();
	}
}