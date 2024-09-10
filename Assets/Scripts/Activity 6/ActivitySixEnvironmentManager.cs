using UnityEngine;

public class ActivitySixEnvironmentManager : ActivityEnvironmentManager
{
	[Header("Activity Manager")]
	[SerializeField] private ActivitySixManager activitySixManager;

    [Header("Views")]
    [SerializeField] private DotProductView dotProductView;

	[Header("Environment Cameras")]
	[SerializeField] private Camera satelliteEnvironmentCamera;
	[SerializeField] private Camera mainSatelliteCamera;
	[SerializeField] private Camera targetObjectCamera;

	[Header("Main Satellite Area Game Objects")]
	[SerializeField] private InteractableViewOpenerObject mainSatelliteControlPanel;
	[SerializeField] private GameObject mainSatelliteAreaIndicatorEffect;

	private void Start()
	{
		SubscribeEnvironmentEvents();
	}

	private void SubscribeEnvironmentEvents()
	{
		// Satellite environment area related events
		dotProductView.OpenViewEvent += () => SetSatelliteEnvironmentState(true);
		dotProductView.QuitViewEvent += () => SetSatelliteEnvironmentState(false);
		activitySixManager.MainSatelliteAreaClearEvent += ClearSatelliteEnvironmentState;
	}

	private void SetSatelliteEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		satelliteEnvironmentCamera.gameObject.SetActive(isActive);
		mainSatelliteCamera.gameObject.SetActive(isActive);
		targetObjectCamera.gameObject.SetActive(isActive);
	}

	private void ClearSatelliteEnvironmentState()
	{
		SetSatelliteEnvironmentState(false);
		mainSatelliteControlPanel.SetInteractable(false);
		mainSatelliteAreaIndicatorEffect.gameObject.SetActive(false);
	}
}