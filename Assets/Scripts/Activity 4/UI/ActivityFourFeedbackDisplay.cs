using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivityFourFeedbackDisplay : ActivityFeedbackDisplay
{
	[Header("Feedback Message Text")]
	[SerializeField] private TextMeshProUGUI projectileMotionFeedbackText;
	[SerializeField] private TextMeshProUGUI circularMotionFeedbackText;

	[Header("Lesson Recommendations Display")]
	[SerializeField] private Image projectileMotionLessonDisplay;
	[SerializeField] private Image circularMotionLessonDisplay;

	protected override void SelectCurrentFeedbackText(SubActivityPerformanceMetric metric)
	{
		switch (metric.subActivityName)
		{
			case "projectile motion":
				currentFeedbackText = projectileMotionFeedbackText;
				break;
			case "circular motion":
				currentFeedbackText = circularMotionFeedbackText;
				break;
		}
	}

	protected override void SelectCurrentLessonDisplay(SubActivityPerformanceMetric metric)
	{
		switch (metric.subActivityName)
		{
			case "projectile motion":
				currentLessonDisplay = projectileMotionLessonDisplay;
				break;
			case "circular motion":
				currentLessonDisplay = circularMotionLessonDisplay;
				break;
		}
	}

	protected override void HideLessonRecommendationDisplays()
	{
		projectileMotionLessonDisplay.gameObject.SetActive(false);
		circularMotionLessonDisplay.gameObject.SetActive(false);
	}
}
