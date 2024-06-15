using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ActivityTwoManager : MonoBehaviour
{
	[SerializeField] private QuantitiesSubActivitySO quantitiesLevelOneSO;
	[SerializeField] private VectorsSubActivitySO vectorsLevelOneSO;

	[SerializeField] private ViewQuantities viewQuantities;
	[SerializeField] private ViewCartesianComponents viewCartesianComponents;

	private bool isQuantitiesSubActivityFinished;
	private int numIncorrectQuantitiesSubmission;
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
		VectorDisplay vectorDisplay = vectorInfo.vectorDisplay;
		// Check submitted equation for x component
		DraggableNumericalExpression[] xComponentNumericalExpressions = vectorDisplay.xComponentEquationHolder.expressionHolder.GetComponentsInChildren<DraggableNumericalExpression>();
		float xComponentEquationResult = ActivityTwoUtilities.EvaluateNumericalExpressions(xComponentNumericalExpressions);
		bool isXComponentEquationCorrect = Mathf.Abs((float)(xComponentEquationResult - vectorInfo.vectorComponents.x)) <= 0.0001;
		Debug.Log($"Resulting x equation: {isXComponentEquationCorrect}");

		// Check submitted value for x component
		bool isXValueCorrect = ActivityTwoUtilities.ValidateValueSubmission(vectorDisplay.xComponentInputText.text, vectorInfo.vectorComponents.x);
		Debug.Log($"Result of submitted x: {isXValueCorrect}");

		// Check submitted equation for y component
		DraggableNumericalExpression[] yComponentNumericalExpressions = vectorDisplay.yComponentEquationHolder.expressionHolder.GetComponentsInChildren<DraggableNumericalExpression>();
		float yComponentEquationResult = ActivityTwoUtilities.EvaluateNumericalExpressions(yComponentNumericalExpressions);
		bool isYComponentEquationCorrect = Mathf.Abs((float)(yComponentEquationResult - vectorInfo.vectorComponents.y)) <= 0.0001;
		Debug.Log($"Resulting y equation: {isYComponentEquationCorrect}");

		// Check submitted value for y component
		bool isYValueCorrect = ActivityTwoUtilities.ValidateValueSubmission(vectorDisplay.yComponentInputText.text, vectorInfo.vectorComponents.y);
		Debug.Log($"Result of submitted y: {isYValueCorrect}");
	}
}
