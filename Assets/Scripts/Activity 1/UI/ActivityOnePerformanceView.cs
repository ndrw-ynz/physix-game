using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivityOnePerformanceView : ActivityPerformanceView
{
	[Header("Scientific Notation Metrics Text")]
	[SerializeField] private TextMeshProUGUI SNStatusText;
	[SerializeField] private TextMeshProUGUI SNNumIncorrectText;
	[SerializeField] private TextMeshProUGUI SNGameplayDurationText;
	[Header("Variance Metrics Text")]
	[SerializeField] private TextMeshProUGUI varianceStatusText;
	[SerializeField] private TextMeshProUGUI varianceNumIncorrectText;
	[SerializeField] private TextMeshProUGUI varianceGameplayDurationText;
	[Header("Accuracy & Precision Metrics Text")]
	[SerializeField] private TextMeshProUGUI APStatusText;
	[SerializeField] private TextMeshProUGUI APNumIncorrectText;
	[SerializeField] private TextMeshProUGUI APGameplayDurationText;
	[Header("Errors Metrics Text")]
	[SerializeField] private TextMeshProUGUI errorsStatusText;
	[SerializeField] private TextMeshProUGUI errorsNumIncorrectText;
	[SerializeField] private TextMeshProUGUI errorsGameplayDurationText;

	public void SetScientificNotationMetricsDisplay(bool isAccomplished, int numIncorrectSubmission, float duration)
	{
		SetStatusText(SNStatusText, isAccomplished);
		SNNumIncorrectText.text = $"{numIncorrectSubmission}";
		SetDurationText(SNGameplayDurationText, duration);
	}

	public void SetVarianceMetricsDisplay(bool isAccomplished, int numIncorrectSubmission, float duration)
	{
		SetStatusText(varianceStatusText, isAccomplished);
		varianceNumIncorrectText.text = $"{numIncorrectSubmission}";
		SetDurationText(varianceGameplayDurationText, duration);
	}

	public void SetAccuracyPrecisionMetricsDisplay(bool isAccomplished, int numIncorrectSubmission, float duration)
	{
		SetStatusText(APStatusText, isAccomplished);
		APNumIncorrectText.text = $"{numIncorrectSubmission}";
		SetDurationText(APGameplayDurationText, duration);
	}

	public void SetErrorsMetricsDisplay(bool isAccomplished, int numIncorrectSubmission, float duration)
	{
		SetStatusText(errorsStatusText, isAccomplished);
		errorsNumIncorrectText.text = $"{numIncorrectSubmission}";
		SetDurationText(errorsGameplayDurationText, duration);
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
