using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubmissionStatusDisplay : MonoBehaviour
{
    [Header("Status Border Display")]
    [SerializeField] private Image statusBorderDisplay;

    [Header("Display Text")]
    [SerializeField] private TextMeshProUGUI statusDescriptionText;

    [Header("Buttons")]
    [SerializeField] protected Button proceedButton;
	[SerializeField] private Button fixErrorButton;

	public void SetSubmissionStatus(bool isCorrect, string descriptionText)
	{
		if (isCorrect)
		{
			statusBorderDisplay.color = new Color32(175, 255, 155, 255);
			statusDescriptionText.text = descriptionText;
			proceedButton.gameObject.SetActive(true);
			fixErrorButton.gameObject.SetActive(false);
		} else
		{
			statusBorderDisplay.color = new Color32(200, 75, 55, 255);
			statusDescriptionText.text = descriptionText;
			proceedButton.gameObject.SetActive(false);
			fixErrorButton.gameObject.SetActive(true);
		}
	}

	protected virtual void OnEnable()
	{
		proceedButton.onClick.AddListener(() => {
			SceneSoundManager.Instance.PlaySFX("Click");

			transform.gameObject.SetActive(false);
		});

		fixErrorButton.onClick.AddListener(() => {
			SceneSoundManager.Instance.PlaySFX("Click");

			transform.gameObject.SetActive(false); 
		});
	}

	protected virtual void OnDisable()
	{
		proceedButton.onClick.RemoveAllListeners();
		fixErrorButton.onClick.RemoveAllListeners();
	}
}