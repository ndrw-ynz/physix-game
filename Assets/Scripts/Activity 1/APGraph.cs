using UnityEngine;

public enum APGraphType
{
	AccurateAndPrecise,
	AccurateButNotPrecise,
	PreciseButNotAccurate,
	NeitherAccurateNorPrecise
}

public class APGraph : MonoBehaviour
{
    public APGraphType graphType { get; private set; }

	[Header("Graph Type Points")]
	[SerializeField] private GameObject accurateAndPrecisePoints;
	[SerializeField] private GameObject accurateButNotPrecisePoints;
	[SerializeField] private GameObject preciseButNotAccuratePoints;
	[SerializeField] private GameObject neitherAccurateNotPrecisePoints;

	public void InitializeGraphContent()
	{
		int randomIndex = Random.Range(0, 4);
		GameObject[] graphTypePoints = { 
			accurateAndPrecisePoints,
			accurateButNotPrecisePoints,
			preciseButNotAccuratePoints,
			neitherAccurateNotPrecisePoints
		};

		graphType = (APGraphType) randomIndex;
		GameObject selectedGraphPoint = graphTypePoints[randomIndex];

		int randomYRotation = Random.Range(0, 360);
		selectedGraphPoint.transform.localEulerAngles = new Vector3(0, randomYRotation, 0);
		selectedGraphPoint.SetActive(true);
	}
}
