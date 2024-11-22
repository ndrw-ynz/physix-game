using System;
using TMPro;
using UnityEngine;

public class ForceMotionView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;
	public event Action<float?> SubmitForceAnswerEvent;
	public event Action<ForceDiagramAnswerSubmission> SubmitForceDiagramAnswerEvent;

	[Header("Displays")]
	[SerializeField] private GameObject forceDiagramSelectionDisplay;
	[SerializeField] private GameObject forceDiagramDisplay;
	[SerializeField] private GameObject forceCalculationDisplay;

	[Header("Force Given Fields")]
	[SerializeField] private TMP_InputField givenAcceleration;
	[SerializeField] private TMP_InputField givenMass;

	[Header("Force Input Fields")]
	[SerializeField] private TMP_InputField forceMultiplicand;
	[SerializeField] private TMP_InputField forceMultiplier;

	[Header("Force Result Field")]
	[SerializeField] private TMP_InputField forceResultField;

	[Header("Force Type Containers")]
	[SerializeField] private ForceTypeContainer upForceContainer;
	[SerializeField] private ForceTypeContainer downForceContainer;
	[SerializeField] private ForceTypeContainer leftForceContainer;
	[SerializeField] private ForceTypeContainer rightForceContainer;


	private void OnEnable()
	{
		OpenViewEvent?.Invoke();
	}

	public void SetupForceCalculationDisplay(ForceData data)
	{
		ClearAllFields();

		switch(ActivityFiveManager.difficultyConfiguration)
		{
			case Difficulty.Easy:
				givenAcceleration.text = $"{data.acceleration} m/s^2";
				givenMass.text = $"{data.mass} kg";
				break;
			case Difficulty.Medium:
			case Difficulty.Hard:
				givenAcceleration.text = $"{data.acceleration * 0.001f} km/s^2";
				givenMass.text = $"{data.mass * 1000f} g";
				break;
		}
	}

	public void ClearAllFields()
	{
		forceMultiplicand.text = "0";
		forceMultiplier.text = "0";
	}

	public void ResetForceDiagram()
	{
		upForceContainer.ClearContainer();
		downForceContainer.ClearContainer();
		leftForceContainer.ClearContainer();
		rightForceContainer.ClearContainer();
	}

	public void SetForceCalculationDisplayState(bool isActive)
	{
		forceCalculationDisplay.gameObject.SetActive(isActive);
	}

	public void SetForceDiagramDisplayState(bool isActive)
	{
		forceDiagramSelectionDisplay.gameObject.SetActive(isActive);
		forceDiagramDisplay.gameObject.SetActive(isActive);
	}

	public void OnSubmitForceButtonClick()
	{
		SceneSoundManager.Instance.PlaySFX("Click");

		bool canParse = float.TryParse(forceResultField.text, out float result);
		SubmitForceAnswerEvent?.Invoke(canParse ? result : null);
	}

	public void OnSubmitForceTypesButtonClick()
	{
		SceneSoundManager.Instance.PlaySFX("Click");

		ForceDiagramAnswerSubmission submission = new ForceDiagramAnswerSubmission(
			upForceType: upForceContainer.GetCurrentForceType(),
			downForceType: downForceContainer.GetCurrentForceType(),
			leftForceType: leftForceContainer.GetCurrentForceType(),
			rightForceType: rightForceContainer.GetCurrentForceType()
			);

		SubmitForceDiagramAnswerEvent?.Invoke(submission);
	}

	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitViewEvent?.Invoke();
	}
}