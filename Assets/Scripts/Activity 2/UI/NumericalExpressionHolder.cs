using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NumericalExpressionHolder : MonoBehaviour, IDropHandler
{
	public HorizontalLayoutGroup expressionHolder;
	public void OnDrop(PointerEventData eventData)
	{
		DraggableNumericalExpression dragObject = eventData.pointerDrag.GetComponent<DraggableNumericalExpression>();
		dragObject.parentAfterDrag = expressionHolder.transform;
	}
}
