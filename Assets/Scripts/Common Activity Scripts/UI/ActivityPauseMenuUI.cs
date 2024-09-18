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

	private List<Button> interactableButtons;
	private int selectedButtonIndex;

	private void Start()
	{
		inputReader.ResumeGameplayEvent += ResumeActivity;
		inputReader.PauseMenuNavigationEvent += HandleButtonNavigationChange;

		InitializeInteractableButtons();
	}

	private void InitializeInteractableButtons()
	{
		interactableButtons = new List<Button>
		{
			resumeActivityButton,
			topicDiscussionButton,
			quitActivityButton
		};
		resumeActivityButton.onClick.AddListener(() => ResumeActivity());
		topicDiscussionButton.onClick.AddListener(() => GoToTopicDiscussion());
		quitActivityButton.onClick.AddListener(() => QuitActivity());
	}

	private void HandleButtonNavigationChange(Vector2 obj)
	{
		selectedButtonIndex = (selectedButtonIndex - (int) obj.y + interactableButtons.Count) % interactableButtons.Count;
		UpdateSelectedButtonState(selectedButtonIndex);
	}

	private void UpdateSelectedButtonState(int selectedIndex = 0)
	{
		selectedButtonIndex = selectedIndex;

		for (int i = 0; i < interactableButtons.Count; i++)
		{
			TextMeshProUGUI currentButtonText = interactableButtons[i].GetComponent<TextMeshProUGUI>();
			if (i == selectedButtonIndex)
			{
				currentButtonText.fontSize = 86;
				currentButtonText.fontStyle = FontStyles.Bold;
			}
			else
			{
				currentButtonText.fontSize = 56;
				currentButtonText.fontStyle = FontStyles.Normal;
			}
		}
	}

	public void UpdateContent(string headerText, List<string> tasksText, List<string> objectivesText)
	{
		if (interactableButtons == null) InitializeInteractableButtons();
		UpdateSelectedButtonState();

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