using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnswerDropHandler : MonoBehaviour, IDropHandler
{
	private TextMeshProUGUI _placeholderText;
	private float _value = 0;

	public void Start()
	{
		_placeholderText = GetComponentInChildren<TextMeshProUGUI>();
		_placeholderText.text = "";
		_value = 0;
	}
	public void OnDrop(PointerEventData eventData)
	{
		Draggable dragObject = eventData.pointerDrag.GetComponent<Draggable>();
		_value = dragObject.GetValue();
		_placeholderText.text = _value.ToString();
	}
}
