using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "QuantitiesSubActivitySO")]
public class QuantitiesSubActivitySO : ScriptableObject
{
	public int numScalarQuantity;
	public int numVectorQuantity;
	public int minimumMagnitudeValue;
	public int maximumMagnitudeValue;
	public List<string> intrinsicallyScalarMeasurements;
	public List<string> vectorizableScalarMeasurements;
	public List<string> directionDescriptors;
}