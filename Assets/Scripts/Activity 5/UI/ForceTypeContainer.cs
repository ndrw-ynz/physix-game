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
		if (currentForceTypeUI != null)
		{
			Destroy(currentForceTypeUI.gameObject);
			currentForceTypeUI = null;
		}
		currentForceTypeUI = forceTypeUI;

		UIUtilities.CenterChildInParent(forceTypeUI.gameObject, gameObject);
	}

	public ForceType? GetCurrentForceType()
	{
		if (currentForceTypeUI == null) return null;
		return currentForceTypeUI.contactForceType;
	}
}