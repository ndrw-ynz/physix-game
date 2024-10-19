using System;
using System.Collections.Generic;
using UnityEngine;

public class AccuracyPrecisionView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;
	public event Action<APGraphType?> SubmitAnswerEvent;

	[Header("Interactive Buttons")]
	[SerializeField] private List<APGraphTypeButton> APGraphTypeButtons;

	private void OnEnable()
	{
		OpenViewEvent?.Invoke();
	}

	public void OnSubmitButtonClick()
	{
		// Checking collisionType
		APGraphType? selectedAPGraphType = null;
		foreach (APGraphTypeButton button in APGraphTypeButtons)
		{
			if (button.isClicked) selectedAPGraphType = button.APGraphType;
		}

		SubmitAnswerEvent?.Invoke(selectedAPGraphType);
	}

	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitViewEvent?.Invoke();
	}
}
