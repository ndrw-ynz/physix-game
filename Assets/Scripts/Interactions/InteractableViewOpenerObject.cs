using System;
using UnityEngine;

public class InteractableViewOpenerObject : IInteractableObject
{
	[SerializeField] private InputReader inputReader;
	[SerializeField] private GameObject viewUI;
	[SerializeField] private string interactionDescription;

	public override void Interact()
	{
		inputReader.SetUI();
		viewUI.SetActive(true);
	}

	public override string GetInteractionDescription()
	{
		return interactionDescription;
	}

	private void Start()
	{
		SetInteractable(true);
	}
}