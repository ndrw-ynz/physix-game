using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivitySevenFeedbackDisplay : ActivityFeedbackDisplay
{
	[Header("Feedback Message Text")]
	[SerializeField] private TextMeshProUGUI centerOfMassFeedbackText;
	[SerializeField] private TextMeshProUGUI momentumImpulseNetForceFeedbackText;
	[SerializeField] private TextMeshProUGUI elasticInelasticCollisionFeedbackText;

	[Header("Lesson Recommendations Display")]
	[SerializeField] private Image centerOfMassLessonDisplay;
	[SerializeField] private Image momentumImpulseNetForceLessonDisplay;
	[SerializeField] private Image elasticInelasticCollisionLessonDisplay;

	protected override void SelectCurrentFeedbackText(SubActivityPerformanceMetric metric)
	{
		switch (metric.subActivityName)
		{
			case "center of mass":
				currentFeedbackText = centerOfMassFeedbackText;
				break;
			case "momentum, impulse, and net force":
				currentFeedbackText = momentumImpulseNetForceFeedbackText;
				break;
			case "elastic and inelastic collision":
				currentFeedbackText = elasticInelasticCollisionFeedbackText;
				break;
		}
	}

	protected override void SelectCurrentLessonDisplay(SubActivityPerformanceMetric metric)
	{
		switch (metric.subActivityName)
		{
			case "center of mass":
				currentLessonDisplay = centerOfMassLessonDisplay;
				break;
			case "momentum, impulse, and net force":
				currentLessonDisplay = momentumImpulseNetForceLessonDisplay;
				break;
			case "elastic and inelastic collision":
				currentLessonDisplay = elasticInelasticCollisionLessonDisplay;
				break;
		}
	}

	protected override void HideLessonRecommendationDisplays()
	{
		centerOfMassLessonDisplay.gameObject.SetActive(false);
		momentumImpulseNetForceLessonDisplay.gameObject.SetActive(false);
		elasticInelasticCollisionLessonDisplay.gameObject.SetActive(false);
	}
}