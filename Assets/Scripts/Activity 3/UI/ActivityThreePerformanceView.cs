using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivityThreePerformanceView : ActivityPerformanceView
{
	[Header("Graphs Metrics Text")]
	[SerializeField] private TextMeshProUGUI graphsStatusText;
	[SerializeField] private TextMeshProUGUI graphsNumIncorrectText;
	[SerializeField] private TextMeshProUGUI graphsGameplayDurationText;
	[Header("1D Kinematics Metrics Text")]
	[SerializeField] private TextMeshProUGUI kinematics1DStatusText;
	[SerializeField] private TextMeshProUGUI kinematics1DGameplayDurationText;
	// Acceleration
	[SerializeField] private TextMeshProUGUI accelerationStatusText;
	[SerializeField] private TextMeshProUGUI accelerationNumIncorrectText;
	// Total Depth
	[SerializeField] private TextMeshProUGUI totalDepthStatusText;
	[SerializeField] private TextMeshProUGUI totalDepthNumIncorrectText;

	public void SetGraphsMetricsDisplay(bool isAccomplished, int numIncorrectSubmission, float duration)
	{
		SetStatusText(graphsStatusText, isAccomplished);
		graphsNumIncorrectText.text = $"{numIncorrectSubmission}";
		SetDurationText(graphsGameplayDurationText, duration);
	}

	public void SetKinematics1DMetricsDisplay(bool isAccelerationAccomplished, bool isTotalDepthAccomplished, int numIncorrectAcceleration, int numIncorrectTotalDepth, float duration)
	{
		SetStatusText(kinematics1DStatusText, isAccelerationAccomplished && isTotalDepthAccomplished);
		SetStatusText(accelerationStatusText, isAccelerationAccomplished);
		SetStatusText(totalDepthStatusText, isTotalDepthAccomplished);

		accelerationNumIncorrectText.text = $"{numIncorrectAcceleration}";
		totalDepthNumIncorrectText.text = $"{numIncorrectTotalDepth}";

		SetDurationText(kinematics1DGameplayDurationText, duration);
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
        Difficulty activityDifficulty = ActivityThreeManager.difficultyConfiguration;

        switch (activityDifficulty)
        {
            case Difficulty.Easy:
                ActivityThreeManager.difficultyConfiguration = Difficulty.Medium;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case Difficulty.Medium:
                ActivityThreeManager.difficultyConfiguration = Difficulty.Hard;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case Difficulty.Hard:
                SceneManager.LoadScene("Topic Discussion 4");
                break;
        }
    }

	public override void GoToSelectionScreen()
	{
        MainMenuManager.selectionScreenToOpen = LessonSelectionScreen.LessonThreeSelectionScreen;
        SceneManager.LoadScene("Main Menu");
    }
}
