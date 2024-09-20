using UnityEngine;

public class SubActivityPerformanceMetric
{
    public string subActivityName { get; private set; }
    public bool isSubActivityFinished { get; private set; }
    public int numIncorrectAnswers { get; private set; }
    public int numCorrectAnswers { get; private set; }

    public SubActivityPerformanceMetric(
        string subActivityName,
        bool isSubActivityFinished,
        int numIncorrectAnswers,
		int numCorrectAnswers
		)
    {
        this.subActivityName = subActivityName;
        this.isSubActivityFinished = isSubActivityFinished;
        this.numIncorrectAnswers = numIncorrectAnswers;
        this.numCorrectAnswers = numCorrectAnswers;
    }
}

public abstract class ActivityFeedbackDisplay : MonoBehaviour
{
    public abstract void UpdateFeedbackDisplay(params SubActivityPerformanceMetric[] performanceMetrics);
}