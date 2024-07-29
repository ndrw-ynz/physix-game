using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MomentumImpulseForceView : MonoBehaviour
{
	[Header("Given Fields")]
	[SerializeField] private TMP_InputField givenMassField;
	[SerializeField] private TMP_InputField givenInitialVelocityField;
	[SerializeField] private TMP_InputField givenFinalVelocityField;
	[SerializeField] private TMP_InputField givenTotalTimeField;

	[Header("Result Fields")]
	[SerializeField] private TMP_InputField initialMomentumResultField;
	[SerializeField] private TMP_InputField finalMomentumResultField;
	[SerializeField] private TMP_InputField changeInMomentumResultField;
	[SerializeField] private TMP_InputField impulseResultField;
	[SerializeField] private TMP_InputField netForceResultField;

	#region View Setup

	public void SetupMomentumImpulseForceView(Dictionary<string, float> momentumImpulseForceGivenData)
	{
		givenMassField.text = $"{momentumImpulseForceGivenData["mass"]} kg";
		givenInitialVelocityField.text = $"{momentumImpulseForceGivenData["initialVelocity"]} m/s";
		givenFinalVelocityField.text = $"{momentumImpulseForceGivenData["finalVelocity"]} m/s";
		givenTotalTimeField.text = $"{momentumImpulseForceGivenData["totalTime"]} s";
	}

	#endregion

	private void OnSubmitButtonClick()
	{

	}
}