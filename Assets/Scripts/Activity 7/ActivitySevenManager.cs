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
	[SerializeField] private MomentumImpulseForceSubmissionStatusDisplay momentumImpulseForceSubmissionStatusDisplay;

	// Given Values - Center of Mass
	private List<MassCoordinatePair> massCoordinatePairs;
	// Given Values - Impulse-Momentum Force
	private Dictionary<string, float> momentumImpulseForceGivenData;

	// Variables for keeping current number of tests
	private int currentNumCenterOfMassTests;
	private int currentNumMomentumImpulseForceTests;

    void Start()
    {
		// Difficulty selection
		difficultyConfiguration = Difficulty.Hard; // IN THE FUTURE, REPLACE WITH WHATEVER SELECTED DIFFICULTY. FOR NOW SET FOR TESTING
        
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
		MomentumImpulseForceView.SubmitAnswerEvent += CheckMomentumImpulseForceAnswers;

        // Initializing given values
        SetupMassCoordinatePairs(currentCenterOfMassLevel);
		SetMomentumImpulseForceGivenData(currentMomentumImpulseForceLevel);

		// Setting number of problems
		currentNumCenterOfMassTests = currentCenterOfMassLevel.numberOfTests;
		currentNumMomentumImpulseForceTests = currentMomentumImpulseForceLevel.numberOfTests;

		// Setting up views
		centerOfMassView.SetupCenterOfMassView(massCoordinatePairs);
		momentumImpulseForceView.SetupMomentumImpulseForceView(momentumImpulseForceGivenData);
		momentumImpulseForceView.UpdateCalibrationTestTextDisplay(0, currentMomentumImpulseForceLevel.numberOfTests);
    }

	#region Center of Mass

    private void SetupMassCoordinatePairs(CenterOfMassSubActivitySO centerOfMassSO)
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
			// TODO: modify here
			currentNumCenterOfMassTests -= 1;
			string displayText = currentNumCenterOfMassTests <= 0 ? "Calculations correct. The power source cube is now accessible." : "Calculations correct. Loading next test.";
			centerOfMassSubmissionStatusDisplay.SetSubmissionStatus(true, "Calculations correct. The power source cube is now accessible.");

			if (currentNumCenterOfMassTests <= 0)
			{
				RoomOneClearEvent?.Invoke();
			}
			else
			{
				Debug.Log("Generating new center of mass test");
				// Generate new given values and update center of mass view
				SetupMassCoordinatePairs(currentCenterOfMassLevel);
				centerOfMassView.SetupCenterOfMassView(massCoordinatePairs);
			}
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

	private void SetMomentumImpulseForceGivenData(MomentumImpulseForceSubActivitySO momentumImpulseForceSubActivitySO)
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

			Debug.Log(results.isChangeInMomentumCorrect);
			Debug.Log(results.isImpulseCorrect);
			Debug.Log(results.isNetForceCorrect);

			UpdateMomentumImpulseForceView(results);
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

			Debug.Log(results.isInitialMomentumCorrect);
			Debug.Log(results.isFinalMomentumCorrect);
			Debug.Log(results.isChangeInMomentumCorrect);
			Debug.Log(results.isImpulseCorrect);
			Debug.Log(results.isNetForceCorrect);

			UpdateMomentumImpulseForceView(results);
		}
	}

	private void UpdateMomentumImpulseForceView(MomentumImpulseForceAnswerSubmissionResults results)// BETTER RENAME THIS TO SOMETHING BETTER. MEANING FOR  EVERHTINHHG
	{
		if (results is EasyMomentumImpulseForceAnswerSubmissionResults easyResults)
		{
			if (easyResults.isAllCorrect())
			{
				currentNumMomentumImpulseForceTests -= 1;
				string displayText = currentNumMomentumImpulseForceTests <= 0 ? "All calibration tests accomplished." : "Calibration test matches calculations. Loaded next test.";
				momentumImpulseForceSubmissionStatusDisplay.SetSubmissionStatus(true, displayText);

				if (currentNumMomentumImpulseForceTests <= 0)
				{
					RoomTwoClearEvent?.Invoke();
				}
				else
				{
					// Generate new given data for momentum-impulse and force subactivity, and update momentum-impulse force view
					SetMomentumImpulseForceGivenData(currentMomentumImpulseForceLevel);
					momentumImpulseForceView.SetupMomentumImpulseForceView(momentumImpulseForceGivenData);
				}
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
				currentNumMomentumImpulseForceTests -= 1;
				string displayText = currentNumMomentumImpulseForceTests == 0 ? "All calibration tests accomplished." : "Calibration test matches calculations. Loaded next test.";
				momentumImpulseForceSubmissionStatusDisplay.SetSubmissionStatus(true, displayText);

				if (currentNumMomentumImpulseForceTests == 0)
				{
					RoomTwoClearEvent?.Invoke();
				}
				else
				{
					// Generate new given data for momentum-impulse and force subactivity, and update momentum-impulse force view
					SetMomentumImpulseForceGivenData(currentMomentumImpulseForceLevel);
					momentumImpulseForceView.SetupMomentumImpulseForceView(momentumImpulseForceGivenData);
				}
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

	#endregion
}