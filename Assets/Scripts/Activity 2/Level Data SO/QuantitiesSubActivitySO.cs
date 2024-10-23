using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/Quantities")]
public class QuantitiesSubActivitySO : ScriptableObject
{
	[Header("Number of Quantities")]
	public int numberOfQuantities;
	[Header("Range of Magnitude")]
	public int minimumMagnitudeValue;
	public int maximumMagnitudeValue;
	[Header("Quantity Unit Measurements")]
	public List<string> intrinsicallyScalarMeasurements;
	public List<string> vectorizableScalarMeasurements;
	[Header("Direction")]
	public List<string> directionDescriptors;
}