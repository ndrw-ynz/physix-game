using UnityEngine;

public class ForceTypeContainer: DraggableUIContainer<ForceTypeDraggableUI>
{
	public ForceType contactForceType { get; private set; }
	public override void HandleDraggableObject(ForceTypeDraggableUI draggableObject)
	{
		contactForceType = draggableObject.contactForceType;
		Debug.Log($"this is now holding type: {draggableObject.contactForceType}");
	}
}