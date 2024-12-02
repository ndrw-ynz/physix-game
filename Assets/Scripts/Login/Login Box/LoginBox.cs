using System;
using TMPro;
using UnityEngine;

public class LoginBox : MonoBehaviour
{
	public event Action<bool> OnAuthenticationCompleted;

	[Header("Input Fields")]
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TextMeshProUGUI errorMessage;

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
		emailInputField.text = "marybrown@gmail.com";
		passwordInputField.text = "marybrown";
        // Take the value of input fields and feed then verify accoun credentials
        if (emailInputField.text == "" || passwordInputField.text == "")
		{
			errorMessage.text = "Input fields cannot be blank. Please enter email and password.";
		}

		StartCoroutine(UserManager.Instance.SignInWithEmailAndPassword(emailInputField.text, passwordInputField.text, (success) =>
		{
			if (success)
			{
				Debug.Log("User signed in successfully.");
				isUserAuthenticated = true;
				// Proceed to next actions
			}
			else
			{
				errorMessage.text = "Your password is incorrect or this account does not exist. Please try again.";
				Debug.Log("Sign-in failed.");
			}
			OnAuthenticationCompleted?.Invoke(isUserAuthenticated);
		}));
	}
}
