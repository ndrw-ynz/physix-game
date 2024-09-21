using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivitySixFeedbackDisplay : ActivityFeedbackDisplay
{
	[Header("Feedback Message Text")]
	[SerializeField] private TextMeshProUGUI dotProductFeedbackText;
	[SerializeField] private TextMeshProUGUI workCalculationFeedbackText;
	[SerializeField] private TextMeshProUGUI workGraphFeedbackText;

	[Header("Lesson Recommendations Display")]
	[SerializeField] private Image dotProductLessonDisplay;
	[SerializeField] private Image workCalculationLessonDisplay;
	[SerializeField] private Image workGraphLessonDisplay;

	protected override void SelectCurrentFeedbackText(SubActivityPerformanceMetric metric)
	{
		switch (metric.subActivityName)
		{
			case "dot product":
				currentFeedbackText = dotProductFeedbackText;
				break;
			case "work calculation":
				currentFeedbackText = workCalculationFeedbackText;
				break;
			case "work graph interpretation":
				currentFeedbackText = workGraphFeedbackText;
				break;
		}
	}

	protected override void SelectCurrentLessonDisplay(SubActivityPerformanceMetric metric)
	{
		switch (metric.subActivityName)
		{
			case "dot product":
				currentLessonDisplay = dotProductLessonDisplay;
				break;
			case "work calculation":
				currentLessonDisplay = workCalculationLessonDisplay;
				break;
			case "work graph interpretation":
				currentLessonDisplay = workGraphLessonDisplay;
				break;
		}
	}

	protected override void HideLessonRecommendationDisplays()
	{
		dotProductLessonDisplay.gameObject.SetActive(false);
		workCalculationLessonDisplay.gameObject.SetActive(false);
		workGraphLessonDisplay.gameObject.SetActive(false);
	}
}