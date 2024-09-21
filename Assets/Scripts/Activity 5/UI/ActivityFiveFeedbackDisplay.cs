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

	protected override void UpdateFeedbackMessageDisplay(params SubActivityPerformanceMetric[] performanceMetrics)
	{
		// Reset feedbackText content
		appleMotionFeedbackText.text = "";
		rockMotionFeedbackText.text = "";
		boatMotionFeedbackText.text = "";

		int totalSubActivityAttemptsCounter = 0;
		bool isCaseOne = false;
		bool isCaseTwo = false;

		foreach (SubActivityPerformanceMetric metric in performanceMetrics)
		{
			// Determine feedbackText to be modified based on the present metric
			SelectCurrentFeedbackText(metric);

			// Determine what part of the feedbackText to be modified, which can either be force calculation (prepend) or force diagram (append)
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

					// Construct message if case one or two
					string caseOneOrTwoMessage = "";
					// Case 1: No submissions for sub activity
					if (metric.numCorrectAnswers + metric.numIncorrectAnswers + totalSubActivityAttemptsCounter <= 0 || (isCaseOne && !isCaseTwo))
					{
						caseOneOrTwoMessage = $"- You did not submit any answers for the <b><color=blue>{subActivityName}</color></b> sub activity <color=red>and have withdrawn from the activity.</color>" +
										  " Don't give up, comrade! Review, practice, and give it another shot. Each attempt brings you closer to your goal.";
						currentFeedbackText.text = caseOneOrTwoMessage;

						// Reset temp variables
						isCaseOne = false;
						isCaseTwo = false;
						totalSubActivityAttemptsCounter = 0;
						continue;
					}

					// Case 2: Sub activity not accomplished
					else if (!metric.isSubActivityFinished || (isCaseTwo && !isCaseOne))
					{
						totalSubActivityAttemptsCounter += metric.numIncorrectAnswers + metric.numCorrectAnswers;
						caseOneOrTwoMessage = $"- You attempted the <b><color=blue>{subActivityName}</color></b> sub activity <b><color=blue>{totalSubActivityAttemptsCounter}</color></b> time(s) but <color=red>didn't finish the sub activity.</color> " +
										  "It seems you're struggling with the concept of either force or force diagrams. Review the material and give it another try.";
						currentFeedbackText.text = caseOneOrTwoMessage;

						// Reset temp variables
						isCaseOne = false;
						isCaseTwo = false;
						totalSubActivityAttemptsCounter = 0;
						continue;
					}

					// Case 3: Sub activity finished
					// Construct feedback message for forces
					string prependedfeedbackMessage = $"You got <b><color=blue>{metric.numCorrectAnswers}</color></b> correct answer(s) out of <b><color=blue>{metric.numIncorrectAnswers + metric.numCorrectAnswers}</color></b> submission(s) for <b><color=blue>forces</color></b>. ";
					
					// Select force feedback status message to be prepended
					string forceCalculationfeedbackStatus = "";
					// Case 3.1: Bad score
					if (metric.numIncorrectAnswers >= metric.badScoreThreshold)
					{
						forceCalculationfeedbackStatus = $"<color=red>You received a bad score due to too many incorrect submissions for forces.</color> ";
					}
					// Case 3.2: Average score
					else if (metric.numIncorrectAnswers >= metric.averageScoreThreshold && metric.numIncorrectAnswers < metric.badScoreThreshold)
					{
						forceCalculationfeedbackStatus = $"<color=#A56340>You received an average score for forces.</color> ";
					}
					// Case 3.3: High score
					else if (metric.numIncorrectAnswers > 0 && metric.numIncorrectAnswers < metric.averageScoreThreshold)
					{
						forceCalculationfeedbackStatus = $"<color=#46A028>You received a high score for forces.</color> ";
					}
					// Case 3.4: Perfect score
					else if (metric.numIncorrectAnswers == 0)
					{
						forceCalculationfeedbackStatus = $"<color=#FFD70E>You received a perfect score for forces.</color> ";
					}
					prependedfeedbackMessage += forceCalculationfeedbackStatus;

					currentFeedbackText.text = prependedfeedbackMessage + currentFeedbackText.text;

					// Prepend Case 3 calculation message
					currentFeedbackText.text = $"- You accomplished the <b><color=blue>{subActivityName}</color></b> sub activity. " + currentFeedbackText.text;
					isCaseOne = false;
					isCaseTwo = false;
					totalSubActivityAttemptsCounter = 0;
					break;


				// Construct force diagram message to be appended to feedback text
				case "AppleForceDiagram":
				case "RockForceDiagram":
				case "BoatForceDiagram":
					// Update counter variable
					totalSubActivityAttemptsCounter += metric.numCorrectAnswers + metric.numIncorrectAnswers;

					// Update case variables and proceed to next metric
					// Case 1: No submissions for sub activity
					if (metric.numCorrectAnswers + metric.numIncorrectAnswers <= 0)
					{
						isCaseOne = true;
						continue;
					}
					// Case 2: Sub activity not accomplished
					else if (!metric.isSubActivityFinished)
					{
						isCaseTwo = true;
						continue;
					}


					// Construct feedbackMessage to be appended
					string appendedfeedbackMessage = $"You got <b><color=blue>{metric.numCorrectAnswers}</color></b> correct answer(s) out of <b><color=blue>{metric.numIncorrectAnswers + metric.numCorrectAnswers}</color></b> submission(s) for <b><color=blue>force diagrams</color></b>. ";

					// Select force diagram feedback status message to be prepended
					string forceDiagramfeedbackStatus = "";
					// Case 3.1: Bad score
					if (metric.numIncorrectAnswers >= metric.badScoreThreshold)
					{
						forceDiagramfeedbackStatus = "<color=red>You received a bad score due to too many incorrect submissions for force diagrams.</color>";
					}
					// Case 3.2: Average score
					else if (metric.numIncorrectAnswers >= metric.averageScoreThreshold && metric.numIncorrectAnswers < metric.badScoreThreshold)
					{
						forceDiagramfeedbackStatus = "<color=#A56340>You received an average score for force diagrams.</color>";
					}
					// Case 3.3: High score
					else if (metric.numIncorrectAnswers > 0 && metric.numIncorrectAnswers < metric.averageScoreThreshold)
					{
						forceDiagramfeedbackStatus = "<color=#46A028>You received a high score for force diagrams.</color>";
					}
					// Case 3.4: Perfect score
					else if (metric.numIncorrectAnswers == 0)
					{
						forceDiagramfeedbackStatus = "<color=#FFD70E>You received a perfect score for force diagrams.</color>";
					}
					appendedfeedbackMessage += forceDiagramfeedbackStatus;

					currentFeedbackText.text += appendedfeedbackMessage;
					break;
			}			
		}
	}

	protected override void UpdateFeedbackRecommendationsDisplay(params SubActivityPerformanceMetric[] performanceMetrics)
	{
		// Set lesson recommendations hidden/inactive first.
		HideLessonRecommendationDisplays();

		foreach (SubActivityPerformanceMetric metric in performanceMetrics)
		{
			// Determine which lesson recommendation to display
			// Either no perfect score, doesn't have a high score result on force calculation, or hasn't finished sub activity
			if (metric.numIncorrectAnswers >= metric.averageScoreThreshold || !metric.isSubActivityFinished)
			{
				forcesLessonDisplay.gameObject.SetActive(true);
				forceDiagramsLessonDisplay.gameObject.SetActive(true);
			}
		}
	}

	protected override void SelectCurrentFeedbackText(SubActivityPerformanceMetric metric)
	{
		switch (metric.subActivityName)
		{
			case "AppleForceCalculation":
			case "AppleForceDiagram":
				currentFeedbackText = appleMotionFeedbackText;
				break;
			case "RockForceCalculation":
			case "RockForceDiagram":
				currentFeedbackText = rockMotionFeedbackText;
				break;
			case "BoatForceCalculation":
			case "BoatForceDiagram":
				currentFeedbackText = boatMotionFeedbackText;
				break;
		}
	}

	protected override void SelectCurrentLessonDisplay(SubActivityPerformanceMetric metric)
	{
		// Will not be implemented, activity five is a special case. Either both lessons are displayed or not, thus no need to select currentLesson.
	}

	protected override void HideLessonRecommendationDisplays()
	{
		forcesLessonDisplay.gameObject.SetActive(false);
		forceDiagramsLessonDisplay.gameObject.SetActive(false);
	}
}