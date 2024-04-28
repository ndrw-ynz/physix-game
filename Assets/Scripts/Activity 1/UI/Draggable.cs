using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	private Image _thisImage;
	private Vector3 _startPosition;
	private TextMeshProUGUI _placeholderText;

	public void Start()
	{
		_thisImage = GetComponent<Image>();
		_startPosition = _thisImage.transform.position;
		_placeholderText = GetComponentInChildren<TextMeshProUGUI>();
		_placeholderText.text = transform.name;
	}
	public void OnBeginDrag(PointerEventData eventData)
	{
		_thisImage.raycastTarget = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		transform.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		_thisImage.raycastTarget = true;
		transform.position = _startPosition;
	}
}
