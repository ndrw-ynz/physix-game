
public enum ForceType
{
    FrictionalForce,
	GravitationalForce,
    DragForce,
    TensionForce,
    SpringForce,
    ThrustForce,
    BuoyantForce,
    NormalForce,
    AppliedForce
}

public class ForceTypeUISpawner : DraggableUISpawner<ForceTypeDraggableUI>
{
    public ForceType forceType;

	public override void Initialize()
	{
        currentDraggableObject.contactForceType = forceType;
		// Set displayed subscript text based from contact force type.
        switch (forceType)
        {
			case ForceType.FrictionalForce:
				currentDraggableObject.SetSubscriptText('f');
				break;
			case ForceType.TensionForce:
				currentDraggableObject.SetSubscriptText('T');
				break;
			case ForceType.SpringForce:
				currentDraggableObject.SetSubscriptText('s');
				break;
			case ForceType.AppliedForce:
				currentDraggableObject.SetSubscriptText('a');
				break;
			case ForceType.ThrustForce:
				currentDraggableObject.SetSubscriptText('t');
				break;
			case ForceType.DragForce:
				currentDraggableObject.SetSubscriptText('d');
				break;
			case ForceType.GravitationalForce:
				currentDraggableObject.SetSubscriptText('g');
				break;
			case ForceType.NormalForce:
				currentDraggableObject.SetSubscriptText('N');
				break;
			case ForceType.BuoyantForce:
				currentDraggableObject.SetSubscriptText('B');
				break;
		}
	}
}