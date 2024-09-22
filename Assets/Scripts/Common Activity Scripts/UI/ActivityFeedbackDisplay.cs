using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubActivityPerformanceMetric
{
    public string subActivityName { get; private set; }
    public bool isSubActivityFinished { get; private set; }
    public int numIncorrectAnswers { get; private set; }
    public int numCorrectAnswers { get; private set; }
	public int badScoreThreshold { get; private set; }
	public int averageScoreThreshold { get; private set; }


	public SubActivityPerformanceMetric(
        string subActivityName,
        bool isSubActivityFinished,
        int numIncorrectAnswers,
		int numCorrectAnswers,
        int badScoreThreshold,
        int averageScoreThreshold
		)
    {
        this.subActivityName = subActivityName;
        this.isSubActivityFinished = isSubActivityFinished;
        this.numIncorrectAnswers = numIncorrectAnswers;
        this.numCorrectAnswers = numCorrectAnswers;
        this.badScoreThreshold = badScoreThreshold;
        this.averageScoreThreshold = averageScoreThreshold;
    }
}

public abstract class ActivityFeedbackDisplay : MonoBehaviour
{
	protected TextMeshProUGUI currentFeedbackText;
	protected Image currentLessonDisplay;

	public void UpdateFeedbackDisplay(params SubActivityPerformanceMetric[] performanceMetrics)
    {
        UpdateFeedbackMessageDisplay(performanceMetrics);
        UpdateFeedbackRecommendationsDisplay(performanceMetrics);
	}
    protected virtual void UpdateFeedbackMessageDisplay(params SubActivityPerformanceMetric[] performanceMetrics)
    {
		foreach (SubActivityPerformanceMetric metric in performanceMetrics)
		{
			// Choose current feedback text
			SelectCurrentFeedbackText(metric);

			// Case 1: No submissions for sub activity
			if (metric.numCorrectAnswers + metric.numIncorrectAnswers <= 0)
			{
				currentFeedbackText.text = $"- You did not submit any answers for the <b><color=blue>{metric.subActivityName}</color></b> sub activity <color=red>and have withdrawn from the activity.</color>" +
								  " Don't give up, comrade! Review, practice, and give it another shot. Each attempt brings you closer to your goal.";
				continue;
			}
			// Case 2: Sub activity not accomplished
			else if (!metric.isSubActivityFinished)
			{
				currentFeedbackText.text = $"- You attempted the <b><color=blue>{metric.subActivityName}</color></b> sub activity <b><color=blue>{metric.numCorrectAnswers + metric.numIncorrectAnswers}</color></b> time(s) but <color=red>didn't finish the sub activity.</color> " +
								  $"It seems you're struggling with the concept of {metric.subActivityName}. Review the material and give it another try.";
				continue;
			}

			// Case 3: Sub activity finished
			currentFeedbackText.text = $"- You accomplished the <b><color=blue>{metric.subActivityName}</color></b> sub activity. ";
			currentFeedbackText.text += $"You got <b><color=blue>{metric.numCorrectAnswers}</color></b> correct answer(s) out of <b><color=blue>{metric.numIncorrectAnswers + metric.numCorrectAnswers}</color></b> submission(s) for <b><color=blue>{metric.subActivityName}</color></b> ";

			// Select force feedback status message to be prepended
			string forceCalculationfeedbackStatus = "";
			// Case 3.1: Bad score
			if (metric.numIncorrectAnswers >= metric.badScoreThreshold)
			{
				forceCalculationfeedbackStatus = $"<color=red>but received a bad score due to too many incorrect submissions.</color> It seems you’re struggling with {metric.subActivityName}, review its concepts and have another go at it.";
			}
			// Case 3.2: Average score
			else if (metric.numIncorrectAnswers >= metric.averageScoreThreshold && metric.numIncorrectAnswers < metric.badScoreThreshold)
			{
				forceCalculationfeedbackStatus = $"<color=#A56340>and received an average score.</color> Not bad, but there’s room for improvement. Review the concepts of {metric.subActivityName} and give it another try";
			}
			// Case 3.3: High score
			else if (metric.numIncorrectAnswers > 0 && metric.numIncorrectAnswers < metric.averageScoreThreshold)
			{
				forceCalculationfeedbackStatus = $"<color=#46A028>and received a good score--well done!.</color> ";
			}
			// Case 3.4: Perfect score
			else
			{
				forceCalculationfeedbackStatus = $"<color=#FFD70E>and received a perfect score--flawless!.</color> ";
			}

			currentFeedbackText.text += forceCalculationfeedbackStatus;
		}
	}
	protected virtual void UpdateFeedbackRecommendationsDisplay(params SubActivityPerformanceMetric[] performanceMetrics)
	{
		// Set lesson recommendations hidden/inactive first.
		HideLessonRecommendationDisplays();

		foreach (SubActivityPerformanceMetric metric in performanceMetrics)
		{
			// Choose current lesson display
			SelectCurrentLessonDisplay(metric);

			if (metric.numIncorrectAnswers >= metric.averageScoreThreshold || !metric.isSubActivityFinished)
			{
				currentLessonDisplay.gameObject.SetActive(true);
			}			
		}
	}

	protected abstract void SelectCurrentFeedbackText(SubActivityPerformanceMetric metric);

	protected abstract void SelectCurrentLessonDisplay(SubActivityPerformanceMetric metric);

	protected abstract void HideLessonRecommendationDisplays();
}