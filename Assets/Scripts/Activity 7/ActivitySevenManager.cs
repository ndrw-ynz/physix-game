using System.Collections.Generic;
using UnityEngine;

public class MassCoordinatePair
{
	public int mass { get; private set; }
	public Vector2 coordinate { get; private set; }

	public MassCoordinatePair(int mass, Vector2 coordinate)
	{
		this.mass = mass;
		this.coordinate = coordinate;
	}
}

public class ActivitySevenManager : MonoBehaviour
{
    [Header("Level Data - Center of Mass")]
    [SerializeField] CenterOfMassSubActivitySO centerOfMassLevelOne;
    private CenterOfMassSubActivitySO currentCenterOfMassLevel;

    [Header("Views")]
    [SerializeField] CenterOfMassView centerOfMassView;

    [Header("Private Given Values - Center of Mass")]
    List<MassCoordinatePair> massCoordinatePairs;

    void Start()
    {
        currentCenterOfMassLevel = centerOfMassLevelOne;
        
        // Initializing given values
        InitializeMassCoordinatePairs(currentCenterOfMassLevel);

		// Setting up views
		centerOfMassView.SetupCenterOfMassView(massCoordinatePairs);
    }

	#region Center of Mass

    private void InitializeMassCoordinatePairs(CenterOfMassSubActivitySO centerOfMassSO)
    {
        massCoordinatePairs = new List<MassCoordinatePair>();
        while (massCoordinatePairs.Count < centerOfMassSO.numberOfMasses) 
        {
			int randomMass = Random.Range(centerOfMassSO.massMinVal, centerOfMassSO.massMaxVal);
			Vector2 randomCoordinate = new Vector2(
				Random.Range(-centerOfMassSO.coordThreshold, centerOfMassSO.coordThreshold),
				Random.Range(-centerOfMassSO.coordThreshold, centerOfMassSO.coordThreshold)
				);

			MassCoordinatePair massCoordinatePair = new MassCoordinatePair(randomMass, randomCoordinate);
			massCoordinatePairs.Add(massCoordinatePair);
		}
    }
	#endregion
}