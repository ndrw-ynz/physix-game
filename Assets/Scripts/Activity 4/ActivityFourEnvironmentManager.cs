using UnityEngine;

public class ActivityFourEnvironmentManager : ActivityEnvironmentManager
{
	[Header("Activity Manager")]
	[SerializeField] private ActivityFourManager activityFourManager;

	[Header("Views")]
	[SerializeField] private ProjectileMotionView projectileMotionView;

	[Header("Environment Cameras")]
	[SerializeField] private Camera projectileTerminalEnvCamera;

	[Header("Interactable Terminal Game Objects")]
	[SerializeField] private InteractableViewOpenerObject projectileMotionTerminal;

	private void Start()
	{
		projectileMotionView.OpenViewEvent += () => SetProjectileTerminalEnvironmentState(true);
		projectileMotionView.QuitViewEvent += () => SetProjectileTerminalEnvironmentState(false);
	}

	#region Projectile Motion
	private void SetProjectileTerminalEnvironmentState(bool isActive)
	{
		SetPlayerActivityState(!isActive);
		// activityFourManager.SetMissionObjectiveDisplay(!isActive);
		projectileTerminalEnvCamera.gameObject.SetActive(isActive);
	}
	#endregion
}
