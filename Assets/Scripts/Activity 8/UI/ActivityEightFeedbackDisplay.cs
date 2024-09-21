using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivityEightFeedbackDisplay : ActivityFeedbackDisplay
{
	[Header("Feedback Message Text")]
	[SerializeField] private TextMeshProUGUI momentOfInertiaFeedbackText;
	[SerializeField] private TextMeshProUGUI torqueFeedbackText;
	[SerializeField] private TextMeshProUGUI equilibriumFeedbackText;

	[Header("Lesson Recommendations Display")]
	[SerializeField] private Image momentOfInertiaLessonDisplay;
	[SerializeField] private Image torqueLessonDisplay;
	[SerializeField] private Image equilibriumLessonDisplay;

	protected override void SelectCurrentFeedbackText(SubActivityPerformanceMetric metric)
	{
		switch (metric.subActivityName)
		{
			case "moment of inertia":
				currentFeedbackText = momentOfInertiaFeedbackText;
				break;
			case "torque":
				currentFeedbackText = torqueFeedbackText;
				break;
			case "equilibrium":
				currentFeedbackText = equilibriumFeedbackText;
				break;
		}
	}

	protected override void SelectCurrentLessonDisplay(SubActivityPerformanceMetric metric)
	{
		switch (metric.subActivityName)
		{
			case "moment of inertia":
				currentLessonDisplay = momentOfInertiaLessonDisplay;
				break;
			case "torque":
				currentLessonDisplay = torqueLessonDisplay;
				break;
			case "equilibrium":
				currentLessonDisplay = equilibriumLessonDisplay;
				break;
		}
	}

	protected override void HideLessonRecommendationDisplays()
	{
		momentOfInertiaLessonDisplay.gameObject.SetActive(false);
		torqueLessonDisplay.gameObject.SetActive(false);
		equilibriumLessonDisplay.gameObject.SetActive(false);
	}
}