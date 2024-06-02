using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnswerDropHandler : MonoBehaviour, IDropHandler
{
	private TextMeshProUGUI _placeholderText;
	public float answerValue = 0;

	public void Start()
	{
		_placeholderText = GetComponentInChildren<TextMeshProUGUI>();
		_placeholderText.text = "";
		answerValue = 0;
	}
	public void OnDrop(PointerEventData eventData)
	{
		Draggable dragObject = eventData.pointerDrag.GetComponent<Draggable>();
		answerValue = dragObject.GetValue();
		_placeholderText.text = answerValue.ToString();
	}
}
