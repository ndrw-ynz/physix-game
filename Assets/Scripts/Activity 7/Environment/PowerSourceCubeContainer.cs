using System;

public class PowerSourceCubeContainer : IInteractableObject
{
	public event Action InteractEvent;
	public override void Interact()
	{
		InteractEvent?.Invoke();
	}

	public override string GetInteractionDescription()
	{
		return "Place Power Source Cube";
	}

	private void Start()
	{
		SetInteractable(false);
	}
}