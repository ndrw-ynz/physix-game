using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DotProductData
{
	public Vector3 satelliteDishVector;
	public Vector3 targetObjectVector;
}

public class WorkSubActivityData
{
	public WorkSubActivityState workSubActivityState;
	public float acceleration;
	public float mass;
	public float displacement;
	public int angleMeasure;
}

public enum ForceDisplacementCurveType
{
	ConstantForceGraph,
	LinearlyIncreasingForceGraph,
	LinearlyDecreasingForceGraph
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

	[Header("Level Data - Work")]
	[SerializeField] private WorkSubActivitySO workLevelOne;
	[SerializeField] private WorkSubActivitySO workLevelTwo;
	[SerializeField] private WorkSubActivitySO workLevelThree;
	private WorkSubActivitySO currentWorkLevel;

	[Header("Views")]
	[SerializeField] private DotProductView dotProductView;
	[SerializeField] private WorkView workView;
	[SerializeField] private WorkGraphInterpretationView workGraphInterpretationView;

	[Header("Submission Status Displays")]
	[SerializeField] private DotProductSubmissionStatusDisplay dotProductSubmissionStatusDisplay;
	[SerializeField] private WorkSubmissionStatusDisplay workSubmissionStatusDisplay;
	[SerializeField] private WorkGraphSubmissionStatusDisplay workGraphSubmissionStatusDisplay;

	// queue for work sub actvity
	private WorkSubActivityStateMachine workSubActivityStateMachine;
	private Queue<WorkSubActivityState> workSubActivityStateQueue;

	// Given data
	private DotProductData dotProductGivenData;
	private WorkSubActivityData workSubActivityGivenData;
	private Dictionary<ForceDisplacementCurveType, List<Vector2>> forceDisplacementGraphData;

	// Track current graph type displayed
	private ForceDisplacementCurveType currentGraphTypeDisplayed;

	// Variables for keeping track of current number of tests
	private int currentNumDotProductTests;

	private void Start()
	{
		ConfigureLevelData(Difficulty.Easy);

		SubscribeViewAndDisplayEvents();

		// Initialize values for state machines
		workSubActivityStateMachine = new WorkSubActivityStateMachine(workView);
		workSubActivityStateMachine.Initialize(WorkSubActivityState.None);
		InitializeWorkSubActivityStateQueues();


		// Initialize given values
		GenerateDotProductGivenData(currentDotProductLevel);
		GenerateWorkSubActivityGivenData(currentWorkLevel, workSubActivityStateQueue.Peek());
		GenerateForceDisplacementGraphData();
		UpdateCurrentGraphTypeRandomly();

		// Setting number of tests
		currentNumDotProductTests = currentDotProductLevel.numberOfTests;

		// Setting up views
		dotProductView.SetupDotProductView(dotProductGivenData);
		workView.SetupWorkView(workSubActivityGivenData, workSubActivityStateQueue.Peek());
		workGraphInterpretationView.SetupWorkGraphInterpretationView(forceDisplacementGraphData, currentGraphTypeDisplayed);
	}

	private void ConfigureLevelData(Difficulty difficulty)
	{
		difficultyConfiguration = difficulty;

		switch (difficulty)
		{
			case Difficulty.Easy:
				currentDotProductLevel = dotProductLevelOne;
				currentWorkLevel = workLevelOne;
				break;
			case Difficulty.Medium:
				currentDotProductLevel = dotProductLevelTwo;
				currentWorkLevel = workLevelTwo;
				break;
			case Difficulty.Hard:
				currentDotProductLevel = dotProductLevelThree;
				currentWorkLevel = workLevelThree;
				break;
		}
	}

	private void SubscribeViewAndDisplayEvents()
	{
		dotProductView.SubmitAnswerEvent += CheckDotProductAnswers;
		dotProductSubmissionStatusDisplay.ProceedEvent += UpdateDotProductViewState;

		workView.OpenViewEvent += UpdateWorkSubActivityStateMachine;
		workView.SubmitAnswerEvent += CheckWorkSubActivityAnswers;
		workSubmissionStatusDisplay.ProceedEvent += UpdateWorkViewState;

		workGraphInterpretationView.SubmitAnswerEvent += CheckWorkGraphInterpretationAnswer;
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

	#region Work
	private void GenerateWorkSubActivityGivenData(WorkSubActivitySO workSubActivitySO, WorkSubActivityState workSubActivityState)
	{
		WorkSubActivityData data = new WorkSubActivityData();
		data.workSubActivityState = workSubActivityState;
		data.acceleration = (float) Math.Round(Random.Range(workSubActivitySO.accelerationMinVal, workSubActivitySO.accelerationMaxVal), 2);
		data.mass = (float) Math.Round(Random.Range(workSubActivitySO.massMinVal, workSubActivitySO.massMinVal), 2);
		data.displacement = (float)Math.Round(Random.Range(workSubActivitySO.displacementMinVal, workSubActivitySO.displacementMaxVal), 2);
		data.angleMeasure = Random.Range(workSubActivitySO.degreeMinVal, workSubActivitySO.degreeMaxVal);
		workSubActivityGivenData = data;
	}

	private void UpdateWorkSubActivityStateMachine()
	{
		if (workSubActivityStateQueue.Count == 0)
		{
			workSubActivityStateMachine.TransitionToState(WorkSubActivityState.None);
		}
		else
		{
			WorkSubActivityState queueSubActivityHead = workSubActivityStateQueue.Peek();
			workSubActivityStateMachine.TransitionToState(queueSubActivityHead);
		}
	}

	private void InitializeWorkSubActivityStateQueues()
	{
		workSubActivityStateQueue = new Queue<WorkSubActivityState>();
		for (int i = 0; i < currentWorkLevel.numberOfRepetitions; i++)
		{
			workSubActivityStateQueue.Enqueue(WorkSubActivityState.LinearWork);
			workSubActivityStateQueue.Enqueue(WorkSubActivityState.AngularWork);
			workSubActivityStateQueue.Enqueue(WorkSubActivityState.LinearWork);
		}
	}

	private void CheckWorkSubActivityAnswers(WorkSubActivityAnswerSubmission answer)
	{
		WorkSubActivityAnswerSubmissionResults results = ActivitySixUtilities.ValidateWorkSubActivitySubmission(answer, workSubActivityGivenData);

		DisplayWorkSubActivitySubmissionResults(results);
	}

	private void DisplayWorkSubActivitySubmissionResults(WorkSubActivityAnswerSubmissionResults results)
	{
		if (results.isAllCorrect())
		{
			workSubmissionStatusDisplay.SetSubmissionStatus(true, "The submitted calculations are correct.");
		}
		else
		{
			workSubmissionStatusDisplay.SetSubmissionStatus(false, "There are errors in your submission. Please review and fix it.");
		}

		workSubmissionStatusDisplay.UpdateStatusBorderDisplaysFromResult(results);

		workSubmissionStatusDisplay.gameObject.SetActive(true);
	}

	private void UpdateWorkViewState()
	{
		workSubActivityStateQueue.Dequeue();
		if (workSubActivityStateQueue.Count > 0)
		{
			UpdateWorkSubActivityStateMachine();
			GenerateWorkSubActivityGivenData(currentWorkLevel, workSubActivityStateQueue.Peek());
			workView.SetupWorkView(workSubActivityGivenData, workSubActivityStateQueue.Peek());
		}
		else
		{
			workView.gameObject.SetActive(false);
			// event for clear...
		}
	}
	#endregion

	#region Work Graph Interpretation

	private void GenerateForceDisplacementGraphData()
	{
		Dictionary<ForceDisplacementCurveType, List<Vector2>> data = new Dictionary<ForceDisplacementCurveType, List<Vector2>>();

		// Constant
		List<Vector2> constantForceGraphPoints = new List<Vector2>();
		int constantVal = Random.Range(0, 10);
		for (int i = 0; i < 11; i++)
		{
			constantForceGraphPoints.Add(new Vector2(i, constantVal));
		}
		data.Add(ForceDisplacementCurveType.ConstantForceGraph, constantForceGraphPoints);

		// Linearly ascending
		List<Vector2> linearlyIncreasingForceGraphPoints = new List<Vector2>();
		for (int i = 0; i < 11; i++)
		{
			linearlyIncreasingForceGraphPoints.Add(new Vector2(i, i));
		}
		data.Add(ForceDisplacementCurveType.LinearlyIncreasingForceGraph, linearlyIncreasingForceGraphPoints);

		// Linearly descending
		List<Vector2> linearlyDecreasingForceGraphPoints = new List<Vector2>();
		for (int i = 0; i < 11; i++)
		{
			linearlyDecreasingForceGraphPoints.Add(new Vector2(i, 10-i));
		}
		data.Add(ForceDisplacementCurveType.LinearlyDecreasingForceGraph, linearlyDecreasingForceGraphPoints);

		forceDisplacementGraphData = data;
	}

	private void UpdateCurrentGraphTypeRandomly()
	{
		List<ForceDisplacementCurveType> keys = forceDisplacementGraphData.Keys.ToList();

		// Randomly select one key
		int randomIndex = Random.Range(0, keys.Count);
		currentGraphTypeDisplayed = keys[randomIndex];
	}

	private void CheckWorkGraphInterpretationAnswer(float? answer)
	{
		bool result = ActivitySixUtilities.ValidateWorkGraphInterpretationSubmission(answer, forceDisplacementGraphData, currentGraphTypeDisplayed);
		if (result) forceDisplacementGraphData.Remove(currentGraphTypeDisplayed);
		DisplayWorkGraphInterpretationSubmissionResults(result);
	}

	private void DisplayWorkGraphInterpretationSubmissionResults(bool result)
	{
		if (result)
		{
			workGraphSubmissionStatusDisplay.SetSubmissionStatus(true, "The submitted calculations are correct.");
		}
		else
		{
			workGraphSubmissionStatusDisplay.SetSubmissionStatus(false, "There are errors in your submission. Please review and fix it.");
		}

		workGraphSubmissionStatusDisplay.UpdateStatusBorderDisplaysFromResult(result);

		workGraphSubmissionStatusDisplay.gameObject.SetActive(true);
	}
	// update of view
	#endregion
}