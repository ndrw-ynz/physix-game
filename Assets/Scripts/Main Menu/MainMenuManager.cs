using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Main Menu Screens")]
    [SerializeField] private GameObject titleScreen;
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

        LessonSelectButton.LessonSelectClick += OpenLessonComponentsScreen;
        TopicDiscussionButton.TopicDiscussionClick += OpenTopicDiscussionScene;
	}

    private void OnDisable()
    {
        LessonSelectButton.LessonSelectClick -= OpenLessonComponentsScreen;
        TopicDiscussionButton.TopicDiscussionClick -= OpenTopicDiscussionScene;
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
}
