using System;
using UnityEngine;
using UnityEngine.UI;

public class Kinematics1DSubmissionStatusDisplay : SubmissionStatusDisplay
{
	public event Action ProceedEvent;

	[Header("Graphs Status Border Displays")]
	[SerializeField] private Image kinematics1DStatusBorderDisplay;

	[Header("1D Kinematics Formula Reference")]
	[SerializeField] private GameObject accelerationFormulaReference;
	[SerializeField] private GameObject totalDepthFormulaReference;

	private GameObject calculationFormulaClone;

	public void UpdateStatusBorderDisplayFromResult(bool result)
	{
		kinematics1DStatusBorderDisplay.color = result ? new Color32(175, 255, 155, 255) : new Color32(200, 75, 55, 255);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		proceedButton.onClick.AddListener(() => ProceedEvent?.Invoke());

		// Create references and attach to associated parents
		if (accelerationFormulaReference.activeSelf)
		{
			// Acceleration calculation
			calculationFormulaClone = Instantiate(accelerationFormulaReference);
			calculationFormulaClone.gameObject.SetActive(true);
			UIUtilities.CenterChildInParent(calculationFormulaClone, kinematics1DStatusBorderDisplay.gameObject);
		} else
		{
			// Total depth calculation
			calculationFormulaClone = Instantiate(totalDepthFormulaReference);
			calculationFormulaClone.gameObject.SetActive(true);
			UIUtilities.CenterChildInParent(calculationFormulaClone, kinematics1DStatusBorderDisplay.gameObject);
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		proceedButton.onClick.RemoveAllListeners();

		Destroy(calculationFormulaClone);
	}
}
