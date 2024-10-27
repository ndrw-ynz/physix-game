using UnityEngine;

public class ActivityThreeEnvironmentManager : ActivityEnvironmentManager
{
	[Header("Views")]
	[SerializeField] private GraphsView graphsView;

	[Header("Environment Cameras")]
	[SerializeField] private Camera graphsEnvironmentCamera;

	private void Start()
	{
		graphsView.OpenViewEvent += () => SetGraphsTerminalEnvironmentState(true);
		graphsView.QuitViewEvent += () => SetGraphsTerminalEnvironmentState(false);
	}

	#region Graphs Terminal Area
	private void SetGraphsTerminalEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		// activityThreeManager.SetMissionObjectiveDisplay(!isActive);
		graphsEnvironmentCamera.gameObject.SetActive(isActive);
	}
	#endregion
}
