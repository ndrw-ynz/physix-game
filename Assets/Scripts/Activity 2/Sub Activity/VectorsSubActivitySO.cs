using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DirectionType
{
    Cardinal,
    Standard,
    FullRange
}

[CreateAssetMenu(menuName = "VectorsSubActivitySO")]
public class VectorsSubActivitySO : ScriptableObject
{
    [Header("Range of Magnitude")]
    public int minimumMagnitudeValue;
    public int maximumMagnitudeValue;
    [Header("Direction Settings")]
    public DirectionType directionType;
    [Header("Number of Vectors")]
    public int numberOfVectors;
}
