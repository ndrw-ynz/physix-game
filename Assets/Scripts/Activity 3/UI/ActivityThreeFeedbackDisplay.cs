using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivityThreeFeedbackDisplay : ActivityFeedbackDisplay
{
	[Header("Feedback Message Text")]
	[SerializeField] private TextMeshProUGUI graphsFeedbackText;
	[SerializeField] private TextMeshProUGUI kinematicsAccelerationFeedbackText;
	[SerializeField] private TextMeshProUGUI kinematicsTotalDepthFeedbackText;

	[Header("Lesson Recommendations Display")]
	[SerializeField] private Image graphsLessonDisplay;
	[SerializeField] private Image kinematicsAccelerationLessonDisplay;
	[SerializeField] private Image kinematicsTotalDepthLessonDisplay;

	protected override void SelectCurrentFeedbackText(SubActivityPerformanceMetric metric)
	{
		switch (metric.subActivityName)
		{
			case "graphs":
				currentFeedbackText = graphsFeedbackText;
				break;
			case "1D Kinematics - acceleration":
				currentFeedbackText = kinematicsAccelerationFeedbackText;
				break;
			case "1D Kinematics - total depth":
				currentFeedbackText = kinematicsTotalDepthFeedbackText;
				break;
		}
	}

	protected override void SelectCurrentLessonDisplay(SubActivityPerformanceMetric metric)
	{
		switch (metric.subActivityName)
		{
			case "graphs":
				currentLessonDisplay = graphsLessonDisplay;
				break;
			case "1D Kinematics - acceleration":
				currentLessonDisplay = kinematicsAccelerationLessonDisplay;
				break;
			case "1D Kinematics - total depth":
				currentLessonDisplay = kinematicsTotalDepthLessonDisplay;
				break;
		}
	}

	protected override void HideLessonRecommendationDisplays()
	{
		graphsLessonDisplay.gameObject.SetActive(false);
		kinematicsAccelerationLessonDisplay.gameObject.SetActive(false);
		kinematicsTotalDepthLessonDisplay.gameObject.SetActive(false);
	}
}
