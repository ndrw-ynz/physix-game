using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/Kinematics 1D")]
public class Kinematics1DSubActivitySO : ScriptableObject
{
	[Header("Number of Tests")]
	public int numberOfAccelerationProblems;
	public int numberOfTotalDepthProblems;
	[Header("Velocity Value Range")]
    public int minimumVelocityValue;
    public int maximumVelocityValue;
    [Header("Time Value Range")]
    public int minimumTimeValue;
    public int maximumTimeValue;
}