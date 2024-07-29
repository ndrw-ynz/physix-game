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

public class CenterOfMassAnswerSubmissionResults
{
	public bool isMassTimesXCoordinatesCorrect { get; private set; }
	public bool isMassTimesYCoordinatesCorrect { get; private set; }
	public bool isSumOfMassTimesXCoordinatesCorrect { get; private set; }
	public bool isSumOfMassTimesYCoordinatesCorrect { get; private set; }
	public bool isTotalMassXCorrect { get; private set; }
	public bool isTotalMassYCorrect { get; private set; }
	public bool isCenterOfMassXCorrect { get; private set; }
	public bool isCenterOfMassYCorrect { get; private set; }

	public CenterOfMassAnswerSubmissionResults(
		bool isMassTimesXCoordinatesCorrect, 
		bool isMassTimesYCoordinatesCorrect,
		bool isSumOfMassTimesXCoordinatesCorrect,
		bool isSumOfMassTimesYCoordinatesCorrect,
		bool isTotalMassXCorrect,
		bool isTotalMassYCorrect,
		bool isCenterOfMassXCorrect,
		bool isCenterOfMassYCorrect)
	{
		this.isMassTimesXCoordinatesCorrect = isMassTimesXCoordinatesCorrect;
		this.isMassTimesYCoordinatesCorrect = isMassTimesYCoordinatesCorrect;
		this.isSumOfMassTimesXCoordinatesCorrect = isSumOfMassTimesXCoordinatesCorrect;
		this.isSumOfMassTimesYCoordinatesCorrect = isSumOfMassTimesYCoordinatesCorrect;
		this.isTotalMassXCorrect = isTotalMassXCorrect;
		this.isTotalMassYCorrect = isTotalMassYCorrect;
		this.isCenterOfMassXCorrect = isCenterOfMassXCorrect;
		this.isCenterOfMassYCorrect = isCenterOfMassYCorrect;
	}

	public bool isAllCorrect()
	{
		return (
			isMassTimesXCoordinatesCorrect &&
			isMassTimesYCoordinatesCorrect &&
			isSumOfMassTimesXCoordinatesCorrect &&
			isSumOfMassTimesYCoordinatesCorrect &&
			isTotalMassXCorrect &&
			isTotalMassYCorrect &&
			isCenterOfMassXCorrect &&
			isCenterOfMassYCorrect
			);
	}
}

// TEMPORARILY STORE HERE. TRANSFER ENUM TO FUTURE CLASS HANDLING DIFFICULTY SELECTION
public enum Difficulty
{
	Easy,
	Medium,
	Hard
}

public class ActivitySevenManager : MonoBehaviour
{
	public static Difficulty difficultyConfiguration;

    [Header("Level Data - Center of Mass")]
    [SerializeField] CenterOfMassSubActivitySO centerOfMassLevelOne;
	[SerializeField] CenterOfMassSubActivitySO centerOfMassLevelTwo;
	[SerializeField] CenterOfMassSubActivitySO centerOfMassLevelThree;
	private CenterOfMassSubActivitySO currentCenterOfMassLevel;

	[Header("Level Data - Momentum Impulse Force")]
	[SerializeField] MomentumImpulseForceSubActivitySO momentumImpulseForceLevelOne;
	[SerializeField] MomentumImpulseForceSubActivitySO momentumImpulseForceLevelTwo;
	[SerializeField] MomentumImpulseForceSubActivitySO momentumImpulseForceLevelThree;
	private MomentumImpulseForceSubActivitySO currentMomentumImpulseForceLevel;

	[Header("Views")]
    [SerializeField] CenterOfMassView centerOfMassView;
	[SerializeField] MomentumImpulseForceView momentumImpulseForceView;

	[Header("Submission Status Displays")]
	[SerializeField] private CenterOfMassSubmissionStatusDisplay centerOfMassSubmissionStatusDisplay;

	// Given Values - Center of Mass
	private List<MassCoordinatePair> massCoordinatePairs;
	// Given Values - Impulse-Momentum Force
	private Dictionary<string, float> momentumImpulseForceGivenData;

    void Start()
    {
		// Difficulty selection
		difficultyConfiguration = Difficulty.Easy; // IN THE FUTURE, REPLACE WITH WHATEVER SELECTED DIFFICULTY. FOR NOW SET FOR TESTING
        
		// Setting level data
		switch (difficultyConfiguration)
		{
			case Difficulty.Easy:
				currentCenterOfMassLevel = centerOfMassLevelOne;
				currentMomentumImpulseForceLevel = momentumImpulseForceLevelOne;
				break;
			case Difficulty.Medium:
				currentCenterOfMassLevel = centerOfMassLevelTwo;
				currentMomentumImpulseForceLevel = momentumImpulseForceLevelTwo;
				break;
			case Difficulty.Hard:
				currentCenterOfMassLevel = centerOfMassLevelThree;
				currentMomentumImpulseForceLevel = momentumImpulseForceLevelThree;
				break;
		}

		// Subscribing to view events
        CenterOfMassView.SubmitAnswerEvent += CheckCenterOfMassAnswers;

        // Initializing given values
        InitializeMassCoordinatePairs(currentCenterOfMassLevel);
		InitializeMomentumImpulseForceGivenData(currentMomentumImpulseForceLevel);

		// Setting up views
		centerOfMassView.SetupCenterOfMassView(massCoordinatePairs);
		momentumImpulseForceView.SetupMomentumImpulseForceView(momentumImpulseForceGivenData);
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

		// Validate submissions and store in CenterOfMassAnswerSubmissionResults class instance.
		CenterOfMassAnswerSubmissionResults results = new CenterOfMassAnswerSubmissionResults(
			isMassTimesXCoordinatesCorrect: ActivitySevenUtilities.ValidateMassTimesCoordinatesSubmission(centerOfMassAnswer.massTimesXCoordinates, massValues, xCoordinateValues),
			isMassTimesYCoordinatesCorrect: ActivitySevenUtilities.ValidateMassTimesCoordinatesSubmission(centerOfMassAnswer.massTimesYCoordinates, massValues, yCoordinateValues),
			isSumOfMassTimesXCoordinatesCorrect: ActivitySevenUtilities.ValidateSumOfMassTimesCoordinatesSubmission(centerOfMassAnswer.sumOfMassTimesXCoordinates, massValues, xCoordinateValues),
			isSumOfMassTimesYCoordinatesCorrect: ActivitySevenUtilities.ValidateSumOfMassTimesCoordinatesSubmission(centerOfMassAnswer.sumOfMassTimesYCoordinates, massValues, yCoordinateValues),
			isTotalMassXCorrect: ActivitySevenUtilities.ValidateTotalMassSubmission(centerOfMassAnswer.totalMassX, massValues),
			isTotalMassYCorrect: ActivitySevenUtilities.ValidateTotalMassSubmission(centerOfMassAnswer.totalMassY, massValues),
			isCenterOfMassXCorrect: ActivitySevenUtilities.ValidateCenterOfMassSubmission(centerOfMassAnswer.centerOfMassX, massValues, xCoordinateValues),
			isCenterOfMassYCorrect: ActivitySevenUtilities.ValidateCenterOfMassSubmission(centerOfMassAnswer.centerOfMassY, massValues, yCoordinateValues)
			);

		// Modify display border and text
		if (results.isAllCorrect())
		{
			centerOfMassSubmissionStatusDisplay.SetSubmissionStatus(true, "Calculations correct. The power source cube is now accessible.");
		} else
		{
			centerOfMassSubmissionStatusDisplay.SetSubmissionStatus(false, "The system found discrepancies in your calculations. Please review and fix it.");
		}

		// Modify status border displays from all results
		centerOfMassSubmissionStatusDisplay.UpdateStatusBorderDisplaysFromResult(results);

		centerOfMassSubmissionStatusDisplay.gameObject.SetActive(true);
	}
	#endregion

	#region Impulse-Momentum

	private void InitializeMomentumImpulseForceGivenData(MomentumImpulseForceSubActivitySO momentumImpulseForceSubActivitySO)
	{
		momentumImpulseForceGivenData = new Dictionary<string, float>
		{
			{ "mass", Random.Range(momentumImpulseForceSubActivitySO.massMinVal, momentumImpulseForceSubActivitySO.massMaxVal) },
			{ "initialVelocity", Random.Range(momentumImpulseForceSubActivitySO.velocityMinVal, momentumImpulseForceSubActivitySO.velocityMaxVal)},
			{ "finalVelocity", Random.Range(momentumImpulseForceSubActivitySO.velocityMinVal, momentumImpulseForceSubActivitySO.velocityMaxVal)},
			{ "totalTime", Random.Range(momentumImpulseForceSubActivitySO.timeMinVal, momentumImpulseForceSubActivitySO.timeMaxVal)}
		};
	}

	#endregion
}