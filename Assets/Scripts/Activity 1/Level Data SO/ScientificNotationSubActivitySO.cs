using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/Scientific Notation")]
public class ScientificNotationSubActivitySO : ScriptableObject
{
	[Header("Number of Tests")]
	[Range(1, 6)]
	public int numberOfTests;
	[Header("Range of Numerical Value")]
	public int minimumNumericalValue;
	public int maximumNumericalValue;
	[Header("Unit of Measurements")]
	public List<string> unitOfMeasurements;
}