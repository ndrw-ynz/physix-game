using UnityEngine.UI;
using UnityEngine;
using System;

public class EquilibriumSubmissionStatusDisplay : SubmissionStatusDisplay
{
	public static event Action ProceedEvent;

	[Header("Equilibrium Status Border Displays")]
	[SerializeField] private Image forceEquilibriumStatusBorderDisplay;
	[SerializeField] private Image torqueEquilibriumStatusBorderDisplay;
	[SerializeField] private Image equilibriumTypeStatusBorderDisplay;

	[Header("Force Equilibrium Calculation Display Reference")]
	[SerializeField] private GameObject forceEquilibriumCalculationReference;

	[Header("Torque Equilibrium Calculation Display Reference")]
	[SerializeField] private GameObject torqueEquilibriumCalculationReference;

	[Header("Equilibrium Type Selection Display Reference")]
	[SerializeField] private GameObject equilibriumTypeSelectionReference;

	[Header("Interactive Buttons")]
	[SerializeField] private Button leftPageButton;
	[SerializeField] private Button rightPageButton;

	// GameObject clones
	private GameObject forceEquilibriumCalculationClone;
	private GameObject torqueEquilibriumCalculationClone;
	private GameObject equilibriumTypeSelectionClone;

	public void DisplayLeftPageInfo()
	{
		leftPageButton.gameObject.SetActive(false);
		rightPageButton.gameObject.SetActive(true);

		forceEquilibriumStatusBorderDisplay.gameObject.SetActive(true);

		torqueEquilibriumStatusBorderDisplay.gameObject.SetActive(false);
		equilibriumTypeStatusBorderDisplay.gameObject.SetActive(false);
	}

	public void DisplayRightPageInfo()
	{
		leftPageButton.gameObject.SetActive(true);
		rightPageButton.gameObject.SetActive(false);

		forceEquilibriumStatusBorderDisplay.gameObject.SetActive(false);

		torqueEquilibriumStatusBorderDisplay.gameObject.SetActive(true);
		equilibriumTypeStatusBorderDisplay.gameObject.SetActive(true);
	}

	public void UpdateStatusBorderDisplaysFromResult(EquilibriumAnswerSubmissionResults results)
	{
		forceEquilibriumStatusBorderDisplay.color = (
			results.isSummationOfDownwardForcesCorrect == true &&
			results.isUpwardForceCorrect == true &&
			results.isSummationOfTotalForcesCorrect
			) ?
			new Color32(175, 255, 155, 255) :
			new Color32(200, 75, 55, 255);

		torqueEquilibriumStatusBorderDisplay.color = (
			results.isCounterclockwiseTorqueCorrect == true &&
			results.isClockwiseTorqueCorrect == true
			) ?
			new Color32(175, 255, 155, 255) :
			new Color32(200, 75, 55, 255);

		equilibriumTypeStatusBorderDisplay.color = (
			results.isEquilibriumTypeCorrect == true
			) ?
			new Color32(175, 255, 155, 255) :
			new Color32(200, 75, 55, 255);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		proceedButton.onClick.AddListener(() => ProceedEvent?.Invoke());

		// Create references and attach to associated parents
		forceEquilibriumCalculationClone = Instantiate(forceEquilibriumCalculationReference);
		forceEquilibriumCalculationClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(forceEquilibriumCalculationClone, forceEquilibriumStatusBorderDisplay.gameObject);

		torqueEquilibriumCalculationClone = Instantiate(torqueEquilibriumCalculationReference);
		torqueEquilibriumCalculationClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(torqueEquilibriumCalculationClone, torqueEquilibriumStatusBorderDisplay.gameObject);

		equilibriumTypeSelectionClone = Instantiate(equilibriumTypeSelectionReference);
		equilibriumTypeSelectionClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(equilibriumTypeSelectionClone, equilibriumTypeStatusBorderDisplay.gameObject);

		// Set default display view
		DisplayLeftPageInfo();
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		proceedButton.onClick.RemoveAllListeners();

		Destroy(forceEquilibriumCalculationClone);
		Destroy(torqueEquilibriumCalculationClone);
		Destroy(equilibriumTypeSelectionClone);
	}
}