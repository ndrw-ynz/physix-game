using System;
using UnityEngine;
using UnityEngine.UI;


public class MomentumImpulseForceSubmissionStatusDisplay : SubmissionStatusDisplay
{
	public static event Action ProceedEvent;

	[Header("Momentum-Impulse Force Status Border Displays")]
	[SerializeField] private Image momentumStatusBorderDisplay;
	[SerializeField] private Image impulseStatusBorderDisplay;
	[SerializeField] private Image netForceStatusBorderDisplay;

	[Header("Momentum Calculation Display References")]
	[SerializeField] private GameObject momentumCalculationReference;

	[Header("Impulse Display Reference")]
	[SerializeField] private GameObject impulseDisplayReference;

	[Header("Net Force Calculation Display Reference")]
	[SerializeField] private GameObject netForceCalculatonReference;

	// GameObject clones
	private GameObject momentumCalculationClone;
	private GameObject impulseDisplayClone;
	private GameObject netForceCalculationClone;

	public void UpdateStatusBorderDisplaysFromResult(MomentumImpulseForceAnswerSubmissionResults results)
	{
		// Momentum status border update
		if (results is EasyMomentumImpulseForceAnswerSubmissionResults easySubmissionResults)
		{
			momentumStatusBorderDisplay.color = easySubmissionResults.isChangeInMomentumCorrect == true ? new Color32(175, 255, 155, 255) : new Color32(200, 75, 55, 255);
		} else if (results is MediumHardMomentumImpulseForceAnswerSubmissionResults mediumHardSubmissionResults)
		{
			momentumStatusBorderDisplay.color = (
				mediumHardSubmissionResults.isInitialMomentumCorrect &&
				mediumHardSubmissionResults.isFinalMomentumCorrect &&
				mediumHardSubmissionResults.isChangeInMomentumCorrect
				)
				? 
				new Color32(175, 255, 155, 255) : 
				new Color32(200, 75, 55, 255);
		}
		// Impulse and net force status border update
		impulseStatusBorderDisplay.color = results.isImpulseCorrect == true ? new Color32(175, 255, 155, 255) : new Color32(200, 75, 55, 255);
		netForceStatusBorderDisplay.color = results.isNetForceCorrect == true ? new Color32(175, 255, 155, 255) : new Color32(200, 75, 55, 255);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		proceedButton.onClick.AddListener(() => ProceedEvent?.Invoke());

		// Create references and attach to associated parents.
		// Momentum
		momentumCalculationClone = Instantiate(momentumCalculationReference);
		UIUtilities.CenterChildInParent(momentumCalculationClone, momentumStatusBorderDisplay.gameObject);
		// Impulse
		impulseDisplayClone = Instantiate(impulseDisplayReference);
		UIUtilities.CenterChildInParent(impulseDisplayClone, impulseStatusBorderDisplay.gameObject);
		// Net force
		netForceCalculationClone = Instantiate(netForceCalculatonReference);
		UIUtilities.CenterChildInParent(netForceCalculationClone, netForceStatusBorderDisplay.gameObject);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		Destroy(momentumCalculationClone);
		Destroy(impulseDisplayClone);
		Destroy(netForceCalculationClone);
	}
}