using System;
using System.Collections.Generic;
using UnityEngine;

public enum ErrorType {
	NoErrors,
	SystematicAndRandomError,
	RandomError,
	SystematicError
}

public class ErrorsView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;
	public event Action<ErrorType?> SubmitAnswerEvent;

	[Header("Interactive Buttons")]
	[SerializeField] private List<ErrorTypeButton> ErrorTypeButtons;

	private void OnEnable()
	{
		OpenViewEvent?.Invoke();
	}

	public void OnSubmitButtonClick()
	{
		SceneSoundManager.Instance.PlaySFX("Click");

		// Checking collisionType
		ErrorType? selectedErrorType = null;
		foreach (ErrorTypeButton button in ErrorTypeButtons)
		{
			if (button.isClicked) selectedErrorType = button.errorType;
		}

		SubmitAnswerEvent?.Invoke(selectedErrorType);
	}

	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitViewEvent?.Invoke();
	}
}
