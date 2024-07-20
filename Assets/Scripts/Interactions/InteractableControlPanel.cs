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
}