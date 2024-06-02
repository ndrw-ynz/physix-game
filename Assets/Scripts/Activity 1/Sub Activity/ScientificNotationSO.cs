using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScientificNotationSO")]
public class ScientificNotationSO : ScriptableObject
{
    public int minimumNumericalValue;
    public int maximumNumericalValue;
    public List<string> unitOfMeasurements;
}
