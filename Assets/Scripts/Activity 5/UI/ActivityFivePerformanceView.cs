using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivityFivePerformanceView : ActivityPerformanceView
{
	[Header("Apple Motion Metrics Text")]
	[SerializeField] private TextMeshProUGUI appleMotionStatusText;
	[SerializeField] private TextMeshProUGUI appleMotionForceNumIncorrectText;
	[SerializeField] private TextMeshProUGUI appleMotionForceDiagramNumIncorrectText;
	[SerializeField] private TextMeshProUGUI appleMotionGameplayDurationText;
	[Header("Rock Motion Metrics Text")]
	[SerializeField] private TextMeshProUGUI rockMotionStatusText;
	[SerializeField] private TextMeshProUGUI rockMotionForceNumIncorrectText;
	[SerializeField] private TextMeshProUGUI rockMotionForceDiagramNumIncorrectText;
	[SerializeField] private TextMeshProUGUI rockMotionGameplayDurationText;
	[Header("Boat Motion Metrics Text")]
	[SerializeField] private TextMeshProUGUI boatMotionStatusText;
	[SerializeField] private TextMeshProUGUI boatMotionForceNumIncorrectText;
	[SerializeField] private TextMeshProUGUI boatMotionForceDiagramNumIncorrectText;
	[SerializeField] private TextMeshProUGUI boatMotionGameplayDurationText;

	public void SetAppleMotionMetricsDisplay(bool isAccomplished, int numIncorrectForceSubmission, int numIncorrectForceDiagramSubmission, float duration)
	{
		SetStatusText(appleMotionStatusText, isAccomplished);
		appleMotionForceNumIncorrectText.text = $"{numIncorrectForceSubmission}";
		appleMotionForceDiagramNumIncorrectText.text = $"{numIncorrectForceDiagramSubmission}";
		SetDurationText(appleMotionGameplayDurationText, duration);
	}

	public void SetRockMotionMetricsDisplay(bool isAccomplished, int numIncorrectForceSubmission, int numIncorrectForceDiagramSubmission, float duration)
	{
		SetStatusText(rockMotionStatusText, isAccomplished);
		rockMotionForceNumIncorrectText.text = $"{numIncorrectForceSubmission}";
		rockMotionForceDiagramNumIncorrectText.text = $"{numIncorrectForceDiagramSubmission}";
		SetDurationText(rockMotionGameplayDurationText, duration);
	}

	public void SetBoatMotionMetricsDisplay(bool isAccomplished, int numIncorrectForceSubmission, int numIncorrectForceDiagramSubmission, float duration)
	{
		SetStatusText(boatMotionStatusText, isAccomplished);
		boatMotionForceNumIncorrectText.text = $"{numIncorrectForceSubmission}";
		boatMotionForceDiagramNumIncorrectText.text = $"{numIncorrectForceDiagramSubmission}";
		SetDurationText(boatMotionGameplayDurationText, duration);
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
        Difficulty activityDifficulty = ActivityFiveManager.difficultyConfiguration;

        switch (activityDifficulty)
        {
            case Difficulty.Easy:
                ActivityFiveManager.difficultyConfiguration = Difficulty.Medium;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case Difficulty.Medium:
                ActivityFiveManager.difficultyConfiguration = Difficulty.Hard;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case Difficulty.Hard:
                SceneManager.LoadScene("Topic Discussion 6");
                break;
        }
    }

	public override void GoToSelectionScreen()
	{
        MainMenuManager.selectionScreenToOpen = LessonSelectionScreen.LessonFiveSelectionScreen;
        SceneManager.LoadScene("Main Menu");
    }
}