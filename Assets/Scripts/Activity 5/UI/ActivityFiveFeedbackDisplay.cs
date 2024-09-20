using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivityFiveFeedbackDisplay : ActivityFeedbackDisplay
{
	[Header("Feedback Message Text")]
	[SerializeField] private TextMeshProUGUI appleMotionFeedbackText;
	[SerializeField] private TextMeshProUGUI rockMotionFeedbackText;
	[SerializeField] private TextMeshProUGUI boatMotionFeedbackText;

	[Header("Lesson Recommendations Display")]
	[SerializeField] private Image forcesLessonDisplay;
	[SerializeField] private Image forceDiagramsLessonDisplay;

	private int forceCalculationBadThreshold = 3;
	private int forceCalculationAverageThreshold = 2;
	private int forceCalculationGoodThreshold = 1;
	private int forceDiagramBadThreshold = 5;
	private int forceDiagramAverageThreshold = 3;
	private int forceDiagramGoodThreshold = 1;

	protected override void UpdateFeedbackMessageDisplay(params SubActivityPerformanceMetric[] performanceMetrics)
	{
		// Reset feedbackText content
		appleMotionFeedbackText.text = "";
		rockMotionFeedbackText.text = "";
		boatMotionFeedbackText.text = "";

		foreach (SubActivityPerformanceMetric metric in performanceMetrics)
		{
			// Determine feedbackText to be modified based on the present metric
			TextMeshProUGUI feedbackText;
			switch (metric.subActivityName)
			{
				case "AppleForceCalculation":
				case "AppleForceDiagram":
					feedbackText = appleMotionFeedbackText;
					break;
				case "RockForceCalculation":
				case "RockForceDiagram":
					feedbackText = rockMotionFeedbackText;
					break;
				case "BoatForceCalculation":
				case "BoatForceDiagram":
					feedbackText = boatMotionFeedbackText;
					break;
				default:
					feedbackText = null;
					Debug.Log("ERROR: improper subActivityName");
					break;
			}

			// Determine what part of the feedbackText to be modified, which can either be force calculation or force diagram
			switch (metric.subActivityName)
			{
				// Construct force calculation message to be prepended to feedback text
				case "AppleForceCalculation":
				case "RockForceCalculation":
				case "BoatForceCalculation":
					
					// Determine subActivityName
					string subActivityName = "";
					switch (metric.subActivityName)
					{
						case "AppleForceCalculation":
							subActivityName = "apple motion";
							break;
						case "RockForceCalculation":
							subActivityName = "rock motion";
							break;
						case "BoatForceCalculation":
							subActivityName = "boat motion";
							break;
					}


					// Construct feedbackMessage to be prepended
					string prependedfeedbackMessage = "";
					// Case 1: No submissions for sub activity
					if (metric.numCorrectAnswers + metric.numIncorrectAnswers <= 0)
					{
						prependedfeedbackMessage = $"- You did not submit any answers for the <b><color=blue>{subActivityName}</color></b> sub activity <color=red>and have withdrawn from the activity.</color>" +
										  " Don't give up, comrade! Review, practice, and give it another shot. Each attempt brings you closer to your goal.";
						feedbackText.text = prependedfeedbackMessage;
						continue;
					}
					// Case 2: Sub activity not accomplished
					else if (!metric.isSubActivityFinished)
					{
						prependedfeedbackMessage = $"- You attempted the <b><color=blue>{subActivityName}</color></b> sub activity <b><color=blue>{metric.numIncorrectAnswers + metric.numCorrectAnswers}</color></b> time(s) but <color=red>didn't finish the sub activity.</color>" +
										  "It seems you're struggling with the concept of either force or force diagrams. Review the material and give it another try.";
						feedbackText.text = prependedfeedbackMessage;
						continue;
					} 
					// Case 3: Sub activity finished
					prependedfeedbackMessage += $"- You accomplished the <b><color=blue>{subActivityName}</color></b> sub activity. ";
					prependedfeedbackMessage += $"You got <b><color=blue>{metric.numCorrectAnswers}</color></b> correct answers out of <b><color=blue>{metric.numIncorrectAnswers + metric.numCorrectAnswers}</color></b> submission(s) for <b><color=blue>forces</color></b>. ";


					// Append feedback for forces
					string forceCalculationfeedbackStatus = "";
					// Case 3: Sub activity finished
					if (metric.numIncorrectAnswers == 0)
					{
						// Case 3.4: Perfect score
						forceCalculationfeedbackStatus = $"<color=#ADA52B>You received a perfect score for forces.</color> ";
					}
					else if (metric.numIncorrectAnswers <= forceCalculationGoodThreshold)
					{
						// Case 3.3: High score
						forceCalculationfeedbackStatus= $"<color=#46A028>You received a high score for forces.</color> ";
					}
					else if (metric.numIncorrectAnswers <= forceCalculationAverageThreshold)
					{
						// Case 3.2: Average score
						forceCalculationfeedbackStatus= $"<color=#A56340>You received an average score for forces.</color> ";
					}
					else if (metric.numIncorrectAnswers <= forceCalculationBadThreshold)
					{
						// Case 3.1: Bad score
						forceCalculationfeedbackStatus= $"<color=red>You received a bad score due to too many incorrect submissions for forces.</color> ";
					}
					prependedfeedbackMessage += forceCalculationfeedbackStatus;


					feedbackText.text = prependedfeedbackMessage + feedbackText.text;
					break;


				// Construct force diagram message to be appended to feedback text
				case "AppleForceDiagram":
				case "RockForceDiagram":
				case "BoatForceDiagram":
					// Case 1 and 2: No submissions for sub activity or sub activity is not accomplished.
					if (metric.numCorrectAnswers + metric.numIncorrectAnswers <= 0 || metric.isSubActivityFinished == false)
					{
						// No message to append.
						continue;
					}

					// Construct feedbackMessage to be prepended
					string appendedfeedbackMessage = "";
					appendedfeedbackMessage += $"You got <b><color=blue>{metric.numCorrectAnswers}</color></b> correct answers out of <b><color=blue>{metric.numIncorrectAnswers + metric.numCorrectAnswers}</color></b> submission(s) for <b><color=blue>force diagrams</color></b>. ";

					// Append feedback for force diagrams
					string forceDiagramfeedbackStatus = "";
					// Case 3: Sub activity finished
					if (metric.numIncorrectAnswers == 0)
					{
						// Case 3.4: Perfect score
						forceDiagramfeedbackStatus = "<color=#ADA52B>You received a perfect score for force diagrams.</color>";
					}
					else if (metric.numIncorrectAnswers <= forceDiagramGoodThreshold)
					{
						// Case 3.3: High score
						forceDiagramfeedbackStatus = "<color=#46A028>You received a high score for force diagrams.</color>";
					}
					else if (metric.numIncorrectAnswers <= forceDiagramAverageThreshold)
					{
						// Case 3.2: Average score
						forceDiagramfeedbackStatus = "<color=#A56340>You received an average score for force diagrams.</color>";
					}
					else if (metric.numIncorrectAnswers <= forceDiagramBadThreshold)
					{
						// Case 3.1: Bad score
						forceDiagramfeedbackStatus = "<color=red>You received a bad score due to too many incorrect submissions for force diagrams.</color>";
					}
					appendedfeedbackMessage += forceDiagramfeedbackStatus;

					feedbackText.text += appendedfeedbackMessage;
					break;
			}			
		}
	}

	protected override void UpdateFeedbackRecommendationsDisplay(params SubActivityPerformanceMetric[] performanceMetrics)
	{
		// Set lesson recommendations hidden/inactive first.
		forcesLessonDisplay.gameObject.SetActive(false);
		forceDiagramsLessonDisplay.gameObject.SetActive(false);

		foreach (SubActivityPerformanceMetric metric in performanceMetrics)
		{
			// Determine which lesson recommendation to display
			switch (metric.subActivityName)
			{
				// Use metrics for force calculation in displaying lesson recommendation
				case "AppleForceCalculation":
				case "RockForceCalculation":
				case "BoatForceCalculation":
					// Either no perfect score or doesn't have a high score result on force calculation
					if (metric.numIncorrectAnswers > forceCalculationGoodThreshold || metric.numIncorrectAnswers != 0 || !metric.isSubActivityFinished)
					{
						forcesLessonDisplay.gameObject.SetActive(true);
						forceDiagramsLessonDisplay.gameObject.SetActive(true);
					}
					break;
				// Use metrics for force diagram in displaying lesson recommendation
				case "AppleForceDiagram":
				case "RockForceDiagram":
				case "BoatForceDiagram":
					// Either no perfect score or doesn't have a high score result on force diagrams
					if (metric.numIncorrectAnswers > forceDiagramGoodThreshold || metric.numIncorrectAnswers != 0 || !metric.isSubActivityFinished)
					{
						forcesLessonDisplay.gameObject.SetActive(true);
						forceDiagramsLessonDisplay.gameObject.SetActive(true);
					}
					break;
			}
		}
	}
}