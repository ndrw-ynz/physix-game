using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewAccuracyPrecision : MonoBehaviour
{
	public static event Action<ViewAccuracyPrecision> OpenViewEvent;
	public static event Action<bool, bool> SubmitAPEvent;
	public Button notAccurateNotPreciseButton;
	public Button accurateNotPreciseButton;
	public Button notAccuratePreciseButton;
	public Button accuratePreciseButton;


	private void OnEnable()
	{
		OpenViewEvent?.Invoke(this);
		// Add listeners to buttons, with associated values for calling SubmitAPAnswer.
		notAccurateNotPreciseButton.onClick.AddListener(() => SubmitAPAnswer(false, false));
		accurateNotPreciseButton.onClick.AddListener(() => SubmitAPAnswer(true, false));
		notAccuratePreciseButton.onClick.AddListener(() => SubmitAPAnswer(false, true));
		accuratePreciseButton.onClick.AddListener(() => SubmitAPAnswer(true, true));
	}

	public void SubmitAPAnswer(bool isAccurate, bool isPrecise)
	{
		// Trigger event for submission of AP answers.
		SubmitAPEvent?.Invoke(isAccurate, isPrecise);
	}

	private void OnDisable()
	{
		// Remove listeners when view is not active.
		notAccurateNotPreciseButton.onClick.RemoveAllListeners();
		accurateNotPreciseButton.onClick.RemoveAllListeners();
		notAccuratePreciseButton.onClick.RemoveAllListeners();
		accuratePreciseButton.onClick.RemoveAllListeners();
	}
}
