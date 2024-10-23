using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public enum QuantityType
{
	Scalar,
	Vector
}

public class DraggableQuantityText : DraggableUIObject<DraggableQuantityText>
{
	[SerializeField] private TextMeshProUGUI displayText;
	public QuantityType quantityType;

	private Transform parentAfterDrag;

	public void SetupQuantityDisplay(QuantityType quantityType, string text, Canvas canvas)
	{
		displayText.text = text;
		this.canvas = canvas;
		this.quantityType = quantityType;
	}

	public override void OnBeginDrag(PointerEventData eventData)
	{
		parentAfterDrag = transform.parent;
		base.OnBeginDrag(eventData);
	}

	public override void OnEndDrag(PointerEventData eventData)
	{
		DraggableQuantityContainer container = (DraggableQuantityContainer) GetContainerUnderMouse(eventData);
		if (container != null)
		{
			container.HandleDraggableObject(this);
		}
		else
		{
			transform.SetParent(parentAfterDrag, false);
		}
	}
}
