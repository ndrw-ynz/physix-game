using TMPro;
using UnityEngine;

public class ActivityFourManager : MonoBehaviour
{
    [Header("Level Data - Projectile Motion")]
    [SerializeField] private ProjectileMotionSubActivitySO projectileMotionLevelOne;
	[SerializeField] private ProjectileMotionSubActivitySO projectileMotionLevelTwo;
	[SerializeField] private ProjectileMotionSubActivitySO projectileMotionLevelThree;
	private ProjectileMotionSubActivitySO currentProjectileMotionLevel;

	[Header("Level Data - Circular Motion")]
	[SerializeField] private CircularMotionSubActivitySO circularMotionLevelOne;
	[SerializeField] private CircularMotionSubActivitySO circularMotionLevelTwo;
	[SerializeField] private CircularMotionSubActivitySO circularMotionLevelThree;
	private CircularMotionSubActivitySO currentCircularMotionLevel;

	[Header("Views")]
	[SerializeField] private ViewProjectileMotion viewProjectileMotion;
	[SerializeField] private ViewCircularMotion viewCircularMotion;

	[Header("Problem Display Content")]
	[SerializeField] private TextMeshProUGUI problemText;

	[Header("Modal Window")]
	[SerializeField] private CalcSubmissionModalWindow submissionModalWindow;

	[Header("Given Values - Projectile Motion")]
	private int initialProjectileVelocityValue;
	private int projectileHeightValue;
	private int projectileAngleValue;

	[Header("Given Values - Circular Motion")]
	private int satelliteRadiusValue;
	private int satelliteTimePeriodValue;

	[Header("Metrics - Projectile Motion")]
	private bool isMaximumHeightCalculationFinished;
	private bool isHorizontalRangeCalculationFinished;
	private bool isTimeOfFlightCalculationFinished;
	private int numIncorrectMaximumHeightSubmission;
	private int numIncorrectHorizontalRangeSubmission;
	private int numIncorrectTimeOfFlightSubmission;

	private void Start()
	{
		ViewProjectileMotion.SubmitMaximumHeightAnswerEvent += CheckMaximumHeightAnswer;
		ViewProjectileMotion.SubmitHorizontalRangeAnswerEvent += CheckHorizontalRangeAnswer;
		ViewProjectileMotion.SubmitTimeOfFlightAnswerEvent += CheckTimeOfFlightAnswer;

		CalcSubmissionModalWindow.RetrySubmission += RestoreViewState;
		CalcSubmissionModalWindow.InitiateNext += UpdateViewState;

		currentProjectileMotionLevel = projectileMotionLevelOne; // modify this in the future, to add change of level
		currentCircularMotionLevel = circularMotionLevelOne; // modify in future, to add change of level

		InitializeProjectileMotionGiven(currentProjectileMotionLevel);
		InitializeCircularMotionGiven(currentCircularMotionLevel);

		viewProjectileMotion.SetupProjectileMotionProblemDisplay(initialProjectileVelocityValue, projectileHeightValue, projectileAngleValue);
	}

	#region Projectile Motion

	private void InitializeProjectileMotionGiven(ProjectileMotionSubActivitySO projectileMotionSO)
	{
		initialProjectileVelocityValue = Random.Range(projectileMotionSO.minimumVelocityValue, projectileMotionSO.maximumVelocityValue);
		projectileHeightValue = Random.Range(projectileMotionSO.minimumHeightValue, projectileMotionSO.maximumHeightValue);

		switch (projectileMotionSO.projectileAngleType)
		{
			case ProjectileAngleType.Standard90Angle:
				int[] standard90AngleValues = new int[] { 30, 45, 60, 90};
				projectileAngleValue = standard90AngleValues[Random.Range(0, standard90AngleValues.Length)];
				break;
			case ProjectileAngleType.Full90Angle:
				projectileAngleValue = Random.Range(1, 90);
				break;
		}
	}

	private void CheckMaximumHeightAnswer(float maximumHeightAnswer)
	{
		bool isMaximumHeightCorrect = ActivityFourUtilities.ValidateMaximumHeightSubmission(maximumHeightAnswer, initialProjectileVelocityValue, projectileAngleValue);
		if (isMaximumHeightCorrect)
		{
			isMaximumHeightCalculationFinished = true;

			problemText.text = "What is the horizontal range of the projectile?";
		}
		else
		{
			numIncorrectMaximumHeightSubmission++;
		}

		viewProjectileMotion.SetOverlays(true);
		viewProjectileMotion.ResetState();
		submissionModalWindow.gameObject.SetActive(true);
		submissionModalWindow.SetDisplayFromSubmissionResult(isMaximumHeightCorrect, "Maximum Height");
	}

	private void CheckHorizontalRangeAnswer(float horizontalRangeAnswer)
	{
		bool isHorizontalRangeCorrect = ActivityFourUtilities.ValidateHorizontalRangeSubmission(horizontalRangeAnswer, initialProjectileVelocityValue);
		if (isHorizontalRangeCorrect)
		{
			isHorizontalRangeCalculationFinished = true;

			problemText.text = "What is the time of flight of the projectile?";
		}
		else
		{
			numIncorrectHorizontalRangeSubmission++;
		}

		viewProjectileMotion.SetOverlays(true);
		viewProjectileMotion.ResetState();
		submissionModalWindow.gameObject.SetActive(true);
		submissionModalWindow.SetDisplayFromSubmissionResult(isHorizontalRangeCalculationFinished, "Horizontal Range");
	}

	private void CheckTimeOfFlightAnswer(float timeOfFlightAnswer)
	{
		bool isTimeOfFlightCorrect = ActivityFourUtilities.ValidateTimeOfFlightSubmission(timeOfFlightAnswer, initialProjectileVelocityValue, projectileAngleValue);
		if (isTimeOfFlightCorrect)
		{
			isTimeOfFlightCalculationFinished = true;

			viewProjectileMotion.gameObject.SetActive(false);
			viewCircularMotion.gameObject.SetActive(true);

			viewCircularMotion.SetupCentripetalAccelerationProblemDisplay(satelliteRadiusValue, satelliteTimePeriodValue);
		}
		else
		{
			numIncorrectTimeOfFlightSubmission++;
		}

		viewProjectileMotion.SetOverlays(true);
		viewProjectileMotion.ResetState();
		submissionModalWindow.gameObject.SetActive(true);
		submissionModalWindow.SetDisplayFromSubmissionResult(isHorizontalRangeCalculationFinished, "Time of Flight");
	}

	#endregion

	#region Circular Motion

	private void InitializeCircularMotionGiven(CircularMotionSubActivitySO circularMotionSO)
	{
		satelliteRadiusValue = Random.Range(circularMotionSO.minimumRadiusValue, circularMotionSO.maximumRadiusValue);
		satelliteTimePeriodValue = Random.Range(circularMotionSO.minimumTimePeriodValue, circularMotionSO.maximumTimePeriodValue);
	}

	#endregion

	private void RestoreViewState()
	{
		submissionModalWindow.gameObject.SetActive(false);
		viewProjectileMotion.SetOverlays(false);
	}

	private void UpdateViewState()
	{
		if (isMaximumHeightCalculationFinished && isHorizontalRangeCalculationFinished && !isTimeOfFlightCalculationFinished)
		{
			// All solved except time of flight
			viewProjectileMotion.SetSubmissionButtonStates(false, false, true);
		} else if (isMaximumHeightCalculationFinished && !isHorizontalRangeCalculationFinished)
		{
			// Maximum height solved but not horizontal range
			viewProjectileMotion.SetSubmissionButtonStates(false, true, false);
		}

		RestoreViewState();
	}
}