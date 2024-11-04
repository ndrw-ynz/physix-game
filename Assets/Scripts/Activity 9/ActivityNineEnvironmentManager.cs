using UnityEngine;

public class ActivityNineEnvironmentManager : MonoBehaviour
{
	[Header("Input Reader")]
	[SerializeField] private InputReader inputReader;


	[Header("Player")]
	[SerializeField] private GameObject player;


	[Header("Cameras")]
	[SerializeField] private Camera planetCamera;
	[SerializeField] private Camera spaceshipRTCamera;
	[SerializeField] private Camera satelliteOneRTCamera;
	[SerializeField] private Camera satelliteTwoRTCamera;

	private void Start()
	{
		InteractableControlPanel.SwitchToTargetCameraEvent += SwitchCameraToTargetCamera;

		GravityView.QuitViewEvent += () => SwitchCameraToPlayerCamera(planetCamera);
		GravityView.UpdateDisplayedOrbittingObjectEvent += ActivateRTCamera;
	}

    private void OnDisable()
    {
        InteractableControlPanel.SwitchToTargetCameraEvent -= SwitchCameraToTargetCamera;

        GravityView.QuitViewEvent -= () => SwitchCameraToPlayerCamera(planetCamera);
        GravityView.UpdateDisplayedOrbittingObjectEvent -= ActivateRTCamera;
    }

    private void SwitchCameraToTargetCamera(Camera targetCamera)
	{
		if (player != null && targetCamera != null)
		{
			player.gameObject.SetActive(false);
			targetCamera.gameObject.SetActive(true);
		}
		inputReader.SetUI();
	}

	public void SwitchCameraToPlayerCamera(Camera targetCamera)
	{
		if (player != null && targetCamera != null)
		{
			player.gameObject.SetActive(true);
			targetCamera.gameObject.SetActive(false);
		}
		inputReader.SetGameplay();
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