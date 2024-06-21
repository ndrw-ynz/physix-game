using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum QuantityType
{
	Scalar,
	Vector
}

public class DraggableQuantityText : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	private Image _image;
	public Vector3 startPosition;
	public Transform parentAfterDrag;
	public TextMeshProUGUI displayText;
	public QuantityType quantityType;

	public void Initialize(QuantityType quantityType, string text)
	{
		_image = GetComponent<Image>();
		startPosition = _image.transform.position;
		displayText = GetComponentInChildren<TextMeshProUGUI>();
		displayText.text = text;
		this.quantityType = quantityType;
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
