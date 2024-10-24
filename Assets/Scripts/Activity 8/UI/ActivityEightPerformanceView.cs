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