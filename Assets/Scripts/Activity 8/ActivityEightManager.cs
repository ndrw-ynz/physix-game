using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MomentOfInertiaData
{
	public InertiaObjectType inertiaObjectType;
    public int? mass;
	public int? length;
	public int? plateLengthA;
	public int? plateLengthB;
	public int? radius;
	public int? innerRadius;
	public int? outerRadius;
}

public class TorqueData
{
	public int force;
	public int distanceVector;
	public TorqueDirection torqueDirection;
}

public class EquilibriumData
{
	public int weighingApparatusWeight;
	public int redBoxWeight;
	public int redBoxDistance;
	public int blueBoxWeight;
	public int blueBoxDistance;
	public int fulcrumForce;
}

/// <summary>
/// A class for storing the validation results from submitted answers
/// for Moment of Inertia.
/// </summary>
public class MomentOfInertiaAnswerSubmissionResults
{
	public bool isInertiaObjectTypeCorrect;
	public bool isMomentOfInertiaCorrect;

	public MomentOfInertiaAnswerSubmissionResults(
		bool isInertiaObjectTypeCorrect,
		bool isMomentOfInertiaCorrect
		)
	{
		this.isInertiaObjectTypeCorrect = isInertiaObjectTypeCorrect;
		this.isMomentOfInertiaCorrect = isMomentOfInertiaCorrect;
	}

	public bool isAllCorrect()
	{
		return isInertiaObjectTypeCorrect == true && isMomentOfInertiaCorrect == true;
	}
}

/// <summary>
/// A class for storing the validation results from submitted answers
/// for Torque.
/// </summary>
public class TorqueAnswerSubmissionResults
{
	public bool isTorqueMagnitudeCorrect;
	public bool isTorqueDirectionCorrect;

	public TorqueAnswerSubmissionResults(
		bool isTorqueMagnitudeCorrect,
		bool isTorqueDirectionCorrect
		)
	{
		this.isTorqueMagnitudeCorrect = isTorqueMagnitudeCorrect;
		this.isTorqueDirectionCorrect = isTorqueDirectionCorrect;
	}

	public bool isAllCorrect()
	{
		return isTorqueMagnitudeCorrect == true && isTorqueDirectionCorrect == true;
	}
}

/// <summary>
/// A class for storing the validation results from submitted answers
/// for Equilibrium.
/// </summary>
public class EquilibriumAnswerSubmissionResults
{
	public bool isSummationOfDownwardForcesCorrect;
	public bool isUpwardForceCorrect;
	public bool isSummationOfTotalForcesCorrect;
	public bool isCounterclockwiseTorqueCorrect;
	public bool isClockwiseTorqueCorrect;
	public bool isEquilibriumTypeCorrect;

	public EquilibriumAnswerSubmissionResults(
		bool isSummationOfDownwardForcesCorrect,
		bool isUpwardForceCorrect,
		bool isSummationOfTotalForcesCorrect,
		bool isCounterclockwiseTorqueCorrect,
		bool isClockwiseTorqueCorrect,
		bool isEquilibriumTypeCorrect
		)
	{
		this.isSummationOfDownwardForcesCorrect = isSummationOfDownwardForcesCorrect;
		this.isUpwardForceCorrect = isUpwardForceCorrect;
		this.isSummationOfTotalForcesCorrect = isSummationOfTotalForcesCorrect;
		this.isCounterclockwiseTorqueCorrect = isCounterclockwiseTorqueCorrect;
		this.isClockwiseTorqueCorrect = isClockwiseTorqueCorrect;
		this.isEquilibriumTypeCorrect = isEquilibriumTypeCorrect;
	}

	public bool isAllCorrect()
	{
		return (
			isSummationOfDownwardForcesCorrect == true &&
			isUpwardForceCorrect == true &&
			isSummationOfTotalForcesCorrect == true &&
			isCounterclockwiseTorqueCorrect == true &&
			isClockwiseTorqueCorrect == true &&
			isEquilibriumTypeCorrect == true
			);
	}

}

public class ActivityEightManager : ActivityManager
{
	public static Difficulty difficultyConfiguration;

	public static event Action GeneratorRoomClearEvent;
	public static event Action WeighingScaleRoomClearEvent;
	public static event Action RebootRoomClearEvent;

	[Header("Level Data - Moment of Inertia")]
	[SerializeField] private MomentOfInertiaSubActivitySO momentOfInertiaLevelOne;
	[SerializeField] private MomentOfInertiaSubActivitySO momentOfInertiaLevelTwo;
	[SerializeField] private MomentOfInertiaSubActivitySO momentOfInertiaLevelThree;
	private MomentOfInertiaSubActivitySO currentMomentOfInertiaLevel;

	[Header("Level Data - Torque")]
	[SerializeField] private TorqueSubActivitySO torqueLevelOne;
	[SerializeField] private TorqueSubActivitySO torqueLevelTwo;
	[SerializeField] private TorqueSubActivitySO torqueLevelThree;
	private TorqueSubActivitySO currentTorqueLevel;

	[Header("Level Data - Equilibrium")]
	[SerializeField] private EquilibriumSubActivitySO equilibriumLevelOne;
	[SerializeField] private EquilibriumSubActivitySO equilibriumLevelTwo;
	[SerializeField] private EquilibriumSubActivitySO equilibriumLevelThree;
	private EquilibriumSubActivitySO currentEquilibriumLevel;


	[Header("Views")]
    [SerializeField] private MomentOfInertiaView momentOfInertiaView;
	[SerializeField] private TorqueView torqueView;
	[SerializeField] private EquilibriumView equilibriumView;
	[SerializeField] private ActivityEightPerformanceView performanceView;


	[Header("Submission Status Displays")]
	[SerializeField] private MomentOfInertiaSubmissionStatusDisplay momentOfInertiaSubmissionStatusDisplay;
	[SerializeField] private TorqueSubmissionStatusDisplay torqueSubmissionStatusDisplay;
	[SerializeField] private EquilibriumSubmissionStatusDisplay equilibriumSubmissionStatusDisplay;

	// Given Data - Moment of Inertia
	private MomentOfInertiaData momentOfInertiaGivenData;
	private List<TorqueData> torqueGivenData;
	private EquilibriumData equilibriumGivenData;

	// Variables for keeping track of current number of tests
	private int currentNumMomentOfInertiaTests;
	private int currentNumOfTorqueTests;
	private int currentNumOfEquilibriumTests;

	// Gameplay performance metrics variables
	// Gameplay Time
	private float gameplayTime;
	// Moment of Inertia
	private bool isMomentOfInertiaCalculationFinished;
	private int numIncorrectMomentOfInertiaSubmission;
	private int numCorrectMomentOfInertiaSubmission;
	private float momentOfInertiaDuration;
	// Torque
	private bool isTorqueCalculationFinished;
	private int numIncorrectTorqueSubmission;
	private int numCorrectTorqueSubmission;
	private float torqueDuration;
	// Equilibrium
	private bool isEquilibriumCalculationFinished;
	private int numIncorrectEquilibriumSubmission;
	private int numCorrectEquilibriumSubmission;
	private float equilibriumDuration;

	protected override void Start()
    {
		base.Start();
		// Set level data based from difficulty configuration.
		ConfigureLevelData(Difficulty.Easy); // IN THE FUTURE, REPLACE WITH WHATEVER SELECTED DIFFICULTY. FOR NOW SET FOR TESTING

		// Subscribing to view events
		MomentOfInertiaView.SubmitAnswerEvent += CheckMomentOfInertiaAnswers;
		MomentOfInertiaSubmissionStatusDisplay.ProceedEvent += GenerateNewMomentOfInertiaTest;
		MomentOfInertiaSubmissionStatusDisplay.ProceedEvent += CloseMomentOfInertiaView;
		TorqueView.SubmitAnswerEvent += CheckTorqueAnswers;
		TorqueSubmissionStatusDisplay.ProceedEvent += GenerateNewTorqueTest;
		TorqueSubmissionStatusDisplay.ProceedEvent += CloseTorqueView;
		EquilibriumView.SubmitAnswerEvent += CheckEquilibriumAnswers;
		EquilibriumSubmissionStatusDisplay.ProceedEvent += GenerateNewEquilibriumTest;
		EquilibriumSubmissionStatusDisplay.ProceedEvent += CloseEquilibriumView;
		RebootButton.PressRebootButtonEvent += DisplayPerformanceView;

		// Initializing given values
		GenerateMomentOfInertiaGivenData(currentMomentOfInertiaLevel);
		GenerateTorqueGivenData(currentTorqueLevel);
		GenerateEquilibriumGivenData(currentEquilibriumLevel);

		// Setting number of tests
		currentNumMomentOfInertiaTests = currentMomentOfInertiaLevel.numberOfTests;
		currentNumOfTorqueTests = currentTorqueLevel.numberOfTests;
		currentNumOfEquilibriumTests = currentEquilibriumLevel.numberOfTests;

		// Setting up views
		momentOfInertiaView.SetupMomentOfInertiaView(momentOfInertiaGivenData);
		momentOfInertiaView.UpdateCalibrationTestTextDisplay(0, currentMomentOfInertiaLevel.numberOfTests);
		torqueView.SetupTorqueView(torqueGivenData);
		torqueView.UpdateCalibrationTestTextDisplay(0, currentTorqueLevel.numberOfTests);
		equilibriumView.SetupEquilibriumView(equilibriumGivenData);
		equilibriumView.UpdateCalibrationTestTextDisplay(0, currentEquilibriumLevel.numberOfTests);
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
				currentMomentOfInertiaLevel = momentOfInertiaLevelOne;
				currentTorqueLevel = torqueLevelOne;
				currentEquilibriumLevel = equilibriumLevelOne;
				break;
			case Difficulty.Medium:
				currentMomentOfInertiaLevel = momentOfInertiaLevelTwo;
				currentTorqueLevel = torqueLevelTwo;
				currentEquilibriumLevel = equilibriumLevelTwo;
				break;
			case Difficulty.Hard:
				currentMomentOfInertiaLevel = momentOfInertiaLevelThree;
				currentTorqueLevel = torqueLevelThree;
				currentEquilibriumLevel = equilibriumLevelThree;
				break;
		}
	}

	#region Moment of Inertia

	/// <summary>
	/// Generates the given data for Moment of Inertia from level data based on <c>MomentOfInertiaSubActivitySO</c>.
	/// </summary>
	/// <param name="momentOfInertiaSO"></param>
	private void GenerateMomentOfInertiaGivenData(MomentOfInertiaSubActivitySO momentOfInertiaSO)
	{
		MomentOfInertiaData data = new MomentOfInertiaData();

		// Randomly pick inertia InertiaObjectType
		List<InertiaObjectType> inertiaObjectTypes = new List<InertiaObjectType>
		{
			InertiaObjectType.SlenderRodCenter,
			InertiaObjectType.SlenderRodEnd,
			InertiaObjectType.RectangularPlateCenter,
			InertiaObjectType.RectangularPlateEdge,
			InertiaObjectType.HollowCylinder,
			InertiaObjectType.SolidCylinder,
			InertiaObjectType.ThinWalledHollowCylinder,
			InertiaObjectType.SolidSphere,
			InertiaObjectType.ThinWalledHollowSphere,
			InertiaObjectType.SolidDisk
		};
		data.inertiaObjectType = inertiaObjectTypes[Random.Range(0, inertiaObjectTypes.Count)];

		// Setup/assign based from inertia object type
		int mass = Random.Range(momentOfInertiaSO.massMinVal, momentOfInertiaSO.massMaxVal);
		int length = Random.Range(momentOfInertiaSO.lengthMinVal, momentOfInertiaSO.lengthMaxVal);
		int plateLengthA = Random.Range(momentOfInertiaSO.lengthMinVal, momentOfInertiaSO.lengthMaxVal);
		int plateLengthB = Random.Range(momentOfInertiaSO.lengthMinVal, momentOfInertiaSO.lengthMaxVal);
		int radius = Random.Range(momentOfInertiaSO.radiusMinVal, momentOfInertiaSO.radiusMaxVal);
		int innerRadius = Random.Range(momentOfInertiaSO.radiusMinVal, momentOfInertiaSO.radiusMaxVal);
		int outerRadius = Random.Range(momentOfInertiaSO.radiusMinVal, momentOfInertiaSO.radiusMaxVal);

		data.mass = mass;
		switch (data.inertiaObjectType)
		{
			case InertiaObjectType.SlenderRodCenter:
			case InertiaObjectType.SlenderRodEnd:
				data.length = length;
				break;
			case InertiaObjectType.RectangularPlateCenter:
				data.plateLengthA = plateLengthA;
				data.plateLengthB = plateLengthB;
				break;
			case InertiaObjectType.RectangularPlateEdge:
				data.plateLengthA = plateLengthA;
				break;
			case InertiaObjectType.HollowCylinder:
				data.innerRadius = innerRadius;
				data.outerRadius = outerRadius;
				break;
			case InertiaObjectType.SolidCylinder:
			case InertiaObjectType.ThinWalledHollowCylinder:
			case InertiaObjectType.SolidSphere:
			case InertiaObjectType.ThinWalledHollowSphere:
			case InertiaObjectType.SolidDisk:
				data.radius = radius;
				break;
		}

		momentOfInertiaGivenData = data;
	}

	/// <summary>
	/// Checks the submitted Moment of Inertia answer from <c>MomentOfInertiaView</c>
	/// </summary>
	/// <param name="answer"></param>
	private void CheckMomentOfInertiaAnswers(MomentOfInertiaAnswerSubmission answer)
	{
		MomentOfInertiaAnswerSubmissionResults results = new MomentOfInertiaAnswerSubmissionResults(
			isInertiaObjectTypeCorrect: answer.inertiaObjectType == momentOfInertiaGivenData.inertiaObjectType,
			isMomentOfInertiaCorrect: ActivityEightUtilities.ValidateMomentOfInertiaSubmission(answer.momentOfInertia, momentOfInertiaGivenData)
			);

		// Display moment of inertia submission results
		DisplayMomentOfInertiaSubmissionResults(results);
	}

	/// <summary>
	/// Displays the validation results of submitted Moment of Inertia answer stored in <c>MomentOfInertiaAnswerSubmissionResults</c>.
	/// </summary>
	/// <param name="results"></param>
	private void DisplayMomentOfInertiaSubmissionResults(MomentOfInertiaAnswerSubmissionResults results)
	{
		if (results.isAllCorrect())
		{
			numCorrectMomentOfInertiaSubmission++;
			currentNumMomentOfInertiaTests--;
			string displayText;
			if (currentNumMomentOfInertiaTests <= 0)
			{
				displayText = "Calculations correct. The Moment of Inertia Calculation module is now calibrated.";
				isMomentOfInertiaCalculationFinished = true;
				momentOfInertiaDuration = gameplayTime;
			}
			else
			{
				displayText = "Calculations correct. Loaded next test.";
			}
			momentOfInertiaSubmissionStatusDisplay.SetSubmissionStatus(true, displayText);

			momentOfInertiaView.UpdateCalibrationTestTextDisplay(currentMomentOfInertiaLevel.numberOfTests - currentNumMomentOfInertiaTests, currentMomentOfInertiaLevel.numberOfTests);
		} else
		{
			numIncorrectMomentOfInertiaSubmission++;
			momentOfInertiaSubmissionStatusDisplay.SetSubmissionStatus(false, "The system found discrepancies in your calculations. Please review and fix it.");
		}

		// Update status border displays from result
		momentOfInertiaSubmissionStatusDisplay.UpdateStatusBorderDisplaysFromResult(results);

		momentOfInertiaSubmissionStatusDisplay.gameObject.SetActive(true);
	}

	/// <summary>
	/// Generates new given data for Moment of Inertia, and sets up <c>MomentOfInertiaView</c>
	/// for display of newly generated given data.
	/// </summary>
	private void GenerateNewMomentOfInertiaTest()
	{
		if (currentNumMomentOfInertiaTests > 0)
		{
			GenerateMomentOfInertiaGivenData(currentMomentOfInertiaLevel);
			momentOfInertiaView.SetupMomentOfInertiaView(momentOfInertiaGivenData);
		}
	}

	/// <summary>
	/// Closes <c>MomentOfInertiaView</c>.
	/// </summary>
	private void CloseMomentOfInertiaView()
	{
		if (currentNumMomentOfInertiaTests <= 0)
		{
			inputReader.SetGameplay();
			momentOfInertiaView.gameObject.SetActive(false);
			GeneratorRoomClearEvent?.Invoke();
		}
	}

	#endregion

	#region Torque
	/// <summary>
	/// Generates the given data for Torque from level data based on <c>TorqueSubActivitySO</c>.
	/// </summary>
	/// <param name="torqueSubActivitySO"></param>
	private void GenerateTorqueGivenData(TorqueSubActivitySO torqueSubActivitySO)
	{
		torqueGivenData = new List<TorqueData>();

		for (int i = 0; i < 3; i++)
		{
			TorqueData data = new TorqueData();
			data.force = Random.Range(torqueSubActivitySO.forceMinVal, torqueSubActivitySO.forceMaxVal);
			data.distanceVector = Random.Range(torqueSubActivitySO.distanceVectorMinVal, torqueSubActivitySO.distanceVectorMaxVal);
			List<TorqueDirection> directions = new List<TorqueDirection> { TorqueDirection.Upward, TorqueDirection.Downward};
			data.torqueDirection = directions[Random.Range(0, 2)];

			torqueGivenData.Add(data);
		}
	}

	/// <summary>
	/// Checks the submitted Torque answer from <c>List</c> containing <c>TorqueAnswerSubmission</c>.
	/// </summary>
	/// <param name="answer"></param>
	private void CheckTorqueAnswers(List<TorqueAnswerSubmission> answer)
	{
		bool isTorqueMagnitudeCorrect = true;
		bool isTorqueDirectionCorrect = true;
		for (int i = 0; i < 3; i++)
		{
			// Check Torque Magnitude
			if (isTorqueMagnitudeCorrect == true)
			{
				isTorqueMagnitudeCorrect = ActivityEightUtilities.ValidateTorqueMagnitudeSubmission(answer[i].torqueMagnitude, torqueGivenData[i]);
			}

			// Check Torque Direction
			if (isTorqueDirectionCorrect == true)
			{
				isTorqueDirectionCorrect = answer[i].torqueDirection == torqueGivenData[i].torqueDirection;
			}
		}

		TorqueAnswerSubmissionResults results = new TorqueAnswerSubmissionResults(
			isTorqueMagnitudeCorrect: isTorqueMagnitudeCorrect,
			isTorqueDirectionCorrect: isTorqueDirectionCorrect
			);

		// Display torque submission results
		DisplayTorqueSubmissionResults(results);
	}

	/// <summary>
	/// Displays the validation results of submitted Torque answer stored in <c>TorqueAnswerSubmissionResults</c>.
	/// </summary>
	/// <param name="results"></param>
	private void DisplayTorqueSubmissionResults(TorqueAnswerSubmissionResults results)
	{
		if (results.isAllCorrect())
		{
			numCorrectTorqueSubmission++;
			currentNumOfTorqueTests--;
			string displayText;
			if (currentNumOfTorqueTests <= 0)
			{
				displayText = "Calculations correct. The Torque Calculation module is now calibrated.";
				isTorqueCalculationFinished = true;
				torqueDuration = gameplayTime - momentOfInertiaDuration;
			}
			else
			{
				displayText = "Calculations correct. Loaded next test.";
			}
			torqueSubmissionStatusDisplay.SetSubmissionStatus(true, displayText);

			torqueView.UpdateCalibrationTestTextDisplay(currentTorqueLevel.numberOfTests - currentNumOfTorqueTests, currentTorqueLevel.numberOfTests);
		} else
		{
			numIncorrectTorqueSubmission++;
			torqueSubmissionStatusDisplay.SetSubmissionStatus(false, "The system found discrepancies in your calculations. Please review and fix it.");
		}

		// Update status border displays from result
		torqueSubmissionStatusDisplay.UpdateStatusBorderDisplaysFromResult(results);

		torqueSubmissionStatusDisplay.gameObject.SetActive(true);
	}

	/// <summary>
	/// Generates new given data for Torque, and sets up <c>TorqueView</c>
	/// for display of newly generated given data.
	/// </summary>
	private void GenerateNewTorqueTest()
	{
		if (currentNumOfTorqueTests > 0)
		{
			GenerateTorqueGivenData(currentTorqueLevel);
			torqueView.SetupTorqueView(torqueGivenData);
		}
	}

	/// <summary>
	/// Closes <c>TorqueView</c>.
	/// </summary>
	private void CloseTorqueView()
	{
		if (currentNumOfTorqueTests <= 0)
		{
			inputReader.SetGameplay();
			torqueView.gameObject.SetActive(false);
			WeighingScaleRoomClearEvent?.Invoke();
		}
	}
	#endregion

	#region Equilibrium

	/// <summary>
	/// Generates the given data for Equilibrium from level data based on <c>EquilibriumSubActivitySO</c>.
	/// </summary>
	/// <param name="equilibriumSubActivitySO"></param>
	private void GenerateEquilibriumGivenData(EquilibriumSubActivitySO equilibriumSubActivitySO)
	{
		EquilibriumData data = new EquilibriumData();

		data.weighingApparatusWeight = Random.Range(equilibriumSubActivitySO.weightMinVal, equilibriumSubActivitySO.weightMaxVal);
		data.redBoxWeight = Random.Range(equilibriumSubActivitySO.weightMinVal, equilibriumSubActivitySO.weightMaxVal);
		data.redBoxDistance = Random.Range(equilibriumSubActivitySO.distanceMinVal, equilibriumSubActivitySO.distanceMaxVal);
		data.blueBoxWeight = Random.Range(equilibriumSubActivitySO.weightMinVal, equilibriumSubActivitySO.weightMaxVal);
		data.blueBoxDistance = Random.Range(equilibriumSubActivitySO.distanceMinVal, equilibriumSubActivitySO.distanceMaxVal);
		data.fulcrumForce = data.weighingApparatusWeight + data.redBoxWeight + data.blueBoxWeight;

		equilibriumGivenData = data;
	}

	/// <summary>
	/// Checks the submitted Equilibrium answer from <c>EquilibriumView</c>.
	/// </summary>
	/// <param name="answer"></param>
	private void CheckEquilibriumAnswers(EquilibriumAnswerSubmission answer)
	{
		// Store counterclockwise and clockwise torque data in TorqueData instances
		TorqueData counterclockwiseTorqueData = new TorqueData();
		counterclockwiseTorqueData.force = equilibriumGivenData.redBoxWeight;
		counterclockwiseTorqueData.distanceVector = equilibriumGivenData.redBoxDistance;

		TorqueData clockwiseTorqueData = new TorqueData();
		clockwiseTorqueData.force = equilibriumGivenData.blueBoxWeight;
		clockwiseTorqueData.distanceVector = equilibriumGivenData.blueBoxDistance;

		EquilibriumAnswerSubmissionResults results = new EquilibriumAnswerSubmissionResults(
			isSummationOfDownwardForcesCorrect: ActivityEightUtilities.ValidateSummationOfDownwardForcesSubmission(answer.summationOfDownwardForces, equilibriumGivenData),
			isUpwardForceCorrect: ActivityEightUtilities.ValidateUpwardForceSubmission(answer.upwardForce, equilibriumGivenData),
			isSummationOfTotalForcesCorrect: ActivityEightUtilities.ValidateSummationOfTotalForcesSubmission(answer.summationOfTotalForces, equilibriumGivenData),
			isCounterclockwiseTorqueCorrect: ActivityEightUtilities.ValidateTorqueMagnitudeSubmission(answer.counterclockwiseTorque, counterclockwiseTorqueData),
			isClockwiseTorqueCorrect: ActivityEightUtilities.ValidateTorqueMagnitudeSubmission(answer.clockwiseTorque, clockwiseTorqueData),
			isEquilibriumTypeCorrect: ActivityEightUtilities.ValidateEquilibriumTypeSubmission(answer.equilibriumType, equilibriumGivenData)
			);

		// Display equilibrium submission results
		DisplayEquilibriumSubmissionResults(results);
	}

	/// <summary>
	/// Displays the validation results of submitted Equilibrium answer stored in <c>EquilibriumAnswerSubmissionResults</c>.
	/// </summary>
	/// <param name="results"></param>
	private void DisplayEquilibriumSubmissionResults(EquilibriumAnswerSubmissionResults results)
	{
		if (results.isAllCorrect())
		{
			numCorrectEquilibriumSubmission++;
			currentNumOfEquilibriumTests--;
			string displayText;
			if (currentNumOfEquilibriumTests <= 0)
			{
				displayText = "Calculations correct. The Equilibium Calculation module is now calibrated.";
				isEquilibriumCalculationFinished = true;
				equilibriumDuration = gameplayTime - torqueDuration - momentOfInertiaDuration;
			} else
			{
				displayText = "Calculations correct. Loaded next test.";
			}
			equilibriumSubmissionStatusDisplay.SetSubmissionStatus(true, displayText);

			equilibriumView.UpdateCalibrationTestTextDisplay(currentEquilibriumLevel.numberOfTests - currentNumOfEquilibriumTests, currentEquilibriumLevel.numberOfTests);
		} else
		{
			numIncorrectEquilibriumSubmission++;
			equilibriumSubmissionStatusDisplay.SetSubmissionStatus(false, "The system found discrepancies in your calculations. Please review and fix it.");
		}

		// Update status border displays from result
		equilibriumSubmissionStatusDisplay.UpdateStatusBorderDisplaysFromResult(results);

		equilibriumSubmissionStatusDisplay.gameObject.SetActive(true);
	}

	/// <summary>
	/// Generates new given data for Equilibrium, and sets up <c>EquilibriumView</c>
	/// for display of newly generated given data.
	/// </summary>
	private void GenerateNewEquilibriumTest()
	{
		if (currentNumOfEquilibriumTests > 0)
		{
			GenerateEquilibriumGivenData(currentEquilibriumLevel);
			equilibriumView.SetupEquilibriumView(equilibriumGivenData);
		}
	}

	/// <summary>
	/// Closes <c>EquilibriumView</c>.
	/// </summary>
	private void CloseEquilibriumView()
	{
		if (currentNumOfEquilibriumTests <= 0)
		{
			inputReader.SetGameplay();
			equilibriumView.gameObject.SetActive(false);
			RebootRoomClearEvent?.Invoke();
		}
	}
	#endregion

	public override void DisplayPerformanceView()
	{
		inputReader.SetUI();
		performanceView.gameObject.SetActive(true);

		performanceView.SetTotalTimeDisplay(gameplayTime);

		performanceView.SetMomentOfInertiaMetricsDisplay(
			isAccomplished: isMomentOfInertiaCalculationFinished,
			numIncorrectSubmission: numIncorrectMomentOfInertiaSubmission,
			duration: momentOfInertiaDuration
			);

		performanceView.SetTorqueMetricsDisplay(
			isAccomplished: isTorqueCalculationFinished,
			numIncorrectSubmission: numIncorrectTorqueSubmission,
			duration: torqueDuration
			);

		performanceView.SetEquilibriumMetricsDisplay(
			isAccomplished: isEquilibriumCalculationFinished,
			numIncorrectSubmission: numIncorrectEquilibriumSubmission,
			duration: equilibriumDuration
			);

		// Update its activity feedback display (three args)
		performanceView.UpdateActivityFeedbackDisplay(
			new SubActivityPerformanceMetric(
				subActivityName: "moment of inertia",
				isSubActivityFinished: isMomentOfInertiaCalculationFinished,
				numIncorrectAnswers: numIncorrectMomentOfInertiaSubmission,
				numCorrectAnswers: numCorrectMomentOfInertiaSubmission,
				badScoreThreshold: 3,
				averageScoreThreshold: 2
				),
			new SubActivityPerformanceMetric(
				subActivityName: "torque",
				isSubActivityFinished: isTorqueCalculationFinished,
				numIncorrectAnswers: numIncorrectTorqueSubmission,
				numCorrectAnswers: numCorrectTorqueSubmission,
				badScoreThreshold: 3,
				averageScoreThreshold: 2
				),
			new SubActivityPerformanceMetric(
				subActivityName: "equilibrium",
				isSubActivityFinished: isEquilibriumCalculationFinished,
				numIncorrectAnswers: numIncorrectEquilibriumSubmission,
				numCorrectAnswers: numCorrectEquilibriumSubmission,
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
		if (!isMomentOfInertiaCalculationFinished)
		{
			taskText.Add("- Fix the generator's power terminal by calibrating its moment of inertia calculation module.");
		}
		if (!isTorqueCalculationFinished)
		{
			taskText.Add("- Secure the bolts of the fulcrum on the weighing scale using its terminal by calibrating its torque calculation module.");
		}
		if (!isEquilibriumCalculationFinished)
		{
			taskText.Add("- Ensure equilibrium of the power switch by calibrating the terminal's equilibrium calculation module.");
		}

		List<string> objectiveText = new List<string>();
		objectiveText.Add("Conduct proper maintenance on the ship�s generator, industrial switch, and properly reboot the ships' system.");

		activityPauseMenuUI.UpdateContent("Lesson 8 - Activity 8", taskText, objectiveText);
	}
}