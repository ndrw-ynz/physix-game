using System;
using UnityEngine;
using UnityEngine.UI;

public class CircularMotionSubmissionStatusDisplay : SubmissionStatusDisplay
{
	public event Action ProceedEvent;

	[Header("Status Border Displays")]
	[SerializeField] private Image centripetalAccelerationStatusBorderDisplay;

	[Header("Calculation References")]
	[SerializeField] private GameObject centripetalAccelerationReference;

	// GameObject clone
	private GameObject centripetalAccelerationClone;

	public void UpdateStatusBorderDisplayFromResult(bool result)
	{
		centripetalAccelerationStatusBorderDisplay.color = result ? new Color32(175, 255, 155, 255) : new Color32(200, 75, 55, 255);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		proceedButton.onClick.AddListener(() => ProceedEvent?.Invoke());

		// Create references and attach to associated parents
		// Centripetal acceleration calculation
		centripetalAccelerationClone = Instantiate(centripetalAccelerationReference);
		centripetalAccelerationClone.gameObject.SetActive(true);
		UIUtilities.CenterChildInParent(centripetalAccelerationClone, centripetalAccelerationStatusBorderDisplay.gameObject);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		proceedButton.onClick.RemoveAllListeners();

		Destroy(centripetalAccelerationClone);
	}
}
