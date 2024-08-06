using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivitySevenPerformanceView : MonoBehaviour
{
	[Header("Input Reader")]
	[SerializeField] InputReader inputReader;

	[Header("Total Time Text")]
	[SerializeField] private TextMeshProUGUI totalTimeText;
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

	public void SetTotalTimeDisplay(float totalTime)
	{
		SetDurationText(totalTimeText, totalTime);
	}

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

	public void GoToTopicDiscussion()
	{
		// IN THE FUTURE, ADD SCENE FOR TOPIC DISCUSSION
	}

	public void GoToSelectionScreen()
	{
		// IN THE FUTURE, ADD SCENE FOR SELECTION
	}
}