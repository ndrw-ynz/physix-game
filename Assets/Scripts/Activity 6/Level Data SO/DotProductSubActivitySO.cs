using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/Dot Product")]
public class DotProductSubActivitySO : ScriptableObject
{
	[Header("Number of Tests")]
	[Range(1, 3)]
	public int numberOfTests;
	[Header("Satellite Dish Vector Threshold")]
    public Vector3 satelliteDishVectorMin;
	public Vector3 satelliteDishVectorMax;
	[Header("Target Object Vector Threshold")]
	public Vector3 targetObjectVectorMin;
	public Vector3 targetObjectVectorMax;
}