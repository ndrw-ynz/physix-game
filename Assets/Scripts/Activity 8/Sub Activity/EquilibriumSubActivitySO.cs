using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/Equilibrium")]
public class EquilibriumSubActivitySO : ScriptableObject
{
	[Header("Number of Tests")]
	[Range(1, 3)]
	public int numberOfTests;
	[Header("Weight Value Range")]
	public int weightMinVal;
	public int weightMaxVal;
	[Header("Distance Value Range")]
	public int distanceMinVal;
	public int distanceMaxVal;
}