using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class VectorData
{
	public int magnitude;
	public int angleMeasure;
}

public class ActivityTwoManager : ActivityManager
{
	public static Difficulty difficultyConfiguration;

	public event Action QuantitiesAreaClearEvent;
	public event Action CartesianComponentsAreaClearEvent;
	public event Action VectorAdditionAreaClearEvent;

	[Header("Level Data - Quantities")]
	[SerializeField] private QuantitiesSubActivitySO quantitiesLevelOne;
	[SerializeField] private QuantitiesSubActivitySO quantitiesLevelTwo;
	[SerializeField] private QuantitiesSubActivitySO quantitiesLevelThree;
	private QuantitiesSubActivitySO currentQuantitiesLevel;

	[Header("Level Data - Vectors")]
	[SerializeField] private VectorsSubActivitySO vectorsLevelOne;
	[SerializeField] private VectorsSubActivitySO vectorsLevelTwo;
	[SerializeField] private VectorsSubActivitySO vectorsLevelThree;
	private VectorsSubActivitySO currentVectorsLevel;

	[Header("Views")]
	[SerializeField] private QuantitiesView quantitiesView;
	[SerializeField] private CartesianComponentsView cartesianComponentsView;
	[SerializeField] private VectorAdditionView vectorAdditionView;
	[SerializeField] private EndConsoleView endConsoleView;
	[SerializeField] private ActivityTwoPerformanceView performanceView;

	[Header("Submission Status Displays")]
	[SerializeField] private QuantitiesSubmissionStatusDisplay quantitiesSubmissionStatusDisplay;
	[SerializeField] private CartesianComponentsSubmissionStatusDisplay cartesianComponentsSubmissionStatusDisplay;
	[SerializeField] private VectorAdditionSubmissionStatusDisplay vectorAdditionSubmissionStatusDisplay;

	// Variables for keeping track of current number of tests
	private int currentNumCartesianComponentsTests;

	private List<VectorData> givenVectorDataList;

	// Variables for tracking which view is currently active
	private bool isQuantitiesViewActive;
	private bool isCartesianComponentsViewActive;
	private bool isVectorAdditionViewActive;

	// Gameplay performance metrics variables
	// Quantities Sub Activity
	private float quantitiesGameplayDuration;
	private bool isQuantitiesSubActivityFinished;
	private int numIncorrectQuantitiesSubmission;
	private int numCorrectQuantitiesSubmission;
	// Cartesian Components Sub Activity
	private float cartesianComponentsGameplayDuration;
	private bool isCartesianComponentsSubActivityFinished;
	private int numIncorrectCartesianComponentsSubmission;
	private int numCorrectCartesianComponentsSubmission;
	// Vector Addition Sub Activity
	private float vectorAdditionGameplayDuration;
	private bool isVectorAdditionSubActivityFinished;
	private int numIncorrectVectorAdditionSubmission;
	private int numCorrectVectorAdditionSubmission;

	protected override void Start()
	{
		base.Start();

		ConfigureLevelData(Difficulty.Easy);

		SubscribeViewAndDisplayEvents();

		// Initialize given values
		GenerateVectorsGivenData(currentVectorsLevel);

		// Setting number of tests
		currentNumCartesianComponentsTests = currentVectorsLevel.numberOfVectors;

		// Setup views
		quantitiesView.SetupQuantitiesView(currentQuantitiesLevel);
		cartesianComponentsView.UpdateNumberOfVectorsTextDisplay(currentVectorsLevel.numberOfVectors - currentNumCartesianComponentsTests, currentVectorsLevel.numberOfVectors);
		cartesianComponentsView.UpdateCartesianComponentsView(givenVectorDataList[currentVectorsLevel.numberOfVectors - currentNumCartesianComponentsTests]);
		vectorAdditionView.SetupVectorAdditionView(givenVectorDataList);

		missionObjectiveDisplayUI.UpdateMissionObjectiveText(1, $"Determine the Cartesian components of the ship's direction vectors along its new course ({currentVectorsLevel.numberOfVectors - currentNumCartesianComponentsTests}/{currentVectorsLevel.numberOfVectors})");
	}
	private void Update()
	{
		if (isQuantitiesViewActive && !isQuantitiesSubActivityFinished) quantitiesGameplayDuration += Time.deltaTime;
		if (isCartesianComponentsViewActive && !isCartesianComponentsSubActivityFinished) cartesianComponentsGameplayDuration += Time.deltaTime;
		if (isVectorAdditionViewActive && !isVectorAdditionSubActivityFinished) vectorAdditionGameplayDuration += Time.deltaTime;
	}

	private void SubscribeViewAndDisplayEvents()
	{
		// Quantities Sub Activity Related Events
		quantitiesView.OpenViewEvent += () => isQuantitiesViewActive = true;
		quantitiesView.QuitViewEvent += () => isQuantitiesViewActive = false;
		quantitiesView.SubmitAnswerEvent += CheckQuantitiesAnswer;
		quantitiesSubmissionStatusDisplay.ProceedEvent += UpdateQuantitiesViewState;

		// Cartesian Components Sub Activity Related Events
		cartesianComponentsView.OpenViewEvent += () => isCartesianComponentsViewActive = true;
		cartesianComponentsView.QuitViewEvent += () => isCartesianComponentsViewActive = false;
		cartesianComponentsView.SubmitAnswerEvent += CheckCartesianComponentsAnswer;
		cartesianComponentsSubmissionStatusDisplay.ProceedEvent += UpdateCartesianComponentsViewState;

		// Vector Addition Sub Activity Related Events
		vectorAdditionView.OpenViewEvent += () => isVectorAdditionViewActive = true;
		vectorAdditionView.QuitViewEvent += () => isVectorAdditionViewActive = false;
		vectorAdditionView.SubmitAnswerEvent += CheckVectorAdditionAnswer;
		vectorAdditionSubmissionStatusDisplay.ProceedEvent += UpdateVectorAdditionViewState;

		endConsoleView.OpenViewEvent += DisplayPerformanceView;
	}

	private void ConfigureLevelData(Difficulty difficulty)
	{
		difficultyConfiguration = difficulty;

		switch (difficulty)
		{
			case Difficulty.Easy:
				currentQuantitiesLevel = quantitiesLevelOne;
				currentVectorsLevel = vectorsLevelOne;
				break;
			case Difficulty.Medium:
				currentQuantitiesLevel = quantitiesLevelTwo;
				currentVectorsLevel = vectorsLevelTwo;
				break;
			case Difficulty.Hard:
				currentQuantitiesLevel = quantitiesLevelThree;
				currentVectorsLevel = vectorsLevelThree;
				break;
		}
	}

	#region Quantities
	private void CheckQuantitiesAnswer(QuantitiesAnswerSubmission answer)
	{
		QuantitiesAnswerSubmissionResults results = ActivityTwoUtilities.ValidateQuantitiesSubmission(answer);

		if (results.isAllCorrect())
		{
			numCorrectQuantitiesSubmission++;
		}
		else
		{
			numIncorrectQuantitiesSubmission++;
		}

		DisplayQuantitiesSubmissionResults(results);
	}

	private void DisplayQuantitiesSubmissionResults(QuantitiesAnswerSubmissionResults results)
	{
		if (results.isAllCorrect())
		{
			quantitiesSubmissionStatusDisplay.SetSubmissionStatus(true, "Quantities are correctly categorized. Fantastic work Captain!");
			missionObjectiveDisplayUI.UpdateMissionObjectiveText(0, $"Categorize the Quantities (1/1)");
		}
		else
		{
			quantitiesSubmissionStatusDisplay.SetSubmissionStatus(false, "Captain, it looks like there's an error. Let's give it another shot!");
		}

		quantitiesSubmissionStatusDisplay.UpdateStatusBorderDisplayFromResults(results);

		quantitiesSubmissionStatusDisplay.gameObject.SetActive(true);
	}

	private void UpdateQuantitiesViewState()
	{
		isQuantitiesSubActivityFinished = true;
		quantitiesView.gameObject.SetActive(false);
		missionObjectiveDisplayUI.ClearMissionObjective(0);
		QuantitiesAreaClearEvent?.Invoke();
	}
	#endregion

	private void GenerateVectorsGivenData(VectorsSubActivitySO vectorsSO)
	{
		givenVectorDataList = new List<VectorData>();

		for (int i = 0; i < vectorsSO.numberOfVectors; i++)
		{
			VectorData generatedVectorData = new VectorData();

			// Setting magnitude value
			generatedVectorData.magnitude = Random.Range(vectorsSO.minimumMagnitudeValue, vectorsSO.maximumMagnitudeValue);
			// Setting direction value
			switch (vectorsSO.directionType)
			{
				case VectorDirectionType.Cardinal:
					int[] cardinalDirectionValues = new int[] { 0, 90, 180, 270 };
					generatedVectorData.angleMeasure = cardinalDirectionValues[Random.Range(0, cardinalDirectionValues.Length)];
					break;
				case VectorDirectionType.Standard:
					int[] standardDirectionValues = new int[] { 0, 30, 45, 60, 90, 120, 135, 1150, 180, 210, 225, 240, 270, 300, 315, 330 };
					generatedVectorData.angleMeasure = standardDirectionValues[Random.Range(0, standardDirectionValues.Length)];
					break;
				case VectorDirectionType.FullRange:
					generatedVectorData.angleMeasure = Random.Range(0, 360);
					break;
			}

			givenVectorDataList.Add(generatedVectorData);
		}
	}

	#region Cartesian Components
	private void CheckCartesianComponentsAnswer(CartesianComponentsAnswerSubmission answer)
	{
		CartesianComponentsAnswerSubmissionResults results = ActivityTwoUtilities.ValidateCartesianComponentsSubmission(answer, givenVectorDataList[currentVectorsLevel.numberOfVectors - currentNumCartesianComponentsTests]);

		if (results.isAllCorrect())
		{
			numCorrectCartesianComponentsSubmission++;
			currentNumCartesianComponentsTests--;
		}
		else
		{
			numIncorrectCartesianComponentsSubmission++;
		}

		DisplayCartesianComponentsSubmissionResults(results);
	}

	private void DisplayCartesianComponentsSubmissionResults(CartesianComponentsAnswerSubmissionResults results)
	{
		if (results.isAllCorrect())
		{
			cartesianComponentsSubmissionStatusDisplay.SetSubmissionStatus(true, "Ship's vector has been readjusted. Nicely done!");
			missionObjectiveDisplayUI.UpdateMissionObjectiveText(1, $"Determine the Cartesian components of the ship's direction vectors along its new course ({currentVectorsLevel.numberOfVectors - currentNumCartesianComponentsTests}/{currentVectorsLevel.numberOfVectors})");
		}
		else
		{
			cartesianComponentsSubmissionStatusDisplay.SetSubmissionStatus(false, "Captain, it looks like there's an error. Let's give it another shot!");
		}

		cartesianComponentsSubmissionStatusDisplay.UpdateStatusBorderDisplaysFromResults(results);

		cartesianComponentsSubmissionStatusDisplay.gameObject.SetActive(true);
	}

	private void UpdateCartesianComponentsViewState()
	{
		if (currentNumCartesianComponentsTests > 0)
		{
			cartesianComponentsView.UpdateNumberOfVectorsTextDisplay(currentVectorsLevel.numberOfVectors - currentNumCartesianComponentsTests, currentVectorsLevel.numberOfVectors);
			cartesianComponentsView.UpdateCartesianComponentsView(givenVectorDataList[currentVectorsLevel.numberOfVectors - currentNumCartesianComponentsTests]);
		}
		else
		{
			isCartesianComponentsSubActivityFinished = true;
			missionObjectiveDisplayUI.ClearMissionObjective(1);
			cartesianComponentsView.gameObject.SetActive(false);
			CartesianComponentsAreaClearEvent?.Invoke();
		}
	}
	#endregion

	#region Vector Addition
	private void CheckVectorAdditionAnswer(VectorAdditionAnswerSubmission answer)
	{
		VectorAdditionAnswerSubmissionResults results = ActivityTwoUtilities.ValidateVectorAdditionSubmission(answer, givenVectorDataList);

		if (results.isAllCorrect())
		{
			numCorrectVectorAdditionSubmission++;
		}  else
		{
			numIncorrectCartesianComponentsSubmission++;
		}
		DisplayVectorAdditionSubmissionResults(results);
	}

	private void DisplayVectorAdditionSubmissionResults(VectorAdditionAnswerSubmissionResults results)
	{
		if (results.isAllCorrect())
		{
			vectorAdditionSubmissionStatusDisplay.SetSubmissionStatus(true, "Congratulations captain! You may now initiate autopilot on the ship captain’s panel.");
			missionObjectiveDisplayUI.UpdateMissionObjectiveText(2, $"Add the ship's direction vectors and determine the magnitude and direction of the resultant vector (1/1)");
		}
		else
		{
			vectorAdditionSubmissionStatusDisplay.SetSubmissionStatus(false, "Captain, it looks like there's an error. Let's give it another shot!");
		}

		vectorAdditionSubmissionStatusDisplay.UpdateStatusBorderDisplaysFromResults(results);

		vectorAdditionSubmissionStatusDisplay.gameObject.SetActive(true);
	}

	private void UpdateVectorAdditionViewState()
	{
		isVectorAdditionSubActivityFinished = true;
		vectorAdditionView.gameObject.SetActive(false);
		missionObjectiveDisplayUI.ClearMissionObjective(2);
		VectorAdditionAreaClearEvent?.Invoke();
	}
	#endregion

	public override void DisplayPerformanceView()
	{
		inputReader.SetUI();
		performanceView.gameObject.SetActive(true);

		performanceView.SetTotalTimeDisplay(quantitiesGameplayDuration + cartesianComponentsGameplayDuration + vectorAdditionGameplayDuration);

		performanceView.SetQuantitiesMetricsDisplay(
			isAccomplished: isQuantitiesSubActivityFinished,
			numIncorrectSubmission: numIncorrectQuantitiesSubmission,
			duration: quantitiesGameplayDuration
			);

		performanceView.SetCartesianComponentsMetricsDisplay(
			isAccomplished: isCartesianComponentsSubActivityFinished,
			numIncorrectSubmission: numIncorrectCartesianComponentsSubmission,
			duration: cartesianComponentsGameplayDuration
			);

		performanceView.SetVectorAdditionMetricsDisplay(
			isAccomplished: isVectorAdditionSubActivityFinished,
			numIncorrectSubmission: numIncorrectVectorAdditionSubmission,
			duration: vectorAdditionGameplayDuration
			);

		// Update its activity feedback display (three args)
		performanceView.UpdateActivityFeedbackDisplay(
			new SubActivityPerformanceMetric(
				subActivityName: "quantities",
				isSubActivityFinished: isQuantitiesSubActivityFinished,
				numIncorrectAnswers: numIncorrectQuantitiesSubmission,
				numCorrectAnswers: numCorrectQuantitiesSubmission,
				badScoreThreshold: 6,
				averageScoreThreshold: 3
				),
			new SubActivityPerformanceMetric(
				subActivityName: "cartesian components",
				isSubActivityFinished: isCartesianComponentsSubActivityFinished,
				numIncorrectAnswers: numIncorrectCartesianComponentsSubmission,
				numCorrectAnswers: numCorrectCartesianComponentsSubmission,
				badScoreThreshold: 4,
				averageScoreThreshold: 2
				),
			new SubActivityPerformanceMetric(
				subActivityName: "vector addition",
				isSubActivityFinished: isVectorAdditionSubActivityFinished,
				numIncorrectAnswers: numIncorrectVectorAdditionSubmission,
				numCorrectAnswers: numCorrectVectorAdditionSubmission,
				badScoreThreshold: 4,
				averageScoreThreshold: 2
				)
			);
	}
}