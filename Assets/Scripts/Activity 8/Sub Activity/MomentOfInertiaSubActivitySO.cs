using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/MomentOfInertia")]
public class MomentOfInertiaSubActivitySO : ScriptableObject
{
	[Header("Number of Tests")]
	[Range(3, 7)]
	public int numberOfTests;
	[Header("Mass Value Range")]
	public int massMinVal;
	public int massMaxVal;
	[Header("Length Value Range")]
	public int lengthMinVal;
	public int lengthMaxVal;
	[Header("Radius Value Range")]
	public int radiusMinVal;
	public int radiusMaxVal;
}