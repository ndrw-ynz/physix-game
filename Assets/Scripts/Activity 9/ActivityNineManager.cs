using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GravityData
{
	public double planetMass;
	public float planetMassSNCoefficient;
	public int planetMassSNExponent;

	public double orbittingObjectMass;
	public float orbittingObjectMassSNCoefficient;
	public int orbittingObjectMassSNExponent;

	public double distanceBetweenObjects;
}

public class ActivityNineManager : MonoBehaviour
{
	public static Difficulty difficultyConfiguration;

	[Header("Level Data - Gravity")]
	[SerializeField] private GravitySubActivitySO gravityLevelOne;
	[SerializeField] private GravitySubActivitySO gravityLevelTwo;
	[SerializeField] private GravitySubActivitySO gravityLevelThree;
	private GravitySubActivitySO currentGravityLevel;

	[Header("View")]
	[SerializeField] private GravityView gravityView;

	// Given Data - Gravity
	private GravityData gravityGivenData;

	// Variable for keeping track of current number of tests.
	private int currentNumGravityTests;

	private void Start()
	{
		// Set level data based from difficulty configuration.
		ConfigureLevelData(Difficulty.Easy); // IN THE FUTURE, REPLACE WITH WHATEVER SELECTED DIFFICULTY. FOR NOW SET FOR TESTING

		GenerateGravityGivenData(currentGravityLevel);

		currentNumGravityTests = currentGravityLevel.numberOfTests;

		gravityView.SetupGravityView(gravityGivenData);
		gravityView.UpdateCalibrationTestTextDisplay(0, currentGravityLevel.numberOfTests);
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
}