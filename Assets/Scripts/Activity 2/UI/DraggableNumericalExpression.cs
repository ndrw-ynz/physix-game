using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableNumericalExpression : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	private Image _image;
	public Vector3 startPosition;
	public Transform parentAfterDrag;
	public TextMeshProUGUI displayText;
	public string numericalExpression;

	public void Initialize(string numericalExpression)
	{
		_image = GetComponent<Image>();
		startPosition = _image.transform.position;
		displayText = GetComponentInChildren<TextMeshProUGUI>();
		displayText.text = numericalExpression;
		this.numericalExpression = numericalExpression;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		_image.raycastTarget = false;
		displayText.raycastTarget = false;
		parentAfterDrag = transform.parent;
		transform.SetParent(transform.root);
	}

	public void OnDrag(PointerEventData eventData)
	{
		transform.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		transform.position = startPosition;
		_image.raycastTarget = true;
		displayText.raycastTarget = true;
		transform.SetParent(parentAfterDrag);
	}
}
