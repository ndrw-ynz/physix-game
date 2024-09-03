using System;
using UnityEngine;
using UnityEngine.UI;

public class AppleForceSubmissionStatusDisplay : SubmissionStatusDisplay
{
	public static event Action ProceedEvent;

	[Header("Apple Force Status Border Display")]
	[SerializeField] private Image appleForceStatusBorderDisplay;

	[Header("Apple Calculation Display Reference")]
	[SerializeField] private GameObject appleForceCalculationReference;

	private GameObject appleForceCalculationClone;

	protected override void OnEnable()
	{
		base.OnEnable();
		proceedButton.onClick.AddListener(() => ProceedEvent?.Invoke());

		// Create references and attach to associated parents
		appleForceCalculationClone = Instantiate(appleForceCalculationReference);
		appleForceCalculationClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(appleForceCalculationClone, appleForceStatusBorderDisplay.gameObject);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		proceedButton.onClick.RemoveAllListeners();

		Destroy(appleForceCalculationClone);
	}
}