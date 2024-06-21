using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActivityTwoManager : MonoBehaviour
{
	[Header("Level Data")]
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
	}
}