using TMPro;
using UnityEngine;

public class LoginBox : MonoBehaviour
{
    [Header("Input Fields")]
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TextMeshProUGUI errorMessage;

    // Practice or mock value for checking if user is authenticated
    // Person in charge of creating database login function can use or delete this in the future.
    private bool isUserAuthenticated;

    private void Start()
    {
        // Set error message text to blank and is user authenticated to false at start
        errorMessage.text = "";
        isUserAuthenticated = false;
    }

    public void ProcessLogin()
    {
        // Take the value of input fields and feed it into the mock login logic with no database functionality
        LoginLogic(emailInputField.text, passwordInputField.text);
    }

    // I dont think this should be void but a coroutine? Like IEnumarator based from the youtube video I watched.
    // For now I just used void to create a mock simulation of a login
    private void LoginLogic(string email, string password)
    {
        // Here, Maybe get data from database using the email and password
        // Query database
        string mockEmail = "student@gmail.com";
        string mockPassword = "student";

        // Process login and set error messages properly
        if (IsEmailBlank() || IsPasswordBlank())
        {
            errorMessage.text = "Input fields cannot be blank. Please enter email and password.";
        }
        else if (email == mockEmail || password == mockPassword) 
        { 
            isUserAuthenticated = true;
        }
        else
        {
            errorMessage.text = "Your password is incorrect or this account does not exist. Please try again.";
        }
    }

    private bool IsEmailBlank()
    {
        // Check if email input field is blank
        return emailInputField.text == "";
    }

    private bool IsPasswordBlank()
    {
        // Check if password input field is blank
        return passwordInputField.text == ""; 
    }

    public bool GetUserAuthentication()
    {
        // Getter of isUserAuthenticated used in LoginManager
        return isUserAuthenticated;
    }
}
