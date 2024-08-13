using System.Collections.Generic;
using UnityEngine;

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
}

public class ActivityEightManager : MonoBehaviour
{
	public static Difficulty difficultyConfiguration;

	[Header("Level Data - Moment of Inertia")]
	[SerializeField] private MomentOfInertiaSubActivitySO momentOfInertiaLevelOne;
	[SerializeField] private MomentOfInertiaSubActivitySO momentOfInertiaLevelTwo;
	[SerializeField] private MomentOfInertiaSubActivitySO momentOfInertiaLevelThree;
	private MomentOfInertiaSubActivitySO currentMomentOfInertiaLevel;


	[Header("Views")]
    [SerializeField] private MomentOfInertiaView momentOfInertiaView;

    private MomentOfInertiaData momentOfInertiaGivenData;

    void Start()
    {
		// Set level data based from difficulty configuration.
		ConfigureLevelData(Difficulty.Easy); // IN THE FUTURE, REPLACE WITH WHATEVER SELECTED DIFFICULTY. FOR NOW SET FOR TESTING

		// Moment of Inertia 
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
				break;
			case Difficulty.Medium:
				currentMomentOfInertiaLevel = momentOfInertiaLevelTwo;
				break;
			case Difficulty.Hard:
				currentMomentOfInertiaLevel = momentOfInertiaLevelThree;
				break;
		}

		// Moment of Inertia setup
		MomentOfInertiaView.SubmitAnswerEvent += CheckMomentOfInertiaAnswers;
		GenerateMomentOfInertiaGivenData(currentMomentOfInertiaLevel);
		momentOfInertiaView.SetupMomentOfInertiaView(momentOfInertiaGivenData);
		momentOfInertiaView.UpdateCalibrationTestTextDisplay(0, currentMomentOfInertiaLevel.numberOfTests);

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

		Debug.Log(results.isInertiaObjectTypeCorrect);
		Debug.Log(results.isMomentOfInertiaCorrect);
	}

	#endregion
}