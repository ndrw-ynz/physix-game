using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/Force")]
public class ForceSubActivitySO : ScriptableObject
{
	[Header("Acceleration Value Range")]
	public float accelerationMinVal;
	public float accelerationMaxVal;
	[Header("Mass Value Range")]
	public float massMinVal;
	public float massMaxVal;
}