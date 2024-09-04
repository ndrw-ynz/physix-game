using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DraggableUIContainer<T> : MonoBehaviour, IDropHandler where T : DraggableUIObject<T>
{
	public void OnDrop(PointerEventData eventData)
	{
		T dragObject = eventData.pointerDrag.GetComponent<T>();
		if (dragObject != null)
		{
			dragObject.transform.SetParent(transform);
			dragObject.transform.localPosition = Vector3.zero;
			HandleDraggableObject(dragObject);
		}
	}

	public abstract void HandleDraggableObject(T draggableUIObject);
}