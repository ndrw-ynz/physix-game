using System;
using TMPro;
using UnityEngine;

public class ContainerPickerView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;

	[Header("Container Display Components")]
	[SerializeField] private TextMeshProUGUI containerValueText;

	public void UpdateContainerDisplay(BoxContainer boxContainer)
	{
		containerValueText.text = boxContainer == null ? "" : $"{boxContainer.measurementText.text}";
		SceneSoundManager.Instance.PlaySFX("Click");
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