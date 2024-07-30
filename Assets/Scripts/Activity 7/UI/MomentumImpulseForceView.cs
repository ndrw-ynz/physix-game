using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MomentumImpulseForceView : MonoBehaviour
{
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

	[Header("Result Fields")]
	[SerializeField] private TMP_InputField changeInMomentumResultField; // Easy config (mass * net velocity)
	[SerializeField] private TMP_InputField initialMomentumResultField; // Medium-hard config
	[SerializeField] private TMP_InputField finalMomentumResultField; // Medium-hard config
	[SerializeField] private TMP_InputField changeInMomentumResultField2; // Medium-hard config (finalMomentum - initialMomentum)
	[SerializeField] private TMP_InputField impulseResultField;
	[SerializeField] private TMP_InputField netForceResultField;

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
				SetGivenFields(momentumImpulseForceGivenData, "g", "km/s", "ms", 1000, 0.0001f, 1000);
				ShowUIForDifficulty(false);
				break;
		}
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
	#endregion

	private void OnSubmitButtonClick()
	{

	}
}