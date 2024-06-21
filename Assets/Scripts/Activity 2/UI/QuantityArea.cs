using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuantityArea : MonoBehaviour, IDropHandler
{
	public VerticalLayoutGroup itemHolder;
	public void OnDrop(PointerEventData eventData)
	{
		DraggableQuantityText dragObject = eventData.pointerDrag.GetComponent<DraggableQuantityText>();
		dragObject.parentAfterDrag = itemHolder.transform;
	}
}
