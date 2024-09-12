using System;
using UnityEngine;

public class WorkSubActivityAnswerSubmission
{
	public float? force { get; private set; }
	public float? work { get; private set; }

	public WorkSubActivityAnswerSubmission(
		float? force,
		float? work
		)
	{
		this.force = force;
		this.work = work;
	}
}

public class WorkView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;
	public event Action<WorkSubActivityAnswerSubmission> SubmitAnswerEvent;

	[Header("Given Variable Displays")]
	[SerializeField] private GivenVariableDisplay givenAcceleration;
	[SerializeField] private GivenVariableDisplay givenMass;
	[SerializeField] private GivenVariableDisplay givenDisplacement;
	[SerializeField] private GivenVariableDisplay givenAngleMeasure;

	[Header("Calculation Displays")]
	[SerializeField] private GameObject linearWorkCalculationDisplay;
	[SerializeField] private GameObject angularWorkCalculationDisplay;

	[Header("Equation Displays")]
	[SerializeField] private ProductEquationDisplay forceEquationDisplay;
	[SerializeField] private ProductEquationDisplay linearWorkEquationDisplay;
	[SerializeField] private AngularWorkEquationDisplay angularWorkEquationDisplay;

	private void OnEnable()
	{
		OpenViewEvent?.Invoke();
	}

	public void SetupWorkView(WorkSubActivityData data, WorkSubActivityState subActivityState)
	{
		ClearAllFields();

		givenAcceleration.SetupGivenVariableDisplay("Acceleration:", $"{data.acceleration} m/s^2");
		givenMass.SetupGivenVariableDisplay("Mass:", $"{data.mass} kg");
		givenDisplacement.SetupGivenVariableDisplay("Displacement:", $"{data.displacement} m");
		givenAngleMeasure.SetupGivenVariableDisplay("Angle Measure:", $"{data.angleMeasure} °");
		givenAngleMeasure.gameObject.SetActive(subActivityState == WorkSubActivityState.AngularWork);
	}

	public void SetLinearEquationDisplayState(bool isActive)
	{
		linearWorkCalculationDisplay.gameObject.SetActive(isActive);
	}

	public void SetAngularEquationDisplayState(bool isActive)
	{
		angularWorkCalculationDisplay.gameObject.SetActive(isActive);
	}

	private void ClearAllFields()
	{
		forceEquationDisplay.ResetState();
		linearWorkEquationDisplay.ResetState();
		angularWorkEquationDisplay.ResetState();
	}

	public void OnSubmitButtonClick()
	{
		WorkSubActivityAnswerSubmission submission = new WorkSubActivityAnswerSubmission(
			force: forceEquationDisplay.productValue,
			work: linearWorkCalculationDisplay.activeSelf ? linearWorkEquationDisplay.productValue : angularWorkEquationDisplay.angularWorkValue
			);

		SubmitAnswerEvent?.Invoke(submission);
	}

	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitViewEvent?.Invoke();
	}
}