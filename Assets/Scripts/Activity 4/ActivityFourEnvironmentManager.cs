using UnityEngine;

public class ActivityFourEnvironmentManager : ActivityEnvironmentManager
{
	[Header("Activity Manager")]
	[SerializeField] private ActivityFourManager activityFourManager;

	[Header("Views")]
	[SerializeField] private ProjectileMotionView projectileMotionView;
	[SerializeField] private CircularMotionView circularMotionView;

	[Header("Environment Cameras")]
	[SerializeField] private Camera projectileTerminalEnvCamera;
	[SerializeField] private Camera circularMotionTerminalEnvCamera;

	[Header("Interactable Terminal Game Objects")]
	[SerializeField] private InteractableViewOpenerObject projectileMotionTerminal;
	[SerializeField] private InteractableViewOpenerObject circularMotionTerminal;

	[Header("Environment Area Game Objects")]
	[SerializeField] private GameObject satelliteLaunchArea;
	[SerializeField] private GameObject outerSpaceArea;

	private void Start()
	{
		projectileMotionView.OpenViewEvent += () => SetProjectileTerminalEnvironmentState(true);
		projectileMotionView.QuitViewEvent += () => SetProjectileTerminalEnvironmentState(false);

		circularMotionView.OpenViewEvent += () => SetCircularMotionTerminalEnvironmentState(true);
		circularMotionView.QuitViewEvent += () => SetCircularMotionTerminalEnvironmentState(false);

		activityFourManager.ProjectileMotionTerminalClearEvent += ClearProjectileTerminalEnvironmentState;
	}

	#region Projectile Motion
	private void SetProjectileTerminalEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		activityFourManager.SetMissionObjectiveDisplay(!isActive);
		projectileTerminalEnvCamera.gameObject.SetActive(isActive);
		satelliteLaunchArea.SetActive(isActive);
	}

	private void ClearProjectileTerminalEnvironmentState()
	{
		SetProjectileTerminalEnvironmentState(false);
		projectileMotionTerminal.SetInteractable(false);
		circularMotionTerminal.SetInteractable(true);
	}
	#endregion

	#region Circular Motion
	private void SetCircularMotionTerminalEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		activityFourManager.SetMissionObjectiveDisplay(!isActive);
		circularMotionTerminalEnvCamera.gameObject.SetActive(isActive);
		outerSpaceArea.SetActive(isActive);
	}
	#endregion
}
