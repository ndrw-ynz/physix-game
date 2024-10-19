using System;
using UnityEngine;
using UnityEngine.UI;

public class ErrorsSubmissionStatusDisplay : SubmissionStatusDisplay
{
	public event Action ProceedEvent;

	[Header("Errors Status Border Displays")]
	[SerializeField] private Image errorsStatusBorderDisplay;

	[Header("Error Type Buttons Reference")]
	[SerializeField] private GameObject errorTypeButtonsReference;

	private GameObject errorTypeButtonsClone;

	public void UpdateStatusBorderDisplayFromResult(bool result)
	{
		errorsStatusBorderDisplay.color = result == true ? new Color32(175, 255, 155, 255) : new Color32(200, 75, 55, 255);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		proceedButton.onClick.AddListener(() => ProceedEvent?.Invoke());

		// Create references and attach to associated parents
		errorTypeButtonsClone = Instantiate(errorTypeButtonsReference);
		errorTypeButtonsClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(errorTypeButtonsClone, errorsStatusBorderDisplay.gameObject);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		proceedButton.onClick.RemoveAllListeners();

		Destroy(errorTypeButtonsClone);
	}
}
