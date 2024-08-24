using System;
using UnityEngine;
using UnityEngine.UI;

public class GravitySubmissionStatusDisplay : SubmissionStatusDisplay
{
	public static event Action ProceedEvent;

	[Header("Gravity Status Border Displays")]
	[SerializeField] private Image gravitationalForceStatusBorderDisplay;
	[SerializeField] private Image GPEStatusBorderDisplay;

    [Header("Gravitational Force Calculation Display Reference")]
    [SerializeField] private GameObject gravitationalForceCalculationReference;

	[Header("GPE Calculation Display Reference")]
	[SerializeField] private GameObject GPECalculationReference;

	[Header("Interactive Buttons")]
	[SerializeField] private Button leftPageButton;
	[SerializeField] private Button rightPageButton;

	// GameObject clones
	private GameObject gravitationalForceCalculationClone;
	private GameObject GPECalculationClone;

	public void DisplayLeftPageInfo()
	{
		leftPageButton.gameObject.SetActive(false);
		rightPageButton.gameObject.SetActive(true);

		gravitationalForceStatusBorderDisplay.gameObject.SetActive(true);

		GPEStatusBorderDisplay.gameObject.SetActive(false);
	}

	public void DisplayRightPageInfo()
	{
		leftPageButton.gameObject.SetActive(true);
		rightPageButton.gameObject.SetActive(false);

		gravitationalForceStatusBorderDisplay.gameObject.SetActive(false);

		GPEStatusBorderDisplay.gameObject.SetActive(true);
	}

	public void UpdateStatusBorderDisplaysFromResult(GravityAnswerSubmissionResults results)
	{
		gravitationalForceStatusBorderDisplay.color = (
			results.isGravitationalForceCorrect == true
			) ?
			new Color32(175, 255, 155, 255) :
			new Color32(200, 75, 55, 255);

		GPEStatusBorderDisplay.color = (
			results.isGPECorrect == true
			) ?
			new Color32(175, 255, 155, 255) :
			new Color32(200, 75, 55, 255);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		proceedButton.onClick.AddListener(() => ProceedEvent?.Invoke());

		// Create references and attach to associated parents
		gravitationalForceCalculationClone = Instantiate(gravitationalForceCalculationReference);
		gravitationalForceCalculationClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(gravitationalForceCalculationClone, gravitationalForceStatusBorderDisplay.gameObject);

		GPECalculationClone = Instantiate(GPECalculationReference);
		GPECalculationClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(GPECalculationClone, GPEStatusBorderDisplay.gameObject);

		// Set default display view
		DisplayLeftPageInfo();
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		proceedButton.onClick.RemoveAllListeners();

		Destroy(gravitationalForceCalculationClone);
		Destroy(GPECalculationClone);
	}
}