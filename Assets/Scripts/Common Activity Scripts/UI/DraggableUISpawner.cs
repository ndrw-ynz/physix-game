using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DraggableUISpawner<T> : MonoBehaviour, IBeginDragHandler, IDragHandler where T : DraggableUIObject<T>
{
	[SerializeField] private T draggableUIPrefab;
	public T currentDraggableObject;
	public Canvas canvas;

	public abstract void Initialize();

	public void OnBeginDrag(PointerEventData eventData)
	{
		SceneSoundManager.Instance.PlaySFX("UI_Select_Stereo_01");

		currentDraggableObject = Instantiate(draggableUIPrefab, canvas.transform, false);
		currentDraggableObject.canvas = canvas;
		Initialize();

		if (currentDraggableObject != null)
		{
			eventData.pointerDrag = currentDraggableObject.gameObject;
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		// This method is required to be present but can remain empty
	}
}