using System.Collections;
using UnityEngine;

public class ActivitySixEnvironmentManager : ActivityEnvironmentManager
{
    [Header("Views")]
    [SerializeField] private DotProductView dotProductView;

	[Header("Environment Cameras")]
	[SerializeField] private Camera satelliteEnvironmentCamera;
	[SerializeField] private Camera mainSatelliteCamera;
	[SerializeField] private Camera targetObjectCamera;

	private void Start()
	{
		SubscribeEnvironmentEvents();
	}

	private void SubscribeEnvironmentEvents()
	{
		// Satellite environment area related events
		dotProductView.OpenViewEvent += () => SetSatelliteEnvironmentState(true);
		dotProductView.QuitViewEvent += () => SetSatelliteEnvironmentState(false);
	}

	private void SetSatelliteEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		satelliteEnvironmentCamera.gameObject.SetActive(isActive);
		mainSatelliteCamera.gameObject.SetActive(isActive);
		targetObjectCamera.gameObject.SetActive(isActive);
	}
}