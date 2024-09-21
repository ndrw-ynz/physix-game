using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivityNinePerformanceView : ActivityPerformanceView
{
	[Header("Gravity Metrics Text")]
	[SerializeField] private TextMeshProUGUI gravityStatusText;
	[SerializeField] private TextMeshProUGUI gravityNumIncorrectText;
	[SerializeField] private TextMeshProUGUI gravityDurationText;

	public void SetGravityMetricsDisplay(bool isAccomplished, int numIncorrectSubmission, float duration)
	{
		SetStatusText(gravityStatusText, isAccomplished);
		gravityNumIncorrectText.text = $"{numIncorrectSubmission}";
		SetDurationText(gravityDurationText, duration);
	}

	public override void RetryLevel()
	{
		/*inputReader.SetGameplay();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);*/
	}

	public override void ReturnMainMenu()
	{
		SceneManager.LoadScene("Main Menu");
	}

	public override void ProceedToNextLevel()
	{
		// IN THE FUTURE, ADD SCENE FOR TOPIC DISCUSSION
	}

	public override void GoToSelectionScreen()
	{
		// IN THE FUTURE, ADD SCENE FOR SELECTION
	}
}