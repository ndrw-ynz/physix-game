using System;
using UnityEngine;
using UnityEngine.UI;

public class WorkSubmissionStatusDisplay : SubmissionStatusDisplay
{
	public event Action ProceedEvent;

	[Header("Status Border Displays")]
	[SerializeField] private Image forceStatusBorderDisplay;
	[SerializeField] private Image linearWorkStatusBorderDisplay;
	[SerializeField] private Image angularWorkStatusBorderDisplay;

	[Header("Display References")]
	[SerializeField] private GameObject forceCalculationReference;
	[SerializeField] private GameObject linearWorkCalculationReference;
	[SerializeField] private GameObject angularWorkCalculationReference;

	private GameObject forceCalculationClone;
	private GameObject linearWorkCalculationClone;
	private GameObject angularWorkCalculationClone;

	public void UpdateStatusBorderDisplaysFromResult(WorkSubActivityAnswerSubmissionResults results)
	{
		forceStatusBorderDisplay.color = (
			results.isForceCorrect
			) ?
			new Color32(68, 140, 50, 255) :
			new Color32(200, 75, 55, 255);

		linearWorkStatusBorderDisplay.color = (
			results.isWorkCorrect
			) ?
			new Color32(68, 140, 50, 255) :
			new Color32(200, 75, 55, 255);

		angularWorkStatusBorderDisplay.color = (
			results.isWorkCorrect
			) ?
			new Color32(68, 140, 50, 255) :
			new Color32(200, 75, 55, 255);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		proceedButton.onClick.AddListener(() => ProceedEvent?.Invoke());

		// Create references and attach to associated parents
		forceCalculationClone = Instantiate(forceCalculationReference);
		forceCalculationClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(forceCalculationClone, forceStatusBorderDisplay.gameObject);

		if (linearWorkCalculationReference.activeSelf)
		{
			linearWorkStatusBorderDisplay.gameObject.SetActive(true);
			angularWorkStatusBorderDisplay.gameObject.SetActive(false);

			linearWorkCalculationClone = Instantiate(linearWorkCalculationReference);
			linearWorkCalculationClone.gameObject.SetActive(true);
			UIUtilities.CenterChildInParent(linearWorkCalculationClone, linearWorkStatusBorderDisplay.gameObject);
		} else
		{
			linearWorkStatusBorderDisplay.gameObject.SetActive(false);
			angularWorkStatusBorderDisplay.gameObject.SetActive(true);

			angularWorkCalculationClone = Instantiate(angularWorkCalculationReference);
			angularWorkCalculationClone.gameObject.SetActive(true);
			UIUtilities.CenterChildInParent(angularWorkCalculationClone, angularWorkStatusBorderDisplay.gameObject);
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		proceedButton.onClick.RemoveAllListeners();

		Destroy(forceCalculationClone);
		Destroy(linearWorkCalculationClone);
		Destroy(angularWorkCalculationClone);
	}
}