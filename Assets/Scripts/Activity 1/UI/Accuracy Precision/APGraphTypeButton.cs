using System;
using TMPro;
using UnityEngine;

public class APGraphTypeButton : MonoBehaviour
{
	public static event Action UpdateClickedEvent;

	public APGraphType APGraphType;
	[SerializeField] private TextMeshProUGUI displayText;
	public bool isClicked { get; private set; }

	private void OnEnable()
	{
		UpdateClickedEvent += ResetState;
	}

	private void OnDisable()
	{
		UpdateClickedEvent -= ResetState;
	}

	public void ResetState()
	{
		isClicked = false;
		displayText.color = new Color32(200, 75, 55, 255);
	}

	public void OnClick()
	{
		// Implemented on all button instances.
		UpdateClickedEvent?.Invoke();

		// Afterwards, this portion is only implemented on clicked instance.
		isClicked = true;
		displayText.color = new Color32(175, 255, 155, 255);
	}
}
