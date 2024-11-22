using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivityFourPerformanceView : ActivityPerformanceView
{
	[Header("Projectile Motion Metrics Text")]
	[SerializeField] private TextMeshProUGUI projectileMotionStatusText;
	[SerializeField] private TextMeshProUGUI projectileMotionNumIncorrectText;
	[SerializeField] private TextMeshProUGUI projectileMotionGameplayDurationText;
	[Header("Circular Motion Metrics Text")]
	[SerializeField] private TextMeshProUGUI circularMotionStatusText;
	[SerializeField] private TextMeshProUGUI circularMotionNumIncorrectText;
	[SerializeField] private TextMeshProUGUI circularMotionGameplayDurationText;

	public void SetProjectileMotionMetricsDisplay(bool isAccomplished, int numIncorrectSubmission, float duration)
	{
		SetStatusText(projectileMotionStatusText, isAccomplished);
		projectileMotionNumIncorrectText.text = $"{numIncorrectSubmission}";
		SetDurationText(projectileMotionGameplayDurationText, duration);
	}

	public void SetCircularMotionMetricsDisplay(bool isAccomplished, int numIncorrectSubmission, float duration)
	{
		SetStatusText(circularMotionStatusText, isAccomplished);
		circularMotionNumIncorrectText.text = $"{numIncorrectSubmission}";
		SetDurationText(circularMotionGameplayDurationText, duration);
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
        Difficulty activityDifficulty = ActivityFourManager.difficultyConfiguration;

        switch (activityDifficulty)
        {
            case Difficulty.Easy:
                ActivityFourManager.difficultyConfiguration = Difficulty.Medium;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case Difficulty.Medium:
                ActivityFourManager.difficultyConfiguration = Difficulty.Hard;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case Difficulty.Hard:
                SceneManager.LoadScene("Topic Discussion 5");
                break;
        }
    }

	public override void GoToSelectionScreen()
	{
        MainMenuManager.selectionScreenToOpen = LessonSelectionScreen.LessonFourSelectionScreen;
        SceneManager.LoadScene("Main Menu");
    }
}
