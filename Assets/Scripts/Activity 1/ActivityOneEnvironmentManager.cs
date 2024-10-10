using UnityEngine;

public class ActivityOneEnvironmentManager : ActivityEnvironmentManager
{
	[Header("Views")]
	[SerializeField] private ContainerPickerView containerPickerView;

	[Header("Environment Cameras")]
	[SerializeField] private Camera containerEnvironmentCamera;
	
	private void Start()
	{
		containerPickerView.OpenViewEvent += () => SetContainerAreaEnvironmentState(true);
		containerPickerView.QuitViewEvent += () => SetContainerAreaEnvironmentState(false);
	}

	private void SetContainerAreaEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		containerEnvironmentCamera.gameObject.SetActive(isActive);
	}
}