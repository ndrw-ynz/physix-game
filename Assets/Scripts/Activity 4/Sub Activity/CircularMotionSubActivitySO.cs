using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/CircularMotionSubActivitySO")]
public class CircularMotionSubActivitySO : ScriptableObject
{
	[Header("Velocity Value Range")]
	public int minimumVelocityValue;
	public int maximumVelocityValue;
	[Header("Radius Value Range")]
	public int minimumRadiusValue;
	public int maximumRadiusValue;
}