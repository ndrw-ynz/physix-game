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
}
