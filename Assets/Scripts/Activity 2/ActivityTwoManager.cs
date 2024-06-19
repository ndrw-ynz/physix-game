using UnityEngine;

public class ActivityTwoManager : MonoBehaviour
{
	[SerializeField] private QuantitiesSubActivitySO quantitiesLevelOneSO;
	[SerializeField] private VectorsSubActivitySO vectorsLevelOneSO;

	[SerializeField] private ViewQuantities viewQuantities;
	[SerializeField] private ViewCartesianComponents viewCartesianComponents;

	[Header("Metrics for Quantities Subactivity")]
	private bool isQuantitiesSubActivityFinished;
	private int numIncorrectQuantitiesSubmission;
	[Header("Metrics for Components Subactivity")]
	private bool isComponentsSubActivityFinished;
	private int numIncorrectComponentsSubmission;
	private void Start()
	{
		ViewQuantities.SubmitQuantitiesAnswerEvent += CheckQuantitiesAnswer;
		ViewCartesianComponents.SubmitCartesianComponentsAnswerEvent += CheckComponentsAnswer;

		viewQuantities.SetupViewQuantities(quantitiesLevelOneSO);
		viewCartesianComponents.SetupViewCartesianComponents(vectorsLevelOneSO);
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
	}
}
