using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/CircularMotionSubActivitySO")]
public class CircularMotionSubActivitySO : ScriptableObject
{
	[Header("Radius Value Range")]
	public int minimumRadiusValue;
	public int maximumRadiusValue;
	[Header("Time Period Value Range")]
	public int minimumTimePeriodValue;
	public int maximumTimePeriodValue;
}