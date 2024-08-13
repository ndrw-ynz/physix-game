using UnityEngine;

/// <summary>
/// A class managing interaction of <c>GameObjects</c> within the environment
/// of Activity Eight.
/// </summary>
public class ActivityEightEnvironmentManager : MonoBehaviour
{
    [Header("Weighing Scale Room")]
	[SerializeField] private GameObject weighingScaleRoomGate;
	[SerializeField] private GameObject weighingScaleRoomGateBlocker;

	[Header("Gate Status Color Material")]
	[SerializeField] private Material openGateColor;

	private void Start()
	{
		ActivityEightManager.GeneratorRoomClearEvent += () => OpenGate(weighingScaleRoomGate, weighingScaleRoomGateBlocker);
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
}