using System;
using UnityEngine;
using UnityEngine.UI;

public class DotProductSubmissionStatusDisplay : SubmissionStatusDisplay
{
	public event Action ProceedEvent;

	[Header("Dot Product Status Border Displays")]
	[SerializeField] private Image scalarProductStatusBorderDisplay;
	[SerializeField] private Image dotProductStatusBorderDisplay;

	[Header("Dot Product Display Reference")]
	[SerializeField] private GameObject scalarProductCalculationReference;
	[SerializeField] private GameObject dotProductCalculationReference;

	private GameObject scalarProductCalculationClone;
	private GameObject dotProductCalculationClone;

	public void UpdateStatusBorderDisplaysFromResult(DotProductAnswerSubmissionResults results)
	{
		scalarProductStatusBorderDisplay.color = (
			results.isXCoordScalarProductCorrect &&
			results.isYCoordScalarProductCorrect &&
			results.isZCoordScalarProductCorrect

			) ?
			new Color32(68, 140, 50, 255) :
			new Color32(200, 75, 55, 255);

		dotProductStatusBorderDisplay.color = (
			results.isDotProductCorrect
			) ?
			new Color32(68, 140, 50, 255) :
			new Color32(200, 75, 55, 255);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		proceedButton.onClick.AddListener(() => ProceedEvent?.Invoke());

		// Create references and attach to associated parents
		scalarProductCalculationClone = Instantiate(scalarProductCalculationReference);
		scalarProductCalculationClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(scalarProductCalculationClone, scalarProductStatusBorderDisplay.gameObject);

		dotProductCalculationClone = Instantiate(dotProductCalculationReference);
		dotProductCalculationClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(dotProductCalculationClone, dotProductStatusBorderDisplay.gameObject);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		proceedButton.onClick.RemoveAllListeners();

		Destroy(scalarProductCalculationClone);
		Destroy(dotProductCalculationClone);
	}
}