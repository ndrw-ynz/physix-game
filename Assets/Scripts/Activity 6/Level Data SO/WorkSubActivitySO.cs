using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/Work")]
public class WorkSubActivitySO : ScriptableObject
{
	[Header("Number of Repetitions")]
	[Range(1, 3)]
	public int numberOfRepetitions;
	[Header("Acceleration Value Range")]
	public float accelerationMinVal;
	public float accelerationMaxVal;
	[Header("Mass Value Range")]
	public float massMinVal;
	public float massMaxVal;
	[Header("Displacement Value Range")]
	public float displacementMinVal;
	public float displacementMaxVal;
	[Header("Degree Value Range")]
	public int degreeMinVal;
	public int degreeMaxVal;
}