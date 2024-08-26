using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum OrbittingObjectType
{
	Spaceship,
	SatelliteOne,
	SatelliteTwo
}

public class GravityData
{
	public OrbittingObjectType orbittingObjectType;

	public double planetMass;
	public float planetMassSNCoefficient;
	public int planetMassSNExponent;

	public double orbittingObjectMass;
	public float orbittingObjectMassSNCoefficient;
	public int orbittingObjectMassSNExponent;

	public double distanceBetweenObjects;
}

public class GravityAnswerSubmissionResults
{
	public bool isGravitationalForceCorrect { get; private set; }
	public bool isGPECorrect { get; private set; }

	public GravityAnswerSubmissionResults(
		bool isGravitationalForceCorrect,
		bool isGPECorrect
		)
	{
		this.isGravitationalForceCorrect = isGravitationalForceCorrect;
		this.isGPECorrect = isGPECorrect;
	}

	public bool isAllCorrect()
	{
		return isGravitationalForceCorrect && isGPECorrect;
	}
}

public class ActivityNineManager : MonoBehaviour
{
	public static Difficulty difficultyConfiguration;

	[Header("Input Reader")]
	[SerializeField] InputReader inputReader;

	[Header("Level Data - Gravity")]
	[SerializeField] private GravitySubActivitySO gravityLevelOne;
	[SerializeField] private GravitySubActivitySO gravityLevelTwo;
	[SerializeField] private GravitySubActivitySO gravityLevelThree;
	private GravitySubActivitySO currentGravityLevel;

	[Header("View")]
	[SerializeField] private GravityView gravityView;
	[SerializeField] private ActivityNinePerformanceView performanceView;

	[Header("Submission Status Display")]
	[SerializeField] private GravitySubmissionStatusDisplay gravitySubmissionStatusDisplay;

	// Given Data - Gravity
	private GravityData gravityGivenData;

	// Variable for keeping track of current number of tests.
	private int currentNumGravityTests;

	// Gameplay performance metrics variables
	// Gameplay Time
	private float gameplayTime;
	// Gravity subactivity
	private bool isGravityCalculationFinished;
	private int numIncorrectGravitySubmission;

	private void Start()
	{
		// Set level data based from difficulty configuration.
		ConfigureLevelData(Difficulty.Easy); // IN THE FUTURE, REPLACE WITH WHATEVER SELECTED DIFFICULTY. FOR NOW SET FOR TESTING

		// Subscribe to view events
		GravityView.SubmitAnswerEvent += CheckGravityAnswers;
		GravitySubmissionStatusDisplay.ProceedEvent += GenerateNewGravityTest;
		GravitySubmissionStatusDisplay.ProceedEvent += CloseGravityView;

		GenerateGravityGivenData(currentGravityLevel);

		currentNumGravityTests = currentGravityLevel.numberOfTests;

		gravityView.SetupGravityView(gravityGivenData);
		gravityView.UpdateCalibrationTestTextDisplay(0, currentGravityLevel.numberOfTests);
	}

	private void Update()
	{
		gameplayTime += Time.deltaTime;
	}

	/// <summary>
	/// Configures current level data throughout Activity 8 based from set difficulty configuration.
	/// </summary>
	/// <param name="difficultyConfiguration"></param>
	private void ConfigureLevelData(Difficulty difficulty)
	{
		difficultyConfiguration = difficulty;

		// Setting level data
		switch (difficultyConfiguration)
		{
			case Difficulty.Easy:
				currentGravityLevel = gravityLevelOne;
				break;
			case Difficulty.Medium:
				currentGravityLevel = gravityLevelTwo;
				break;
			case Difficulty.Hard:
				currentGravityLevel = gravityLevelThree;
				break;
		}
	}

	private void GenerateGravityGivenData(GravitySubActivitySO gravitySubActivitySO)
	{
		GravityData data = new GravityData();

		// Determine orbitting object type based on number of solved tests
		List<OrbittingObjectType> orbittingObjectTypes = new List<OrbittingObjectType> {
			OrbittingObjectType.Spaceship,
			OrbittingObjectType.SatelliteOne,
			OrbittingObjectType.SatelliteTwo
		};

		if (currentNumGravityTests >= orbittingObjectTypes.Count)
		{
			data.orbittingObjectType = OrbittingObjectType.SatelliteTwo;
		}
		else
		{
			data.orbittingObjectType = orbittingObjectTypes[currentNumGravityTests];
		}

		// Generate planet mass related data
		data.planetMassSNCoefficient = (float) Math.Round(Random.Range(
			gravitySubActivitySO.planetMassCoefficientMinVal,
			gravitySubActivitySO.planetMassCoefficientMaxVal
			), 4);
		data.planetMassSNExponent = Random.Range(
			gravitySubActivitySO.planetMassExponentMinVal,
			gravitySubActivitySO.planetMassExponentMaxVal
			);
		data.planetMass = ActivityNineUtilities.EvaluateScientificNotation(
			data.planetMassSNCoefficient,
			data.planetMassSNExponent
			);

		// Generate orbitting object mass related data
		data.orbittingObjectMassSNCoefficient = (float) Math.Round (Random.Range(
			gravitySubActivitySO.orbittingObjectMassCoefficientMinVal,
			gravitySubActivitySO.orbittingObjectMassCoefficientMaxVal
			), 4);
		data.orbittingObjectMassSNExponent = Random.Range(
			gravitySubActivitySO.orbittingObjectMassExponentMinVal,
			gravitySubActivitySO.orbittingObjectMassExponentMaxVal
			);
		data.orbittingObjectMass = ActivityNineUtilities.EvaluateScientificNotation(
			data.orbittingObjectMassSNCoefficient,
			data.orbittingObjectMassSNExponent
			);

		// Generate distance between objects
		data.distanceBetweenObjects = Math.Round(Random.Range(
			gravitySubActivitySO.centerPointDistanceMinVal,
			gravitySubActivitySO.centerPointDistanceMaxVal
			), 2);

		gravityGivenData = data;
	}

	private void CheckGravityAnswers(GravityAnswerSubmission answer)
	{
		GravityAnswerSubmissionResults results = new GravityAnswerSubmissionResults(
			isGravitationalForceCorrect: ActivityNineUtilities.ValidateGravitationalForceSubmission(answer.gravitationalForce, gravityGivenData),
			isGPECorrect: ActivityNineUtilities.ValidateGPESubmission(answer.GPE, gravityGivenData)
			);

		// Display gravity answer submission results
		DisplayGravitySubmissionResults(results);
	}

	private void DisplayGravitySubmissionResults(GravityAnswerSubmissionResults results)
	{
		if (results.isAllCorrect())
		{
			currentNumGravityTests--;
			string displayText;
			if (currentNumGravityTests <= 0)
			{
				displayText = "Calculations correct. The Moment of Inertia Calculation module is now calibrated.";
				isGravityCalculationFinished = true;
			}
			else
			{
				displayText = "Calculations correct. Loaded next test.";
			}
			gravitySubmissionStatusDisplay.SetSubmissionStatus(true, displayText);

			gravityView.UpdateCalibrationTestTextDisplay(currentGravityLevel.numberOfTests - currentNumGravityTests, currentGravityLevel.numberOfTests);
		} else
		{
			numIncorrectGravitySubmission++;
			gravitySubmissionStatusDisplay.SetSubmissionStatus(false, "The system found discrepancies in your calculations. Please review and fix it.");
		}

		// Update status border displays from result.
		gravitySubmissionStatusDisplay.UpdateStatusBorderDisplaysFromResult(results);

		gravitySubmissionStatusDisplay.gameObject.SetActive(true);
	}

	private void GenerateNewGravityTest()
	{
		if (currentNumGravityTests > 0)
		{
			GenerateGravityGivenData(currentGravityLevel);
			gravityView.SetupGravityView(gravityGivenData);
		}
	}

	private void CloseGravityView()
	{
		if (currentNumGravityTests <= 0)
		{
			gravityView.gameObject.SetActive(false);
			DisplayPerformanceView();
		}
	}

	private void DisplayPerformanceView()
	{
		inputReader.SetUI();
		performanceView.gameObject.SetActive(true);

		performanceView.SetTotalTimeDisplay(gameplayTime);

		performanceView.SetGravityMetricsDisplay(
			isAccomplished: isGravityCalculationFinished,
			numIncorrectSubmission: numIncorrectGravitySubmission,
			duration: gameplayTime
			);
	}
}