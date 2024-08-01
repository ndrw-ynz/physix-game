using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CollisionType
{
	Elastic,
	Inelastic
}

public class CollisionTypeButton : MonoBehaviour
{
	public static event Action UpdateClickedEvent;

	[SerializeField] private TextMeshProUGUI displayText;
	private bool isClicked;
	public CollisionType unitSymbol;

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