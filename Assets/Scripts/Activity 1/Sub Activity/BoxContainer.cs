using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class BoxContainer : IInteractable
{
    [SerializeField] private TextMeshPro measurementText;
    private int _numericalValue;
    private string _unitOfMeasurement;

    public override void Interact()
    {

    }

    public void SetValues(ScientificNotationSO levelData)
    {
        _numericalValue = Random.Range(levelData.minimumNumericalValue, levelData.maximumNumericalValue);
        int randomIndex = Random.Range(0, levelData.unitOfMeasurements.Count);
        _unitOfMeasurement = levelData.unitOfMeasurements[randomIndex];

        measurementText.text = $"{_numericalValue} {_unitOfMeasurement}";
    }
}
