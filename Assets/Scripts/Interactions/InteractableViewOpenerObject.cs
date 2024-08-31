using System;
using UnityEngine;

public class InteractableViewOpenerObject : IInteractableObject
{
	public static event Action<Camera> SwitchToTargetCameraEvent;

	[SerializeField] private InputReader inputReader;
	[SerializeField] private GameObject viewUI;
	[SerializeField] private Camera targetCamera;
	[SerializeField] private string interactionDescription;

	public override void Interact()
	{
		inputReader.SetUI();
		viewUI.SetActive(true);
		SwitchToTargetCameraEvent?.Invoke(targetCamera);
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