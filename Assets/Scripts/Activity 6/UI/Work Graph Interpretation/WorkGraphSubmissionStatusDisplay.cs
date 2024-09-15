using System;
using UnityEngine;
using UnityEngine.UI;

public class WorkGraphSubmissionStatusDisplay : SubmissionStatusDisplay
{
	public event Action ProceedEvent;

	[Header("Status Border Displays")]
	[SerializeField] private Image constantWorkStatusBorderDisplay;
	[SerializeField] private Image linearWorkStatusBorderDisplay;

	[Header("Display References")]
	[SerializeField] private GameObject constantWorkCalculationReference;
	[SerializeField] private GameObject linearWorkCalculationReference;

	private GameObject constantWorkCalculationClone;
	private GameObject linearWorkCalculationClone;

	public void UpdateStatusBorderDisplaysFromResult(bool result)
	{
		constantWorkStatusBorderDisplay.color = (
			result
			) ?
			new Color32(68, 140, 50, 255) :
			new Color32(200, 75, 55, 255);

		linearWorkStatusBorderDisplay.color = (
			result
			) ?
			new Color32(68, 140, 50, 255) :
			new Color32(200, 75, 55, 255);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		proceedButton.onClick.AddListener(() => ProceedEvent?.Invoke());

		// Create references and attach to associated parents
		if (constantWorkCalculationReference.activeSelf)
		{
			constantWorkStatusBorderDisplay.gameObject.SetActive(true);
			linearWorkStatusBorderDisplay.gameObject.SetActive(false);

			constantWorkCalculationClone = Instantiate(constantWorkCalculationReference);
			constantWorkCalculationClone.gameObject.SetActive(true);
			UIUtilities.CenterChildInParent(constantWorkCalculationClone, constantWorkStatusBorderDisplay.gameObject);
		}
		else
		{
			constantWorkStatusBorderDisplay.gameObject.SetActive(false);
			linearWorkStatusBorderDisplay.gameObject.SetActive(true);

			linearWorkCalculationClone = Instantiate(linearWorkCalculationReference);
			linearWorkCalculationClone.gameObject.SetActive(true);
			UIUtilities.CenterChildInParent(linearWorkCalculationClone, linearWorkStatusBorderDisplay.gameObject);
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		proceedButton.onClick.RemoveAllListeners();

		Destroy(constantWorkCalculationClone);
		Destroy(linearWorkCalculationClone);
	}
}