using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivityTwoPerformanceView : ActivityPerformanceView
{
	[Header("Quantities Metrics Text")]
	[SerializeField] private TextMeshProUGUI quantitiesStatusText;
	[SerializeField] private TextMeshProUGUI quantitiesNumIncorrectText;
	[SerializeField] private TextMeshProUGUI quantitiesGameplayDurationText;
	[Header("Cartesian Components Metrics Text")]
	[SerializeField] private TextMeshProUGUI cartesianComponentsStatusText;
	[SerializeField] private TextMeshProUGUI cartesianComponentsNumIncorrectText;
	[SerializeField] private TextMeshProUGUI cartesianComponentsGameplayDurationText;
	[Header("Vector Addition Metrics Text")]
	[SerializeField] private TextMeshProUGUI vectorAdditionStatusText;
	[SerializeField] private TextMeshProUGUI vectorAdditionNumIncorrectText;
	[SerializeField] private TextMeshProUGUI vectorAdditionGameplayDurationText;

	public void SetQuantitiesMetricsDisplay(bool isAccomplished, int numIncorrectSubmission, float duration)
	{
		SetStatusText(quantitiesStatusText, isAccomplished);
		quantitiesNumIncorrectText.text = $"{numIncorrectSubmission}";
		SetDurationText(quantitiesGameplayDurationText, duration);
	}

	public void SetCartesianComponentsMetricsDisplay(bool isAccomplished, int numIncorrectSubmission, float duration)
	{
		SetStatusText(cartesianComponentsStatusText, isAccomplished);
		cartesianComponentsNumIncorrectText.text = $"{numIncorrectSubmission}";
		SetDurationText(cartesianComponentsGameplayDurationText, duration);
	}

	public void SetVectorAdditionMetricsDisplay(bool isAccomplished, int numIncorrectSubmission, float duration)
	{
		SetStatusText(vectorAdditionStatusText, isAccomplished);
		vectorAdditionNumIncorrectText.text = $"{numIncorrectSubmission}";
		SetDurationText(vectorAdditionGameplayDurationText, duration);
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
        MainMenuManager.selectionScreenToOpen = LessonSelectionScreen.LessonTwoSelectionScreen;
        SceneManager.LoadScene("Main Menu");
    }
}
