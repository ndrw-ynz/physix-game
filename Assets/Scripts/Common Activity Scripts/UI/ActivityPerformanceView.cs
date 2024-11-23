using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ActivityPerformanceView : MonoBehaviour
{
	[Header("Input Reader")]
	[SerializeField] InputReader inputReader;

	[Header("Total Time Text")]
	[SerializeField] private TextMeshProUGUI totalTimeText;

	[Header("Activity Feedback Display")]
	[SerializeField] private ActivityFeedbackDisplay activityFeedbackDisplay;

	[Header("Next Level Button")]
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private TextMeshProUGUI nextLevelButtonText;
	
    public void SetTotalTimeDisplay(float totalTime)
	{
		SetDurationText(totalTimeText, totalTime);
	}

	protected void SetStatusText(TextMeshProUGUI statusText, bool isAccomplished)
	{
		if (isAccomplished)
		{
			statusText.text = "Accomplished";
			statusText.color = new Color32(70, 160, 40, 255);
		}
		else
		{
			statusText.text = "Not Accomplished";
			statusText.color = new Color32(160, 50, 40, 255);
		}
	}

	protected void SetDurationText(TextMeshProUGUI durationText, float duration)
	{
		int minutes = Mathf.FloorToInt(duration / 60);
		int seconds = Mathf.FloorToInt(duration % 60);
		durationText.text = string.Format("{0:00}m : {1:00}s", minutes, seconds);
	}

	public void UpdateActivityFeedbackDisplay(params SubActivityPerformanceMetric[] performanceMetrics)
	{
		activityFeedbackDisplay.UpdateFeedbackDisplay(performanceMetrics);
	}

    public void SetNextLevelButtonState(bool isInteractable)
    {
		nextLevelButton.interactable = isInteractable;
		nextLevelButtonText.color = Color.gray;
    }

    public abstract void RetryLevel();

	public abstract void ReturnMainMenu();

	public abstract void ProceedToNextLevel();

	public abstract void GoToSelectionScreen();
}