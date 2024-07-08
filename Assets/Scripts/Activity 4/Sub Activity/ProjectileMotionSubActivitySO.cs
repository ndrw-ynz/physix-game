using UnityEngine;

public enum ProjectileAngleType
{
	Standard90Angle,
	Full90Angle
}

[CreateAssetMenu(menuName = "Level Data/ProjectileMotionSubActivitySO")]
public class ProjectileMotionSubActivitySO : ScriptableObject
{
	[Header("Velocity Value Range")]
    public int minimumVelocityValue;
	public int maximumVelocityValue;
	[Header("Height Value Range")]
	public int minimumHeightValue;
	public int maximumHeightValue;
	[Header("Angle Type")]
	public ProjectileAngleType projectileAngleType;
}