using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

public abstract class MomentumImpulseForceAnswerSubmissionResults
{
	public bool isChangeInMomentumCorrect { get; private set; }
	public bool isImpulseCorrect { get; private set; }
	public bool isNetForceCorrect { get; private set; }

	public MomentumImpulseForceAnswerSubmissionResults(
		bool isChangeInMomentumCorrect,
		bool isImpulseCorrect,
		bool isNetForceCorrect)
	{
		this.isChangeInMomentumCorrect = isChangeInMomentumCorrect;
		this.isImpulseCorrect = isImpulseCorrect;
		this.isNetForceCorrect = isNetForceCorrect;
	}

	public virtual bool isAllCorrect()
	{
		return (
			isChangeInMomentumCorrect &&
			isImpulseCorrect &&
			isNetForceCorrect
			);
	}
}

public class EasyMomentumImpulseForceAnswerSubmissionResults : MomentumImpulseForceAnswerSubmissionResults
{
	public EasyMomentumImpulseForceAnswerSubmissionResults(
		bool isChangeInMomentumCorrect,
		bool isImpulseCorrect,
		bool isNetForceCorrect)
		:
		base(isChangeInMomentumCorrect, isImpulseCorrect, isNetForceCorrect)
	{
	}
}

public class MediumHardMomentumImpulseForceAnswerSubmissionResults : MomentumImpulseForceAnswerSubmissionResults
{
	public bool isInitialMomentumCorrect { get; private set; }
	public bool isFinalMomentumCorrect { get; private set; }
	public MediumHardMomentumImpulseForceAnswerSubmissionResults(
		bool isInitialMomentumCorrect,
		bool isFinalMomentumCorrect,
		bool isChangeInMomentumCorrect,
		bool isImpulseCorrect,
		bool isNetForceCorrect) 
		: 
		base(isChangeInMomentumCorrect, isImpulseCorrect, isNetForceCorrect)
	{
		this.isInitialMomentumCorrect = isInitialMomentumCorrect;
		this.isFinalMomentumCorrect = isFinalMomentumCorrect;
	}

	public override bool isAllCorrect()
	{
		return (
			base.isAllCorrect() &&
			isInitialMomentumCorrect &&
			isFinalMomentumCorrect
			);
	}
}

public class CollisionObject
{
	public float mass { get; private set; }
	public float initialVelocity { get; private set; }
	public float finalVelocity { get; private set; }

	public CollisionObject(float mass, float initialVelocity, float finalVelocity)
	{
		this.mass = mass;
		this.initialVelocity = initialVelocity;
		this.finalVelocity = finalVelocity;
	}
}

public class CollisionData
{
	public CollisionObject cubeOne { get; private set; }
	public CollisionObject cubeTwo { get; private set; }

	public CollisionData(CollisionObject cubeOne, CollisionObject cubeTwo)
	{
		this.cubeOne = cubeOne;
		this.cubeTwo = cubeTwo;
	}
}

public class ElasticInelasticCollisionAnswerSubmissionResults
{
	public bool isCubeOneInitialMomentumCorrect;
	public bool isCubeTwoInitialMomentumCorrect;
	public bool isCubeOneFinalMomentumCorrect;
	public bool isCubeTwoFinalMomentumCorrect;
	public bool isNetInitialMomentumCorrect;
	public bool isNetFinalMomentumCorrect;
	public bool isCollisionTypeCorrect;

	public ElasticInelasticCollisionAnswerSubmissionResults(
		bool isCubeOneInitialMomentumCorrect,
		bool isCubeTwoInitialMomentumCorrect,
		bool isCubeOneFinalMomentumCorrect,
		bool isCubeTwoFinalMomentumCorrect,
		bool isNetInitialMomentumCorrect,
		bool isNetFinalMomentumCorrect,
		bool isCollisionTypeCorrect
		)
	{
		this.isCubeOneInitialMomentumCorrect = isCubeOneInitialMomentumCorrect;
		this.isCubeTwoInitialMomentumCorrect = isCubeTwoInitialMomentumCorrect;
		this.isCubeOneFinalMomentumCorrect = isCubeOneFinalMomentumCorrect;
		this.isCubeTwoFinalMomentumCorrect = isCubeTwoFinalMomentumCorrect;
		this.isNetInitialMomentumCorrect = isNetInitialMomentumCorrect;
		this.isNetFinalMomentumCorrect = isNetFinalMomentumCorrect;
		this.isCollisionTypeCorrect = isCollisionTypeCorrect;
	}

	public bool isAllCorrect()
	{
		return (
			isCubeOneInitialMomentumCorrect &&
			isCubeTwoInitialMomentumCorrect &&
			isCubeOneFinalMomentumCorrect &&
			isCubeTwoFinalMomentumCorrect &&
			isNetInitialMomentumCorrect &&
			isNetFinalMomentumCorrect &&
			isCollisionTypeCorrect
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

	public static event Action RoomOneClearEvent;
	public static event Action RoomTwoClearEvent;
	public static event Action RoomThreeClearEvent;

	[Header("Input Reader")]
	[SerializeField] InputReader inputReader;

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

	[Header("Level Data - Elastic Inelastic Collision")]
	[SerializeField] ElasticInelasticCollisionSubActivitySO elasticInelasticCollisionLevelOne;
	[SerializeField] ElasticInelasticCollisionSubActivitySO elasticInelasticCollisionLevelTwo;
	[SerializeField] ElasticInelasticCollisionSubActivitySO elasticInelasticCollisionLevelThree;
	private ElasticInelasticCollisionSubActivitySO currentElasticInelasticCollisionLevel;

	[Header("Views")]
    [SerializeField] CenterOfMassView centerOfMassView;
	[SerializeField] MomentumImpulseForceView momentumImpulseForceView;
	[SerializeField] ElasticInelasticCollisionView elasticInelasticCollisionView;

	[Header("Submission Status Displays")]
	[SerializeField] private CenterOfMassSubmissionStatusDisplay centerOfMassSubmissionStatusDisplay;
	[SerializeField] private MomentumImpulseForceSubmissionStatusDisplay momentumImpulseForceSubmissionStatusDisplay;
	[SerializeField] private ElasticInelasticCollisionSubmissionStatusDisplay elasticInelasticCollisionSubmissionStatusDisplay;

	// Given Values - Center of Mass
	private List<MassCoordinatePair> massCoordinatePairs;
	// Given Values - Impulse-Momentum Force
	private Dictionary<string, float> momentumImpulseForceGivenData;
	// Given Values - Elastic Inelastic Collision
	private CollisionData elasticInelasticCollisionData;

	// Variables for keeping current number of tests
	private int currentNumCenterOfMassTests;
	private int currentNumMomentumImpulseForceTests;
	private int currentNumElasticInelasticCollisionTests;

    void Start()
    {
		// Difficulty selection
		difficultyConfiguration = Difficulty.Medium; // IN THE FUTURE, REPLACE WITH WHATEVER SELECTED DIFFICULTY. FOR NOW SET FOR TESTING
        
		// Setting level data
		switch (difficultyConfiguration)
		{
			case Difficulty.Easy:
				currentCenterOfMassLevel = centerOfMassLevelOne;
				currentMomentumImpulseForceLevel = momentumImpulseForceLevelOne;
				currentElasticInelasticCollisionLevel = elasticInelasticCollisionLevelOne;
				break;
			case Difficulty.Medium:
				currentCenterOfMassLevel = centerOfMassLevelTwo;
				currentMomentumImpulseForceLevel = momentumImpulseForceLevelTwo;
				currentElasticInelasticCollisionLevel = elasticInelasticCollisionLevelTwo;
				break;
			case Difficulty.Hard:
				currentCenterOfMassLevel = centerOfMassLevelThree;
				currentMomentumImpulseForceLevel = momentumImpulseForceLevelThree;
				currentElasticInelasticCollisionLevel = elasticInelasticCollisionLevelThree;
				break;
		}

		// Subscribing to view events
        CenterOfMassView.SubmitAnswerEvent += CheckCenterOfMassAnswers;
		CenterOfMassSubmissionStatusDisplay.ProceedEvent += GenerateNewCenterOfMassTest;
		CenterOfMassSubmissionStatusDisplay.ProceedEvent += CloseCenterOfMassView;
		MomentumImpulseForceView.SubmitAnswerEvent += CheckMomentumImpulseForceAnswers;
		MomentumImpulseForceSubmissionStatusDisplay.ProceedEvent += GenerateNewMomentumImpulseForceTest;
		MomentumImpulseForceSubmissionStatusDisplay.ProceedEvent += CloseMomentumImpulseForceView;
		ElasticInelasticCollisionView.SubmitAnswerEvent += CheckElasticInelasticCollisionAnswers;
		ElasticInelasticCollisionSubmissionStatusDisplay.ProceedEvent += GenerateNewElasticInelasticCollisionTest;
		ElasticInelasticCollisionSubmissionStatusDisplay.ProceedEvent += CloseElasticInelasticCollisionView;

		// Initializing given values
		GenerateMassCoordinatePairs(currentCenterOfMassLevel);
		GenerateMomentumImpulseForceGivenData(currentMomentumImpulseForceLevel);
		GenerateElasticInelasticCollisionData(currentElasticInelasticCollisionLevel);

		// Setting number of problems
		currentNumCenterOfMassTests = currentCenterOfMassLevel.numberOfTests;
		currentNumMomentumImpulseForceTests = currentMomentumImpulseForceLevel.numberOfTests;
		currentNumElasticInelasticCollisionTests = currentElasticInelasticCollisionLevel.numberOfTests;

		// Setting up views
		centerOfMassView.SetupCenterOfMassView(massCoordinatePairs);
		momentumImpulseForceView.SetupMomentumImpulseForceView(momentumImpulseForceGivenData);
		momentumImpulseForceView.UpdateCalibrationTestTextDisplay(0, currentMomentumImpulseForceLevel.numberOfTests);
		elasticInelasticCollisionView.SetupElasicInelasticCollisionView(elasticInelasticCollisionData);
		elasticInelasticCollisionView.UpdateCalibrationTestTextDisplay(0, currentElasticInelasticCollisionLevel.numberOfTests);
	}

	#region Center of Mass

	private void GenerateMassCoordinatePairs(CenterOfMassSubActivitySO centerOfMassSO)
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

		// Display answer validation results
		DisplayCenterOfMassSubmissionResult(results);
	}

	private void DisplayCenterOfMassSubmissionResult(CenterOfMassAnswerSubmissionResults results)
	{
		// Modify display border and text
		if (results.isAllCorrect())
		{
			currentNumCenterOfMassTests--;
			string displayText = currentNumCenterOfMassTests <= 0 ? "Calculations correct. The power source cube is now accessible." : "Calculations correct. Loaded next test.";
			centerOfMassSubmissionStatusDisplay.SetSubmissionStatus(true, displayText);
		}
		else
		{
			centerOfMassSubmissionStatusDisplay.SetSubmissionStatus(false, "The system found discrepancies in your calculations. Please review and fix it.");
		}

		// Modify status border displays from all results
		centerOfMassSubmissionStatusDisplay.UpdateStatusBorderDisplaysFromResult(results);

		centerOfMassSubmissionStatusDisplay.gameObject.SetActive(true);
	}

	private void GenerateNewCenterOfMassTest()
	{
		// Generate new given values and update center of mass view
		if (currentNumCenterOfMassTests > 0)
		{
			GenerateMassCoordinatePairs(currentCenterOfMassLevel);
			centerOfMassView.SetupCenterOfMassView(massCoordinatePairs);
		}
	}

	private void CloseCenterOfMassView()
	{
		if (currentNumCenterOfMassTests <= 0)
		{
			inputReader.SetGameplay();
			centerOfMassView.gameObject.SetActive(false);
			RoomOneClearEvent?.Invoke();
		}
	}
	#endregion

	#region Impulse-Momentum

	private void GenerateMomentumImpulseForceGivenData(MomentumImpulseForceSubActivitySO momentumImpulseForceSubActivitySO)
	{
		switch (difficultyConfiguration)
		{
			case Difficulty.Easy:
				// Easy configuration
				momentumImpulseForceGivenData = new Dictionary<string, float>
				{
					{ "mass", Random.Range(momentumImpulseForceSubActivitySO.massMinVal, momentumImpulseForceSubActivitySO.massMaxVal) },
					{ "deltaVelocity", Random.Range(momentumImpulseForceSubActivitySO.velocityMinVal, momentumImpulseForceSubActivitySO.velocityMaxVal)},
					{ "totalTime", Random.Range(momentumImpulseForceSubActivitySO.timeMinVal, momentumImpulseForceSubActivitySO.timeMaxVal)}
				};
				break;
			case Difficulty.Medium: case Difficulty.Hard:
				// Medium-hard configuration
				momentumImpulseForceGivenData = new Dictionary<string, float>
				{
					{ "mass", Random.Range(momentumImpulseForceSubActivitySO.massMinVal, momentumImpulseForceSubActivitySO.massMaxVal) },
					{ "initialVelocity", Random.Range(momentumImpulseForceSubActivitySO.velocityMinVal, momentumImpulseForceSubActivitySO.velocityMaxVal)},
					{ "finalVelocity", Random.Range(momentumImpulseForceSubActivitySO.velocityMinVal, momentumImpulseForceSubActivitySO.velocityMaxVal)},
					{ "totalTime", Random.Range(momentumImpulseForceSubActivitySO.timeMinVal, momentumImpulseForceSubActivitySO.timeMaxVal)}
				};
				break;
		}
	}

	private void CheckMomentumImpulseForceAnswers(MomentumImpulseForceAnswerSubmission momentumImpulseForceAnswer)
	{
		if (momentumImpulseForceAnswer is EasyMomentumImpulseForceAnswerSubmission)
		{
			// Easy problem verification
			EasyMomentumImpulseForceAnswerSubmission answer = (EasyMomentumImpulseForceAnswerSubmission) momentumImpulseForceAnswer;

			EasyMomentumImpulseForceAnswerSubmissionResults results = new EasyMomentumImpulseForceAnswerSubmissionResults(
				// Change in momentum
				ActivitySevenUtilities.ValidateMomentumImpulse(
					answer.changeInMomentum,
					momentumImpulseForceGivenData["mass"],
					momentumImpulseForceGivenData["deltaVelocity"]
					),
				// Impulse
				ActivitySevenUtilities.ValidateMomentumImpulse(
					answer.impulse,
					momentumImpulseForceGivenData["mass"],
					momentumImpulseForceGivenData["deltaVelocity"]
					),
				// Net force
				ActivitySevenUtilities.ValidateNetForce(
					answer.netForce,
					momentumImpulseForceGivenData["mass"],
					momentumImpulseForceGivenData["deltaVelocity"],
					momentumImpulseForceGivenData["totalTime"]
					)
				);

			// Display answer validation results
			DisplayMomentumImpulseForceSubmissionResult(results);
		}
		else if (momentumImpulseForceAnswer is MediumHardMomentumImpulseForceAnswerSubmission) {
			// Medium-Hard problem answer verification
			MediumHardMomentumImpulseForceAnswerSubmission answer = (MediumHardMomentumImpulseForceAnswerSubmission) momentumImpulseForceAnswer;
			
			float deltaVelocity = momentumImpulseForceGivenData["finalVelocity"] - momentumImpulseForceGivenData["initialVelocity"];
			
			MediumHardMomentumImpulseForceAnswerSubmissionResults results = new MediumHardMomentumImpulseForceAnswerSubmissionResults(
				// Initial momentum
				ActivitySevenUtilities.ValidateMomentumImpulse(
					answer.initialMomentum, 
					momentumImpulseForceGivenData["mass"],
					momentumImpulseForceGivenData["initialVelocity"]
					),
				// Final momentum
				ActivitySevenUtilities.ValidateMomentumImpulse(
					answer.finalMomentum,
					momentumImpulseForceGivenData["mass"],
					momentumImpulseForceGivenData["finalVelocity"]
					),
				// Change in momentum
				ActivitySevenUtilities.ValidateMomentumImpulse(
					answer.changeInMomentum,
					momentumImpulseForceGivenData["mass"],
					deltaVelocity
					),
				// Impulse
				ActivitySevenUtilities.ValidateMomentumImpulse(
					answer.impulse,
					momentumImpulseForceGivenData["mass"],
					deltaVelocity
					),
				// Net force
				ActivitySevenUtilities.ValidateNetForce(
					answer.netForce,
					momentumImpulseForceGivenData["mass"],
					deltaVelocity,
					momentumImpulseForceGivenData["totalTime"]
					)
				);

			// Display answer validation results
			DisplayMomentumImpulseForceSubmissionResult(results);
		}
	}

	private void DisplayMomentumImpulseForceSubmissionResult(MomentumImpulseForceAnswerSubmissionResults results)
	{
		if (results is EasyMomentumImpulseForceAnswerSubmissionResults easyResults)
		{
			if (easyResults.isAllCorrect())
			{
				currentNumMomentumImpulseForceTests--;
				string displayText = currentNumMomentumImpulseForceTests <= 0 ? "All calibration tests accomplished." : "Calibration test matches calculations. Loaded next test.";
				momentumImpulseForceSubmissionStatusDisplay.SetSubmissionStatus(true, displayText);

				momentumImpulseForceView.UpdateCalibrationTestTextDisplay(currentMomentumImpulseForceLevel.numberOfTests - currentNumMomentumImpulseForceTests, currentMomentumImpulseForceLevel.numberOfTests);
			}
			else
			{
				momentumImpulseForceSubmissionStatusDisplay.SetSubmissionStatus(false, "Calibration tests found discrepancies in your calculations. Please review and fix it.");
			}

			momentumImpulseForceSubmissionStatusDisplay.UpdateStatusBorderDisplaysFromResult(results);
			momentumImpulseForceSubmissionStatusDisplay.gameObject.SetActive(true);

		} else if (results is MediumHardMomentumImpulseForceAnswerSubmissionResults mediumHardResults)
		{
			if (mediumHardResults.isAllCorrect())
			{
				currentNumMomentumImpulseForceTests--;
				string displayText = currentNumMomentumImpulseForceTests <= 0 ? "All calibration tests accomplished." : "Calibration test matches calculations. Loaded next test.";
				momentumImpulseForceSubmissionStatusDisplay.SetSubmissionStatus(true, displayText);

				momentumImpulseForceView.UpdateCalibrationTestTextDisplay(currentMomentumImpulseForceLevel.numberOfTests - currentNumMomentumImpulseForceTests, currentMomentumImpulseForceLevel.numberOfTests);
			}
			else
			{
				momentumImpulseForceSubmissionStatusDisplay.SetSubmissionStatus(false, "Calibration tests found discrepancies in your calculations. Please review and fix it.");
			}

			momentumImpulseForceSubmissionStatusDisplay.UpdateStatusBorderDisplaysFromResult(results);
			momentumImpulseForceSubmissionStatusDisplay.gameObject.SetActive(true);
		}
	}

	private void GenerateNewMomentumImpulseForceTest()
	{
		// Generate new given data for momentum-impulse and force subactivity, and update momentum-impulse force view
		if (currentNumMomentumImpulseForceTests > 0)
		{
			GenerateMomentumImpulseForceGivenData(currentMomentumImpulseForceLevel);
			momentumImpulseForceView.SetupMomentumImpulseForceView(momentumImpulseForceGivenData);
		}
	}

	private void CloseMomentumImpulseForceView()
	{
		if (currentNumMomentumImpulseForceTests <= 0)
		{
			inputReader.SetGameplay();
			momentumImpulseForceView.gameObject.SetActive(false);
			RoomTwoClearEvent?.Invoke();
		}
	}
	#endregion

	#region Elastic Inelastic Collision
	private void GenerateElasticInelasticCollisionData(ElasticInelasticCollisionSubActivitySO elasticInelasticCollisionSubActivitySO)
	{
		bool isElastic = Random.Range(0, 2) == 0;

		// Variables for collision
		int m1, m2;
		int u1, u2;
		int v1, v2;

		if (isElastic)
		{
			// Elastic Collision

			// Setting values for elastic collision

			int netInitialMomentum, netFinalMomentum;
			netInitialMomentum = netFinalMomentum = Random.Range(elasticInelasticCollisionSubActivitySO.momentumMinVal, elasticInelasticCollisionSubActivitySO.momentumMaxVal);

			// rand generate netInitialMomentum
			int nim1, nim2; // nim1 > nim2
			nim1 = Random.Range(netInitialMomentum / 2, netInitialMomentum);
			nim2 = (int) Math.Abs(nim1 - netInitialMomentum);

			// rand generate netFinalMomentum
			int nfm1, nfm2; // nfm2 > nfm1
			nfm2 = Random.Range(netFinalMomentum / 2, netFinalMomentum);
			nfm1 = (int)Math.Abs(nfm2 - netFinalMomentum);

			// randomly generate cube one mass, u1, and v1 using nim1 and nfm1
			m1 = ActivitySevenUtilities.FindGCD(nim1, nfm1);
			u1 = nim1 / m1;
			v1 = nfm1 / m1;

			// randomly generate cube two mass, u2, and v2 using nim2 and nfm2
			m2 = ActivitySevenUtilities.FindGCD(nim2, nfm2);
			u2 = nim2 / m2;
			v2 = nfm2 / m2;
		} else
		{
			// Inelastic Collision

			// Setting values for inelastic collision

			// Mass variables
			m1 = Random.Range(elasticInelasticCollisionSubActivitySO.massMinVal, elasticInelasticCollisionSubActivitySO.massMaxVal);
			m2 = Random.Range(elasticInelasticCollisionSubActivitySO.massMinVal, elasticInelasticCollisionSubActivitySO.massMaxVal);
			// Generate initial velocity values
			u1 = Random.Range(elasticInelasticCollisionSubActivitySO.velocityMinVal, elasticInelasticCollisionSubActivitySO.velocityMaxVal);
			u2 = Random.Range(elasticInelasticCollisionSubActivitySO.velocityMinVal, elasticInelasticCollisionSubActivitySO.velocityMaxVal);
			// Generate final velocity values
			v1 = Random.Range(elasticInelasticCollisionSubActivitySO.velocityMinVal, elasticInelasticCollisionSubActivitySO.velocityMaxVal);
			v2 = Random.Range(elasticInelasticCollisionSubActivitySO.velocityMinVal, elasticInelasticCollisionSubActivitySO.velocityMaxVal);
		}

		// Store data in CollisionObjects
		CollisionObject cubeOne = new CollisionObject(m1, u1, v1);
		CollisionObject cubeTwo = new CollisionObject(m2, u2, v2);

		elasticInelasticCollisionData = new CollisionData(cubeOne, cubeTwo);
	}

	private void CheckElasticInelasticCollisionAnswers(ElasticInelasticCollisionAnswerSubmission answer)
	{
		CollisionObject cubeOne = elasticInelasticCollisionData.cubeOne;
		CollisionObject cubeTwo = elasticInelasticCollisionData.cubeTwo;

		ElasticInelasticCollisionAnswerSubmissionResults results = new ElasticInelasticCollisionAnswerSubmissionResults(
			isCubeOneInitialMomentumCorrect: ActivitySevenUtilities.ValidateMomentumImpulse(answer.cubeOneInitialMomentum, cubeOne.mass, cubeOne.initialVelocity),
			isCubeTwoInitialMomentumCorrect: ActivitySevenUtilities.ValidateMomentumImpulse(answer.cubeTwoInitialMomentum, cubeTwo.mass, cubeTwo.initialVelocity),
			isCubeOneFinalMomentumCorrect: ActivitySevenUtilities.ValidateMomentumImpulse(answer.cubeOneFinalMomentum, cubeOne.mass, cubeOne.finalVelocity),
			isCubeTwoFinalMomentumCorrect: ActivitySevenUtilities.ValidateMomentumImpulse(answer.cubeTwoFinalMomentum, cubeTwo.mass, cubeTwo.finalVelocity),
			isNetInitialMomentumCorrect: ActivitySevenUtilities.ValidateNetMomentum(answer.netInitialMomentum, cubeOne.mass, cubeOne.initialVelocity, cubeTwo.mass, cubeTwo.initialVelocity),
			isNetFinalMomentumCorrect: ActivitySevenUtilities.ValidateNetMomentum(answer.netFinalMomentum, cubeOne.mass, cubeOne.finalVelocity, cubeTwo.mass, cubeTwo.finalVelocity),
			isCollisionTypeCorrect: ActivitySevenUtilities.ValidateCollisionType(answer.collisionType, elasticInelasticCollisionData)
			);

		// Display answer validation results
		DisplayElasticInelasticCollisionSubmissionResult(results);
	}

	private void DisplayElasticInelasticCollisionSubmissionResult(ElasticInelasticCollisionAnswerSubmissionResults results)
	{
		// Modify display border and text
		if (results.isAllCorrect())
		{
			currentNumElasticInelasticCollisionTests--;
			string displayText = currentNumElasticInelasticCollisionTests <= 0 ? "Calculations correct. The data module is now accessible." : "Calculations correct. Loaded next test.";
			elasticInelasticCollisionSubmissionStatusDisplay.SetSubmissionStatus(true, displayText);

			elasticInelasticCollisionView.UpdateCalibrationTestTextDisplay(currentElasticInelasticCollisionLevel.numberOfTests - currentNumElasticInelasticCollisionTests, currentElasticInelasticCollisionLevel.numberOfTests);
		}
		else
		{
			elasticInelasticCollisionSubmissionStatusDisplay.SetSubmissionStatus(false, "The system found discrepancies in your calculations. Please review and fix it.");
		}

		// Modify status border displays from all results
		elasticInelasticCollisionSubmissionStatusDisplay.UpdateStatusBorderDisplaysFromResult(results);

		elasticInelasticCollisionSubmissionStatusDisplay.gameObject.SetActive(true);
	}

	private void GenerateNewElasticInelasticCollisionTest()
	{
		// Generate new given data for elastic inelastic collision subactivity, and update elastic inelastic collision view
		if (currentNumElasticInelasticCollisionTests > 0)
		{
			GenerateElasticInelasticCollisionData(currentElasticInelasticCollisionLevel);
			elasticInelasticCollisionView.SetupElasicInelasticCollisionView(elasticInelasticCollisionData);
		}
	}

	private void CloseElasticInelasticCollisionView()
	{
		if (currentNumElasticInelasticCollisionTests <= 0)
		{
			inputReader.SetGameplay();
			elasticInelasticCollisionView.gameObject.SetActive(false);
			RoomThreeClearEvent?.Invoke();
		}
	}
	#endregion
}