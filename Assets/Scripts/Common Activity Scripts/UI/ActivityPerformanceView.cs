using TMPro;
using UnityEngine;

public abstract class ActivityPerformanceView : MonoBehaviour
{
	[Header("Input Reader")]
	[SerializeField] InputReader inputReader;

	[Header("Total Time Text")]
	[SerializeField] private TextMeshProUGUI totalTimeText;

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

	public abstract void RetryLevel();

	public abstract void ReturnMainMenu();

	public abstract void ProceedToNextLevel();

	public abstract void GoToSelectionScreen();
}