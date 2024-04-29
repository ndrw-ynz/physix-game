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
	private float _value = 0;

	public void Awake()
	{
		_thisImage = GetComponent<Image>();
		_startPosition = _thisImage.transform.position;
		_placeholderText = GetComponentInChildren<TextMeshProUGUI>();
		_placeholderText.text = transform.name;
	}
	public void OnBeginDrag(PointerEventData eventData)
	{
		_thisImage.raycastTarget = false;
		_placeholderText.raycastTarget = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		transform.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		transform.position = _startPosition;
		_thisImage.raycastTarget = true;
		_placeholderText.raycastTarget = true;
	}

	public void SetValue(float value)
	{
		_value = value;
		_placeholderText.text = _value.ToString();
	}
}
