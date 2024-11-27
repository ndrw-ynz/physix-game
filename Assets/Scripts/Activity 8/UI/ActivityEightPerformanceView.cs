using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivityEightPerformanceView : ActivityPerformanceView
{
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
        Difficulty activityDifficulty = ActivityEightManager.difficultyConfiguration;

        switch (activityDifficulty)
        {
            case Difficulty.Easy:
                ActivityEightManager.difficultyConfiguration = Difficulty.Medium;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case Difficulty.Medium:
                ActivityEightManager.difficultyConfiguration = Difficulty.Hard;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case Difficulty.Hard:
                SceneManager.LoadScene("Topic Discussion 9");
                break;
        }
    }

	public override void GoToSelectionScreen()
	{
        MainMenuManager.selectionScreenToOpen = LessonSelectionScreen.LessonEightSelectionScreen;
        SceneManager.LoadScene("Main Menu");
    }
}