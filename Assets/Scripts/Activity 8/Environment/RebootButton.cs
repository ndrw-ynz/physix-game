using System;

public class RebootButton : IInteractableObject
{
	public static event Action PressRebootButtonEvent;
	public override void Interact()
	{
		PressRebootButtonEvent?.Invoke();
		Destroy(gameObject);
	}

	public override string GetInteractionDescription()
	{
		return "Press Reboot Button";
	}

	private void Start()
	{
		SetInteractable(false);
	}
}