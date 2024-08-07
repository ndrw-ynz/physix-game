using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/MomentumImpulseForceSO")]
public class MomentumImpulseForceSubActivitySO : ScriptableObject
{
    [Header("Number of Tests")]
    [Range(1, 3)]
    public int numberOfTests;
	[Header("Mass Value Range")]
	public int massMinVal;
	public int massMaxVal;
	[Header("Velocity Value Range")]
	public int velocityMinVal;
	public int velocityMaxVal;
	[Header("Time Value Range")]
	[Range(1, 10)]
	public int timeMinVal;
	[Range(10, 100)]
	public int timeMaxVal;
}