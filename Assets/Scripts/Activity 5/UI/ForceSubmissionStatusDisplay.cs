using System;
using UnityEngine;
using UnityEngine.UI;

public class ForceSubmissionStatusDisplay : SubmissionStatusDisplay
{
	public event Action ProceedEvent;

	[Header("Force Status Border Display")]
	[SerializeField] private Image forceStatusBorderDisplay;

	[Header("Force Calculation Display Reference")]
	[SerializeField] private GameObject forceCalculationReference;

	private GameObject forceCalculationClone;

	protected override void OnEnable()
	{
		base.OnEnable();
		proceedButton.onClick.AddListener(() => ProceedEvent?.Invoke());

		// Create references and attach to associated parents
		forceCalculationClone = Instantiate(forceCalculationReference);
		forceCalculationClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(forceCalculationClone, forceStatusBorderDisplay.gameObject);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		proceedButton.onClick.RemoveAllListeners();

		Destroy(forceCalculationClone);
	}
}