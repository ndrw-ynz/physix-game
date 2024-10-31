using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/Circular Motion")]
public class CircularMotionSubActivitySO : ScriptableObject
{
	[Header("Number of Tests")]
	public int numberOfTests;
	[Header("Radius Value Range")]
	public int minimumRadiusValue;
	public int maximumRadiusValue;
	[Header("Time Period Value Range")]
	public int minimumTimePeriodValue;
	public int maximumTimePeriodValue;
}