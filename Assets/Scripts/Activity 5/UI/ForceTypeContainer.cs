public enum ForceDirection
{
	Up,
	Down,
	Left,
	Right
}

public class ForceTypeContainer: DraggableUIContainer<ForceTypeDraggableUI>
{
	public ForceDirection forceDirection;
	private ForceTypeDraggableUI currentForceTypeUI;
	public override void HandleDraggableObject(ForceTypeDraggableUI forceTypeUI)
	{
		ClearContainer();
		currentForceTypeUI = forceTypeUI;

		UIUtilities.CenterChildInParent(forceTypeUI.gameObject, gameObject);
	}

	public ForceType? GetCurrentForceType()
	{
		if (currentForceTypeUI == null) return null;
		return currentForceTypeUI.contactForceType;
	}

	public void ClearContainer()
	{
		if (currentForceTypeUI == null) return;
		Destroy(currentForceTypeUI.gameObject);
		currentForceTypeUI = null;
	}
}