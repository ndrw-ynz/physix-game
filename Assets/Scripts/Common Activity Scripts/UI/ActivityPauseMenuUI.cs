using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivityPauseMenuUI : MonoBehaviour
{
	[SerializeField] protected InputReader inputReader;

	[Header("Activity Manager")]
	[SerializeField] private ActivityManager activityManager;

	[Header("Display Text")]
	[SerializeField] private TextMeshProUGUI headerText;
	[SerializeField] private TextMeshProUGUI tasksText;
	[SerializeField] private TextMeshProUGUI objectivesText;

	[Header("Interactable Pause Menu Buttons")]
    [SerializeField] private Button resumeActivityButton;
	[SerializeField] private Button topicDiscussionButton;
	[SerializeField] private Button quitActivityButton;

	private void Start()
	{
		inputReader.ResumeGameplayEvent += ResumeActivity;

		resumeActivityButton.onClick.AddListener(() => ResumeActivity());
		topicDiscussionButton.onClick.AddListener(() => GoToTopicDiscussion());
		quitActivityButton.onClick.AddListener(() => QuitActivity());
	}

	public void UpdateContent(string headerText, List<string> tasksText, List<string> objectivesText)
	{
		this.headerText.text = headerText;

		this.tasksText.text = "";
		foreach (string activityTask in tasksText)
		{
			this.tasksText.text += $"{activityTask}\n";
		}

		this.objectivesText.text = "";
		foreach (string activityTask in objectivesText)
		{
			this.objectivesText.text += $"{activityTask}\n";
		}
	}

	public void ResumeActivity()
	{
		inputReader.SetGameplayPreviousState();
		gameObject.SetActive(false);
	}

	public void GoToTopicDiscussion()
	{
		
	}

	public void QuitActivity()
	{
		inputReader.SetUI();
		gameObject.SetActive(false);
		activityManager.DisplayPerformanceView();
	}
}