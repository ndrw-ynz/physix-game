using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropHandler : MonoBehaviour, IDropHandler
{
	private TextMeshProUGUI _placeholderText;

	public void Start()
	{
		_placeholderText = GetComponentInChildren<TextMeshProUGUI>();
		_placeholderText.text = "";
	}

	public void OnDrop(PointerEventData eventData)
	{
		Draggable dragObject = eventData.pointerDrag.GetComponent<Draggable>();
		_placeholderText.text = dragObject.GetValue().ToString();
	}
}

