using System;

public class PowerSourceCubeContainer : IInteractableObject
{
	public static event Action InteractEvent;
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