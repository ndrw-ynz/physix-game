using System;
using UnityEngine;
using UnityEngine.UI;

public class CenterOfMassSubmissionStatusDisplay : SubmissionStatusDisplay
{
	public static event Action ProceedEvent;

	[Header("Center Of Mass Info Displays")]
	[SerializeField] private GameObject centerOfMassXInfo;
	[SerializeField] private GameObject centerOfMassYInfo;

	[Header("Center Of Mass Status Border Displays")]
	[SerializeField] private Image massTimesXCoordsStatusBorderDisplay;
	[SerializeField] private Image massTimesYCoordsStatusBorderDisplay;
	[SerializeField] private Image centerOfMassXCalculationStatusBorderDisplay;
	[SerializeField] private Image centerOfMassYCalculationStatusBorderDisplay;

	[Header("Mass Times Coordinates References")]
	[SerializeField] private GameObject massTimesXCoordsReference;
	[SerializeField] private GameObject massTimesYCoordsReference;

	[Header("Center Of Mass Calculation References")]
	[SerializeField] private GameObject centerOfMassXCalculationReference;
	[SerializeField] private GameObject centerOfMassYCalculationReference;

	[Header("Interactive Buttons")]
	[SerializeField] private Button leftPageButton;
	[SerializeField] private Button rightPageButton;

	private GameObject massTimesXCoordsClone;
	private GameObject massTimesYCoordsClone;
	private GameObject centerOfMassXCalculationClone;
	private GameObject centerOfMassYCalculationClone;


	public void DisplayCenterOfMassXInfo()
	{
		leftPageButton.gameObject.SetActive(false);
		rightPageButton.gameObject.SetActive(true);

		centerOfMassXInfo.gameObject.SetActive(true);

		centerOfMassYInfo.gameObject.SetActive(false);
	}

	public void DisplayCenterOfMassYInfo()
	{
		leftPageButton.gameObject.SetActive(true);
		rightPageButton.gameObject.SetActive(false);

		centerOfMassXInfo.gameObject.SetActive(false);

		centerOfMassYInfo.gameObject.SetActive(true);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		proceedButton.onClick.AddListener(() => ProceedEvent?.Invoke());

		// Create references and attach to associated parents
		massTimesXCoordsClone = Instantiate(massTimesXCoordsReference);
		massTimesXCoordsClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(massTimesXCoordsClone, massTimesXCoordsStatusBorderDisplay.gameObject);

		massTimesYCoordsClone = Instantiate(massTimesYCoordsReference);
		massTimesYCoordsClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(massTimesYCoordsClone, massTimesYCoordsStatusBorderDisplay.gameObject);

		centerOfMassXCalculationClone = Instantiate(centerOfMassXCalculationReference);
		centerOfMassXCalculationClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(centerOfMassXCalculationClone, centerOfMassXCalculationStatusBorderDisplay.gameObject);

		centerOfMassYCalculationClone = Instantiate(centerOfMassYCalculationReference);
		centerOfMassYCalculationClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(centerOfMassYCalculationClone, centerOfMassYCalculationStatusBorderDisplay.gameObject);

		// Set default view.
		DisplayCenterOfMassXInfo();
	}


	protected override void OnDisable()
	{
		base.OnDisable();
		Destroy(massTimesXCoordsClone);
		Destroy(massTimesYCoordsClone);
		Destroy(centerOfMassXCalculationClone);
		Destroy(centerOfMassYCalculationClone);
	}
}