using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class BoxContainer : IInteractable
{
    public TextMeshPro measurementText;
    private int _numericalValue;
    private string _unitOfMeasurement;

    public static event Action<BoxContainer> BoxInteractEvent;

    public override void Interact()
    {
        Debug.Log("Interacted " + measurementText.text);
        BoxInteractEvent?.Invoke(this);
    }

    public void SetValues(ScientificNotationSO levelData)
    {
        _numericalValue = Random.Range(levelData.minimumNumericalValue, levelData.maximumNumericalValue);
        int randomIndex = Random.Range(0, levelData.unitOfMeasurements.Count);
        _unitOfMeasurement = levelData.unitOfMeasurements[randomIndex];

        measurementText.text = $"{_numericalValue} {_unitOfMeasurement}";
    }
}
