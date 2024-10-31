using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Main Menu Screens")]
    [SerializeField] private TitleScreen titleScreen;
    [SerializeField] private LessonSelectScreen lessonSelectScreen;
    [SerializeField] private LessonComponentsScreens lessonComponentsScreens;
    [SerializeField] private DifficultySelectOverlays difficultySelectOverlays;
    [SerializeField] private GameObject optionsScreen;
    [SerializeField] private GameObject creditsScreen;

    // User values to be loaded from firestore database
    private string firstName;
    private string lastName;
    private string section;
    private int highestUnlockedLesson;
    private int highestLessonUnlockedDifficulties;
    private void Start()
	{

        // Setup mock user profile and set it up in the title screen
        // Change first name, last name, and section into the loaded user value in the future
        firstName = "Superman";
        lastName = "Balatayo";
        section = "4";
        titleScreen.SetUserProfile(firstName, lastName, section);

        // Setup current user's mock highest unlocked lesson and load all unlocked lessons
        // 1 is the lowest unlocked lesson and 9 is the highest unlocked lesson
        // Change into the loaded user highest unlocked lesson value in the future
        highestUnlockedLesson = 1;
        if (highestUnlockedLesson > 0)
        {
            lessonSelectScreen.LockAllLessons();
            lessonSelectScreen.LoadUnlockedLessons(highestUnlockedLesson);
        }

        // Setup current user's mock highest lesson unlocked level and load all lesson's difficulties
        // 1 is the lowest unlocked difficulty(Easy) and 3 is the highest unlocked difficulty(Hard)
        // Change into the loaded user highest highest lesson unlocked difficulties value in the future
        highestLessonUnlockedDifficulties = 1;
        if (highestLessonUnlockedDifficulties > 0)
        {
            difficultySelectOverlays.LockAllDifficulty();
            difficultySelectOverlays.LoadLessonDifficultyButtons(highestUnlockedLesson, highestLessonUnlockedDifficulties);
        }

        // Add event listeners
        LessonSelectButton.LessonSelectClick += OpenLessonComponentsScreen;
        TopicDiscussionButton.TopicDiscussionClick += OpenTopicDiscussionScene;
        ActivityButton.ActivityClick += OpenDifficultySelectOverlay;
        ActivityDifficultyButton.DifficultyClick += OpenActivityWithDifficultyType;
        OverlayCloseButton.OverlayCloseClicked += CloseDifficultySelectOverlay;
	}

    private void OnDisable()
    {
        // Remove event listeners
        LessonSelectButton.LessonSelectClick -= OpenLessonComponentsScreen;
        TopicDiscussionButton.TopicDiscussionClick -= OpenTopicDiscussionScene;
        ActivityButton.ActivityClick -= OpenDifficultySelectOverlay;
        ActivityDifficultyButton.DifficultyClick -= OpenActivityWithDifficultyType;
        OverlayCloseButton.OverlayCloseClicked -= CloseDifficultySelectOverlay;
    }

    private void OpenLessonComponentsScreen(int keyValue)
    {
        // Close lesson select screen and open specified lesson components screen
        lessonSelectScreen.gameObject.SetActive(false);
        lessonComponentsScreens.LoadLessonComponentsScreen(keyValue);
    }

    private void OpenTopicDiscussionScene(int topicDiscussionNumber)
    {
        // Load specified topic discussion
        SceneManager.LoadScene(topicDiscussionNumber);
    }

    private void OpenDifficultySelectOverlay(int keyValue)
    {
        // Load specified lesson's difficulty select overlay
        difficultySelectOverlays.LoadActivityDifficultyOverlay(keyValue);
    }

    private void OpenActivityWithDifficultyType(Activity activity, Difficulty difficulty)
    {
        // Set proper difficulty of the activity manager and load scene of the specified activity
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
        // Close current difficulty select overlay
        difficultySelectOverlays.CloseDifficultySelectOverlay(lessonNumber);
    }
}