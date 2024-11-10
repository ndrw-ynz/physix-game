using System;
using UnityEngine;
using UnityEngine.UI;

public class ElasticInelasticCollisionSubmissionStatusDisplay : SubmissionStatusDisplay
{
	public event Action ProceedEvent;

	[Header("Elastic Inelastic Collision Info Displays")]
	[SerializeField] private GameObject leftPageInfo;
	[SerializeField] private GameObject rightPageInfo;

	[Header("Elastic Inelastic Collision Status Border Displays")]
	[SerializeField] private Image initialMomentumsStatusBorderDisplay;
	[SerializeField] private Image finalMomentumsStatusBorderDisplay;
	[SerializeField] private Image netMomentumStatusBorderDisplay;
	[SerializeField] private Image collisionTypeStatusBorderDisplay;

	[Header("Initial Momentums Calculation Display Reference")]
	[SerializeField] private GameObject initialMomentumsCalculationReference;

	[Header("Final Momentums Calculation Display Reference")]
	[SerializeField] private GameObject finalMomentumsCalculationReference;

	[Header("Net Momentum Calculation Display Reference")]
	[SerializeField] private GameObject netMomentumCalculationReference;

	[Header("Collision Type Display Reference")]
	[SerializeField] private GameObject collisionTypeDisplayReference;

	[Header("Interactive Buttons")]
	[SerializeField] private Button leftPageButton;
	[SerializeField] private Button rightPageButton;

	// GameObject clones
	private GameObject initialMomentumsCalculationClone;
	private GameObject finalMomentumsCalculationClone;
	private GameObject netMomentumCalculationClone;
	private GameObject collisionTypeDisplayClone;

	public void DisplayLeftPageInfo()
	{
		leftPageButton.gameObject.SetActive(false);
		rightPageButton.gameObject.SetActive(true);

		leftPageInfo.gameObject.SetActive(true);
		rightPageInfo.gameObject.SetActive(false);
	}

	public void DisplayRightPageInfo()
	{
		leftPageButton.gameObject.SetActive(true);
		rightPageButton.gameObject.SetActive(false);

		leftPageInfo.gameObject.SetActive(false);
		rightPageInfo.gameObject.SetActive(true);
	}

	public void UpdateStatusBorderDisplaysFromResult(ElasticInelasticCollisionAnswerSubmissionResults results)
	{
		initialMomentumsStatusBorderDisplay.color = (
			results.isCubeOneInitialMomentumCorrect == true &&
			results.isCubeTwoInitialMomentumCorrect == true
			) ? 
			new Color32(175, 255, 155, 255) : 
			new Color32(200, 75, 55, 255);

		finalMomentumsStatusBorderDisplay.color = (
			results.isCubeOneFinalMomentumCorrect == true &&
			results.isCubeTwoFinalMomentumCorrect == true
			) ?
			new Color32(175, 255, 155, 255) :
			new Color32(200, 75, 55, 255);

		netMomentumStatusBorderDisplay.color = (
			results.isNetInitialMomentumCorrect == true &&
			results.isNetFinalMomentumCorrect == true
			) ?
			new Color32(175, 255, 155, 255) :
			new Color32(200, 75, 55, 255);

		collisionTypeStatusBorderDisplay.color = (
			results.isCollisionTypeCorrect == true
			) ?
			new Color32(175, 255, 155, 255) :
			new Color32(200, 75, 55, 255);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		proceedButton.onClick.AddListener(() => ProceedEvent?.Invoke());

		// Create references and attach to associated parents
		initialMomentumsCalculationClone = Instantiate(initialMomentumsCalculationReference);
		initialMomentumsCalculationClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(initialMomentumsCalculationClone, initialMomentumsStatusBorderDisplay.gameObject);

		finalMomentumsCalculationClone = Instantiate(finalMomentumsCalculationReference);
		finalMomentumsCalculationClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(finalMomentumsCalculationClone, finalMomentumsStatusBorderDisplay.gameObject);

		netMomentumCalculationClone = Instantiate(netMomentumCalculationReference);
		netMomentumCalculationClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(netMomentumCalculationClone, netMomentumStatusBorderDisplay.gameObject);

		collisionTypeDisplayClone = Instantiate(collisionTypeDisplayReference);
		collisionTypeDisplayClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(collisionTypeDisplayClone, collisionTypeStatusBorderDisplay.gameObject);

		// Set default display view
		DisplayLeftPageInfo();
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		proceedButton.onClick.RemoveAllListeners();

		Destroy(initialMomentumsCalculationClone);
		Destroy(finalMomentumsCalculationClone);
		Destroy(netMomentumCalculationClone);
		Destroy(collisionTypeDisplayClone);
	}
}