using UnityEngine;

public class ActivityFiveEnvironmentManager : ActivityEnvironmentManager
{
	[Header("Cameras")]
	[SerializeField] private Camera appleTreeCamera;

	public override void Start()
	{
		base.Start();

		AppleMotionView.QuitViewEvent += () => SwitchCameraToPlayerCamera(appleTreeCamera);
	}
}