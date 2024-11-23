using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] LoginBox loginBox;
    [SerializeField] GameObject closeMessagePrompt;

    private void Start()
    {
		SceneSoundManager.Instance.PlayMusic("With love from Vertex Studio (2)");

		// Subscribe all listeners
		LoginButton.LoginButtonClick += ValidateUserLogin;
        LoginXButton.LoginXButtonClicked += OpenCloseMessagePrompt;
        PromptChoice.ChoiceButtonClick += CheckUserChoice;
        loginBox.OnAuthenticationCompleted += LoadMainScene;

	}

    private void OnDisable()
    {
        // Unsubscribe all listeners on disable
        LoginButton.LoginButtonClick -= ValidateUserLogin;
        LoginXButton.LoginXButtonClicked -= OpenCloseMessagePrompt;
        PromptChoice.ChoiceButtonClick -= CheckUserChoice;
    }

    private void ValidateUserLogin()
    {
        // Process the login based on the text fields' input and if user gets autheticated, load main menu.
        // In the future maybe there should be a function that records user data for loading in main menu such us UID, names, etc.
        loginBox.ProcessLogin();       
    }

    private void LoadMainScene(bool isAuthenticated)
    {
        if (isAuthenticated) SceneManager.LoadScene("Main Menu");
	}

	private void OpenCloseMessagePrompt()
    {
        // Open the "are you sure you want to close?" message prompt
        closeMessagePrompt.SetActive(true);
    }

    private void CheckUserChoice(ClosePromptChoice choice)
    {
        switch (choice)
        {
            case ClosePromptChoice.Yes:
                // Quits application in build mode, not in editor
                Application.Quit();

#if UNITY_EDITOR
                // Only for showcasing quit application function in unity editor
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                break;
            case ClosePromptChoice.No:
                if (closeMessagePrompt.activeSelf)
                {
                    closeMessagePrompt.SetActive(false);
                }
                break;
        }
    }
}
