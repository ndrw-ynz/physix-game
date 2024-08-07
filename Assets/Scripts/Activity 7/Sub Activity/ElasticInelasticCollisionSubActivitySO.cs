using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/ElasticInelasticCollisionSO")]
public class ElasticInelasticCollisionSubActivitySO : ScriptableObject
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
	[Header("Net Momentum Value Range")]
	public int momentumMinVal;
	public int momentumMaxVal;
}