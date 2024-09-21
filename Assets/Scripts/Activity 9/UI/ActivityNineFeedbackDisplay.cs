using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivityNineFeedbackDisplay : ActivityFeedbackDisplay
{
	[Header("Feedback Message Text")]
	[SerializeField] private TextMeshProUGUI gravityFeedbackText;

	[Header("Lesson Recommendations Display")]
	[SerializeField] private Image gravityLessonDisplay;

	protected override void SelectCurrentFeedbackText(SubActivityPerformanceMetric metric)
	{
		switch (metric.subActivityName)
		{
			case "gravity":
				currentFeedbackText = gravityFeedbackText;
				break;
		}
	}

	protected override void SelectCurrentLessonDisplay(SubActivityPerformanceMetric metric)
	{
		switch (metric.subActivityName)
		{
			case "gravity":
				currentLessonDisplay = gravityLessonDisplay;
				break;
		}
	}

	protected override void HideLessonRecommendationDisplays()
	{
		gravityLessonDisplay.gameObject.SetActive(false);
	}
}