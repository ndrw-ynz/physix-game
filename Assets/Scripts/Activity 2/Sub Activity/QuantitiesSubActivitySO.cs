using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "QuantitiesSubActivitySO")]
public class QuantitiesSubActivitySO : ScriptableObject
{
	[Header("Number of Quantities")]
	public int numScalarQuantity;
	public int numVectorQuantity;
	[Header("Range of Magnitude")]
	public int minimumMagnitudeValue;
	public int maximumMagnitudeValue;
	[Header("Quantity Unit Measurements")]
	public List<string> intrinsicallyScalarMeasurements;
	public List<string> vectorizableScalarMeasurements;
	[Header("Direction")]
	public List<string> directionDescriptors;
}