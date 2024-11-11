using UnityEngine;

/// <summary>
/// A class managing interaction of <c>GameObjects</c> within the environment
/// of Activity Eight.
/// </summary>
public class ActivityEightEnvironmentManager : ActivityEnvironmentManager
{
	[Header("Activity Manager")]
	[SerializeField] private ActivityEightManager activityEightManager;

	[Header("Views")]
	[SerializeField] private MomentOfInertiaView momentOfInertiaView;
	[SerializeField] private TorqueView torqueView;
	[SerializeField] private EquilibriumView equilibriumView;

	[Header("Environment Cameras")]
	[SerializeField] private Camera generatorCamera;
	[SerializeField] private Camera fulcrumCamera;
	[SerializeField] private Camera weighingScaleCamera;


	[Header("Generator Room")]
	[SerializeField] private InteractableViewOpenerObject generatorControlTerminal;


	[Header("Moment of Inertia Objects")]
	[SerializeField] private GameObject slenderRodCenter;
	[SerializeField] private GameObject slenderRodEnd;
	[SerializeField] private GameObject rectangularPlateCenter;
	[SerializeField] private GameObject rectangularPlateEdge;
	[SerializeField] private GameObject cylinder;
	[SerializeField] private GameObject sphere;
	[SerializeField] private GameObject disk;


	[Header("Weighing Scale Room")]
	[SerializeField] private GameObject weighingScaleRoomGate;
	[SerializeField] private GameObject weighingScaleRoomGateBlocker;
	[SerializeField] private InteractableViewOpenerObject weighingScaleControlTerminal;


	[Header("Reboot Room")]
	[SerializeField] private GameObject rebootRoomGate;
	[SerializeField] private GameObject rebootRoomGateBlocker;
	[SerializeField] private InteractableViewOpenerObject equilibriumControlTerminal;
	[SerializeField] private RebootButton rebootButton;
	[SerializeField] private GameObject rebootButtonGlassCover;


	[Header("Gate Status Color Material")]
	[SerializeField] private Material openGateColor;

	private void Start()
	{
		// Generator Room Environment Events
		momentOfInertiaView.OpenViewEvent += () => SetGeneratorRoomEnvironmentState(true);
		momentOfInertiaView.QuitViewEvent += () => SetGeneratorRoomEnvironmentState(false);
		momentOfInertiaView.UpdateObjectDisplayEvent += UpdateDisplayedMomentOfInertiaObject;
		activityEightManager.GeneratorRoomClearEvent += ClearGeneratorRoomEnvironmentState;

		// Weighing Scale Room Environment Events
		torqueView.OpenViewEvent += () => SetWeighingScaleRoomEnvironmentState(true);
		torqueView.QuitViewEvent += () => SetWeighingScaleRoomEnvironmentState(false);
		activityEightManager.WeighingScaleRoomClearEvent += ClearWeighingScaleRoomEnvironmentState;

		// Reboot Room Environment Events
		equilibriumView.OpenViewEvent += () => SetRebootRoomEnvironmentState(true);
		equilibriumView.QuitViewEvent += () => SetRebootRoomEnvironmentState(false);
		activityEightManager.RebootRoomClearEvent += ClearRebootRoomEnvironmentState;
	}

    private void OnDisable()
    {
		// Generator Room Environment Events
		momentOfInertiaView.OpenViewEvent -= () => SetGeneratorRoomEnvironmentState(true);
		momentOfInertiaView.QuitViewEvent -= () => SetGeneratorRoomEnvironmentState(false);
		momentOfInertiaView.UpdateObjectDisplayEvent -= UpdateDisplayedMomentOfInertiaObject;
		activityEightManager.GeneratorRoomClearEvent -= ClearGeneratorRoomEnvironmentState;

		// Weighing Scale Room Environment Events
		torqueView.OpenViewEvent -= () => SetWeighingScaleRoomEnvironmentState(true);
		torqueView.QuitViewEvent -= () => SetWeighingScaleRoomEnvironmentState(false);
		activityEightManager.WeighingScaleRoomClearEvent -= ClearWeighingScaleRoomEnvironmentState;

		// Reboot Room Environment Events
		equilibriumView.OpenViewEvent -= () => SetRebootRoomEnvironmentState(true);
		equilibriumView.QuitViewEvent -= () => SetRebootRoomEnvironmentState(false);
		activityEightManager.RebootRoomClearEvent -= ClearRebootRoomEnvironmentState;
    }

	#region Generator Room Environment
	private void SetGeneratorRoomEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		activityEightManager.SetMissionObjectiveDisplay(!isActive);
		generatorCamera.gameObject.SetActive(isActive);
	}

	private void ClearGeneratorRoomEnvironmentState()
	{
		SetGeneratorRoomEnvironmentState(false);
		generatorControlTerminal.SetInteractable(false);
		weighingScaleControlTerminal.SetInteractable(true);
		OpenGate(weighingScaleRoomGate, weighingScaleRoomGateBlocker);
	}

	/// <summary>
	/// Updates the currently displayed Moment of Inertia Object for view.
	/// </summary>
	/// <param name="inertiaObjectType"></param>
	private void UpdateDisplayedMomentOfInertiaObject(InertiaObjectType inertiaObjectType)
	{
		DeactivateMomentOfInertiaObjects();

		switch (inertiaObjectType)
		{
			case InertiaObjectType.SlenderRodCenter:
				slenderRodCenter.SetActive(true);
				break;
			case InertiaObjectType.SlenderRodEnd:
				slenderRodEnd.SetActive(true);
				break;
			case InertiaObjectType.RectangularPlateCenter:
				rectangularPlateCenter.SetActive(true);
				break;
			case InertiaObjectType.RectangularPlateEdge:
				rectangularPlateEdge.SetActive(true);
				break;
			case InertiaObjectType.HollowCylinder:
			case InertiaObjectType.SolidCylinder:
			case InertiaObjectType.ThinWalledHollowCylinder:
				cylinder.SetActive(true);
				break;
			case InertiaObjectType.SolidSphere:
			case InertiaObjectType.ThinWalledHollowSphere:
				sphere.SetActive(true);
				break;
			case InertiaObjectType.SolidDisk:
				disk.SetActive(true);
				break;
		}
	}

	/// <summary>
	/// Deactivates all Moment of Inertia objects.
	/// </summary>
	private void DeactivateMomentOfInertiaObjects()
	{
		slenderRodCenter.SetActive(false);
		slenderRodEnd.SetActive(false);
		rectangularPlateCenter.SetActive(false);
		rectangularPlateEdge.SetActive(false);
		cylinder.SetActive(false);
		sphere.SetActive(false);
		disk.SetActive(false);
	}
	#endregion

	#region Weighing Scale Room Environment
	private void SetWeighingScaleRoomEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		activityEightManager.SetMissionObjectiveDisplay(!isActive);
		fulcrumCamera.gameObject.SetActive(isActive);
	}

	private void ClearWeighingScaleRoomEnvironmentState()
	{
		SetWeighingScaleRoomEnvironmentState(false);
		weighingScaleControlTerminal.SetInteractable(false);
		equilibriumControlTerminal.SetInteractable(true);
		OpenGate(rebootRoomGate, rebootRoomGateBlocker);
	}
	#endregion

	#region Reboot Room Environment
	private void SetRebootRoomEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		activityEightManager.SetMissionObjectiveDisplay(!isActive);
		weighingScaleCamera.gameObject.SetActive(isActive);
	}

	private void ClearRebootRoomEnvironmentState()
	{
		SetRebootRoomEnvironmentState(false);
		equilibriumControlTerminal.SetInteractable(false);
		rebootButtonGlassCover.SetActive(false);
		rebootButton.SetInteractable(true);
	}
	#endregion

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