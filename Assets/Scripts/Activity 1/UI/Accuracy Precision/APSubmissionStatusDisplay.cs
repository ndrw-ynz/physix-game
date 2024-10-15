using System;
using UnityEngine;
using UnityEngine.UI;

public class APSubmissionStatusDisplay : SubmissionStatusDisplay
{
	public static event Action ProceedEvent;

	[Header("Accuracy Precision Status Border Displays")]
	[SerializeField] private Image APStatusBorderDisplay;

	[Header("AP Graph Type Buttons Reference")]
	[SerializeField] private GameObject APGraphTypeButtonsReference;

	private GameObject APGraphTypeButtonsClone;

	public void UpdateStatusBorderDisplayFromResult(bool result)
	{
		APStatusBorderDisplay.color = result == true ? new Color32(175, 255, 155, 255) : new Color32(200, 75, 55, 255);

	}

	protected override void OnEnable()
	{
		base.OnEnable();
		proceedButton.onClick.AddListener(() => ProceedEvent?.Invoke());

		// Create references and attach to associated parents
		APGraphTypeButtonsClone = Instantiate(APGraphTypeButtonsReference);
		APGraphTypeButtonsClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(APGraphTypeButtonsClone, APStatusBorderDisplay.gameObject);
	}


	protected override void OnDisable()
	{
		base.OnDisable();
		proceedButton.onClick.RemoveAllListeners();

		Destroy(APGraphTypeButtonsClone);
	}
}
