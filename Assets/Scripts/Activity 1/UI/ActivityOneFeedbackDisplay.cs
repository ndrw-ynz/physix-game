using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivityOneFeedbackDisplay : ActivityFeedbackDisplay
{
	[Header("Feedback Message Text")]
	[SerializeField] private TextMeshProUGUI SNFeedbackText;
	[SerializeField] private TextMeshProUGUI varianceFeedbackText;
	[SerializeField] private TextMeshProUGUI APFeedbackText;
	[SerializeField] private TextMeshProUGUI errorsFeedbackText;

	[Header("Lesson Recommendations Display")]
	[SerializeField] private Image SNSLessonDisplay;
	[SerializeField] private Image varianceLessonDisplay;
	[SerializeField] private Image APLessonDisplay;
	[SerializeField] private Image errorsLessonDisplay;

	protected override void SelectCurrentFeedbackText(SubActivityPerformanceMetric metric)
	{
		switch (metric.subActivityName)
		{
			case "scientific notation":
				currentFeedbackText = SNFeedbackText;
				break;
			case "variance":
				currentFeedbackText = varianceFeedbackText;
				break;
			case "accuracy & precision":
				currentFeedbackText = APFeedbackText;
				break;
			case "errors":
				currentFeedbackText = errorsFeedbackText;
				break;
		}
	}

	protected override void SelectCurrentLessonDisplay(SubActivityPerformanceMetric metric)
	{
		switch (metric.subActivityName)
		{
			case "scientific notation":
				currentLessonDisplay = SNSLessonDisplay;
				break;
			case "variance":
				currentLessonDisplay = varianceLessonDisplay;
				break;
			case "accuracy & precision":
				currentLessonDisplay = APLessonDisplay;
				break;
			case "errors":
				currentLessonDisplay = errorsLessonDisplay;
				break;
		}
	}

	protected override void HideLessonRecommendationDisplays()
	{
		SNSLessonDisplay.gameObject.SetActive(false);
		varianceLessonDisplay.gameObject.SetActive(false);
		APLessonDisplay.gameObject.SetActive(false);
		errorsLessonDisplay.gameObject.SetActive(false);
	}
}
