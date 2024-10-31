using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Main Menu Screens")]
    [SerializeField] private TitleScreen titleScreen;
    [SerializeField] private GameObject lessonSelectScreen;
    [SerializeField] private GameObject optionsScreen;
    [SerializeField] private GameObject creditsScreen;

    [Header("Lesson Components Screens")]
	[SerializeField] private GameObject lessonOneComponentsScreen;
    [SerializeField] private GameObject lessonTwoComponentsScreen;
    [SerializeField] private GameObject lessonThreeComponentsScreen;
    [SerializeField] private GameObject lessonFourComponentsScreen;
    [SerializeField] private GameObject lessonFiveComponentsScreen;
    [SerializeField] private GameObject lessonSixComponentsScreen;
    [SerializeField] private GameObject lessonSevenComponentsScreen;
    [SerializeField] private GameObject lessonEightComponentsScreen;
    [SerializeField] private GameObject lessonNineComponentsScreen;

    [Header("Lesson Difficulty Select Overlays")]
    [SerializeField] private GameObject lessonOneDifficultySelectOverlay;
    [SerializeField] private GameObject lessonTwoDifficultySelectOverlay;
    [SerializeField] private GameObject lessonThreeDifficultySelectOverlay;
    [SerializeField] private GameObject lessonFourDifficultySelectOverlay;
    [SerializeField] private GameObject lessonFiveDifficultySelectOverlay;
    [SerializeField] private GameObject lessonSixDifficultySelectOverlay;
    [SerializeField] private GameObject lessonSevenDifficultySelectOverlay;
    [SerializeField] private GameObject lessonEightDifficultySelectOverlay;
    [SerializeField] private GameObject lessonNineDifficultySelectOverlay;

    // Lesson Components Dictionary
    private Dictionary<int, GameObject> lessonComponentKeyValuePairs;
    // Lesson Difficulty Select Overlays Dictionary
    private Dictionary<int, GameObject> lessonDifficultyKeyValuePairs;


    private string firstName;
    private string lastName;
    private string section;

    private void Start()
	{

        // Initialize Lesson Components Screens Key Value Pairs
        lessonComponentKeyValuePairs = new Dictionary<int, GameObject>()
        {
            {1, lessonOneComponentsScreen},
            {2, lessonTwoComponentsScreen},
            {3, lessonThreeComponentsScreen},
            {4, lessonFourComponentsScreen},
            {5, lessonFiveComponentsScreen},
            {6, lessonSixComponentsScreen},
            {7, lessonSevenComponentsScreen},
            {8, lessonEightComponentsScreen},
            {9, lessonNineComponentsScreen}
        };

        // Initialize Lesson Difficulty Select Screens Key Value Pairs
        lessonDifficultyKeyValuePairs = new Dictionary<int, GameObject>()
        {
            {1, lessonOneDifficultySelectOverlay },
            {2, lessonTwoDifficultySelectOverlay },
            {3, lessonThreeDifficultySelectOverlay },
            {4, lessonFourDifficultySelectOverlay },
            {5, lessonFiveDifficultySelectOverlay},
            {6, lessonSixDifficultySelectOverlay},
            {7, lessonSevenDifficultySelectOverlay},
            {8, lessonEightDifficultySelectOverlay},
            {9, lessonNineDifficultySelectOverlay}
        };

        firstName = "Superman";
        lastName = "Balatayo";
        section = "4";
        titleScreen.SetUserProfile(firstName, lastName, section);

        LessonSelectButton.LessonSelectClick += OpenLessonComponentsScreen;
        TopicDiscussionButton.TopicDiscussionClick += OpenTopicDiscussionScene;
        ActivityButton.ActivityClick += OpenDifficultySelectOverlay;
        ActivityDifficultyButton.DifficultyClick += OpenActivityWithDifficultyType;
        OverlayCloseButton.OverlayCloseClicked += CloseDifficultySelectOverlay;
	}

    private void OnDisable()
    {
        LessonSelectButton.LessonSelectClick -= OpenLessonComponentsScreen;
        TopicDiscussionButton.TopicDiscussionClick -= OpenTopicDiscussionScene;
        ActivityButton.ActivityClick -= OpenDifficultySelectOverlay;
        ActivityDifficultyButton.DifficultyClick -= OpenActivityWithDifficultyType;
        OverlayCloseButton.OverlayCloseClicked -= CloseDifficultySelectOverlay;
    }

    private void OpenLessonComponentsScreen(int keyValue)
    {
        lessonSelectScreen.SetActive(false);
        lessonComponentKeyValuePairs[keyValue].SetActive(true);
    }

    private void OpenTopicDiscussionScene(int topicDiscussionNumber)
    {
        SceneManager.LoadScene(topicDiscussionNumber);
    }

    private void OpenDifficultySelectOverlay(int keyValue)
    {
        lessonDifficultyKeyValuePairs[keyValue].SetActive(true);
    }

    private void OpenActivityWithDifficultyType(Activity activity, Difficulty difficulty)
    {
        switch (activity)
        {
            case Activity.ActivityOne:
                ActivityOneManager.difficultyConfiguration = difficulty;
                SceneManager.LoadScene("Activity 1");
                break;

            case Activity.ActivityTwo:
                ActivityTwoManager.difficultyConfiguration = difficulty;
                SceneManager.LoadScene("Activity 2");
                break;

            case Activity.ActivityThree:
                //ActivityThreeManager.difficultyConfiguration = difficulty;
                //SceneManager.LoadScene("Activity 3");
                Debug.Log("Activity Three Development is still in Progress");
                break;

            case Activity.ActivityFour:
                //ActivityFourManager.difficultyConfiguration = difficulty;
                //SceneManager.LoadScene("Activity 4");
                Debug.Log("Activity Four Development is still in Progress");
                break;

            case Activity.ActivityFive:
                ActivityFiveManager.difficultyConfiguration = difficulty;
                SceneManager.LoadScene("Activity 5");
                break;

            case Activity.ActivitySix:
                ActivitySixManager.difficultyConfiguration = difficulty;
                SceneManager.LoadScene("Activity 6");
                break;

            case Activity.ActivitySeven:
                ActivitySevenManager.difficultyConfiguration = difficulty;
                SceneManager.LoadScene("Activity 7");
                break;

            case Activity.ActivityEight:
                ActivityEightManager.difficultyConfiguration = difficulty;
                SceneManager.LoadScene("Activity 8");
                break;

            case Activity.ActivityNine:
                ActivityNineManager.difficultyConfiguration = difficulty;
                SceneManager.LoadScene("Activity 9");
                break;
        }
    }

    private void CloseDifficultySelectOverlay(int lessonNumber)
    {
        if (lessonDifficultyKeyValuePairs[lessonNumber].gameObject.activeSelf)
        {
            lessonDifficultyKeyValuePairs[lessonNumber].gameObject.SetActive(false);
        }
        else
        {
            Debug.Log(lessonDifficultyKeyValuePairs[lessonNumber].gameObject + "is already not active. Maybe there's something wrong with the indexing?");
        }
    }
}
