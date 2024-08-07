using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/CenterOfMass")]
public class CenterOfMassSubActivitySO : ScriptableObject
{
	[Header("Number of Tests")]
	[Range(1, 3)]
	public int numberOfTests;
	[Header("Number of Masses")]
	[Range(3,5)]
	public int numberOfMasses;
	[Header("Coordinate Distance Threshold")]
	public int coordThreshold;
	[Header("Mass Value Range")]
	public int massMinVal;
	public int massMaxVal;
}