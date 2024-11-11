using UnityEngine;

public class ActivitySevenEnvironmentManager : ActivityEnvironmentManager
{
	[Header("Activity Manager")]
	[SerializeField] private ActivitySevenManager activitySevenManager;

	[Header("Views")]
	[SerializeField] CenterOfMassView centerOfMassView;
	[SerializeField] MomentumImpulseForceView momentumImpulseForceView;
	[SerializeField] ElasticInelasticCollisionView elasticInelasticCollisionView;

    [Header("Environment Cameras")]
    [SerializeField] private Camera centerOfMassTerminalEnvCamera;
    [SerializeField] private Camera impulseMomentumTerminalEnvCamera;
    [SerializeField] private Camera collisionTerminalEnvCamera;
	
	[Header("Center Of Mass Terminal Room Game Objects")]
    [SerializeField] private GameObject containerGlassOne;
	[SerializeField] private GameObject containerGlassTwo;
    [SerializeField] private PowerSourceCube powerSourceCubeOne;
	[SerializeField] private PowerSourceCube powerSourceCubeTwo;
    [SerializeField] private GameObject roomOneGate;
    [SerializeField] private GameObject roomOneGateBlocker;
    [SerializeField] private PowerSourceCubeContainer powerContainer;
    [SerializeField] private InteractableViewOpenerObject centerOfMassTerminal;

    [Header("Impulse-Momentum Terminal Room Game Objects")]
	[SerializeField] private GameObject roomTwoGate;
	[SerializeField] private GameObject roomTwoGateBlocker;
    [SerializeField] private CubePusher cubePusher;
	[SerializeField] private InteractableViewOpenerObject impulseMomentumTerminal;

    [Header("Collision Terminal Room Game Objects")]
	[SerializeField] private GameObject containerGlassThree;
	[SerializeField] private DataModuleCube dataModuleCube;
    [SerializeField] private Camera collisionVideoCamera;
	[SerializeField] private InteractableViewOpenerObject collisionTerminal;

	[Header("Gate Status Color Material")]
    [SerializeField] private Material openGateColor;

    private bool canPlayerPlaceCube;

	void Start()
    {
		// Center of Mass Terminal Environment Events
		centerOfMassView.OpenViewEvent += () => SetCenterOfMassTerminalEnvironmentState(true);
		centerOfMassView.QuitViewEvent += () => SetCenterOfMassTerminalEnvironmentState(false);
		activitySevenManager.CenterOfMassTerminalClearEvent += ClearCenterOfMassTerminalEnvironmentState;
		powerSourceCubeOne.RetrieveEvent += () => canPlayerPlaceCube = true;
		powerContainer.InteractEvent += UpdateCenterOfMassRoomGateState;

		// Impulse Momentum Terminal Environment Events
		momentumImpulseForceView.OpenViewEvent += () => SetImpulseMomentumTerminalEnvironmentState(true);
		momentumImpulseForceView.QuitViewEvent += () => SetImpulseMomentumTerminalEnvironmentState(false);
		activitySevenManager.MomentumImpulseTerminalClearEvent += ClearImpulseMomentumTerminalEnvironmentState;

		// Elastic Inelastic Collision Terminal Environment Events
		elasticInelasticCollisionView.OpenViewEvent += () => SetCollisionTerminalEnvironmentState(true);
		elasticInelasticCollisionView.QuitViewEvent += () => SetCollisionTerminalEnvironmentState(false);
		activitySevenManager.CollisionTerminalClearEvent += ClearCollisionTerminalEnvironmentState;
    }

	#region Center of Mass Terminal Room
	private void SetCenterOfMassTerminalEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		activitySevenManager.SetMissionObjectiveDisplay(!isActive);
		centerOfMassTerminalEnvCamera.gameObject.SetActive(isActive);
	}

	private void ClearCenterOfMassTerminalEnvironmentState()
	{
		SetCenterOfMassTerminalEnvironmentState(false);
		centerOfMassTerminal.SetInteractable(false);
		impulseMomentumTerminal.SetInteractable(true);
		// Release power cube
		powerSourceCubeOne.SetInteractable(true);
		powerContainer.SetInteractable(true);
		containerGlassOne.gameObject.SetActive(false);
	}

    private void UpdateCenterOfMassRoomGateState()
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

	#region Impulse-Momentum Terminal Room
	private void SetImpulseMomentumTerminalEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		activitySevenManager.SetMissionObjectiveDisplay(!isActive);
		impulseMomentumTerminalEnvCamera.gameObject.SetActive(isActive);
	}

	private void ClearImpulseMomentumTerminalEnvironmentState()
	{
		SetImpulseMomentumTerminalEnvironmentState(false);
		impulseMomentumTerminal.SetInteractable(false);
		collisionTerminal.SetInteractable(true);
		// Open gate and activate pusher
		OpenGate(roomTwoGate, roomTwoGateBlocker);
		cubePusher.SetWorkState(true);
	}

	#endregion

	#region Collision Terminal Room
	private void SetCollisionTerminalEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		activitySevenManager.SetMissionObjectiveDisplay(!isActive);
		collisionTerminalEnvCamera.gameObject.SetActive(isActive);
	}

	private void ClearCollisionTerminalEnvironmentState()
	{
		SetCollisionTerminalEnvironmentState(false);
		collisionTerminal.SetInteractable(false);
		// Release data module cube
		containerGlassThree.gameObject.SetActive(false);
		dataModuleCube.SetInteractable(true);
		collisionVideoCamera.gameObject.SetActive(true);
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