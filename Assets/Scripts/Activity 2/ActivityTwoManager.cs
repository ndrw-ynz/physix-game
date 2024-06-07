using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActivityTwoManager : MonoBehaviour
{
    [SerializeField] private QuantitiesSubActivitySO quantitiesLevelOneSO;
	[SerializeField] private ViewQuantities viewQuantities;

	public DraggableQuantityText draggableQuantityTextPrefab;

	private bool isQuantitiesSubActivityFinished;
	private int numIncorrectQuantitiesSubmission;
	private void Start()
	{
		ViewQuantities.SubmitQuantitiesAnswerEvent += CheckQuantitiesAnswer;

		SetupViewQuantities();
	}

	// Randomly generate values for DraggableQuantityText for Quantities subactivity.
	private void SetupViewQuantities()
	{
		QuantityArea given = viewQuantities.givenQuantitiesHolder;
		int numScalarQuantity = quantitiesLevelOneSO.numScalarQuantity;
		int numVectorQuantity = quantitiesLevelOneSO.numVectorQuantity;
		int minimumMagnitudeValue = quantitiesLevelOneSO.minimumMagnitudeValue;
		int maximumMagnitudeValue = quantitiesLevelOneSO.maximumMagnitudeValue;
		List<string> intrinsicScalarMeasurements = quantitiesLevelOneSO.intrinsicallyScalarMeasurements;
		List<string> vectorizableScalarMeasurements = quantitiesLevelOneSO.vectorizableScalarMeasurements;
		List<string> directionDescriptors = quantitiesLevelOneSO.directionDescriptors;

		// Generate Scalars
		for (int i = 0; i < numScalarQuantity; i++)
		{
			DraggableQuantityText scalarQuantity = Instantiate(draggableQuantityTextPrefab);

			// Generate Scalar quantity text display
			int magnitudeValue = Random.Range(minimumMagnitudeValue, maximumMagnitudeValue);
			List<string> availableScalarMeasurements = intrinsicScalarMeasurements.Concat(vectorizableScalarMeasurements).ToList();
			string scalarMeasurementText = availableScalarMeasurements[Random.Range(0, availableScalarMeasurements.Count)];

			scalarQuantity.Initialize(QuantityType.Scalar, $"{magnitudeValue} {scalarMeasurementText}");

			scalarQuantity.transform.SetParent(given.itemHolder.transform, false);
		}

		// Generate Vectors
		for (int i = 0; i < numVectorQuantity; i++)
		{
			DraggableQuantityText vectorQuantity = Instantiate(draggableQuantityTextPrefab);

			// Generate Vector quantity text display
			int magnitudeValue = Random.Range(minimumMagnitudeValue, maximumMagnitudeValue);
			string vectorMeasurementText = vectorizableScalarMeasurements[Random.Range(0, vectorizableScalarMeasurements.Count)];
			string directionDescriptorText = directionDescriptors[Random.Range(0, directionDescriptors.Count)];
			
			vectorQuantity.Initialize(QuantityType.Vector, $"{magnitudeValue} {vectorMeasurementText} {directionDescriptorText}");

			vectorQuantity.transform.SetParent(given.itemHolder.transform, false);
		}
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
