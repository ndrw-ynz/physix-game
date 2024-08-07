using UnityEngine;

public class InteractableControlPanel : IInteractableObject
{
	[SerializeField] private InputReader inputReader;
	[SerializeField] private GameObject viewUI;

	public override void Interact()
	{
		inputReader.SetUI();
		viewUI.SetActive(true);
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