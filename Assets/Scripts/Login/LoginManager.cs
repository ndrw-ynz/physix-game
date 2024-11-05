using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] LoginBox loginBox;
    [SerializeField] GameObject closeMessagePrompt;

    private void Start()
    {
        // Subscribe all listeners
        LoginButton.LoginButtonClick += ValidateUserLogin;
        LoginXButton.LoginXButtonClicked += OpenCloseMessagePrompt;
    }

    private void OnDisable()
    {
        // Unsubscribe all listeners on disable
        LoginButton.LoginButtonClick -= ValidateUserLogin;
        LoginXButton.LoginXButtonClicked -= OpenCloseMessagePrompt;
    }

    private void ValidateUserLogin()
    {
        // Process the login based on the text fields' input and if user gets autheticated, load main menu.
        // In the future maybe there should be a function that records user data for loading in main menu such us UID, names, etc.
        loginBox.ProcessLogin();
        bool isUserAuthenticated = loginBox.GetUserAuthentication();

        if (isUserAuthenticated)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

    private void OpenCloseMessagePrompt()
    {
        // Open the "are you sure you want to close?" message prompt
        closeMessagePrompt.SetActive(true);
    }
}
