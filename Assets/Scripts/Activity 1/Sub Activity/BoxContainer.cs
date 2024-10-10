using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class BoxContainer : MonoBehaviour
{
    public TextMeshPro measurementText;
    public int numericalValue;
    public string unitOfMeasurement;

    public void SetValues(ScientificNotationSubActivitySO levelData)
    {
        numericalValue = Random.Range(levelData.minimumNumericalValue, levelData.maximumNumericalValue);
        int randomIndex = Random.Range(0, levelData.unitOfMeasurements.Count);
        unitOfMeasurement = levelData.unitOfMeasurements[randomIndex];

        measurementText.text = $"{numericalValue} {unitOfMeasurement}";
    }
}