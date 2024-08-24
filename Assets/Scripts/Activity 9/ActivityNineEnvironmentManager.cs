using UnityEngine;

public class ActivityNineEnvironmentManager : MonoBehaviour
{
	[Header("Input Reader")]
	[SerializeField] private InputReader inputReader;


	[Header("Player")]
	[SerializeField] private GameObject player;


	[Header("Cameras")]
	[SerializeField] private Camera planetCamera;

	private void Start()
	{
		InteractableControlPanel.SwitchToTargetCameraEvent += SwitchCameraToTargetCamera;

		GravityView.QuitViewEvent += () => SwitchCameraToPlayerCamera(planetCamera);
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
}