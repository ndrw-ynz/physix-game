using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/Torque")]
public class TorqueSubActivitySO : ScriptableObject
{
	[Header("Number of Tests")]
	[Range(1, 3)]
	public int numberOfTests;
	[Header("Force Value Range")]
	public int forceMinVal;
	public int forceMaxVal;
	[Header("Distance Vector Value Range")]
	public int distanceVectorMinVal;
	public int distanceVectorMaxVal;
}