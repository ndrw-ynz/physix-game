using System;
using System.Collections;
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

public class ActivityNineManager : ActivityManager
{
	public static Difficulty difficultyConfiguration;

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
	private int numCorrectGravitySubmission;

	protected override void Start()
	{
		base.Start();

		SceneSoundManager.Instance.PlayMusic("Space Walk");

		ConfigureLevelData(difficultyConfiguration);

		SubscribeViewAndDisplayEvents();

		// Initializing given values
		GenerateGravityGivenData(currentGravityLevel);

		// Setting number of tests
		currentNumGravityTests = currentGravityLevel.numberOfTests;

		// Setting up views
		gravityView.SetupGravityView(gravityGivenData);
		gravityView.UpdateCalibrationTestTextDisplay(0, currentGravityLevel.numberOfTests);

		// Update mission objective display
		missionObjectiveDisplayUI.UpdateMissionObjectiveText(0, $"Calculate the Gravitational Potential Energy and Gravitational Force of the satellite on the Gravity Control Terminal ({currentGravityLevel.numberOfTests - currentNumGravityTests}/{currentGravityLevel.numberOfTests})");

		inputReader.SetGameplay();
	}

    private void OnDisable()
    {
        gravityView.SubmitAnswerEvent -= CheckGravityAnswers;
        gravitySubmissionStatusDisplay.ProceedEvent -= UpdateGravityViewState;
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

	private void SubscribeViewAndDisplayEvents()
	{
		// Gravity Sub Activity Related Events
		gravityView.SubmitAnswerEvent += CheckGravityAnswers;
		gravitySubmissionStatusDisplay.ProceedEvent += UpdateGravityViewState;
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
			SceneSoundManager.Instance.PlaySFX("UI_Buttonconfirm_Stereo_01");

			numCorrectGravitySubmission++;
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
			SceneSoundManager.Instance.PlaySFX("UI_Forbidden_Stereo_02");

			numIncorrectGravitySubmission++;
			gravitySubmissionStatusDisplay.SetSubmissionStatus(false, "The system found discrepancies in your calculations. Please review and fix it.");
		}

		// Update status border displays from result.
		gravitySubmissionStatusDisplay.UpdateStatusBorderDisplaysFromResult(results);

		gravitySubmissionStatusDisplay.gameObject.SetActive(true);
	}

	private void UpdateGravityViewState()
	{
		missionObjectiveDisplayUI.UpdateMissionObjectiveText(0, $"Calculate the Gravitational Potential Energy and Gravitational Force of the satellite on the Gravity Control Terminal ({currentGravityLevel.numberOfTests - currentNumGravityTests}/{currentGravityLevel.numberOfTests})");

		if (currentNumGravityTests > 0)
		{
			GenerateGravityGivenData(currentGravityLevel);
			gravityView.SetupGravityView(gravityGivenData);
		}
		else
		{
			gravityView.gameObject.SetActive(false);
			DisplayPerformanceView();
			missionObjectiveDisplayUI.ClearMissionObjective(0);
		}
	}

	protected override void AddAttemptRecord()
	{
		Dictionary<string, object> gravityResults = new Dictionary<string, object>
		{
			{ "fields", new Dictionary<string, FirestoreField>
				{
					{ "isAccomplished", new FirestoreField(isGravityCalculationFinished) },
					{ "mistakes", new FirestoreField(numIncorrectGravitySubmission) },
					{ "durationInSec", new FirestoreField((int)gameplayTime) }
				}
			}
		};

		Dictionary<string, object> results = new Dictionary<string, object>
		{
			{ "fields", new Dictionary<string, object>
				{
					{ "gravity", new FirestoreField(gravityResults)},
				}
			}
		};

		Dictionary<string, FirestoreField> fields = new Dictionary<string, FirestoreField>
		{
			{ "dateAttempted", new FirestoreField(DateTime.UtcNow) },
			{ "difficulty", new FirestoreField($"{difficultyConfiguration}") },
			{ "results", new FirestoreField(results) },
			{ "isAccomplished", new FirestoreField(isGravityCalculationFinished) },
			{ "studentId", new FirestoreField(UserManager.Instance.CurrentUser.localId) },
			{ "totalDurationInSec", new FirestoreField((int)gameplayTime)}
		};

		StartCoroutine(UserManager.Instance.CreateAttemptDocument(fields, "activityNineAttempts"));
	}

	public override void DisplayPerformanceView()
	{
		base.DisplayPerformanceView();

		missionObjectiveDisplayUI.ClearMissionObjective(1);
		SetMissionObjectiveDisplay(false);

		inputReader.SetUI();
		performanceView.gameObject.SetActive(true);

		performanceView.SetTotalTimeDisplay(gameplayTime);

		performanceView.SetGravityMetricsDisplay(
			isAccomplished: isGravityCalculationFinished,
			numIncorrectSubmission: numIncorrectGravitySubmission,
			duration: gameplayTime
			);

		// Update its activity feedback display (one arg)
		performanceView.UpdateActivityFeedbackDisplay(
			new SubActivityPerformanceMetric(
				subActivityName: "gravity",
				isSubActivityFinished: isGravityCalculationFinished,
				numIncorrectAnswers: numIncorrectGravitySubmission,
				numCorrectAnswers: numCorrectGravitySubmission,
				badScoreThreshold: 3,
				averageScoreThreshold: 2
				)
			);
	}

    protected override IEnumerator SetNextLevelButtonState()
    {
        throw new NotImplementedException();
    }

    protected override void HandleGameplayPause()
	{
		base.HandleGameplayPause();
		// Update content of activity pause menu UI
		List<string> taskText = new List<string>();
		if (!isGravityCalculationFinished)
		{
			taskText.Add("- Calculate the gravitational force and gravity potential energy of the satellite orbiting planet Terra.");
		}

		List<string> objectiveText = new List<string>();
		objectiveText.Add("Make final preparations and calculations to land on planet Terra.");

		activityPauseMenuUI.UpdateContent("Lesson 9 - Activity 9", taskText, objectiveText);
	}
}