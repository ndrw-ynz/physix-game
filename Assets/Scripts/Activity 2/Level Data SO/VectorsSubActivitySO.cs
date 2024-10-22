using UnityEngine;

public enum VectorDirectionType
{
    Cardinal,
    Standard,
    FullRange
}

[CreateAssetMenu(menuName = "Level Data/Vectors")]
public class VectorsSubActivitySO : ScriptableObject
{
	[Header("Number of Vectors")]
	public int numberOfVectors;
	[Header("Range of Magnitude")]
	public int minimumMagnitudeValue;
	public int maximumMagnitudeValue;
	[Header("Direction Settings")]
	public VectorDirectionType directionType;
}
