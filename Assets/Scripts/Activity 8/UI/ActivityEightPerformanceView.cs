using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivityEightPerformanceView : MonoBehaviour
{
	[Header("Input Reader")]
	[SerializeField] InputReader inputReader;

	[Header("Total Time Text")]
	[SerializeField] private TextMeshProUGUI totalTimeText;

	[Header("Moment of Inertia Metrics Text")]
	[SerializeField] private TextMeshProUGUI momentOfInertiaStatusText;
	[SerializeField] private TextMeshProUGUI momentOfInertiaNumIncorrectText;
	[SerializeField] private TextMeshProUGUI momentOfInertiaDurationText;
	[Header("Torque Metrics Text")]
	[SerializeField] private TextMeshProUGUI torqueStatusText;
	[SerializeField] private TextMeshProUGUI torqueNumIncorrectText;
	[SerializeField] private TextMeshProUGUI torqueDurationText;
	[Header("Equilibrium Metrics Text")]
	[SerializeField] private TextMeshProUGUI equilibriumStatusText;
	[SerializeField] private TextMeshProUGUI equilibriumNumIncorrectText;
	[SerializeField] private TextMeshProUGUI equilibriumDurationText;

	public void SetTotalTimeDisplay(float totalTime)
	{
		SetDurationText(totalTimeText, totalTime);
	}
	public void SetMomentOfInertiaMetricsDisplay(bool isAccomplished, int numIncorrectSubmission, float duration)
	{
		SetStatusText(momentOfInertiaStatusText, isAccomplished);
		momentOfInertiaNumIncorrectText.text = $"{numIncorrectSubmission}";
		SetDurationText(momentOfInertiaDurationText, duration);
	}

	public void SetTorqueMetricsDisplay(bool isAccomplished, int numIncorrectSubmission, float duration)
	{
		SetStatusText(torqueStatusText, isAccomplished);
		torqueNumIncorrectText.text = $"{numIncorrectSubmission}";
		SetDurationText(torqueDurationText, duration);

	}

	public void SetEquilibriumMetricsDisplay(bool isAccomplished, int numIncorrectSubmission, float duration)
	{
		SetStatusText(equilibriumStatusText, isAccomplished);
		equilibriumNumIncorrectText.text = $"{numIncorrectSubmission}";
		SetDurationText(equilibriumDurationText, duration);
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