using System;
using UnityEngine;
using UnityEngine.UI;

public class CartesianComponentsSubmissionStatusDisplay : SubmissionStatusDisplay
{
	public event Action ProceedEvent;

	[Header("Cartesian Components Status Border Displays")]
	[SerializeField] private Image xComponentStatusBorderDisplay;
	[SerializeField] private Image yComponentStatusBorderDisplay;

	[Header("Component Calculations References")]
	[SerializeField] private GameObject xComponentCalcReference;
	[SerializeField] private GameObject yComponentCalcReference;

	private GameObject xComponentCalcClone;
	private GameObject yComponentCalcClone;

	public void UpdateStatusBorderDisplaysFromResults(CartesianComponentsAnswerSubmissionResults results)
	{
		xComponentStatusBorderDisplay.color = results.isVectorXComponentCorrect == true ? new Color32(175, 255, 155, 255) : new Color32(200, 75, 55, 255);
		yComponentStatusBorderDisplay.color = results.isVectorYComponentCorrect == true ? new Color32(175, 255, 155, 255) : new Color32(200, 75, 55, 255);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		proceedButton.onClick.AddListener(() => ProceedEvent?.Invoke());

		// Create references and attach to associated parents
		// Vector x-component
		xComponentCalcClone = Instantiate(xComponentCalcReference);
		xComponentCalcClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(xComponentCalcClone, xComponentStatusBorderDisplay.gameObject);

		// Vector y-component
		yComponentCalcClone = Instantiate(yComponentCalcReference);
		yComponentCalcClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(yComponentCalcClone, yComponentStatusBorderDisplay.gameObject);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		proceedButton.onClick.RemoveAllListeners();

		Destroy(xComponentCalcClone);
		Destroy(yComponentCalcClone);
	}
}
