using UnityEngine;

public class ActivityTwoEnvironmentManager : ActivityEnvironmentManager
{
	[Header("Activity Manager")]
	[SerializeField] private ActivityTwoManager activityTwoManager;

	[Header("Views")]
	[SerializeField] private QuantitiesView quantitiesView;
	[SerializeField] private CartesianComponentsView cartesianComponentsView;
	[SerializeField] private VectorAdditionView vectorAdditionView;
	[SerializeField] private EndConsoleView endConsoleView;

	[Header("Environment Cameras")]
	[SerializeField] private Camera quantitiesTerminalAreaCamera;
	[SerializeField] private Camera cartesianComponentsTerminalAreaCamera;
	[SerializeField] private Camera vectorAdditionTerminalAreaCamera;
	[SerializeField] private Camera endTerminalAreaCamera;

	[Header("Interactable Terminal Game Objects")]
	[SerializeField] private InteractableViewOpenerObject quantitiesTerminal;
	[SerializeField] private InteractableViewOpenerObject cartesianComponentsTerminal;
	[SerializeField] private InteractableViewOpenerObject vectorAdditionTerminal;
	[SerializeField] private InteractableViewOpenerObject endConsoleTerminal;

	private void Start()
	{
		quantitiesView.OpenViewEvent += () => SetQuantitiesTerminalEnvironmentState(true);
		quantitiesView.QuitViewEvent += () => SetQuantitiesTerminalEnvironmentState(false);

		cartesianComponentsView.OpenViewEvent += () => SetCartesianComponentsTerminalEnvironmentState(true);
		cartesianComponentsView.QuitViewEvent += () => SetCartesianComponentsTerminalEnvironmentState(false);

		vectorAdditionView.OpenViewEvent += () => SetVectorAdditionTerminalEnvironmentState(true);
		vectorAdditionView.QuitViewEvent += () => SetVectorAdditionTerminalEnvironmentState(false);

		endConsoleView.OpenViewEvent += SetupEndEnvironmentState;

		activityTwoManager.QuantitiesAreaClearEvent += ClearQuantitiesTerminalEnvironmentState;
		activityTwoManager.CartesianComponentsAreaClearEvent += ClearCartesianComponentsTerminalEnvironmentState;
		activityTwoManager.VectorAdditionAreaClearEvent += ClearVectorAdditionTerminalEnvironmentState;
	}

	#region Quantities Terminal Area
	private void SetQuantitiesTerminalEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		activityTwoManager.SetMissionObjectiveDisplay(!isActive);
		quantitiesTerminalAreaCamera.gameObject.SetActive(isActive);
	}

	private void ClearQuantitiesTerminalEnvironmentState()
	{
		SetQuantitiesTerminalEnvironmentState(false);
		quantitiesTerminal.SetInteractable(false);
		cartesianComponentsTerminal.SetInteractable(true);
	}

	#endregion

	#region Cartesian Components Terminal Area
	private void SetCartesianComponentsTerminalEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		activityTwoManager.SetMissionObjectiveDisplay(!isActive);
		cartesianComponentsTerminalAreaCamera.gameObject.SetActive(isActive);
	}

	private void ClearCartesianComponentsTerminalEnvironmentState()
	{
		SetCartesianComponentsTerminalEnvironmentState(false);
		cartesianComponentsTerminal.SetInteractable(false);
		vectorAdditionTerminal.SetInteractable(true);
	}
	#endregion

	#region Vector Addition Terminal Area
	private void SetVectorAdditionTerminalEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		activityTwoManager.SetMissionObjectiveDisplay(!isActive);
		vectorAdditionTerminalAreaCamera.gameObject.SetActive(isActive);
	}

	private void ClearVectorAdditionTerminalEnvironmentState()
	{
		SetVectorAdditionTerminalEnvironmentState(false);
		vectorAdditionTerminal.SetInteractable(false);
		endConsoleTerminal.SetInteractable(true);
	}
	#endregion

	private void SetupEndEnvironmentState()
	{
		SetPlayerActivityState(false);
		activityTwoManager.SetMissionObjectiveDisplay(false);
		endTerminalAreaCamera.gameObject.SetActive(true);
	}
}
