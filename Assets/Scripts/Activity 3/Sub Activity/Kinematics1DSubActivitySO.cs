using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/Kinematics1DSubActivitySO")]
public class Kinematics1DSubActivitySO : ScriptableObject
{
    [Header("Velocity Value Range")]
    public int minimumVelocityValue;
    public int maximumVelocityValue;
    [Header("Time Value Range")]
    public int minimumTimeValue;
    public int maximumTimeValue;
}