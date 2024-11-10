using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class MomentumImpulseForceAnswerSubmission
{
	public float? impulse { get; set; }
	public float? netForce { get; set; }
	protected MomentumImpulseForceAnswerSubmission(float? impulse, float? netForce)
	{
		this.impulse = impulse;
		this.netForce = netForce;
	}
}

public class EasyMomentumImpulseForceAnswerSubmission : MomentumImpulseForceAnswerSubmission
{
	public float? changeInMomentum { get; set; } // mass * deltaVelocity

	public EasyMomentumImpulseForceAnswerSubmission(float? impulse, float? netForce, float? changeInMomentum)
		: base(impulse, netForce)
	{
		this.changeInMomentum = changeInMomentum;
	}
}

public class MediumHardMomentumImpulseForceAnswerSubmission : MomentumImpulseForceAnswerSubmission
{
	public float? initialMomentum { get; set; }
	public float? finalMomentum { get; set; }
	public float? changeInMomentum { get; set; } // finalMomentum - initialMomentum

	public MediumHardMomentumImpulseForceAnswerSubmission(float? impulse, float? netForce, float? initialMomentum, float? finalMomentum, float? changeInMomentum)
		: base(impulse, netForce)
	{
		this.initialMomentum = initialMomentum;
		this.finalMomentum = finalMomentum;
		this.changeInMomentum = changeInMomentum;
	}
}

public class MomentumImpulseForceView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;
	public static event Action<MomentumImpulseForceAnswerSubmission> SubmitAnswerEvent;

	[Header("Text")]
	[SerializeField] private TextMeshProUGUI calibrationTestText;

	[Header("Holders")]
	[SerializeField] private GameObject easyGivenHolder;
	[SerializeField] private GameObject mediumHardGivenHolder;
	[SerializeField] private GameObject easyCalculationHolder;
	[SerializeField] private GameObject mediumHardCalculationHolder;

	[Header("Given Fields")]
	[SerializeField] private TMP_InputField givenMassField;
	[SerializeField] private TMP_InputField givenDeltaVelocityField; // Easy config
	[SerializeField] private TMP_InputField givenInitialVelocityField; // Medium-hard config
	[SerializeField] private TMP_InputField givenFinalVelocityField; // Medium-hard config
	[SerializeField] private TMP_InputField givenTotalTimeField;

	[Header("Input Fields")]
	[SerializeField] private TMP_InputField initialMomentumMultiplicandField;
	[SerializeField] private TMP_InputField initialMomentumMultiplierField;
	[SerializeField] private TMP_InputField finalMomentumMultiplicandField;
	[SerializeField] private TMP_InputField finalMomentumMultiplierField;
	[SerializeField] private TMP_InputField deltaMomentumMultiplicandField; // Easy config
	[SerializeField] private TMP_InputField deltaMomentumMultiplierField; // Easy config
	[SerializeField] private TMP_InputField deltaMomentumMinuendField; // Medium-Hard config 
	[SerializeField] private TMP_InputField deltaMomentumSubtrahendField; // Medium-Hard config
	[SerializeField] private TMP_InputField impulseField;
	[SerializeField] private TMP_InputField netForceDividendField;
	[SerializeField] private TMP_InputField netForceDivisorField;

	[Header("Result Fields")]
	[SerializeField] private TMP_InputField changeInMomentumResultField; // Easy config (mass * net velocity)
	[SerializeField] private TMP_InputField initialMomentumResultField; // Medium-hard config
	[SerializeField] private TMP_InputField finalMomentumResultField; // Medium-hard config
	[SerializeField] private TMP_InputField changeInMomentumResultField2; // Medium-hard config (finalMomentum - initialMomentum)
	[SerializeField] private TMP_InputField impulseResultField;
	[SerializeField] private TMP_InputField netForceResultField;

	private void OnEnable()
	{
		OpenViewEvent?.Invoke();
	}

	#region View Setup

	public void SetupMomentumImpulseForceView(Dictionary<string, float> momentumImpulseForceGivenData)
	{
		// Setting given fields text and displaying UI
		switch (ActivitySevenManager.difficultyConfiguration)
		{
			case Difficulty.Easy:
				givenDeltaVelocityField.text = $"{momentumImpulseForceGivenData["deltaVelocity"]} m/s";
				givenMassField.text = $"{momentumImpulseForceGivenData["mass"]} kg";
				givenTotalTimeField.text = $"{momentumImpulseForceGivenData["totalTime"]} s";

				ShowUIForDifficulty(true);
				break;
			case Difficulty.Medium:
				SetGivenFields(momentumImpulseForceGivenData, "kg", "m/s", "s");
				ShowUIForDifficulty(false);
				break;
			case Difficulty.Hard:
				SetGivenFields(momentumImpulseForceGivenData, "g", "km/s", "ms", 1000, 0.001f, 1000);
				ShowUIForDifficulty(false);
				break;
		}

		// Set default view
		ClearAllInputFields();
	}

	private void SetGivenFields(Dictionary<string, float> data, string massUnit, string velocityUnit, string timeUnit, float massMultiplier = 1, float velocityMultiplier = 1, float timeMultiplier = 1)
	{
		givenMassField.text = $"{data["mass"] * massMultiplier} {massUnit}";
		givenInitialVelocityField.text = $"{data["initialVelocity"] * velocityMultiplier} {velocityUnit}";
		givenFinalVelocityField.text = $"{data["finalVelocity"] * velocityMultiplier} {velocityUnit}";
		givenTotalTimeField.text = $"{data["totalTime"] * timeMultiplier} {timeUnit}";
	}

	private void ShowUIForDifficulty(bool isEasy)
	{
		easyGivenHolder.gameObject.SetActive(isEasy);
		mediumHardGivenHolder.gameObject.SetActive(!isEasy);
		easyCalculationHolder.gameObject.SetActive(isEasy);
		mediumHardCalculationHolder.gameObject.SetActive(!isEasy);
	}

	public void UpdateCalibrationTestTextDisplay(int testNumber, int totalTests)
	{
		calibrationTestText.text = $"Calibration Test: {testNumber} / {totalTests}";
	}

	#endregion

	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitViewEvent?.Invoke();
	}

	public void OnSubmitButtonClick()
	{
		switch (ActivitySevenManager.difficultyConfiguration)
		{
			case Difficulty.Easy:
				{
					EasyMomentumImpulseForceAnswerSubmission submission = new EasyMomentumImpulseForceAnswerSubmission(
						float.Parse(impulseResultField.text),
						float.Parse(netForceResultField.text),
						float.Parse(changeInMomentumResultField.text)
						);
					SubmitAnswerEvent?.Invoke(submission);
				}
				break;
			case Difficulty.Medium: case Difficulty.Hard:
				{
					MediumHardMomentumImpulseForceAnswerSubmission submission = new MediumHardMomentumImpulseForceAnswerSubmission(
						float.Parse(impulseResultField.text),
						float.Parse(netForceResultField.text),
						float.Parse(initialMomentumResultField.text),
						float.Parse(finalMomentumResultField.text),
						float.Parse(changeInMomentumResultField2.text)
						);
					SubmitAnswerEvent?.Invoke(submission);
				}
				break;
		}
	}

	private void ClearAllInputFields()
	{
		initialMomentumMultiplicandField.text = "";
		initialMomentumMultiplierField.text = "";
		finalMomentumMultiplicandField.text = "";
		deltaMomentumMultiplicandField.text = "";
		deltaMomentumMultiplierField.text = "";
		finalMomentumMultiplierField.text = "";
		deltaMomentumMinuendField.text = "";
		deltaMomentumSubtrahendField.text = "";
		impulseField.text = "";
		netForceDividendField.text = "";
		netForceDivisorField.text = "";
	}
}