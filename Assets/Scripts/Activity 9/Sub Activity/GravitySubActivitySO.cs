using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/Gravity")]
public class GravitySubActivitySO : ScriptableObject
{
	[Header("Number of Tests")]
	[Range(1, 3)]
	public int numberOfTests;
	[Header("Planet Mass")]
	[Header("Planet Mass - Coefficient Value Range")]
	public float planetMassCoefficientMinVal;
	public float planetMassCoefficientMaxVal;
	[Header("Planet Mass - Exponent Value Range")]
	public int planetMassExponentMinVal;
	public int planetMassExponentMaxVal;
	[Header("Orbitting Object Mass")]
	[Header("Orbitting Object Mass - Coefficient Value Range")]
	public float orbittingObjectMassCoefficientMinVal;
	public float orbittingObjectMassCoefficientMaxVal;
	[Header("Orbitting Object Mass - Exponent Value Range")]
	public int orbittingObjectMassExponentMinVal;
	public int orbittingObjectMassExponentMaxVal;
	[Header("Object Center Point Distance")]
	public float centerPointDistanceMinVal;
	public float centerPointDistanceMaxVal;
}