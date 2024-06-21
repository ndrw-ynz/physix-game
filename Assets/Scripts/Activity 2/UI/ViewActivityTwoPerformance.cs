using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewActivityTwoPerformance : MonoBehaviour
{
    public static event Action<ViewActivityTwoPerformance> OpenViewEvent;

	[Header("Quantities Metrics")]
    public TextMeshProUGUI QuantitiesStatusText;
	public TextMeshProUGUI QuantitiesIncorrectNumText;
	[Header("Cartesian Components Metrics")]
	public TextMeshProUGUI CartesianComponentsStatusText;
	public TextMeshProUGUI CartesianComponentsIncorrectNumText;
	[Header("Vector Addition Metrics")]
	public TextMeshProUGUI VectorAdditionStatusText;
	public TextMeshProUGUI VectorAdditionIncorrectNumText;
	private void OnEnable()
	{
		OpenViewEvent?.Invoke(this);
	}

	public void ReturnButtonClick()
	{
		SceneManager.LoadScene("Main Menu");
	}
}