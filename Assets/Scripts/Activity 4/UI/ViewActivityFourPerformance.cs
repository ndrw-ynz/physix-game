using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewActivityFourPerformance : MonoBehaviour
{
	public static event Action<ViewActivityFourPerformance> OpenViewEvent;

	[Header("Projectile Motion Metrics")]
	public TextMeshProUGUI MaximumHeightProblemStatusText;
	public TextMeshProUGUI MaximumHeightProblemIncorrectNumText;
	public TextMeshProUGUI HorizontalRangeProblemStatusText;
	public TextMeshProUGUI HorizontalRangeProblemIncorrectNumText;
	public TextMeshProUGUI TimeOfFlightProblemStatusText;
	public TextMeshProUGUI TimeOfFlightProblemIncorrectNumText;
	[Header("Circular Motion Metrics")]
	public TextMeshProUGUI CentripetalAccelerationProblemStatusText;
	public TextMeshProUGUI CentripetalAccelerationProblemIncorrectNumText;

	private void OnEnable()
	{
		OpenViewEvent?.Invoke(this);
	}

	public void ReturnButtonClick()
	{
		SceneManager.LoadScene("Main Menu");
	}
}