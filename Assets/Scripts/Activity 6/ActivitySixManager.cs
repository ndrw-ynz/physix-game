using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class DotProductData
{
	public Vector3 satelliteDishVector;
	public Vector3 targetObjectVector;
}

public class ActivitySixManager : MonoBehaviour
{
	public static Difficulty difficultyConfiguration;

	public event Action MainSatelliteAreaClearEvent;

	[Header("Input Reader")]
	[SerializeField] InputReader inputReader;

	[Header("Level Data - Dot Product")]
	[SerializeField] private DotProductSubActivitySO dotProductLevelOne;
	[SerializeField] private DotProductSubActivitySO dotProductLevelTwo;
	[SerializeField] private DotProductSubActivitySO dotProductLevelThree;
	private DotProductSubActivitySO currentDotProductLevel;

	[Header("Views")]
	[SerializeField] private DotProductView dotProductView;

	[Header("Submission Status Displays")]
	[SerializeField] private DotProductSubmissionStatusDisplay dotProductSubmissionStatusDisplay;

	// Given data
	private DotProductData dotProductGivenData;

	// Variables for keeping track of current number of tests
	private int currentNumDotProductTests;

	private void Start()
	{
		ConfigureLevelData(Difficulty.Easy);

		SubscribeViewAndDisplayEvents();

		// Initialize given values
		GenerateDotProductGivenData(currentDotProductLevel);

		// Setting number of tests
		currentNumDotProductTests = currentDotProductLevel.numberOfTests;

		// Setting up views
		dotProductView.SetupDotProductView(dotProductGivenData);
	}

	private void ConfigureLevelData(Difficulty difficulty)
	{
		difficultyConfiguration = difficulty;

		switch (difficulty)
		{
			case Difficulty.Easy:
				currentDotProductLevel = dotProductLevelOne;
				break;
			case Difficulty.Medium:
				currentDotProductLevel = dotProductLevelTwo;
				break;
			case Difficulty.Hard:
				currentDotProductLevel = dotProductLevelThree;
				break;
		}
	}

	private void SubscribeViewAndDisplayEvents()
	{
		dotProductView.SubmitAnswerEvent += CheckDotProductAnswers;
		dotProductSubmissionStatusDisplay.ProceedEvent += UpdateDotProductViewState;
	}

	#region Dot Product
	private void GenerateDotProductGivenData(DotProductSubActivitySO dotProductSO)
	{
		DotProductData data = new DotProductData();
		data.satelliteDishVector = GenerateRandomVector(dotProductSO.satelliteDishVectorMin, dotProductSO.satelliteDishVectorMax);
		data.targetObjectVector = GenerateRandomVector(dotProductSO.targetObjectVectorMin, dotProductSO.targetObjectVectorMax);
		dotProductGivenData = data;
	}

	private Vector3 GenerateRandomVector(Vector3 vectorMinThreshold, Vector3 vectorMaxThreshold)
	{
		Vector3 randomVector = new Vector3();
		randomVector.x = (float) Math.Round(Random.Range(vectorMinThreshold.x, vectorMaxThreshold.x), 1);
		randomVector.y = (float) Math.Round(Random.Range(vectorMinThreshold.y, vectorMaxThreshold.y), 1);
		randomVector.z = (float) Math.Round(Random.Range(vectorMinThreshold.z, vectorMaxThreshold.z), 1);
		return randomVector;
	}

	private void CheckDotProductAnswers(DotProductAnswerSubmission answer)
	{
		DotProductAnswerSubmissionResults results = ActivitySixUtilities.ValidateDotProductSubmission(answer, dotProductGivenData);

		// add func here for updating gameplay metrics variables
		if (results.isAllCorrect()) currentNumDotProductTests--; 
		DisplayDotProductSubmissionResults(results);
	}

	private void DisplayDotProductSubmissionResults(DotProductAnswerSubmissionResults results)
	{
		if (results.isAllCorrect())
		{
			dotProductSubmissionStatusDisplay.SetSubmissionStatus(true, "The submitted calculations are correct.");
		}
		else
		{
			dotProductSubmissionStatusDisplay.SetSubmissionStatus(false, "There are errors in your submission. Please review and fix it.");
		}

		dotProductSubmissionStatusDisplay.UpdateStatusBorderDisplaysFromResult(results);

		dotProductSubmissionStatusDisplay.gameObject.SetActive(true);
	}

	private void UpdateDotProductViewState()
	{
		if (currentNumDotProductTests > 0)
		{
			GenerateDotProductGivenData(currentDotProductLevel);
			dotProductView.SetupDotProductView(dotProductGivenData);
		} else
		{
			dotProductView.gameObject.SetActive(false);
			MainSatelliteAreaClearEvent?.Invoke();
		}
	}
	#endregion
}