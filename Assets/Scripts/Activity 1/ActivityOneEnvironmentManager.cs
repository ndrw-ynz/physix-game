using UnityEngine;

public class ActivityOneEnvironmentManager : ActivityEnvironmentManager
{
	[Header("Views")]
	[SerializeField] private ContainerPickerView containerPickerView;
	[SerializeField] private ScientificNotationView scientificNotationView;

	[Header("Environment Cameras")]
	[SerializeField] private Camera containerEnvironmentCamera;
	[SerializeField] private Camera SNRoomEnvironmentCamera;

	private void Start()
	{
		containerPickerView.OpenViewEvent += () => SetContainerAreaEnvironmentState(true);
		containerPickerView.QuitViewEvent += () => SetContainerAreaEnvironmentState(false);

		scientificNotationView.OpenViewEvent += () => SetSNRoomAreaEnvironmentState(true);
		scientificNotationView.QuitViewEvent += () => SetSNRoomAreaEnvironmentState(false);
	}

	private void SetContainerAreaEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		containerEnvironmentCamera.gameObject.SetActive(isActive);
	}

	private void SetSNRoomAreaEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		SNRoomEnvironmentCamera.gameObject.SetActive(isActive);
	}
}