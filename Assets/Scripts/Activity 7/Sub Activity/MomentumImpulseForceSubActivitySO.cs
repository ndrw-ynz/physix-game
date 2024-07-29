using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/MomentumImpulseForceSO")]
public class MomentumImpulseForceSubActivitySO : ScriptableObject
{
    [Header("Number of Tests")]
    [Range(3, 7)]
    public int numberOfTests;
	[Header("Mass Value Range")]
	public int massMinVal;
	public int massMaxVal;
	[Header("Velocity Value Range")]
	public int velocityMinVal;
	public int velocityMaxVal;
	[Header("Time Value Range")]
	[Range(0, 10)]
	public int timeMinVal;
	[Range(10, 100)]
	public int timeMaxVal;
}