using UnityEngine;

public class ActivityThreeEnvironmentManager : ActivityEnvironmentManager
{
	[Header("Activity Manager")]
	[SerializeField] private ActivityThreeManager activityThreeManager;

	[Header("Views")]
	[SerializeField] private GraphsView graphsView;

	[Header("Environment Cameras")]
	[SerializeField] private Camera graphsEnvironmentCamera;

	[Header("Interactable Terminal Game Objects")]
	[SerializeField] private InteractableViewOpenerObject graphsTerminal;

	[Header("Doors")]
	[SerializeField] private GameObject graphsTransitionDoor;

	private void Start()
	{
		graphsView.OpenViewEvent += () => SetGraphsTerminalEnvironmentState(true);
		graphsView.QuitViewEvent += () => SetGraphsTerminalEnvironmentState(false);

		activityThreeManager.GraphsAreaClearEvent += ClearGraphsTerminalEnvironmentState;

		// Close door on start
		graphsTransitionDoor.GetComponent<Animator>().SetBool("door_open", false);
	}

	#region Graphs Terminal Area
	private void SetGraphsTerminalEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		// activityThreeManager.SetMissionObjectiveDisplay(!isActive);
		graphsEnvironmentCamera.gameObject.SetActive(isActive);
	}

	private void ClearGraphsTerminalEnvironmentState()
	{
		SetGraphsTerminalEnvironmentState(false);
		graphsTerminal.SetInteractable(false);
		graphsTransitionDoor.GetComponent<Animator>().SetBool("door_open", true);
	}
	#endregion
}
