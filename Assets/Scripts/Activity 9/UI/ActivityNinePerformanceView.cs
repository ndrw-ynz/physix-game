using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivityNinePerformanceView : MonoBehaviour
{
	[Header("Input Reader")]
	[SerializeField] InputReader inputReader;

	[Header("Total Time Text")]
	[SerializeField] private TextMeshProUGUI totalTimeText;

	[Header("Gravity Metrics Text")]
	[SerializeField] private TextMeshProUGUI gravityStatusText;
	[SerializeField] private TextMeshProUGUI gravityNumIncorrectText;
	[SerializeField] private TextMeshProUGUI gravityDurationText;

	public void SetTotalTimeDisplay(float totalTime)
	{
		SetDurationText(totalTimeText, totalTime);
	}
	public void SetGravityMetricsDisplay(bool isAccomplished, int numIncorrectSubmission, float duration)
	{
		SetStatusText(gravityStatusText, isAccomplished);
		gravityNumIncorrectText.text = $"{numIncorrectSubmission}";
		SetDurationText(gravityDurationText, duration);
	}

	private void SetStatusText(TextMeshProUGUI statusText, bool isAccomplished)
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

	private void SetDurationText(TextMeshProUGUI durationText, float duration)
	{
		int minutes = Mathf.FloorToInt(duration / 60);
		int seconds = Mathf.FloorToInt(duration % 60);
		durationText.text = string.Format("{0:00}m : {1:00}s", minutes, seconds);
	}

	public void RetryLevel()
	{
		/*inputReader.SetGameplay();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);*/
	}

	public void ReturnMainMenu()
	{
		SceneManager.LoadScene("Main Menu");
	}

	public void ProceedToNextLevel()
	{
		// IN THE FUTURE, ADD SCENE FOR TOPIC DISCUSSION
	}

	public void GoToSelectionScreen()
	{
		// IN THE FUTURE, ADD SCENE FOR SELECTION
	}
}