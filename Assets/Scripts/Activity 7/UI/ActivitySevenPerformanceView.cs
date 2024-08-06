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
	[Header("Momentum Impulse Force Metrics Text")]
	[SerializeField] private TextMeshProUGUI momentumImpulseForceStatusText;
	[SerializeField] private TextMeshProUGUI momentumImpulseForceNumIncorrectText;
	[Header("Elastic Inelastic Collision Metrics Text")]
	[SerializeField] private TextMeshProUGUI elasticInelasticCollisionStatusText;
	[SerializeField] private TextMeshProUGUI elasticInelasticCollisionNumIncorrectText;

	public void SetTotalTimeDisplay(float totalTime)
	{
		int minutes = Mathf.FloorToInt(totalTime / 60);
		int seconds = Mathf.FloorToInt(totalTime % 60);
		totalTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
	}

	public void SetCenterOfMassMetricsDisplay(bool isAccomplished, int numIncorrectSubmission)
	{
		SetStatusText(centerOfMassStatusText, isAccomplished);
		centerOfMassNumIncorrectText.text = $"{numIncorrectSubmission}";
	}

	public void SetMomentumImpulseForceMetricsDisplay(bool isAccomplished, int numIncorrectSubmission)
	{
		SetStatusText(momentumImpulseForceStatusText, isAccomplished);
		momentumImpulseForceNumIncorrectText.text = $"{numIncorrectSubmission}";
	}

	public void SetElasticInelasticCollisionMetricsDisplay(bool isAccomplished, int numIncorrectSubmission)
	{
		SetStatusText(elasticInelasticCollisionStatusText, isAccomplished);
		elasticInelasticCollisionNumIncorrectText.text = $"{numIncorrectSubmission}";
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