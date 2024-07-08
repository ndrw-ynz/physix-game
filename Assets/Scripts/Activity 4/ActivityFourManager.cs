using TMPro;
using UnityEngine;

public class ActivityFourManager : MonoBehaviour
{
    [Header("Level Data - Projectile Motion")]
    [SerializeField] private ProjectileMotionSubActivitySO projectileMotionLevelOne;
	[SerializeField] private ProjectileMotionSubActivitySO projectileMotionLevelTwo;
	[SerializeField] private ProjectileMotionSubActivitySO projectileMotionLevelThree;
	private ProjectileMotionSubActivitySO currentProjectileMotionLevel;

	[Header("Views")]
	[SerializeField] private ViewProjectileMotion viewProjectileMotion;

	[Header("Problem Display Content")]
	[SerializeField] private TextMeshProUGUI problemText;

	[Header("Modal Window")]
	[SerializeField] private CalcSubmissionModalWindow submissionModalWindow;

	[Header("Given Values - Projectile Motion")]
	private int initialProjectileVelocityValue;
	private int projectileHeightValue;
	private int projectileAngleValue;

	[Header("Metrics - Projectile Motion")]
	public bool isMaximumHeightCalculationFinished;
	public bool isHorizontalRangeCalculationFinished;
	public bool isTimeOfFlightCalculationFinished;
	public int numIncorrectMaximumHeightSubmission;
	public int numIncorrectHorizontalRangeSubmission;
	public int numIncorrectTimeOfFlightSubmission;

	private void Start()
	{
		ViewProjectileMotion.SubmitMaximumHeightAnswerEvent += CheckMaximumHeightAnswer;

		CalcSubmissionModalWindow.RetrySubmission += RestoreViewState;
		CalcSubmissionModalWindow.InitiateNext += UpdateViewState;

		currentProjectileMotionLevel = projectileMotionLevelOne; // modify this in the future, to add change of level

		InitializeProjectileMotionGiven(currentProjectileMotionLevel);

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