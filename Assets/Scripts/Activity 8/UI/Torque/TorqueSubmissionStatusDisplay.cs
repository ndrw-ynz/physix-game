using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the result based from submitted Torque answers.
/// </summary>
public class TorqueSubmissionStatusDisplay : SubmissionStatusDisplay
{
	public static event Action ProceedEvent;

	[Header("Torque Status Border Displays")]
	[SerializeField] private Image torqueMagnitudeStatusBorderDisplay;
	[SerializeField] private Image torqueDirectionStatusBorderDisplay;

	[Header("Torque Magnitude Calculation Reference")]
	[SerializeField] private GameObject torqueMagnitudeCalculationReference;

	[Header("Torque Direction Selection Reference")]
	[SerializeField] private GameObject torqueDirectionSelectionReference;

	// GameObject clones
	private GameObject torqueMagnitudeCalculationClone;
	private GameObject torqueDirectionSelectionClone;

	/// <summary>
	/// Updates the status borders displayed in <c>TorqueSubmissionStatusDisplay</c> 
	/// based from <c>TorqueAnswerSubmissionResults</c>.
	/// </summary>
	/// <param name="results"></param>
	public void UpdateStatusBorderDisplaysFromResult(TorqueAnswerSubmissionResults results)
	{
		torqueMagnitudeStatusBorderDisplay.color = (
			results.isTorqueMagnitudeCorrect
			) ?
			new Color32(175, 255, 155, 255) :
			new Color32(200, 75, 55, 255);

		torqueDirectionStatusBorderDisplay.color = (
			results.isTorqueDirectionCorrect
			) ?
			new Color32(175, 255, 155, 255) :
			new Color32(200, 75, 55, 255);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		proceedButton.onClick.AddListener(() => ProceedEvent?.Invoke());

		// Create references and attach to associated parents
		torqueMagnitudeCalculationClone = Instantiate(torqueMagnitudeCalculationReference);
		UIUtilities.CenterChildInParent(torqueMagnitudeCalculationClone, torqueMagnitudeStatusBorderDisplay.gameObject);

		torqueDirectionSelectionClone = Instantiate(torqueDirectionSelectionReference);
		UIUtilities.CenterChildInParent(torqueDirectionSelectionClone, torqueDirectionStatusBorderDisplay.gameObject);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		Destroy(torqueMagnitudeCalculationClone);
		Destroy(torqueDirectionSelectionClone);
	}
}