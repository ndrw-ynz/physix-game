using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the result based from submitted Moment of Inertia answers.
/// </summary>
public class MomentOfInertiaSubmissionStatusDisplay : SubmissionStatusDisplay
{
	public event Action ProceedEvent;

	[Header("Moment of Inertia Status Border Displays")]
	[SerializeField] private Image objectTypeStatusBorderDisplay;
	[SerializeField] private Image momentOfInertiaStatusBorderDisplay;

	[Header("Object Type Selection Reference")]
	[SerializeField] private GameObject objectTypeSelectionReference;

	[Header("Moment of Inertia Calculation Reference")]
	[SerializeField] private GameObject momentOfInertiaCalculationReference;

	// GameObject clones
	private GameObject objectTypeSelectionClone;
	private GameObject momentOfInertiaCalculationClone;

	/// <summary>
	/// Updates the status borders displayed in <c>MomentOfInertiaSubmissionStatusDisplay</c> 
	/// based from <c>MomentOfInertiaAnswerSubmissionResults</c>.
	/// </summary>
	/// <param name="results"></param>
	public void UpdateStatusBorderDisplaysFromResult(MomentOfInertiaAnswerSubmissionResults results)
	{
		objectTypeStatusBorderDisplay.color = (
			results.isInertiaObjectTypeCorrect
			) ?
			new Color32(175, 255, 155, 255) :
			new Color32(200, 75, 55, 255);

		momentOfInertiaStatusBorderDisplay.color = (
			results.isMomentOfInertiaCorrect
			) ?
			new Color32(175, 255, 155, 255) :
			new Color32(200, 75, 55, 255);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		proceedButton.onClick.AddListener(() => ProceedEvent?.Invoke());

		// Create references and attach to associated parents
		objectTypeSelectionClone = Instantiate(objectTypeSelectionReference);
		UIUtilities.CenterChildInParent(objectTypeSelectionClone, objectTypeStatusBorderDisplay.gameObject);

		momentOfInertiaCalculationClone = Instantiate(momentOfInertiaCalculationReference);
		UIUtilities.CenterChildInParent(momentOfInertiaCalculationClone, momentOfInertiaStatusBorderDisplay.gameObject);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		Destroy(objectTypeSelectionClone);
		Destroy(momentOfInertiaCalculationClone);
	}
}