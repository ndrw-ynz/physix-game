using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalcSubmissionModalWindow : MonoBehaviour
{
	public static event Action InitiateNext;
	public static event Action RetrySubmission;

	[Header("Images")]
	public Image modalWindowBorderImage;
	[Header("Display Text")]
	public TextMeshProUGUI statusText;
	[Header("Buttons")]
	public Button nextButton;
	public Button tryAgainButton;

	public void SetDisplayFromSubmissionResult(bool isCorrect, string prependText)
	{
		// Set modal window border image color, status text, and buttons
		if (isCorrect)
		{
			modalWindowBorderImage.color = new Color32(175, 255, 155, 255);
			statusText.text = $"{prependText} submission is correct!";
			nextButton.gameObject.SetActive(true);
			tryAgainButton.gameObject.SetActive(false);
		}
		else
		{
			modalWindowBorderImage.color = new Color32(200, 75, 55, 255);
			statusText.text = $"{prependText} submission is incorrect!";
			nextButton.gameObject.SetActive(false);
			tryAgainButton.gameObject.SetActive(true);
		}
	}

	private void OnEnable()
	{
		nextButton.onClick.AddListener(() => InitiateNext?.Invoke());
		tryAgainButton.onClick.AddListener(() => RetrySubmission?.Invoke());
	}

	private void OnDisable()
	{
		nextButton.onClick.RemoveAllListeners();
		nextButton.onClick.RemoveAllListeners();
	}
}