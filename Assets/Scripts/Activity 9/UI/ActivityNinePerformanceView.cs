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
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public override void ReturnMainMenu()
	{
		SceneManager.LoadScene("Main Menu");
	}

	public override void ProceedToNextLevel()
	{
        Difficulty activityDifficulty = ActivityNineManager.difficultyConfiguration;

        switch (activityDifficulty)
        {
            case Difficulty.Easy:
                ActivityNineManager.difficultyConfiguration = Difficulty.Medium;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case Difficulty.Medium:
                ActivityNineManager.difficultyConfiguration = Difficulty.Hard;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case Difficulty.Hard:
				Debug.Log("NO ACTION: Last lesson, last difficulty reached. Ending");
                break;
        }
    }

	public override void GoToSelectionScreen()
	{
        MainMenuManager.selectionScreenToOpen = LessonSelectionScreen.LessonNineSelectionScreen;
        SceneManager.LoadScene("Main Menu");
    }
}