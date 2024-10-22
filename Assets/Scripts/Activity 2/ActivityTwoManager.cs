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

	[Header("Submission Status Displays")]
	[SerializeField] private QuantitiesSubmissionStatusDisplay quantitiesSubmissionStatusDisplay;
	[SerializeField] private CartesianComponentsSubmissionStatusDisplay cartesianComponentsSubmissionStatusDisplay;


	// Variables for keeping track of current number of tests
	private int currentNumCartesianComponentsTests;

	private List<VectorData> givenVectorDataList;

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
	}

	private void SubscribeViewAndDisplayEvents()
	{
		// Quantities Sub Activity Related Events
		quantitiesView.SubmitAnswerEvent += CheckQuantitiesAnswer;
		quantitiesSubmissionStatusDisplay.ProceedEvent += UpdateQuantitiesViewState;

		cartesianComponentsView.SubmitAnswerEvent += CheckCartesianComponentsAnswer;
		cartesianComponentsSubmissionStatusDisplay.ProceedEvent += UpdateCartesianComponentsViewState;
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
			// missionObjectiveDisplayUI.UpdateMissionObjectiveText(0, $"");
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
		//missionObjectiveDisplayUI.ClearMissionObjective(0);
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
			// missionObjectiveDisplayUI.UpdateMissionObjectiveText(1, $"");
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
			//missionObjectiveDisplayUI.ClearMissionObjective(1);
			cartesianComponentsView.gameObject.SetActive(false);
			CartesianComponentsAreaClearEvent?.Invoke();
		}
	}
	#endregion

	public override void DisplayPerformanceView()
	{

	}

	/*[Header("Level Data")]
	[SerializeField] private QuantitiesSubActivitySO quantitiesLevelOneSO;
	[SerializeField] private VectorsSubActivitySO vectorsLevelOneSO;

	[Header("Views")]
	[SerializeField] private ViewQuantities viewQuantities;
	[SerializeField] private ViewCartesianComponents viewCartesianComponents;
	[SerializeField] private ViewVectorAddition viewVectorAddition;
	[SerializeField] private ViewActivityTwoPerformance viewActivityTwoPerformance;

	[Header("Metrics for Quantities Subactivity")]
	public bool isQuantitiesSubActivityFinished;
	private int numIncorrectQuantitiesSubmission;

	[Header("Metrics for Components Subactivity")]
	public bool isComponentsSubActivityFinished;
	private int numIncorrectComponentsSubmission;

	[Header("Metrics for Vector Addition Subactivity")]
	public bool isVectorAdditionSubActivityFinished;
	private int numIncorrectVectorAdditionSubmission;

	private void Start()
	{
		ViewQuantities.SubmitQuantitiesAnswerEvent += CheckQuantitiesAnswer;
		ViewCartesianComponents.SubmitCartesianComponentsAnswerEvent += CheckComponentsAnswer;
		ViewVectorAddition.SubmitVectorAdditionAnswerEvent += CheckVectorAdditionAnswer;
		ViewActivityTwoPerformance.OpenViewEvent += OnOpenViewActivityTwoPerformance;

		viewQuantities.SetupViewQuantities(quantitiesLevelOneSO);
		viewCartesianComponents.SetupViewCartesianComponents(vectorsLevelOneSO);
		viewVectorAddition.SetupViewVectorAddition(viewCartesianComponents.vectorInfoList);

		isQuantitiesSubActivityFinished = false;
		isComponentsSubActivityFinished = false;
		isVectorAdditionSubActivityFinished = false;
	}

	private void CheckQuantitiesAnswer(DraggableQuantityText[] unsolvedQuantities, DraggableQuantityText[] scalarQuantities, DraggableQuantityText[] vectorQuantities)
	{
		bool isCorrect = true;

		// Checking of submitted unsolved quantities.
		if (unsolvedQuantities.Length > 0)
		{
			numIncorrectQuantitiesSubmission += unsolvedQuantities.Length;
			isCorrect = false;
		}

		// Checking of submitted scalar quantities.
		foreach (DraggableQuantityText quantity in scalarQuantities)
		{
			if (quantity.quantityType != QuantityType.Scalar)
			{
				numIncorrectQuantitiesSubmission += 1;
				if (isCorrect) isCorrect = false;
			}
		}

		// Checking of submitted vector quantities.
		foreach (DraggableQuantityText quantity in vectorQuantities)
		{
			if (quantity.quantityType != QuantityType.Vector)
			{
				numIncorrectQuantitiesSubmission += 1;
				if (isCorrect) isCorrect = false;
			}
		}

		// Turn isQuantitiesSubActivityFinished metric to true if no mistakes.
		if (isCorrect)
		{
			isQuantitiesSubActivityFinished = true;
		}

		Debug.Log($"Number of incorrect submissions: {numIncorrectQuantitiesSubmission}");
	}


	private void CheckComponentsAnswer(VectorInfo vectorInfo)
	{
		VectorInfoDisplay vectorDisplay = vectorInfo.vectorInfoDisplay;
		// Check submitted equation for x component
		DraggableNumericalExpression[] xComponentNumericalExpressions = vectorDisplay.xComponentEquationHolder.expressionHolder.GetComponentsInChildren<DraggableNumericalExpression>();
		float xComponentEquationResult = ActivityTwoUtilities.EvaluateNumericalExpressions(xComponentNumericalExpressions);
		bool isXComponentEquationCorrect = Mathf.Abs((float)(xComponentEquationResult - vectorInfo.vectorComponent.x)) <= 0.0001;
		if (!isXComponentEquationCorrect) numIncorrectComponentsSubmission += 1;
		Debug.Log($"Resulting x equation: {isXComponentEquationCorrect}");

		// Check submitted value for x component
		bool isXValueCorrect = ActivityTwoUtilities.ValidateValueSubmission(vectorDisplay.xComponentInputText.text, vectorInfo.vectorComponent.x);
		if (!isXValueCorrect) numIncorrectComponentsSubmission += 1;
		Debug.Log($"Result of submitted x: {isXValueCorrect}");

		// Check submitted equation for y component
		DraggableNumericalExpression[] yComponentNumericalExpressions = vectorDisplay.yComponentEquationHolder.expressionHolder.GetComponentsInChildren<DraggableNumericalExpression>();
		float yComponentEquationResult = ActivityTwoUtilities.EvaluateNumericalExpressions(yComponentNumericalExpressions);
		bool isYComponentEquationCorrect = Mathf.Abs((float)(yComponentEquationResult - vectorInfo.vectorComponent.y)) <= 0.0001;
		if (!isYComponentEquationCorrect) numIncorrectComponentsSubmission += 1;
		Debug.Log($"Resulting y equation: {isYComponentEquationCorrect}");

		// Check submitted value for y component
		bool isYValueCorrect = ActivityTwoUtilities.ValidateValueSubmission(vectorDisplay.yComponentInputText.text, vectorInfo.vectorComponent.y);
		if (!isYValueCorrect) numIncorrectComponentsSubmission += 1;
		Debug.Log($"Result of submitted y: {isYValueCorrect}");

		if (isXComponentEquationCorrect && isXValueCorrect && isYComponentEquationCorrect && isYValueCorrect)
		{
			vectorInfo.isComponentSolved = true;
		}

		Debug.Log($"Number of incorrect submissions: {numIncorrectComponentsSubmission}");

		// Update metrics for cartesian components subactivity
		isComponentsSubActivityFinished = AreComponentsSolved();
	}

	private bool AreComponentsSolved()
	{
		// Check if every component is solved.
		bool isSolved = true;
		foreach (VectorInfo vectorInfo in viewCartesianComponents.vectorInfoList)
		{
			if (vectorInfo.isComponentSolved == false)
			{
				isSolved = false;
				break;
			}
		}
		return isSolved;
	}

	private void CheckVectorAdditionAnswer()
	{
		// Check submission for x and y component addition.
		VectorComponentDisplay[] componentDisplays = viewVectorAddition.vectorComponentDisplayContainer.GetComponentsInChildren<VectorComponentDisplay>();
		List<float> trueXComponents = new List<float>();
		List<float> trueYComponents = new List<float>();
		foreach (VectorComponentDisplay componentDisplay in componentDisplays)
		{
			trueXComponents.Add(float.Parse(componentDisplay.xComponentField.text));
			trueYComponents.Add(float.Parse(componentDisplay.yComponentField.text));
		}

		bool isXComponentFieldsSubmissionCorrect = ActivityTwoUtilities.ValidateComponentInputFields(trueXComponents, viewVectorAddition.xInputFieldContainer);
		bool isYComponentFieldsSubmissionCorrect = ActivityTwoUtilities.ValidateComponentInputFields(trueYComponents, viewVectorAddition.yInputFieldContainer);

		Debug.Log($"x addition submission: {isXComponentFieldsSubmissionCorrect}");
		Debug.Log($"y addition submission: {isYComponentFieldsSubmissionCorrect}");

		// Check magnitude submission 
		bool isMagnitudeSubmissionCorrect = ActivityTwoUtilities.ValidateMagnitudeSubmission(trueXComponents.Sum(), trueYComponents.Sum(), viewVectorAddition.magnitudeResultField);
		Debug.Log($"Magnitude submission: {isMagnitudeSubmissionCorrect}");

		// Check direction submission
		bool isDirectionSubmissionCorrect = ActivityTwoUtilities.ValidateDirectionSubmission(trueXComponents.Sum(), trueYComponents.Sum(), viewVectorAddition.directionResultField);
		Debug.Log($"Direction submission: {isDirectionSubmissionCorrect}");

		// Update metrics for vector addition subactivity
		if (isXComponentFieldsSubmissionCorrect && isYComponentFieldsSubmissionCorrect && isMagnitudeSubmissionCorrect && isDirectionSubmissionCorrect)
		{
			isVectorAdditionSubActivityFinished = true;
		} else
		{
			numIncorrectVectorAdditionSubmission += 1;
		}
	}

	private void OnOpenViewActivityTwoPerformance(ViewActivityTwoPerformance view)
	{
		view.QuantitiesStatusText.text += isQuantitiesSubActivityFinished ? "Accomplished" : "Not accomplished";
		view.QuantitiesIncorrectNumText.text = $"Number of Incorrect Submissions: {numIncorrectQuantitiesSubmission}";

		view.CartesianComponentsStatusText.text += isComponentsSubActivityFinished ? "Accomplished" : "Not accomplished";
		view.CartesianComponentsIncorrectNumText.text = $"Number of Incorrect Submissions: {numIncorrectComponentsSubmission}";

		view.VectorAdditionStatusText.text += isVectorAdditionSubActivityFinished ? "Accomplished" : "Not accomplished";
		view.VectorAdditionIncorrectNumText.text = $"Number of Incorrect Submissions: {numIncorrectVectorAdditionSubmission}";
	}*/
}