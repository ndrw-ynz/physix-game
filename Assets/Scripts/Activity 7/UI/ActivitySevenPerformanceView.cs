using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivitySevenPerformanceView : ActivityPerformanceView
{
	[Header("Center of Mass Metrics Text")]
    [SerializeField] private TextMeshProUGUI centerOfMassStatusText;
	[SerializeField] private TextMeshProUGUI centerOfMassNumIncorrectText;
	[SerializeField] private TextMeshProUGUI centerOfMassDurationText;
	[Header("Momentum Impulse Force Metrics Text")]
	[SerializeField] private TextMeshProUGUI momentumImpulseForceStatusText;
	[SerializeField] private TextMeshProUGUI momentumImpulseForceNumIncorrectText;
	[SerializeField] private TextMeshProUGUI momentumImpulseForceDurationText;
	[Header("Elastic Inelastic Collision Metrics Text")]
	[SerializeField] private TextMeshProUGUI elasticInelasticCollisionStatusText;
	[SerializeField] private TextMeshProUGUI elasticInelasticCollisionNumIncorrectText;
	[SerializeField] private TextMeshProUGUI elasticInelasticCollisionDurationText;

	public void SetCenterOfMassMetricsDisplay(bool isAccomplished, int numIncorrectSubmission, float duration)
	{
		SetStatusText(centerOfMassStatusText, isAccomplished);
		centerOfMassNumIncorrectText.text = $"{numIncorrectSubmission}";
		SetDurationText(centerOfMassDurationText, duration);
	}

	public void SetMomentumImpulseForceMetricsDisplay(bool isAccomplished, int numIncorrectSubmission, float duration)
	{
		SetStatusText(momentumImpulseForceStatusText, isAccomplished);
		momentumImpulseForceNumIncorrectText.text = $"{numIncorrectSubmission}";
		SetDurationText(momentumImpulseForceDurationText, duration);

	}

	public void SetElasticInelasticCollisionMetricsDisplay(bool isAccomplished, int numIncorrectSubmission, float duration)
	{
		SetStatusText(elasticInelasticCollisionStatusText, isAccomplished);
		elasticInelasticCollisionNumIncorrectText.text = $"{numIncorrectSubmission}";
		SetDurationText(elasticInelasticCollisionDurationText, duration);
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
        Difficulty activityDifficulty = ActivitySevenManager.difficultyConfiguration;

        switch (activityDifficulty)
        {
            case Difficulty.Easy:
                ActivitySevenManager.difficultyConfiguration = Difficulty.Medium;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case Difficulty.Medium:
                ActivitySevenManager.difficultyConfiguration = Difficulty.Hard;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case Difficulty.Hard:
                SceneManager.LoadScene("Topic Discussion 8");
                break;
        }
    }

	public override void GoToSelectionScreen()
	{
        MainMenuManager.selectionScreenToOpen = LessonSelectionScreen.LessonSevenSelectionScreen;
        SceneManager.LoadScene("Main Menu");
    }
}