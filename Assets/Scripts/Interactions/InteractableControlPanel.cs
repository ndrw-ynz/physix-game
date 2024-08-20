using System;
using UnityEngine;

public class InteractableControlPanel : IInteractableObject
{
	public static event Action<Camera> SwitchToTargetCameraEvent;

	[SerializeField] private InputReader inputReader; // IN THE FUTURE, REMOVE THIS. (on act 7 refactoring)
	[SerializeField] private GameObject viewUI;
	[SerializeField] private Camera targetCamera;

	public override void Interact()
	{
		inputReader.SetUI(); // REMOVE THIS IN THE FUTURE. ALREADY DELEGATED TO ENV MANAGERS.
		viewUI.SetActive(true);

		SwitchToTargetCameraEvent?.Invoke(targetCamera);
	}

	public override string GetInteractionDescription()
	{
		return "Open Terminal";
	}

	private void Start()
	{
		SetInteractable(true);
	}
}