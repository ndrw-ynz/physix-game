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

    [Header("Gate Status Color Material")]
    [SerializeField] private Material openGateColor;

    private bool canPlayerPlaceCube;

	void Start()
    {
        // Room One Environment Events
        CenterOfMassSubmissionStatusDisplay.ProceedEvent += ReleasePowerCube;
        PowerSourceCube.RetrieveEvent += () => canPlayerPlaceCube = true;
        PowerSourceCubeContainer.InteractEvent += UpdateRoomOneGateState;
    }

	#region Room One
	private void ReleasePowerCube()
    {
        powerSourceCubeOne.SetInteractable(true);
        powerContainer.SetInteractable(true);
		containerGlassOne.gameObject.SetActive(false);
    }

    private void UpdateRoomOneGateState()
    {
        if (canPlayerPlaceCube)
        {
            // Place power source cube with container glass.
            powerSourceCubeTwo.gameObject.SetActive(true);
            containerGlassTwo.gameObject.SetActive(true);

            // Change gate color.
            MeshRenderer roomOneGateRend = roomOneGate.GetComponent<MeshRenderer>();
            Material[] roomOneGateMats = roomOneGateRend.materials;
            roomOneGateMats[2] = openGateColor;
            roomOneGateRend.materials = roomOneGateMats;

            // Remove gate blocker.
            roomOneGateBlocker.gameObject.SetActive(false);

            // Disable interaction on powerContainer
            powerContainer.SetInteractable(false);
		}
    }
    #endregion
}