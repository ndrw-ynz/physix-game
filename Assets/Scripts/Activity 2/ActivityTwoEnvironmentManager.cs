using UnityEngine;

public class ActivityTwoEnvironmentManager : ActivityEnvironmentManager
{
	[Header("Activity Manager")]
	[SerializeField] private ActivityTwoManager activityTwoManager;

	[Header("Views")]
	[SerializeField] private QuantitiesView quantitiesView;
	[SerializeField] private CartesianComponentsView cartesianComponentsView;

	[Header("Environment Cameras")]
	[SerializeField] private Camera quantitiesTerminalAreaCamera;
	[SerializeField] private Camera cartesianComponentsTerminalAreaCamera;

	[Header("Interactable Terminal Game Objects")]
	[SerializeField] private InteractableViewOpenerObject quantitiesTerminal;
	[SerializeField] private InteractableViewOpenerObject cartesianComponentsTerminal;

	private void Start()
	{
		quantitiesView.OpenViewEvent += () => SetQuantitiesTerminalEnvironmentState(true);
		quantitiesView.QuitViewEvent += () => SetQuantitiesTerminalEnvironmentState(false);

		cartesianComponentsView.OpenViewEvent += () => SetCartesianComponentsTerminalEnvironmentState(true);
		cartesianComponentsView.QuitViewEvent += () => SetCartesianComponentsTerminalEnvironmentState(false);

		activityTwoManager.QuantitiesAreaClearEvent += ClearQuantitiesTerminalEnvironmentState;
	}

	#region Quantities Terminal Area
	private void SetQuantitiesTerminalEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		// activityTwoManager.SetMissionObjectiveDisplay(!isActive);
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
		// activityTwoManager.SetMissionObjectiveDisplay(!isActive);
		cartesianComponentsTerminalAreaCamera.gameObject.SetActive(isActive);
	}
	#endregion
}
