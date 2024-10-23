using UnityEngine.UI;

public class DraggableQuantityContainer : DraggableUIContainer<DraggableQuantityText>
{
	public VerticalLayoutGroup itemHolder;

	public override void HandleDraggableObject(DraggableQuantityText draggableQuantityText)
	{
		draggableQuantityText.transform.SetParent(itemHolder.transform, false);
	}
}
