using UnityEngine;

public abstract class ActivityEnvironmentManager : MonoBehaviour
{
	[Header("Input Reader")]
	[SerializeField] private InputReader inputReader;


	[Header("Player")]
	[SerializeField] private GameObject player;

	public virtual void Start()
	{
		InteractableViewOpenerObject.SwitchToTargetCameraEvent += SwitchCameraToTargetCamera;
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