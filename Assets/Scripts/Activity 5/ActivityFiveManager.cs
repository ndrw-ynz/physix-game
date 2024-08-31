using UnityEngine;

public enum ForceObjectMotionType
{
	Apple_OnBranch,
	Rock_Stationary,
	Rock_RollingRight,
	Rock_Bouncing,
	Rock_Flying,
	Boat_Stationary,
	Boat_MovingRight,
	Boat_MovingLeft
}

public class ForceTypeAnswerSubmission
{
	public ForceObjectMotionType forceObjectMotionType;
	public ForceType? upForceType { get; private set; }
	public ForceType? downForceType { get; private set; }
	public ForceType? leftForceType {get; private set;}
	public ForceType? rightForceType { get; private set; }

	public ForceTypeAnswerSubmission(
		ForceObjectMotionType forceObjectMotionType,
		ForceType? upForceType,
		ForceType? downForceType,
		ForceType? leftForceType,
		ForceType? rightForceType
		)
	{
		this.forceObjectMotionType = forceObjectMotionType;
		this.upForceType = upForceType;
		this.downForceType = downForceType;
		this.leftForceType = leftForceType;
		this.rightForceType = rightForceType;
	}
}

public class ActivityFiveManager : MonoBehaviour
{
	public static Difficulty difficultyConfiguration;

	[Header("Input Reader")]
	[SerializeField] InputReader inputReader;

	[Header("Views")]
	[SerializeField] private AppleMotionView appleMotionView;

	[Header("Submission Status Displays")]
	[Header("Force Type Submission Status Displays")]
	[SerializeField] private AppleForceTypeSubmissionStatusDisplay appleForceTypeSubmissionStatusDisplay;

	private void Start()
	{
		AppleMotionView.SubmitForceTypesAnswerEvent += CheckForceTypeAnswers;

	}

	private void CheckForceTypeAnswers(ForceTypeAnswerSubmission answer)
	{
		// DO SWITCH CASES TO UPDATE GAMEPLAY METRICS VARIABLES BASED ON ENUM/TYPE
		ForceTypeAnswerSubmissionResults results = ActivityFiveUtilities.ValidateForceTypeSubmission(answer);

		DisplayForceTypeSubmissionResults(answer, results);

	}

	private void DisplayForceTypeSubmissionResults(ForceTypeAnswerSubmission answer, ForceTypeAnswerSubmissionResults results)
	{
		if (results.isAllCorrect())
		{
			appleForceTypeSubmissionStatusDisplay.SetSubmissionStatus(true, "correct");
		}
		else
		{
			appleForceTypeSubmissionStatusDisplay.SetSubmissionStatus(false, "wrong");
		}

		appleForceTypeSubmissionStatusDisplay.UpdateForceDiagramDisplay(answer, results);

		appleForceTypeSubmissionStatusDisplay.gameObject.SetActive(true);
	}
}