using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewActivityThreePerformance : MonoBehaviour
{
	public static event Action<ViewActivityThreePerformance> OpenViewEvent;

	[Header("Graphs Metrics")]
	public TextMeshProUGUI GraphsStatusText;
	public TextMeshProUGUI GraphsIncorrectNumText;
	[Header("1D Kinematics Metrics")]
	public TextMeshProUGUI Kinematics1DStatusText;
	public TextMeshProUGUI AccelerationProblemStatusText;
	public TextMeshProUGUI AccelerationProblemIncorrectNumText;
	public TextMeshProUGUI FreeFallProblemStatusText;
	public TextMeshProUGUI FreeFallProblemIncorrectNumText;

	private void OnEnable()
	{
		OpenViewEvent?.Invoke(this);
	}

	public void ReturnButtonClick()
	{
		SceneManager.LoadScene("Main Menu");
	}
}