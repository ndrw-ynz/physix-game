using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class TopicDiscussionManager : MonoBehaviour
{
    public static event Action BackToActivityEvent;

    // Current sector and page index values
    public static int currentSectorIndex;
    public static int currentPageIndex;

    // Is there an active activity scene
    public static bool isActivitySceneActive;

    [Header("Game Objects with Attached Display Scripts")]
    [SerializeField] private DiscussionPagesDisplay discussionPagesDisplay;
    [SerializeField] private ProgressBarsDisplay progressDisplay;
    [SerializeField] private PageJumpButtonsDisplay pageJumpDisplay;
    [SerializeField] private PreviousNextButtonsDisplay previousNextButtonsDisplay;
    [SerializeField] private ReadIndicatorsDisplay readIndicatorsDisplay;
    [SerializeField] private SceneNavigationButtons sceneNavigationDisplay;

    [Header("Difficulty Select Overlay")]
    [SerializeField] private DiscussionDifficultySelectOverlay discussionDifficultySelectOverlay;

    [Header("Current Topic Discussion Number")]
    [SerializeField] private int _topicDiscussionNumber;

    [Header("Audio Source")]
    [SerializeField] private AudioListener audioListener;

    // Current topic discussion's sub topics list count
    private int _subTopicsListCount;

    // Read pages map data to be loaded from cloud firestore database
    private Dictionary<string, List<int>> _readPagesMapData;
    private void Start()
    {
		// Get read pages data from database and update UI according to the data
		GetReadPagesDataFromDB();

        // Activate only the back to game button when an activity scene is active
        if (isActivitySceneActive)
        {
            audioListener.gameObject.SetActive(false);
            sceneNavigationDisplay.ActivateBackToGameButtonOnly();
        }
        else
        {
            SceneSoundManager.Instance.PlayMusic("With love from Vertex Studio (8)");
            discussionDifficultySelectOverlay.LockDifficultyButtons();
            discussionDifficultySelectOverlay.ActivateDifficultyButtons(_topicDiscussionNumber);
        }

        // Get the sub topics' list count
        _subTopicsListCount = discussionPagesDisplay.GetSubTopicListCount();

        // Load the current sector and page of the topic discussion
        discussionPagesDisplay.ChangePage(currentSectorIndex, currentPageIndex);

        // Activate the proper previous and next button(s) based from the current sector and page of the topic discussion
        ChangePreviousAndNextButtonState();

        // Add button click listeners
        PagePrevNextButton.PagePrevNextClickEvent += HandlePrevNextClick;
        SectorPrevNextButton.SectorPrevNextClickEvent += HandlePrevNextClick;
        ProgressBarButton.ProgressBarClickEvent += HandleProgressBarClick;
        PageJumpButton.PageCircleClick += HandlePageCircleClick;
        ReadIndicatorButton.ReadIndicatorClickEvent += HandleReadIndicatorClick;
        DiscussionMainMenuButton.BackToMainMenuClickEvent += HandleBackToMainMenuClick;
        DiscussionActivityButton.StartActivityClickEvent += HandleStartActivityClick;
        DiscussionBackToActivityButton.BackToActivityClickEvent += HandleBackToGameClick;
        ActivityDifficultyButton.DifficultyClick += HandleDifficultySelectClick;
        OverlayCloseButton.OverlayCloseClicked += CloseCurrentOverlay;
    }

    private void OnDisable()
    {
        currentSectorIndex = 0;
        currentPageIndex = 0;

        isActivitySceneActive = false;

        // Remove button click listeners
        PagePrevNextButton.PagePrevNextClickEvent -= HandlePrevNextClick;
        SectorPrevNextButton.SectorPrevNextClickEvent -= HandlePrevNextClick;
        ProgressBarButton.ProgressBarClickEvent -= HandleProgressBarClick;
        PageJumpButton.PageCircleClick -= HandlePageCircleClick;
        ReadIndicatorButton.ReadIndicatorClickEvent -= HandleReadIndicatorClick;
        DiscussionMainMenuButton.BackToMainMenuClickEvent -= HandleBackToMainMenuClick;
        DiscussionActivityButton.StartActivityClickEvent -= HandleStartActivityClick;
        DiscussionBackToActivityButton.BackToActivityClickEvent -= HandleBackToGameClick;
        ActivityDifficultyButton.DifficultyClick -= HandleDifficultySelectClick;
        OverlayCloseButton.OverlayCloseClicked -= CloseCurrentOverlay;
    }

    #region Function for Event Handling
    private void HandlePrevNextClick(Direction direction)
    {
        // Change current sector and page index values based on direction
        switch (direction)
        {
            case Direction.PreviousPage:
                currentPageIndex -= 1;

                discussionPagesDisplay.ChangePage(currentSectorIndex, currentPageIndex);
                ChangePreviousAndNextButtonState();

                ChangeReadIndicatorButtonState();

                pageJumpDisplay.UpdatePageJumpButtonsOutline(currentPageIndex);
                break;

            case Direction.NextPage:
                currentPageIndex += 1;

                discussionPagesDisplay.ChangePage(currentSectorIndex, currentPageIndex);

                ChangePreviousAndNextButtonState();

                ChangeReadIndicatorButtonState();

                pageJumpDisplay.UpdatePageJumpButtonsOutline(currentPageIndex);
                break;

            case Direction.PreviousSector:
                discussionPagesDisplay.CloseCurrentPage(currentSectorIndex, currentPageIndex);

                int previousSectorLastPageIndex = discussionPagesDisplay.GetCurrentSectorPagesCount((currentSectorIndex) - 1) - 1;
                currentSectorIndex -= 1;
                currentPageIndex = previousSectorLastPageIndex;

                discussionPagesDisplay.ChangePage(currentSectorIndex, currentPageIndex);

                ChangePreviousAndNextButtonState();

                ChangeReadIndicatorButtonState();

                progressDisplay.UpdateIndicatorLinePosition(currentSectorIndex);

                LoadPageJumpButtons();
                pageJumpDisplay.UpdatePageJumpButtonsOutline(currentPageIndex);
                UpdatePageJumpButtonColors();
                break;

            case Direction.NextSector:
                discussionPagesDisplay.CloseCurrentPage(currentSectorIndex, currentPageIndex);

                int nextSectorFirstPageIndex = 0;
                currentSectorIndex += 1;
                currentPageIndex = nextSectorFirstPageIndex;

                discussionPagesDisplay.ChangePage(currentSectorIndex, currentPageIndex);

                ChangePreviousAndNextButtonState();

                ChangeReadIndicatorButtonState();

                progressDisplay.UpdateIndicatorLinePosition(currentSectorIndex);

                LoadPageJumpButtons();
                pageJumpDisplay.UpdatePageJumpButtonsOutline(currentPageIndex);
                UpdatePageJumpButtonColors();
                break;
        }
    }
    private void HandleProgressBarClick(int sectorIndex)
    {
        // Close current page and jump to the specified sector index if not sectorIndex is not the current sector
        if (currentSectorIndex != sectorIndex)
        {
            discussionPagesDisplay.CloseCurrentPage(currentSectorIndex, currentPageIndex);

            currentSectorIndex = sectorIndex;
            currentPageIndex = 0;

            discussionPagesDisplay.ChangePage(currentSectorIndex, currentPageIndex);

            ChangePreviousAndNextButtonState();

            ChangeReadIndicatorButtonState();

            progressDisplay.UpdateIndicatorLinePosition(currentSectorIndex);
            LoadPageJumpButtons();

            pageJumpDisplay.UpdatePageJumpButtonsOutline(currentPageIndex);
            UpdatePageJumpButtonColors();
        }
    }
    private void HandlePageCircleClick(int pageIndex)
    {
        // Jump to a specified page if not pageIndex is not the current page
        if (currentPageIndex != pageIndex)
        {
            currentPageIndex = pageIndex;
            discussionPagesDisplay.ChangePage(currentSectorIndex, pageIndex);

            ChangePreviousAndNextButtonState();

            ChangeReadIndicatorButtonState();

            pageJumpDisplay.UpdatePageJumpButtonsOutline(currentPageIndex);
        }
    }
    private void HandleReadIndicatorClick(ReadState readState)
    {
        /* Change read state, read indicator button state. 
         * Then update progress bar text and color and page jump button colors */
        discussionPagesDisplay.ChangeReadState(readState, currentSectorIndex, currentPageIndex);

        ChangeReadIndicatorButtonState();

        UpdateProgressBarButtonTextAndColor();

        UpdatePageJumpButtonColors();
    }
    private void CloseCurrentOverlay(int activityNumber)
    {
        discussionDifficultySelectOverlay.gameObject.SetActive(false);
    }
    #endregion

    #region Discussion Scene Close Related Buttons
    private void HandleBackToMainMenuClick()
    {
        StartCoroutine(BackToMenuSequence());
    }
    private void HandleStartActivityClick()
    {
        // Opens Difficulty Select Overlay
        discussionDifficultySelectOverlay.gameObject.SetActive(true);
    }

    private void HandleDifficultySelectClick(Activity activity, Difficulty difficulty)
    {
        // Save the marked pages data and load activity based on difficulty
        StartCoroutine(StartActivitySequence(activity, difficulty));
    }

    private void HandleBackToGameClick()
    {
        StartCoroutine(BackToGameSequence());
    }
    // Used for handling the sequences of going back to main menu while also saving current pages data
    private IEnumerator BackToMenuSequence()
    {
        yield return StartCoroutine(SaveReadPagesDataToDB());
        sceneNavigationDisplay.LoadMainMenu();
    }
    // Used for handling the sequences of starting the activity while also saving current pages data
    private IEnumerator StartActivitySequence(Activity activity, Difficulty difficulty)
    {
        yield return StartCoroutine(SaveReadPagesDataToDB());
        OpenActivityWithDifficultyType(activity, difficulty);
    }

    // Used for handling the sequences of going back to current activity while also saving current pages data
    private IEnumerator BackToGameSequence()
    {
        yield return StartCoroutine(SaveReadPagesDataToDB());
        EventSystem.current.gameObject.SetActive(false);
        BackToActivityEvent?.Invoke();
        sceneNavigationDisplay.CloseCurrentDiscussion(_topicDiscussionNumber);
    }

    // Helper function of starting activity with difficulty
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
                ActivityThreeManager.difficultyConfiguration = difficulty;
                SceneManager.LoadScene("Activity 3");
                break;

            case Activity.ActivityFour:
                ActivityFourManager.difficultyConfiguration = difficulty;
                SceneManager.LoadScene("Activity 4");
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
    #endregion

    #region Helper Functions of Events
    private void ChangePreviousAndNextButtonState()
    {
        /* Set the current sector's pages count, previous sector title,next sector title, and update
           and update previous and next button state*/
        int currentSectorPagesCount = discussionPagesDisplay.GetCurrentSectorPagesCount(currentSectorIndex);
        string previousSectorTitle = currentSectorIndex > 0 ? discussionPagesDisplay.GetPreviousSectorTitle(currentSectorIndex) : null;
        string nextSectorTitle = currentSectorIndex < _subTopicsListCount - 1 ? discussionPagesDisplay.GetNextSectorTitle(currentSectorIndex) : null;
        previousNextButtonsDisplay.ChangePrevNextButtonsState(currentSectorIndex, currentPageIndex, _subTopicsListCount, currentSectorPagesCount, previousSectorTitle, nextSectorTitle);
    }
    private void ChangeReadIndicatorButtonState()
    {
        // Check if page is marked as read and update read indicator state
        bool isPageMarkedRead = discussionPagesDisplay.CurrentPageIsMarkedRead(currentSectorIndex, currentPageIndex);
        readIndicatorsDisplay.ChangeReadIndicatorButtonsState(currentSectorIndex, currentPageIndex, isPageMarkedRead);
    }
    private void LoadProgressBarButtons()
    {
        // Create progress bar buttons with corresponding titles and progress count
        for (int i = 0; i < _subTopicsListCount; i++)
        {
            string sectorTitle = discussionPagesDisplay.GetSectorTitle(i);
            string progressCount = $"{discussionPagesDisplay.CountReadPages(i).ToString()}/{discussionPagesDisplay.CountTotalPages(i).ToString()}";
            progressDisplay.GenerateProgressBarButton(i, sectorTitle, progressCount);
        }

        // Load progress bar button colors
        int progressDisplayLength = progressDisplay.GetProgressBarButtonsLength();
        for (int i = 0; i < progressDisplayLength; i++)
        {
            double currentReadPagesCount = discussionPagesDisplay.CountReadPages(i);
            double currentTotalPagesCount = discussionPagesDisplay.CountTotalPages(i);
            progressDisplay.LoadProgressBarButtonsColors(currentReadPagesCount, currentTotalPagesCount, i);
        }
    }
    private void UpdateProgressBarButtonTextAndColor()
    {
        // Update the current sector's progress bar button color
        double currReadPagesCount = discussionPagesDisplay.CountReadPages(currentSectorIndex);
        double currSectorPagesCount = discussionPagesDisplay.CountTotalPages(currentSectorIndex);
        progressDisplay.UpdateProgressBarButtonTextAndColor(currentSectorIndex, currReadPagesCount, currSectorPagesCount);
    }
    private void LoadPageJumpButtons()
    {
        // Load page jump buttons for the current sector
        int pageJumpButtonsLength = pageJumpDisplay.GetPageJumpButtonsLength();
        if (pageJumpButtonsLength > 0)
        {
            pageJumpDisplay.DestroyImmediateAllPageJumpButtons();
        }

        int currentSectorPagesCount = discussionPagesDisplay.GetCurrentSectorPagesCount(currentSectorIndex);
        // Create buttons for the new sector
        for (int i = 0; i < currentSectorPagesCount; i++)
        {
            pageJumpDisplay.GeneratePageJumpButton(i);
        }
    }
    private void UpdatePageJumpButtonColors()
    {
        // Update page jump button colors
        int pageJumpButtonsLength = pageJumpDisplay.GetPageJumpButtonsLength();
        for (int i = 0; i < pageJumpButtonsLength; i++)
        {
            bool isPageMarkedRead = discussionPagesDisplay.IsPageMarkedRead(currentSectorIndex, i);
            pageJumpDisplay.UpdatePageJumpButtonColor(currentSectorIndex, isPageMarkedRead, i);
        }
    }
    #endregion

    #region Database Functionalities
    // Get read pages data from database
    private void GetReadPagesDataFromDB()
    {
        FirestoreDocument currentDiscussionDocument = UserManager.Instance.DiscussionOneMarkedPagesData;
        string currentUserLocalID = UserManager.Instance.CurrentUser.localId;

        switch (_topicDiscussionNumber)
        {
            case 1:
                StartCoroutine(UserManager.Instance.GetDiscussionPagesProgress(_topicDiscussionNumber, "discussionOnePageProgress", currentUserLocalID, OnPagesGetResult));
                break;
            case 2:
                StartCoroutine(UserManager.Instance.GetDiscussionPagesProgress(_topicDiscussionNumber, "discussionTwoPageProgress", currentUserLocalID, OnPagesGetResult));
                break;
            case 3:
                StartCoroutine(UserManager.Instance.GetDiscussionPagesProgress(_topicDiscussionNumber, "discussionThreePageProgress", currentUserLocalID, OnPagesGetResult));
                break;
            case 4:
                StartCoroutine(UserManager.Instance.GetDiscussionPagesProgress(_topicDiscussionNumber, "discussionFourPageProgress", currentUserLocalID, OnPagesGetResult));
                break;
            case 5:
                StartCoroutine(UserManager.Instance.GetDiscussionPagesProgress(_topicDiscussionNumber, "discussionFivePageProgress", currentUserLocalID, OnPagesGetResult));
                break;
            case 6:
                StartCoroutine(UserManager.Instance.GetDiscussionPagesProgress(_topicDiscussionNumber, "discussionSixPageProgress", currentUserLocalID, OnPagesGetResult));
                break;
            case 7:
                StartCoroutine(UserManager.Instance.GetDiscussionPagesProgress(_topicDiscussionNumber, "discussionSevenPageProgress", currentUserLocalID, OnPagesGetResult));
                break;
            case 8:
                StartCoroutine(UserManager.Instance.GetDiscussionPagesProgress(_topicDiscussionNumber, "discussionEightPageProgress", currentUserLocalID, OnPagesGetResult));
                break;
            case 9:
                StartCoroutine(UserManager.Instance.GetDiscussionPagesProgress(_topicDiscussionNumber, "discussionNinePageProgress", currentUserLocalID, OnPagesGetResult));
                break;
        }
    }

    // Save new read pages data to database
    private IEnumerator SaveReadPagesDataToDB()
    {
        // Create generalize data to be used in all cases
        Dictionary<string, List<int>> recordedMarkedPages = discussionPagesDisplay.RecordReadPagesData();
        List<List<object>> sectors;
        Dictionary<string, FirestoreField> updatedFields;

        //To Do: Add a checker to see if the recordedMarkedPages is == to the current discussion's 

        string currentUserLocalID = UserManager.Instance.CurrentUser.localId;

        switch (_topicDiscussionNumber)
        {
            case 1: // 5 Sectors
                // Create the appropriate amount of lists needed for a discussion's sector count
                sectors = new List<List<object>>()
                {
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                };

                // Process and create the updated fields dictionary
                updatedFields = CreateUpdatedFieldsDictionary(sectors, recordedMarkedPages);
                yield return StartCoroutine(UserManager.Instance.UpdateDiscussionPageProgress(updatedFields, "discussionOnePageProgress", currentUserLocalID));
                break;

            case 2: // 4 Sectors
                // Create the appropriate amount of lists needed for a discussion's sector count
                sectors = new List<List<object>>()
                {
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                };

                // Process and create the updated fields dictionary
                updatedFields = CreateUpdatedFieldsDictionary(sectors, recordedMarkedPages);
                yield return StartCoroutine(UserManager.Instance.UpdateDiscussionPageProgress(updatedFields, "discussionTwoPageProgress", currentUserLocalID));
                break;

            case 3: // 3 Sectors
                // Create the appropriate amount of lists needed for a discussion's sector count
                sectors = new List<List<object>>()
                {
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                };

                // Process and create the updated fields dictionary
                updatedFields = CreateUpdatedFieldsDictionary(sectors, recordedMarkedPages);
                yield return StartCoroutine(UserManager.Instance.UpdateDiscussionPageProgress(updatedFields, "discussionThreePageProgress", currentUserLocalID));
                break;

            case 4: // 3 Sectors
                // Create the appropriate amount of lists needed for a discussion's sector count
                sectors = new List<List<object>>()
                {
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                };

                // Process and create the updated fields dictionary
                updatedFields = CreateUpdatedFieldsDictionary(sectors, recordedMarkedPages);
                yield return StartCoroutine(UserManager.Instance.UpdateDiscussionPageProgress(updatedFields, "discussionFourPageProgress", currentUserLocalID));
                break;

            case 5: // 3 Sectors
                // Create the appropriate amount of lists needed for a discussion's sector count
                sectors = new List<List<object>>()
                {
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                };

                // Process and create the updated fields dictionary
                updatedFields = CreateUpdatedFieldsDictionary(sectors, recordedMarkedPages);
                yield return StartCoroutine(UserManager.Instance.UpdateDiscussionPageProgress(updatedFields, "discussionFivePageProgress", currentUserLocalID));
                break;

            case 6: // 4 Sectors
                // Create the appropriate amount of lists needed for a discussion's sector count
                sectors = new List<List<object>>()
                {
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                };

                // Process and create the updated fields dictionary
                updatedFields = CreateUpdatedFieldsDictionary(sectors, recordedMarkedPages);
                yield return StartCoroutine(UserManager.Instance.UpdateDiscussionPageProgress(updatedFields, "discussionSixPageProgress", currentUserLocalID));
                break;

            case 7: // 4 Sectors
                // Create the appropriate amount of lists needed for a discussion's sector count
                sectors = new List<List<object>>()
                {
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                };

                // Process and create the updated fields dictionary
                updatedFields = CreateUpdatedFieldsDictionary(sectors, recordedMarkedPages);
                yield return StartCoroutine(UserManager.Instance.UpdateDiscussionPageProgress(updatedFields, "discussionSevenPageProgress", currentUserLocalID));
                break;

            case 8: // 4 Sectors
                // Create the appropriate amount of lists needed for a discussion's sector count
                sectors = new List<List<object>>()
                {
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                };

                // Process and create the updated fields dictionary
                updatedFields = CreateUpdatedFieldsDictionary(sectors, recordedMarkedPages);
                yield return StartCoroutine(UserManager.Instance.UpdateDiscussionPageProgress(updatedFields, "discussionEightPageProgress", currentUserLocalID));
                break;

            case 9: // 2 Sectors
                // Create the appropriate amount of lists needed for a discussion's sector count
                sectors = new List<List<object>>()
                {
                    new List<object>(),
                    new List<object>(),
                };

                // Process and create the updated fields dictionary
                updatedFields = CreateUpdatedFieldsDictionary(sectors, recordedMarkedPages);
                yield return StartCoroutine(UserManager.Instance.UpdateDiscussionPageProgress(updatedFields, "discussionNinePageProgress", currentUserLocalID));
                break;
        }
    }

    // Callback for GetReadPagesDataFromDB()
    private void OnPagesGetResult(bool success)
    {
        if (success)
        {
            Debug.Log("Read Pages Data Get Successful");
            Dictionary<string, object> markedPages;
            switch (_topicDiscussionNumber)
            {
                case 1:
                    markedPages = UserManager.Instance.DiscussionOneMarkedPagesData.fields["markedPages"].mapValue;
                    _readPagesMapData = ParseDocumentToDict(markedPages);
                    break;
                case 2:
                    markedPages = UserManager.Instance.DiscussionTwoMarkedPagesData.fields["markedPages"].mapValue;
                    _readPagesMapData = ParseDocumentToDict(markedPages);
                    break;
                case 3:
                    markedPages = UserManager.Instance.DiscussionThreeMarkedPagesData.fields["markedPages"].mapValue;
                    _readPagesMapData = ParseDocumentToDict(markedPages);
                    break;
                case 4:
                    markedPages = UserManager.Instance.DiscussionFourMarkedPagesData.fields["markedPages"].mapValue;
                    _readPagesMapData = ParseDocumentToDict(markedPages);
                    break;
                case 5:
                    markedPages = UserManager.Instance.DiscussionFiveMarkedPagesData.fields["markedPages"].mapValue;
                    _readPagesMapData = ParseDocumentToDict(markedPages);
                    break;
                case 6:
                    markedPages = UserManager.Instance.DiscussionSixMarkedPagesData.fields["markedPages"].mapValue;
                    _readPagesMapData = ParseDocumentToDict(markedPages);
                    break;
                case 7:
                    markedPages = UserManager.Instance.DiscussionSevenMarkedPagesData.fields["markedPages"].mapValue;
                    _readPagesMapData = ParseDocumentToDict(markedPages);
                    break;
                case 8:
                    markedPages = UserManager.Instance.DiscussionEightMarkedPagesData.fields["markedPages"].mapValue;
                    _readPagesMapData = ParseDocumentToDict(markedPages);
                    break;
                case 9:
                    markedPages = UserManager.Instance.DiscussionNineMarkedPagesData.fields["markedPages"].mapValue;
                    _readPagesMapData = ParseDocumentToDict(markedPages);
                    break;
            }
            // Load read pages data into the sub topics list of discussion pages display
            discussionPagesDisplay.LoadReadPagesData(_readPagesMapData);
            // Activate the proper read indicator button based on the current sector and page read state
            ChangeReadIndicatorButtonState();
            // Load all progress bar buttons' texts and colors to the left of the screen based on the amount of sectors of the topic discussion
            LoadProgressBarButtons();
            // Set the indicator's line position to the bottom of the current sector's progress bar button
            progressDisplay.UpdateIndicatorLinePosition(currentSectorIndex);
            // Load all page jump buttons based on the current sector's page count
            LoadPageJumpButtons();
            // Set the page jump button outline to the current page's page jump button
            pageJumpDisplay.UpdatePageJumpButtonsOutline(currentPageIndex);
            // Update the page jump button colors of the current sector based on their read states
            UpdatePageJumpButtonColors();
        }
        else
        {
            StartCoroutine(CreateNewDiscussionPageProgressData());
        }
    }

    // Helper function for creating the dictionary that will be used to load all pages data colors and text
    private Dictionary<string, List<int>> ParseDocumentToDict(Dictionary<string, object> markedPages)
    {
        Dictionary<string, List<int>> sectorArrayPairs = new Dictionary<string, List<int>>();

        foreach (var fields in markedPages)
        {
            // Access each sector in the "fields"
            foreach (var sector in fields.Value as JObject)
            {
                // Extract the array of "values"
                JArray valuesArray = sector.Value["arrayValue"]?["values"] as JArray;

                if (valuesArray != null)
                {
                    // Initialize a List for the sector
                    List<int> sectorValues = new List<int>();

                    // Loop through the values array and extract integer values
                    foreach (var value in valuesArray)
                    {
                        if (value["integerValue"] != null)
                        {
                            // Add the integer value to the HashSet
                            sectorValues.Add(int.Parse(value["integerValue"].ToString()));
                        }
                    }

                    // Store the List in the dictionary
                    sectorArrayPairs[sector.Key] = sectorValues;
                }
            }
        }
        return sectorArrayPairs;
    }
    // Helper function for creating the current updates pages data to be saved in the database
    private Dictionary<string, FirestoreField> CreateUpdatedFieldsDictionary(List<List<object>> sectors, Dictionary<string, List<int>> recordedMarkedPages)
    {
        // Add the values of each array from the recorded marked pages as FirestoreField data
        for (int i = 0; i < sectors.Count; i++)
        {
            foreach (var value in recordedMarkedPages[$"sector{i + 1}"])
            {
                sectors[i].Add(new FirestoreField(value));
            }
        }

        // Create the sectorListPairs dictionary to be used in the markedPages Dictionary
        Dictionary<string, object> sectorListPairs = new Dictionary<string, object>();

        // Add all sector key and array values that were converted to FirestoreField data
        for (int i = 0; i < sectors.Count; i++)
        {
            sectorListPairs[$"sector{i + 1}"] = new { arrayValue = new { values = sectors[i] } };
        }

        Dictionary<string, object> markedPages = new Dictionary<string, object>()
        {
            {"fields", sectorListPairs}
        };

        Dictionary<string, FirestoreField> updatedFields = new Dictionary<string, FirestoreField>()
        {
            {"markedPages", new FirestoreField(markedPages) }
        };

        return updatedFields;
    }
    // Helper function for creating new blank pages data if current user pages data not found
    private Dictionary<string, FirestoreField> CreateBlankFieldsDictionary(List<List<object>> sectors)
    {
        // Create the sectorListPairs dictionary to be used in the markedPages Dictionary
        Dictionary<string, object> sectorListPairs = new Dictionary<string, object>();

        // Add all sector key and array values that were converted to FirestoreField data
        for (int i = 0; i < sectors.Count; i++)
        {
            sectorListPairs[$"sector{i + 1}"] = new { arrayValue = new { values = sectors[i] } };
        }

        Dictionary<string, object> markedPages = new Dictionary<string, object>()
        {
            {"fields", sectorListPairs}
        };

        Dictionary<string, FirestoreField> updatedFields = new Dictionary<string, FirestoreField>()
        {
            {"markedPages", new FirestoreField(markedPages) }
        };

        return updatedFields;
    }

    private IEnumerator CreateNewDiscussionPageProgressData()
    {
        List<List<object>> sectors;
        Dictionary<string, FirestoreField> updatedFields;

        string currentUserLocalID = UserManager.Instance.CurrentUser.localId;
        switch (_topicDiscussionNumber)
        {
            case 1: // 5 Sectors
                    // Create the appropriate amount of lists needed for a discussion's sector count
                sectors = new List<List<object>>()
                {
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                };

                // Process and create the updated fields dictionary
                updatedFields = CreateBlankFieldsDictionary(sectors);
                yield return StartCoroutine(UserManager.Instance.UpdateDiscussionPageProgress(updatedFields, "discussionOnePageProgress", currentUserLocalID));
                break;

            case 2: // 4 Sectors
                    // Create the appropriate amount of lists needed for a discussion's sector count
                sectors = new List<List<object>>()
                {
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                };

                // Process and create the updated fields dictionary
                updatedFields = CreateBlankFieldsDictionary(sectors);
                yield return StartCoroutine(UserManager.Instance.UpdateDiscussionPageProgress(updatedFields, "discussionTwoPageProgress", currentUserLocalID));
                break;

            case 3: // 3 Sectors
                    // Create the appropriate amount of lists needed for a discussion's sector count
                sectors = new List<List<object>>()
                {
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                };

                // Process and create the updated fields dictionary
                updatedFields = CreateBlankFieldsDictionary(sectors);
                yield return StartCoroutine(UserManager.Instance.UpdateDiscussionPageProgress(updatedFields, "discussionThreePageProgress", currentUserLocalID));
                break;

            case 4: // 3 Sectors
                    // Create the appropriate amount of lists needed for a discussion's sector count
                sectors = new List<List<object>>()
                {
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                };

                // Process and create the updated fields dictionary
                updatedFields = CreateBlankFieldsDictionary(sectors);
                yield return StartCoroutine(UserManager.Instance.UpdateDiscussionPageProgress(updatedFields, "discussionFourPageProgress", currentUserLocalID));
                break;

            case 5: // 3 Sectors
                    // Create the appropriate amount of lists needed for a discussion's sector count
                sectors = new List<List<object>>()
                {
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                };

                // Process and create the updated fields dictionary
                updatedFields = CreateBlankFieldsDictionary(sectors);
                yield return StartCoroutine(UserManager.Instance.UpdateDiscussionPageProgress(updatedFields, "discussionFivePageProgress", currentUserLocalID));
                break;

            case 6: // 4 Sectors
                    // Create the appropriate amount of lists needed for a discussion's sector count
                sectors = new List<List<object>>()
                {
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                };

                // Process and create the updated fields dictionary
                updatedFields = CreateBlankFieldsDictionary(sectors);
                yield return StartCoroutine(UserManager.Instance.UpdateDiscussionPageProgress(updatedFields, "discussionSixPageProgress", currentUserLocalID));
                break;

            case 7: // 4 Sectors
                    // Create the appropriate amount of lists needed for a discussion's sector count
                sectors = new List<List<object>>()
                {
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                };

                // Process and create the updated fields dictionary
                updatedFields = CreateBlankFieldsDictionary(sectors);
                yield return StartCoroutine(UserManager.Instance.UpdateDiscussionPageProgress(updatedFields, "discussionSevenPageProgress", currentUserLocalID));
                break;

            case 8: // 4 Sectors
                    // Create the appropriate amount of lists needed for a discussion's sector count
                sectors = new List<List<object>>()
                {
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                    new List<object>(),
                };

                // Process and create the updated fields dictionary
                updatedFields = CreateBlankFieldsDictionary(sectors);
                yield return StartCoroutine(UserManager.Instance.UpdateDiscussionPageProgress(updatedFields, "discussionEightPageProgress", currentUserLocalID));
                break;

            case 9: // 2 Sectors
                    // Create the appropriate amount of lists needed for a discussion's sector count
                sectors = new List<List<object>>()
                {
                    new List<object>(),
                    new List<object>(),
                };

                // Process and create the updated fields dictionary
                updatedFields = CreateBlankFieldsDictionary(sectors);
                yield return StartCoroutine(UserManager.Instance.UpdateDiscussionPageProgress(updatedFields, "discussionNinePageProgress", currentUserLocalID));
                break;
        }
        GetReadPagesDataFromDB();
    }
    #endregion
}
