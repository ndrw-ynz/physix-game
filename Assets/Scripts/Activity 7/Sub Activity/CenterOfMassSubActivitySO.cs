using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/CenterOfMass")]
public class CenterOfMassSubActivitySO : ScriptableObject
{
	[Header("Coordinate Distance Threshold")]
	public int coordThreshold;
	[Header("Mass Value Range")]
	public int massMinVal;
	public int massMaxVal;
}