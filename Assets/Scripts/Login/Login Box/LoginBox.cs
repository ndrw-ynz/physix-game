using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginBox : MonoBehaviour
{
	public event Action<bool> OnAuthenticationCompleted;

	[Header("Input Fields")]
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TextMeshProUGUI errorMessage;

    [Header("Password Visibility Buttons")]
    [SerializeField] private GameObject visibleButton;
    [SerializeField] private GameObject invisibleButton;

    // Practice or mock value for checking if user is authenticated
    // Person in charge of creating database login function can use or delete this in the future.
    public bool isUserAuthenticated { get; private set; }

    private void Start()
    {
        // Set error message text to blank and is user authenticated to false at start
        errorMessage.text = "";
        isUserAuthenticated = false;
    }

    public void ProcessLogin()
    {
        // Take the value of input fields and feed then verify accoun credentials
        if (emailInputField.text == "" || passwordInputField.text == "")
		{
			errorMessage.text = "Input fields cannot be blank. Please enter email and password.";
			return;
		}

		StartCoroutine(UserManager.Instance.SignInWithEmailAndPassword(emailInputField.text, passwordInputField.text, (success) =>
		{
			switch (success)
			{
				case LoginType.UserNotFound:
                    errorMessage.text = "Your password is incorrect or this account does not exist. Please try again.";
                    Debug.Log("Sign-in failed.");
                    break;

				case LoginType.UserIsInactive:
                    errorMessage.text = "This account is inactive, please contact your teacher.";
                    Debug.Log("Account is inactive");
                    break;

				case LoginType.UserLoginSuccess:
                    Debug.Log("User signed in successfully.");
                    isUserAuthenticated = true;
                    // Proceed to next actions
                    break;
			}
			OnAuthenticationCompleted?.Invoke(isUserAuthenticated);
		}));
	}

    public void SetPasswordVisibilty(bool isVisible)
    {
        if (isVisible)
        {
            passwordInputField.inputType = TMP_InputField.InputType.Standard;  // Show password
            visibleButton.gameObject.SetActive(true); // Switch to open eye icon
            invisibleButton.gameObject.SetActive(false);
        }
        else
        {
            passwordInputField.inputType = TMP_InputField.InputType.Password;  // Hide password
            visibleButton.gameObject.SetActive(false); // Switch to open eye icon
            invisibleButton.gameObject.SetActive(true);
        }

        passwordInputField.textComponent.text = passwordInputField.text;  // Force text update
    }
}
