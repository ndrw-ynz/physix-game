using UnityEngine;

public class ActivityNineEnvironmentManager : ActivityEnvironmentManager
{
	[Header("Activity Manager")]
	[SerializeField] private ActivityNineManager activityNineManager;

	[Header("Views")]
	[SerializeField] private GravityView gravityView;

	[Header("Interactable Terminals")]
	[SerializeField] private InteractableViewOpenerObject gravityControlTerminal;

	[Header("Cameras")]
	[SerializeField] private Camera planetCamera;
	[SerializeField] private Camera spaceshipRTCamera;
	[SerializeField] private Camera satelliteOneRTCamera;
	[SerializeField] private Camera satelliteTwoRTCamera;

	private void Start()
	{
		// Gravity Terminal Environment Events
		gravityView.OpenViewEvent += () => SetGravityTerminalEnvironmentState(true);
		gravityView.QuitViewEvent += () => SetGravityTerminalEnvironmentState(false);
		gravityView.UpdateDisplayedOrbittingObjectEvent += ActivateRTCamera;
	}

    private void OnDisable()
    {
		gravityView.OpenViewEvent -= () => SetGravityTerminalEnvironmentState(true);
		gravityView.QuitViewEvent -= () => SetGravityTerminalEnvironmentState(false);
		gravityView.UpdateDisplayedOrbittingObjectEvent -= ActivateRTCamera;
	}

	private void SetGravityTerminalEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		activityNineManager.SetMissionObjectiveDisplay(!isActive);
		planetCamera.gameObject.SetActive(isActive);
	}

	private void ActivateRTCamera(OrbittingObjectType orbittingObjectType)
	{
		// Disable all RT cameras.
		spaceshipRTCamera.gameObject.SetActive(false);
		satelliteOneRTCamera.gameObject.SetActive(false);
		satelliteTwoRTCamera.gameObject.SetActive(false);

		// Only activate necessary camera to render.
		switch (orbittingObjectType)
		{
			case OrbittingObjectType.Spaceship:
				spaceshipRTCamera.gameObject.SetActive(true);
				break;
			case OrbittingObjectType.SatelliteOne:
				satelliteOneRTCamera.gameObject.SetActive(true);
				break;
			case OrbittingObjectType.SatelliteTwo:
				satelliteTwoRTCamera.gameObject.SetActive(true);
				break;
		}
	}
}