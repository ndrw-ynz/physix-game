using UnityEngine;

public class ActivitySevenEnvironmentManager : MonoBehaviour
{
    [Header("Room One")]
    [SerializeField] private GameObject containerGlassOne;
	[SerializeField] private GameObject containerGlassTwo;
    [SerializeField] private PowerSourceCube powerSourceCubeOne;
	[SerializeField] private PowerSourceCube powerSourceCubeTwo;
    [SerializeField] private GameObject roomOneGate;
    [SerializeField] private GameObject roomOneGateBlocker;
    [SerializeField] private PowerSourceCubeContainer powerContainer;
    [SerializeField] private InteractableControlPanel roomOneControlPanel;

    [Header("Room Two")]
	[SerializeField] private GameObject roomTwoGate;
	[SerializeField] private GameObject roomTwoGateBlocker;
    [SerializeField] private CubePusher cubePusher;
	[SerializeField] private InteractableControlPanel roomTwoControlPanel;

    [Header("Room Three")]
	[SerializeField] private GameObject containerGlassThree;
	[SerializeField] private DataModuleCube dataModuleCube;
    [SerializeField] private Camera collisionVideoCamera;
	[SerializeField] private InteractableControlPanel roomThreeControlPanel;

	[Header("Gate Status Color Material")]
    [SerializeField] private Material openGateColor;

    private bool canPlayerPlaceCube;

	void Start()
    {
        // Room One Environment Events
        ActivitySevenManager.RoomOneClearEvent += ReleasePowerCube;
        PowerSourceCube.RetrieveEvent += () => canPlayerPlaceCube = true;
        PowerSourceCubeContainer.InteractEvent += UpdateRoomOneGateState;

        // Room Two Environment Events
        ActivitySevenManager.RoomTwoClearEvent += UpdateRoomTwoGateState;

        // Room Three Environment Events
        ActivitySevenManager.RoomThreeClearEvent += ReleaseDataModuleCube;
    }

	#region Room One
	private void ReleasePowerCube()
    {
        powerSourceCubeOne.SetInteractable(true);
        powerContainer.SetInteractable(true);
		containerGlassOne.gameObject.SetActive(false);
        roomOneControlPanel.SetInteractable(false);
    }

    private void UpdateRoomOneGateState()
    {
        if (canPlayerPlaceCube)
        {
            // Place power source cube with container glass.
            powerSourceCubeTwo.gameObject.SetActive(true);
            containerGlassTwo.gameObject.SetActive(true);

            // Open room one gate
            OpenGate(roomOneGate, roomOneGateBlocker);

            // Disable interaction on powerContainer
            powerContainer.SetInteractable(false);
		}
    }
	#endregion

	#region Room Two
    private void UpdateRoomTwoGateState()
    {
        roomTwoControlPanel.SetInteractable(false);
        // Open room two gate
        OpenGate(roomTwoGate, roomTwoGateBlocker);
        cubePusher.SetWorkState(true);
    }

	#endregion

	#region Room Three

    private void ReleaseDataModuleCube()
    {
        containerGlassThree.gameObject.SetActive(false);
        dataModuleCube.SetInteractable(true);
		collisionVideoCamera.gameObject.SetActive(true);
        roomThreeControlPanel.SetInteractable(false);
    }

	#endregion

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