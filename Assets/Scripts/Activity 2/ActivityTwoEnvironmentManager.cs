using UnityEngine;

public class ActivityTwoEnvironmentManager : ActivityEnvironmentManager
{
	[Header("Activity Manager")]
	[SerializeField] private ActivityTwoManager activityTwoManager;

	[Header("Views")]
	[SerializeField] private QuantitiesView quantitiesView;

	[Header("Environment Cameras")]
	[SerializeField] private Camera quantitiesTerminalAreaCamera;

	private void Start()
	{
		quantitiesView.OpenViewEvent += () => SetQuantitiesTerminalEnvironmentState(true);
		quantitiesView.QuitViewEvent += () => SetQuantitiesTerminalEnvironmentState(false);
	}

	#region Quantities Terminal Area
	private void SetQuantitiesTerminalEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		// activityTwoManager.SetMissionObjectiveDisplay(!isActive);
		quantitiesTerminalAreaCamera.gameObject.SetActive(isActive);
	}

	#endregion
}
