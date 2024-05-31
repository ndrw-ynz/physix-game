using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewErrors : MonoBehaviour
{
	public static event Action<bool, bool> SubmitErrorsEvent;
	public Button systematicErrorButton;
    public Button randomErrorButton;
    public Button bothErrorsButton;
	public Button noErrorsButton;

	private void OnEnable()
	{
		// Add listeners to buttons, with associated values for calling SubmitErrorsAnswer.
		systematicErrorButton.onClick.AddListener(() => SubmitErrorsAnswer(true, false));
		randomErrorButton.onClick.AddListener(() => SubmitErrorsAnswer(false, true));
		bothErrorsButton.onClick.AddListener(() => SubmitErrorsAnswer(true, true));
		noErrorsButton.onClick.AddListener(() => SubmitErrorsAnswer(false, false));
	}

	public void SubmitErrorsAnswer(bool isSystematicError, bool isRandomError)
	{
		// Trigger event for submission of Errors answer.
		SubmitErrorsEvent?.Invoke(isSystematicError, isRandomError);
	}

	private void OnDisable()
	{
		// Remove listeners when view is not active.
		systematicErrorButton.onClick.RemoveAllListeners();
		randomErrorButton.onClick.RemoveAllListeners();
		bothErrorsButton.onClick.RemoveAllListeners();
		noErrorsButton.onClick.RemoveAllListeners();
	}
}
