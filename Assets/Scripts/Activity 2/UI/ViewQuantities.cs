using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewQuantities : MonoBehaviour
{
    public static event Action<DraggableQuantityText[], DraggableQuantityText[], DraggableQuantityText[]> SubmitQuantitiesAnswerEvent;

	public QuantityArea givenQuantitiesHolder;
    public QuantityArea scalarQuantitiesHolder;
    public QuantityArea vectorQuantitiesHolder;
    public Button submitQuantitiesAnswerButton;

	private void OnEnable()
	{
		submitQuantitiesAnswerButton.onClick.AddListener(() => SubmitQuantitiesAnswer());
	}

	private void OnDisable()
	{
		submitQuantitiesAnswerButton.onClick.RemoveAllListeners();
	}

	private void SubmitQuantitiesAnswer()
	{
		DraggableQuantityText[] unsolvedQuantities = givenQuantitiesHolder.itemHolder.GetComponentsInChildren<DraggableQuantityText>();
		DraggableQuantityText[] scalarQuantities = scalarQuantitiesHolder.itemHolder.GetComponentsInChildren<DraggableQuantityText>();
		DraggableQuantityText[] vectorQuantities = vectorQuantitiesHolder.itemHolder.GetComponentsInChildren<DraggableQuantityText>();

		SubmitQuantitiesAnswerEvent?.Invoke(unsolvedQuantities, scalarQuantities, vectorQuantities);
	}
}
