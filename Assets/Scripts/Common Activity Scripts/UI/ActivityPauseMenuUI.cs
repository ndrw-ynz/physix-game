using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitActivityButton;

	[Header("Overlay Screens")]
	[SerializeField] private GameObject optionsScreen;

	[Header("Locked Grey Images")]
	[SerializeField] private Image topicDiscussionLockImage;

	[Header("Activity Event System")]
	[SerializeField] private EventSystem eventSystem;

	[Header("Topic Discussion To Load")]
	[SerializeField] private int discussionToLoad;

	private List<Button> interactableButtons;
	private int selectedButtonIndex;
	private Difficulty currentDifficulty;

	private void Start()
	{
		inputReader.ResumeGameplayEvent += ResumeActivity;
		inputReader.PauseMenuNavigationEvent += HandleButtonNavigationChange;
		inputReader.PauseMenuSelectChoiceEvent += HandleMenuSelectChoice;

		GetCurrentDifficulty();
        InitializeInteractableButtons();

		TopicDiscussionManager.BackToActivityEvent += HandleBackToActivity;
	}

    private void OnDestroy()
    {
        inputReader.ResumeGameplayEvent -= ResumeActivity;
        inputReader.PauseMenuNavigationEvent -= HandleButtonNavigationChange;
        inputReader.PauseMenuSelectChoiceEvent -= HandleMenuSelectChoice;

        TopicDiscussionManager.BackToActivityEvent -= HandleBackToActivity;
    }

    private void InitializeInteractableButtons()
    {
		if (currentDifficulty == Difficulty.Hard)
		{
            interactableButtons = new List<Button>
            {
                    resumeActivityButton,
                    optionsButton,
                    quitActivityButton
            };

            resumeActivityButton.onClick.RemoveAllListeners();
            resumeActivityButton.onClick.AddListener(() => ResumeActivity());

			topicDiscussionLockImage.gameObject.SetActive(true);

            topicDiscussionButton.enabled = false;

            optionsButton.onClick.RemoveAllListeners();
			optionsButton.onClick.AddListener(() => OpenOptionsScreen());

            quitActivityButton.onClick.RemoveAllListeners();
            quitActivityButton.onClick.AddListener(() => QuitActivity());
        }
		else
		{
            interactableButtons = new List<Button>
            {
                    resumeActivityButton,
                    topicDiscussionButton,
                    optionsButton,
                    quitActivityButton
            };

            resumeActivityButton.onClick.RemoveAllListeners();
            resumeActivityButton.onClick.AddListener(() => ResumeActivity());

            topicDiscussionButton.onClick.RemoveAllListeners();
            topicDiscussionButton.onClick.AddListener(() => GoToTopicDiscussion());

            optionsButton.onClick.RemoveAllListeners();
            optionsButton.onClick.AddListener(() => OpenOptionsScreen());

            quitActivityButton.onClick.RemoveAllListeners();
            quitActivityButton.onClick.AddListener(() => QuitActivity());
        }

        
    }

    private void HandleButtonNavigationChange(Vector2 obj)
	{
		selectedButtonIndex = (selectedButtonIndex - (int) obj.y + interactableButtons.Count) % interactableButtons.Count;
		UpdateSelectedButtonState(selectedButtonIndex);
	}

	private void HandleMenuSelectChoice()
	{
		interactableButtons[selectedButtonIndex].onClick.Invoke();
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
		if (optionsScreen.gameObject.activeSelf)
		{
			optionsScreen.gameObject.SetActive(false);
		}

		inputReader.SetGameplayPreviousState();
		gameObject.SetActive(false);
	}

	public void GoToTopicDiscussion()
	{
        eventSystem.gameObject.SetActive(false);
        inputReader.SetInActivityTopicDiscussion();
        TopicDiscussionManager.isActivitySceneActive = true;

        SceneManager.LoadScene($"Topic Discussion {discussionToLoad}", LoadSceneMode.Additive);
    }

	public void OpenOptionsScreen()
	{
		optionsScreen.gameObject.SetActive(true);
	}

	public void QuitActivity()
	{
		inputReader.SetUI();
		gameObject.SetActive(false);
		activityManager.DisplayPerformanceView();
	}

	private void HandleBackToActivity()
	{
        eventSystem.gameObject.SetActive(true);
        inputReader.SetGameplayPauseMenu();
    }

	private void GetCurrentDifficulty()
	{
		switch (discussionToLoad)
		{
			case 1:
				currentDifficulty = ActivityOneManager.difficultyConfiguration;
				break;

			case 2:
                currentDifficulty = ActivityTwoManager.difficultyConfiguration;
                break;

            case 3:
                currentDifficulty = ActivityThreeManager.difficultyConfiguration;
                break;

            case 4:
                currentDifficulty = ActivityFourManager.difficultyConfiguration;
                break;

            case 5:
                currentDifficulty = ActivityFiveManager.difficultyConfiguration;
                break;

            case 6:
                currentDifficulty = ActivitySixManager.difficultyConfiguration;
                break;

            case 7:
                currentDifficulty = ActivitySevenManager.difficultyConfiguration;
                break;

            case 8:
                currentDifficulty = ActivityEightManager.difficultyConfiguration;
                break;

            case 9:
                currentDifficulty = ActivityNineManager.difficultyConfiguration;
                break;
        }
	}
}