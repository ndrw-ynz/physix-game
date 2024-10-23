using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivityTwoFeedbackDisplay : ActivityFeedbackDisplay
{
	[Header("Feedback Message Text")]
	[SerializeField] private TextMeshProUGUI quantitiesFeedbackText;
	[SerializeField] private TextMeshProUGUI cartesianComponentsFeedbackText;
	[SerializeField] private TextMeshProUGUI vectorAdditionFeedbackText;

	[Header("Lesson Recommendations Display")]
	[SerializeField] private Image quantitiesLessonDisplay;
	[SerializeField] private Image cartesianComponentsLessonDisplay;
	[SerializeField] private Image vectorAdditionLessonDisplay;

	protected override void SelectCurrentFeedbackText(SubActivityPerformanceMetric metric)
	{
		switch (metric.subActivityName)
		{
			case "quantities":
				currentFeedbackText = quantitiesFeedbackText;
				break;
			case "cartesian components":
				currentFeedbackText = cartesianComponentsFeedbackText;
				break;
			case "vector addition":
				currentFeedbackText = vectorAdditionFeedbackText;
				break;
		}
	}

	protected override void SelectCurrentLessonDisplay(SubActivityPerformanceMetric metric)
	{
		switch (metric.subActivityName)
		{
			case "quantities":
				currentLessonDisplay = quantitiesLessonDisplay;
				break;
			case "cartesian components":
				currentLessonDisplay = cartesianComponentsLessonDisplay;
				break;
			case "vector addition":
				currentLessonDisplay = vectorAdditionLessonDisplay;
				break;
		}
	}

	protected override void HideLessonRecommendationDisplays()
	{
		quantitiesLessonDisplay.gameObject.SetActive(false);
		cartesianComponentsLessonDisplay.gameObject.SetActive(false);
		vectorAdditionLessonDisplay.gameObject.SetActive(false);
	}
}
