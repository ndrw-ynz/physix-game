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


	// Given Values - Center of Mass
    private List<MassCoordinatePair> massCoordinatePairs;

    void Start()
    {
        CenterOfMassView.SubmitAnswerEvent += CheckCenterOfMassAnswers;

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

    private void CheckCenterOfMassAnswers(CenterOfMassAnswerSubmission centerOfMassAnswer)
    {
		// Extracting necessary info from massCoordinatePairs
		int[] massValues = new int[massCoordinatePairs.Count];
		int[] xCoordinateValues = new int[massCoordinatePairs.Count];
		int[] yCoordinateValues = new int[massCoordinatePairs.Count];
		for (int i = 0; i < massCoordinatePairs.Count; i++)
		{
			massValues[i] = massCoordinatePairs[i].mass;
			xCoordinateValues[i] = (int) massCoordinatePairs[i].coordinate.x;
			yCoordinateValues[i] = (int) massCoordinatePairs[i].coordinate.y;
		}

		// Validating massTimesXCoordinates submission
		bool isMassTimesXCoordinatesCorrect = ActivitySevenUtilities.ValidateMassTimesCoordinatesSubmission(centerOfMassAnswer.massTimesXCoordinates, massValues, xCoordinateValues);

		// Validating massTimesYCoordinates submission
		bool isMassTimesYCoordinatesCorrect = ActivitySevenUtilities.ValidateMassTimesCoordinatesSubmission(centerOfMassAnswer.massTimesYCoordinates, massValues, yCoordinateValues);

		// Validating sumOfMassTimesXCoordinates
		bool isSumOfMassTimesXCoordinatesCorrect = ActivitySevenUtilities.ValidateSumOfMassTimesCoordinatesSubmission(centerOfMassAnswer.sumOfMassTimesXCoordinates, massValues, xCoordinateValues);
		
		// Validating sumOfMassTimesYCoordinates
		bool isSumOfMassTimesYCoordinatesCorrect = ActivitySevenUtilities.ValidateSumOfMassTimesCoordinatesSubmission(centerOfMassAnswer.sumOfMassTimesYCoordinates, massValues, yCoordinateValues);

		// Validating totalMassX
		bool isTotalMassXCorrect = ActivitySevenUtilities.ValidateTotalMassSubmission(centerOfMassAnswer.totalMassX, massValues);

		// Validating totalMassY
		bool isTotalMassYCorrect = ActivitySevenUtilities.ValidateTotalMassSubmission(centerOfMassAnswer.totalMassY, massValues);

		// Validating centerOfMassX
		bool isCenterOfMassXCorrect = ActivitySevenUtilities.ValidateCenterOfMassSubmission(centerOfMassAnswer.centerOfMassX, massValues, xCoordinateValues);
		
		// Validating centerOfMassY
		bool isCenterOfMassYCorrect = ActivitySevenUtilities.ValidateCenterOfMassSubmission(centerOfMassAnswer.centerOfMassY, massValues, yCoordinateValues);

		Debug.Log(isMassTimesXCoordinatesCorrect);
		Debug.Log(isMassTimesYCoordinatesCorrect);
		Debug.Log(isSumOfMassTimesXCoordinatesCorrect);
		Debug.Log(isSumOfMassTimesYCoordinatesCorrect);
		Debug.Log(isTotalMassXCorrect);
		Debug.Log(isTotalMassYCorrect);
		Debug.Log(isCenterOfMassXCorrect);
		Debug.Log(isCenterOfMassYCorrect);
	}
	#endregion
}