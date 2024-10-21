using System;
using UnityEngine;
using UnityEngine.UI;

public class QuantitiesSubmissionStatusDisplay : SubmissionStatusDisplay
{
	public event Action ProceedEvent;

	[Header("Quantities Status Border Displays")]
	[SerializeField] private Image unsolvedQuantitiesStatusBorderDisplay;
	[SerializeField] private Image scalarQuantitiesStatusBorderDisplay;
	[SerializeField] private Image vectorQuantitiesStatusBorderDisplay;

	[Header("Quantities Container Reference")]
	[SerializeField] private GameObject unsolvedQuantitiesReference;
	[SerializeField] private GameObject scalarQuantitiesReference;
	[SerializeField] private GameObject vectorQuantitiesReference;

	private GameObject unsolvedQuantitiesClone;
	private GameObject scalarQuantitiesClone;
	private GameObject vectorQuantitiesClone;

	public void UpdateStatusBorderDisplayFromResults(QuantitiesAnswerSubmissionResults results)
	{
		unsolvedQuantitiesStatusBorderDisplay.color = results.hasUnsolvedQuantities == false ? new Color32(175, 255, 155, 255) : new Color32(200, 75, 55, 255);
		scalarQuantitiesStatusBorderDisplay.color = results.isScalarListCorrect == true ? new Color32(175, 255, 155, 255) : new Color32(200, 75, 55, 255);
		vectorQuantitiesStatusBorderDisplay.color = results.isVectorListCorrect == true ? new Color32(175, 255, 155, 255) : new Color32(200, 75, 55, 255);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		proceedButton.onClick.AddListener(() => ProceedEvent?.Invoke());

		// Create references and attach to associated parents
		// Sum of mass calculation
		unsolvedQuantitiesClone = Instantiate(unsolvedQuantitiesReference);
		unsolvedQuantitiesClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(unsolvedQuantitiesClone, unsolvedQuantitiesStatusBorderDisplay.gameObject);

		// Mean calculation
		scalarQuantitiesClone = Instantiate(scalarQuantitiesReference);
		scalarQuantitiesClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(scalarQuantitiesClone, scalarQuantitiesStatusBorderDisplay.gameObject);

		// Variance calculation
		vectorQuantitiesClone = Instantiate(vectorQuantitiesReference);
		vectorQuantitiesClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(vectorQuantitiesClone, vectorQuantitiesStatusBorderDisplay.gameObject);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		proceedButton.onClick.RemoveAllListeners();

		Destroy(unsolvedQuantitiesClone);
		Destroy(scalarQuantitiesClone);
		Destroy(vectorQuantitiesClone);
	}
}
