using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivitySixPerformanceView : ActivityPerformanceView
{
	[Header("Dot Product Metrics Text")]
	[SerializeField] private TextMeshProUGUI dotProductStatusText;
	[SerializeField] private TextMeshProUGUI dotProductNumIncorrectText;
	[SerializeField] private TextMeshProUGUI dotProductGameplayDurationText;
	[Header("Work Sub Activity Metrics Text")]
	[SerializeField] private TextMeshProUGUI workSubActivityStatusText;
	[SerializeField] private TextMeshProUGUI workSubActivityNumIncorrectText;
	[SerializeField] private TextMeshProUGUI workSubActivityGameplayDurationText;
	[Header("Work Graph Interpretation Metrics Text")]
	[SerializeField] private TextMeshProUGUI workGraphInterpretationStatusText;
	[SerializeField] private TextMeshProUGUI workGraphInterpretationNumIncorrectText;
	[SerializeField] private TextMeshProUGUI workGraphInterpretationGameplayDurationText;

	public void SetDotProductMetricsDisplay(bool isAccomplished, int numIncorrectSubmission, float duration)
	{
		SetStatusText(dotProductStatusText, isAccomplished);
		dotProductNumIncorrectText.text = $"{numIncorrectSubmission}";
		SetDurationText(dotProductGameplayDurationText, duration);
	}

	public void SetWorkSubActivityMetricsDisplay(bool isAccomplished, int numIncorrectSubmission, float duration)
	{
		SetStatusText(workSubActivityStatusText, isAccomplished);
		workSubActivityNumIncorrectText.text = $"{numIncorrectSubmission}";
		SetDurationText(workSubActivityGameplayDurationText, duration);
	}

	public void SetWorkGraphInterpretationMetricsDisplay(bool isAccomplished, int numIncorrectSubmission, float duration)
	{
		SetStatusText(workGraphInterpretationStatusText, isAccomplished);
		workGraphInterpretationNumIncorrectText.text = $"{numIncorrectSubmission}";
		SetDurationText(workGraphInterpretationGameplayDurationText, duration);
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
        Difficulty activityDifficulty = ActivitySixManager.difficultyConfiguration;

        switch (activityDifficulty)
        {
            case Difficulty.Easy:
                ActivitySixManager.difficultyConfiguration = Difficulty.Medium;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case Difficulty.Medium:
                ActivitySixManager.difficultyConfiguration = Difficulty.Hard;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case Difficulty.Hard:
                SceneManager.LoadScene("Topic Discussion 7");
                break;
        }
    }

	public override void GoToSelectionScreen()
	{
        MainMenuManager.selectionScreenToOpen = LessonSelectionScreen.LessonSixSelectionScreen;
        SceneManager.LoadScene("Main Menu");
    }
}