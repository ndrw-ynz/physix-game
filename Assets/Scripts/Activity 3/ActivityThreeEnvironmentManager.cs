using UnityEngine;

public class ActivityThreeEnvironmentManager : ActivityEnvironmentManager
{
	[Header("Activity Manager")]
	[SerializeField] private ActivityThreeManager activityThreeManager;

	[Header("Views")]
	[SerializeField] private GraphsView graphsView;
	[SerializeField] private Kinematics1DView kinematics1DView;

	[Header("Environment Cameras")]
	[SerializeField] private Camera graphsEnvironmentCamera;
	[SerializeField] private Camera kinematics1DEnvironmentCamera;

	[Header("Interactable Terminal Game Objects")]
	[SerializeField] private InteractableViewOpenerObject graphsTerminal;

	[Header("Doors")]
	[SerializeField] private GameObject graphsTransitionDoor;

	private void Start()
	{
		graphsView.OpenViewEvent += () => SetGraphsTerminalEnvironmentState(true);
		graphsView.QuitViewEvent += () => SetGraphsTerminalEnvironmentState(false);

		kinematics1DView.OpenViewEvent += () => SetKinematics1DTerminalEnvironmentState(true);
		kinematics1DView.QuitViewEvent += () => SetKinematics1DTerminalEnvironmentState(false);

		activityThreeManager.GraphsAreaClearEvent += ClearGraphsTerminalEnvironmentState;

		// Close door on start
		graphsTransitionDoor.GetComponent<Animator>().SetBool("door_open", false);
	}

	#region Graphs Terminal Area
	private void SetGraphsTerminalEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		activityThreeManager.SetMissionObjectiveDisplay(!isActive);
		graphsEnvironmentCamera.gameObject.SetActive(isActive);
	}

	private void ClearGraphsTerminalEnvironmentState()
	{
		SetGraphsTerminalEnvironmentState(false);
		graphsTerminal.SetInteractable(false);
		graphsTransitionDoor.GetComponent<Animator>().SetBool("door_open", true);
	}
	#endregion

	#region Kinematics 1D Terminal Area
	private void SetKinematics1DTerminalEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		activityThreeManager.SetMissionObjectiveDisplay(!isActive);
		kinematics1DEnvironmentCamera.gameObject.SetActive(isActive);
	}
	#endregion
}
