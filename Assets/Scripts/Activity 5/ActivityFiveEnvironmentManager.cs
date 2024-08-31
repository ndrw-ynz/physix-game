using UnityEngine;

public class ActivityFiveEnvironmentManager : ActivityEnvironmentManager
{
	[Header("Cameras")]
	[SerializeField] private Camera appleTreeCamera;

	[Header("Motion - Apple Tree")]
	[SerializeField] private AppleFallMotionAnimate motionApple;

	public override void Start()
	{
		base.Start();

		AppleMotionView.OpenViewEvent += () => DisplayAppleMotion(true);
		AppleMotionView.QuitViewEvent += () => SwitchCameraToPlayerCamera(appleTreeCamera);
		AppleMotionView.QuitViewEvent += () => DisplayAppleMotion(false);
	}

	private void DisplayAppleMotion(bool display)
	{
		motionApple.gameObject.SetActive(display);
	}
}