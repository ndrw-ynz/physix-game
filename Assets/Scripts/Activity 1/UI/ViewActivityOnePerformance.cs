using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ViewActivityOnePerformance : MonoBehaviour
{
	public static event Action<ViewActivityOnePerformance> OpenViewEvent;
	public TextMeshProUGUI SNStatusText;
	public TextMeshProUGUI SNIncorrectNumText;
	public TextMeshProUGUI APStatusText;
	public TextMeshProUGUI APIncorrectNumText;
	public TextMeshProUGUI VarianceStatusText;
	public TextMeshProUGUI VarianceIncorrectNumText;
	public TextMeshProUGUI ErrorsStatusText;
	public TextMeshProUGUI ErrorsIncorrectNumText;

	private void OnEnable()
	{
		OpenViewEvent?.Invoke(this);
	}

	public void Submit()
	{
		SceneManager.LoadScene("Main Menu");
	}
}
