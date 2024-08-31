using System;
using UnityEngine;

public class AppleMotionView : MonoBehaviour
{
	public static event Action OpenViewEvent;
	public static event Action QuitViewEvent;
	public static event Action<ForceTypeAnswerSubmission> SubmitForceTypesAnswerEvent;

	[Header("Force Type Containers")]
	[SerializeField] private ForceTypeContainer upForceContainer;
	[SerializeField] private ForceTypeContainer downForceContainer;
	[SerializeField] private ForceTypeContainer leftForceContainer;
	[SerializeField] private ForceTypeContainer rightForceContainer;


	private void OnEnable()
	{
		OpenViewEvent?.Invoke();
	}

	public void OnSubmitForceTypesButtonClick()
	{
		ForceTypeAnswerSubmission submission = new ForceTypeAnswerSubmission(
			forceObjectMotionType: ForceObjectMotionType.Apple_OnBranch,
			upForceType: upForceContainer.GetCurrentForceType(),
			downForceType: downForceContainer.GetCurrentForceType(),
			leftForceType: leftForceContainer.GetCurrentForceType(),
			rightForceType: rightForceContainer.GetCurrentForceType()
			);

		SubmitForceTypesAnswerEvent?.Invoke(submission);
	}

	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitViewEvent?.Invoke();
	}
}