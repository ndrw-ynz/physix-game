using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DraggableUIObject<T> : MonoBehaviour, IDragHandler, IEndDragHandler where T : DraggableUIObject<T>
{
	public void OnDrag(PointerEventData eventData)
	{
		transform.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		DraggableUIContainer<T> container = GetContainerUnderMouse(eventData);
		if (container != null)
		{
			container.HandleDraggableObject((T)this);
		}
		Destroy(gameObject); // Destroy after processing
	}

	private DraggableUIContainer<T> GetContainerUnderMouse(PointerEventData eventData)
	{
		// Check if the draggable object is dropped over a container
		PointerEventData pointerData = new PointerEventData(EventSystem.current)
		{
			position = eventData.position
		};

		var results = new System.Collections.Generic.List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerData, results);

		foreach (var result in results)
		{
			DraggableUIContainer<T> container = result.gameObject.GetComponent<DraggableUIContainer<T>>();
			if (container != null)
			{
				return container;
			}
		}

		return null;
	}
}