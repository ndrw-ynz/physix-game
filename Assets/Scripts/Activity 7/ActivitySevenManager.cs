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

public class ActivitySevenManager : ActivityManager
{
	public static Difficulty difficultyConfiguration;

	public event Action CenterOfMassTerminalClearEvent;
	public event Action MomentumImpulseTerminalClearEvent;
	public event Action CollisionTerminalClearEvent;

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
	[SerializeField] ActivitySevenPerformanceView performanceView;

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

	// Gameplay performance metrics variables
	// Gameplay Time
	private float gameplayTime;
	// Center of Mass
	private bool isCenterOfMassCalculationFinished;
	private int numIncorrectCenterOfMassSubmission;
	private int numCorrectCenterOfMassSubmission;
	private float centerOfMassDuration;
	// Momentum Impulse Force
	private bool isMomentumImpulseForceCalculationFinished;
	private int numIncorrectMomentumImpulseForceSubmission;
	private int numCorrectMomentumImpulseForceSubmission;
	private float momentumImpulseForceDuration;
	// Elastic Inelastic Collision
	private bool isElasticInelasticCollisionCalculationFinished;
	private int numIncorrectElasticInelasticCollisionSubmission;
	private int numCorrectElasticInelasticCollisionSubmission;
	private float elasticInelasticCollisionDuration;

	protected override void Start()
    {
		base.Start();

		ConfigureLevelData(difficultyConfiguration);

		SubscribeViewAndDisplayEvents();

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

		// Update mission objective display
		missionObjectiveDisplayUI.UpdateMissionObjectiveText(0, $"Retrieve the power cube by computing for its center of mass on the Center of Mass Terminal ({currentCenterOfMassLevel.numberOfTests - currentNumCenterOfMassTests}/{currentCenterOfMassLevel.numberOfTests})");
		missionObjectiveDisplayUI.UpdateMissionObjectiveText(1, $"Unlock the impulse momentum mechanism of the door containing the ships' data module on the Impulse-Momentum Terminal ({currentMomentumImpulseForceLevel.numberOfTests - currentNumMomentumImpulseForceTests}/{currentMomentumImpulseForceLevel.numberOfTests})");
		missionObjectiveDisplayUI.UpdateMissionObjectiveText(2, $"Release the ships' data module from its container by overriding the Collisions Terminal ({currentElasticInelasticCollisionLevel.numberOfTests - currentNumElasticInelasticCollisionTests}/{currentElasticInelasticCollisionLevel.numberOfTests})");

		inputReader.SetGameplay();
	}

    private void OnDisable()
    {
        centerOfMassView.SubmitAnswerEvent -= CheckCenterOfMassAnswers;
        centerOfMassSubmissionStatusDisplay.ProceedEvent -= UpdateCenterOfMassViewState;
		momentumImpulseForceView.SubmitAnswerEvent -= CheckMomentumImpulseForceAnswers;
        momentumImpulseForceSubmissionStatusDisplay.ProceedEvent -= UpdateMomentumImpulseForceViewState;
        elasticInelasticCollisionView.SubmitAnswerEvent -= CheckElasticInelasticCollisionAnswers;
        elasticInelasticCollisionSubmissionStatusDisplay.ProceedEvent -= UpdateElasticInelasticCollisionViewState;
        DataModuleCube.RetrieveEvent -= DisplayPerformanceView;
    }

    private void Update()
	{
		gameplayTime += Time.deltaTime;
	}

	private void ConfigureLevelData(Difficulty difficulty)
	{
		difficultyConfiguration = difficulty;

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
	}

	private void SubscribeViewAndDisplayEvents()
	{
		// Center of Mass Sub Activity Related Events
		centerOfMassView.SubmitAnswerEvent += CheckCenterOfMassAnswers;
		centerOfMassSubmissionStatusDisplay.ProceedEvent += UpdateCenterOfMassViewState;

		// Momentum Impulse Force Sub Activity Related Events
		momentumImpulseForceView.SubmitAnswerEvent += CheckMomentumImpulseForceAnswers;
		momentumImpulseForceSubmissionStatusDisplay.ProceedEvent += UpdateMomentumImpulseForceViewState;

		// Collision Sub Activity Related Events
		elasticInelasticCollisionView.SubmitAnswerEvent += CheckElasticInelasticCollisionAnswers;
		elasticInelasticCollisionSubmissionStatusDisplay.ProceedEvent += UpdateElasticInelasticCollisionViewState;

		DataModuleCube.RetrieveEvent += DisplayPerformanceView;
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
			numCorrectCenterOfMassSubmission++;
			currentNumCenterOfMassTests--;
			string displayText;
			if (currentNumCenterOfMassTests <= 0)
			{
				displayText = "Calculations correct. The power source cube is now accessible.";
				isCenterOfMassCalculationFinished = true;
				centerOfMassDuration = gameplayTime;
			} else {
				displayText = "Calculations correct. Loaded next test.";
			}
			centerOfMassSubmissionStatusDisplay.SetSubmissionStatus(true, displayText);

			missionObjectiveDisplayUI.UpdateMissionObjectiveText(0, $"Retrieve the power cube by computing for its center of mass on the Center of Mass Terminal ({currentCenterOfMassLevel.numberOfTests - currentNumCenterOfMassTests}/{currentCenterOfMassLevel.numberOfTests})");
		}
		else
		{
			numIncorrectCenterOfMassSubmission++;
			centerOfMassSubmissionStatusDisplay.SetSubmissionStatus(false, "The system found discrepancies in your calculations. Please review and fix it.");
		}

		// Modify status border displays from all results
		centerOfMassSubmissionStatusDisplay.UpdateStatusBorderDisplaysFromResult(results);

		centerOfMassSubmissionStatusDisplay.gameObject.SetActive(true);
	}

	private void UpdateCenterOfMassViewState()
	{
		if (currentNumCenterOfMassTests > 0)
		{
			GenerateMassCoordinatePairs(currentCenterOfMassLevel);
			centerOfMassView.SetupCenterOfMassView(massCoordinatePairs);
		}
		else
		{
			inputReader.SetGameplay();
			centerOfMassView.gameObject.SetActive(false);
			CenterOfMassTerminalClearEvent?.Invoke();
			missionObjectiveDisplayUI.ClearMissionObjective(0);
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
				numCorrectMomentumImpulseForceSubmission++;
				currentNumMomentumImpulseForceTests--;
				string displayText;
				if (currentNumMomentumImpulseForceTests <= 0)
				{
					displayText = "All calibration tests accomplished.";
					isMomentumImpulseForceCalculationFinished = true;
					momentumImpulseForceDuration = gameplayTime - centerOfMassDuration;
				}
				else
				{
					displayText = "Calibration test matches calculations. Loaded next test.";
				}
				momentumImpulseForceSubmissionStatusDisplay.SetSubmissionStatus(true, displayText);

				momentumImpulseForceView.UpdateCalibrationTestTextDisplay(currentMomentumImpulseForceLevel.numberOfTests - currentNumMomentumImpulseForceTests, currentMomentumImpulseForceLevel.numberOfTests);

				missionObjectiveDisplayUI.UpdateMissionObjectiveText(1, $"Unlock the impulse momentum mechanism of the door containing the ships' data module on the Impulse-Momentum Terminal ({currentMomentumImpulseForceLevel.numberOfTests - currentNumMomentumImpulseForceTests}/{currentMomentumImpulseForceLevel.numberOfTests})");
			}
			else
			{
				numIncorrectMomentumImpulseForceSubmission++;
				momentumImpulseForceSubmissionStatusDisplay.SetSubmissionStatus(false, "Calibration tests found discrepancies in your calculations. Please review and fix it.");
			}

			momentumImpulseForceSubmissionStatusDisplay.UpdateStatusBorderDisplaysFromResult(results);
			momentumImpulseForceSubmissionStatusDisplay.gameObject.SetActive(true);

		} else if (results is MediumHardMomentumImpulseForceAnswerSubmissionResults mediumHardResults)
		{
			if (mediumHardResults.isAllCorrect())
			{
				numCorrectMomentumImpulseForceSubmission++;
				currentNumMomentumImpulseForceTests--;
				string displayText;
				if (currentNumMomentumImpulseForceTests <= 0)
				{
					displayText = "All calibration tests accomplished.";
					isMomentumImpulseForceCalculationFinished = true;
					momentumImpulseForceDuration = gameplayTime - centerOfMassDuration;
				}
				else
				{
					displayText = "Calibration test matches calculations. Loaded next test.";
				}
				momentumImpulseForceSubmissionStatusDisplay.SetSubmissionStatus(true, displayText);

				momentumImpulseForceView.UpdateCalibrationTestTextDisplay(currentMomentumImpulseForceLevel.numberOfTests - currentNumMomentumImpulseForceTests, currentMomentumImpulseForceLevel.numberOfTests);
			}
			else
			{
				numIncorrectMomentumImpulseForceSubmission++;
				momentumImpulseForceSubmissionStatusDisplay.SetSubmissionStatus(false, "Calibration tests found discrepancies in your calculations. Please review and fix it.");
			}

			momentumImpulseForceSubmissionStatusDisplay.UpdateStatusBorderDisplaysFromResult(results);
			momentumImpulseForceSubmissionStatusDisplay.gameObject.SetActive(true);
		}
	}

	private void UpdateMomentumImpulseForceViewState()
	{
		if (currentNumCenterOfMassTests > 0)
		{
			GenerateMomentumImpulseForceGivenData(currentMomentumImpulseForceLevel);
			momentumImpulseForceView.SetupMomentumImpulseForceView(momentumImpulseForceGivenData);
		}
		else
		{
			inputReader.SetGameplay();
			momentumImpulseForceView.gameObject.SetActive(false);
			MomentumImpulseTerminalClearEvent?.Invoke();
			missionObjectiveDisplayUI.ClearMissionObjective(1);
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
			numCorrectElasticInelasticCollisionSubmission++;
			currentNumElasticInelasticCollisionTests--;
			string displayText;
			if (currentNumElasticInelasticCollisionTests <= 0)
			{
				displayText = "Calculations correct. The data module is now accessible.";
				isElasticInelasticCollisionCalculationFinished = true;
				elasticInelasticCollisionDuration = gameplayTime - momentumImpulseForceDuration - centerOfMassDuration;
			} else
			{
				displayText = "Calculations correct. Loaded next test.";
			}
			elasticInelasticCollisionSubmissionStatusDisplay.SetSubmissionStatus(true, displayText);

			elasticInelasticCollisionView.UpdateCalibrationTestTextDisplay(currentElasticInelasticCollisionLevel.numberOfTests - currentNumElasticInelasticCollisionTests, currentElasticInelasticCollisionLevel.numberOfTests);

			missionObjectiveDisplayUI.UpdateMissionObjectiveText(2, $"Release the ships' data module from its container by overriding the Collisions Terminal ({currentElasticInelasticCollisionLevel.numberOfTests - currentNumElasticInelasticCollisionTests}/{currentElasticInelasticCollisionLevel.numberOfTests})");
		}
		else
		{
			numIncorrectElasticInelasticCollisionSubmission++;
			elasticInelasticCollisionSubmissionStatusDisplay.SetSubmissionStatus(false, "The system found discrepancies in your calculations. Please review and fix it.");
		}

		// Modify status border displays from all results
		elasticInelasticCollisionSubmissionStatusDisplay.UpdateStatusBorderDisplaysFromResult(results);

		elasticInelasticCollisionSubmissionStatusDisplay.gameObject.SetActive(true);
	}

	private void UpdateElasticInelasticCollisionViewState()
	{
		if (currentNumCenterOfMassTests > 0)
		{
			GenerateElasticInelasticCollisionData(currentElasticInelasticCollisionLevel);
			elasticInelasticCollisionView.SetupElasicInelasticCollisionView(elasticInelasticCollisionData);
		}
		else
		{
			inputReader.SetGameplay();
			elasticInelasticCollisionView.gameObject.SetActive(false);
			CollisionTerminalClearEvent?.Invoke();
			missionObjectiveDisplayUI.ClearMissionObjective(2);
		}
	}
	#endregion

	public override void DisplayPerformanceView()
	{
		missionObjectiveDisplayUI.ClearMissionObjective(3);
		SetMissionObjectiveDisplay(false);

		inputReader.SetUI();
		performanceView.gameObject.SetActive(true);

		performanceView.SetTotalTimeDisplay(gameplayTime);

		performanceView.SetCenterOfMassMetricsDisplay(
			isAccomplished: isCenterOfMassCalculationFinished,
			numIncorrectSubmission: numIncorrectCenterOfMassSubmission,
			duration: centerOfMassDuration
			);

		performanceView.SetMomentumImpulseForceMetricsDisplay(
			isAccomplished: isMomentumImpulseForceCalculationFinished,
			numIncorrectSubmission: numIncorrectMomentumImpulseForceSubmission,
			duration: momentumImpulseForceDuration
			);

		performanceView.SetElasticInelasticCollisionMetricsDisplay(
			isAccomplished: isElasticInelasticCollisionCalculationFinished,
			numIncorrectSubmission: numIncorrectElasticInelasticCollisionSubmission,
			duration: elasticInelasticCollisionDuration
			);

		// Update its activity feedback display (three args)
		performanceView.UpdateActivityFeedbackDisplay(
			new SubActivityPerformanceMetric(
				subActivityName: "center of mass",
				isSubActivityFinished: isCenterOfMassCalculationFinished,
				numIncorrectAnswers: numIncorrectCenterOfMassSubmission,
				numCorrectAnswers: numCorrectCenterOfMassSubmission,
				badScoreThreshold: 5,
				averageScoreThreshold: 3
				),
			new SubActivityPerformanceMetric(
				subActivityName: "momentum, impulse, and net force",
				isSubActivityFinished: isMomentumImpulseForceCalculationFinished,
				numIncorrectAnswers: numIncorrectMomentumImpulseForceSubmission,
				numCorrectAnswers: numCorrectMomentumImpulseForceSubmission,
				badScoreThreshold: 3,
				averageScoreThreshold: 2
				),
			new SubActivityPerformanceMetric(
				subActivityName: "elastic and inelastic collision",
				isSubActivityFinished: isElasticInelasticCollisionCalculationFinished,
				numIncorrectAnswers: numIncorrectElasticInelasticCollisionSubmission,
				numCorrectAnswers: numCorrectElasticInelasticCollisionSubmission,
				badScoreThreshold: 5,
				averageScoreThreshold: 3
				)
			);
	}

	protected override void HandleGameplayPause()
	{
		base.HandleGameplayPause();
		// Update content of activity pause menu UI
		List<string> taskText = new List<string>();
		if (!isCenterOfMassCalculationFinished)
		{
			taskText.Add("- Unlock the power cube using the center of mass terminal.");
		}
		if (!isMomentumImpulseForceCalculationFinished)
		{
			taskText.Add("- Activate the impulse unlock mechanism of the door using the impulse-momentum terminal.");
		}
		if (!isElasticInelasticCollisionCalculationFinished)
		{
			taskText.Add("- Acquire the ships' data module by overriding the collisions terminal");
		}

		List<string> objectiveText = new List<string>();
		objectiveText.Add("Traverse through a series of heavily secured rooms to acquire the ships' data module for analysis.");

		activityPauseMenuUI.UpdateContent("Lesson 7 - Activity 7", taskText, objectiveText);
	}
}