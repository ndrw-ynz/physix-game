using System;
using TMPro;
using UnityEngine;

public class ContainerPickerView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;

	[SerializeField] private ContainerSelectionHandler containerSelectionHandler;

	[Header("Container Display Components")]
	[SerializeField] private TextMeshProUGUI containerValueText;
	// container image
	// container text

	private void Start()
	{
		containerSelectionHandler.UpdateSelectedContainerEvent += UpdateContainerDisplay;
	}

	private void UpdateContainerDisplay(BoxContainer? boxContainer)
	{
		if (boxContainer == null)
		{
			containerValueText.text = "";
		} else
		{
			containerValueText.text = $"{boxContainer.measurementText.text}";
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