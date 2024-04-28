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
		string pointerName = eventData.pointerDrag.transform.name;
		Debug.Log(pointerName);
		if (pointerName == "1")
		{
			Debug.Log("One is dropped!");
			_placeholderText.text = eventData.pointerDrag.transform.name;
		}
	}
}
