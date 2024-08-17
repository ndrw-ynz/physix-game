using System;
using UnityEngine;

/// <summary>
/// A class managing interaction of <c>GameObjects</c> within the environment
/// of Activity Eight.
/// </summary>
public class ActivityEightEnvironmentManager : MonoBehaviour
{
	[Header("Input Reader")]
	[SerializeField] private InputReader inputReader;


	[Header("Player")]
	[SerializeField] private GameObject player;


	[Header("Cameras")]
	[SerializeField] private Camera generatorCamera;
	[SerializeField] private Camera weighingScaleCamera;


	[Header("Generator Room")]
	[SerializeField] private InteractableControlPanel generatorControlPanel;


	[Header("Weighing Scale Room")]
	[SerializeField] private GameObject weighingScaleRoomGate;
	[SerializeField] private GameObject weighingScaleRoomGateBlocker;
	[SerializeField] private InteractableControlPanel weighingScaleControlPanel;


	[Header("Reboot Room")]
	[SerializeField] private GameObject rebootRoomGate;
	[SerializeField] private GameObject rebootRoomGateBlocker;


	[Header("Gate Status Color Material")]
	[SerializeField] private Material openGateColor;

	private void Start()
	{
		ActivityEightManager.GeneratorRoomClearEvent += UpdateGeneratorRoomState;
		ActivityEightManager.WeighingScaleRoomClearEvent += UpdateWeighingScaleRoomState;

		ActivityEightManager.GeneratorRoomClearEvent += () => SwitchCameraToPlayerCamera(generatorCamera);
		ActivityEightManager.WeighingScaleRoomClearEvent += () => SwitchCameraToPlayerCamera(weighingScaleCamera);

		InteractableControlPanel.SwitchToTargetCameraEvent += SwitchCameraToTargetCamera;

		MomentOfInertiaView.QuitViewEvent += () => SwitchCameraToPlayerCamera(generatorCamera);
		TorqueView.QuitViewEvent += () => SwitchCameraToPlayerCamera(weighingScaleCamera);
	}

	private void UpdateGeneratorRoomState()
	{
		generatorControlPanel.SetInteractable(false);
		OpenGate(weighingScaleRoomGate, weighingScaleRoomGateBlocker);
	}

	private void UpdateWeighingScaleRoomState()
	{
		weighingScaleControlPanel.SetInteractable(false);
		OpenGate(rebootRoomGate, rebootRoomGateBlocker);
	}

	/// <summary>
	/// Updates the color indicator of a gate and removes associated gate blocker.
	/// </summary>
	/// <param name="roomGate"></param>
	/// <param name="roomGateBlocker"></param>
	private void OpenGate(GameObject roomGate, GameObject roomGateBlocker)
	{
		// Change gate color.
		MeshRenderer roomGateRend = roomGate.GetComponent<MeshRenderer>();
		Material[] roomGateMats = roomGateRend.materials;
		roomGateMats[2] = openGateColor;
		roomGateRend.materials = roomGateMats;

		// Remove gate blocker.
		roomGateBlocker.gameObject.SetActive(false);
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