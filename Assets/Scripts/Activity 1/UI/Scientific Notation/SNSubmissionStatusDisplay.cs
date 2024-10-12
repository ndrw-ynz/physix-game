using System;
using UnityEngine;
using UnityEngine.UI;

public class SNSubmissionStatusDisplay : SubmissionStatusDisplay
{
	public event Action ProceedEvent;

	[Header("Field Status Border Display")]
	[SerializeField] private Image coefficientStatusBorderDisplay;
	[SerializeField] private Image exponentStatusBorderDisplay;

	[Header("Force Calculation Display Reference")]
	[SerializeField] private GameObject coefficientFieldReference;
	[SerializeField] private GameObject exponentFieldReference;

	private GameObject coefficientFieldClone;
	private GameObject exponentFieldClone;

	public void UpdateStatusBorderDisplaysFromResult(ScientificNotationAnswerSubmissionResults results)
	{
		coefficientStatusBorderDisplay.color = (
			results.isCoefficientValueCorrect
			) ?
			new Color32(175, 255, 155, 255) :
			new Color32(200, 75, 55, 255);

		exponentStatusBorderDisplay.color = (
			results.isExponentValueCorrect
			) ?
			new Color32(175, 255, 155, 255) :
			new Color32(200, 75, 55, 255);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		proceedButton.onClick.AddListener(() => ProceedEvent?.Invoke());

		// Create references and attach to associated parents
		coefficientFieldClone = Instantiate(coefficientFieldReference);
		coefficientFieldClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(coefficientFieldClone, coefficientStatusBorderDisplay.gameObject);

		exponentFieldClone = Instantiate(exponentFieldReference);
		exponentFieldClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(exponentFieldClone, exponentStatusBorderDisplay.gameObject);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		proceedButton.onClick.RemoveAllListeners();

		Destroy(coefficientFieldClone);
		Destroy(exponentFieldClone);
	}
}