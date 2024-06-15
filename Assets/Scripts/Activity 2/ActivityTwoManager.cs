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
		SetupViewCartesianComponents();
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

	private void SetupViewCartesianComponents()
	{
		// Randomly generate vectors from SO
		int minimumMagnitudeValue = vectorsLevelOneSO.minimumMagnitudeValue;
		int maximumMagnitudeValue = vectorsLevelOneSO.maximumMagnitudeValue;
		int numberOfVectors = vectorsLevelOneSO.numberOfVectors;
		DirectionType directionType = vectorsLevelOneSO.directionType;

		for (int i = 0; i < numberOfVectors; i++)
		{
			// Setting magnitude value
			int magnitudeValue = Random.Range(minimumMagnitudeValue, maximumMagnitudeValue);
			// Setting direction value
			int directionValue = 0;
			switch (directionType)
			{
				case DirectionType.Cardinal:
					int[] cardinalDirectionValues = new int[] { 0, 90, 180, 270 };
					directionValue = cardinalDirectionValues[Random.Range(0, cardinalDirectionValues.Length)];
					break;
				case DirectionType.Standard:
					int[] standardDirectionValues = new int[] { 0, 30, 45, 60, 90, 120, 135, 1150, 180, 210, 225, 240, 270, 300, 315, 330 };
					directionValue = standardDirectionValues[Random.Range(0, standardDirectionValues.Length)];
					break;

				case DirectionType.FullRange:
					directionValue = Random.Range(0, 360);
					break;
			}
			Debug.Log($"Magnitude: {magnitudeValue} \nDirection: {directionValue}");

			viewCartesianComponents.AddVectorInfo(magnitudeValue, directionValue, directionType);
		}

	}
}
