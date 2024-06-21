using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ViewQuantities : MonoBehaviour
{
	public static event Action<DraggableQuantityText[], DraggableQuantityText[], DraggableQuantityText[]> SubmitQuantitiesAnswerEvent;

	public QuantityArea givenQuantitiesHolder;
	public QuantityArea scalarQuantitiesHolder;
	public QuantityArea vectorQuantitiesHolder;
	public DraggableQuantityText draggableQuantityTextPrefab;
	public Button submitQuantitiesAnswerButton;

	// Randomly generate values for DraggableQuantityText for Quantities subactivity.
	public void SetupViewQuantities(QuantitiesSubActivitySO quantitiesSO)
	{
		QuantityArea given = givenQuantitiesHolder;
		int numScalarQuantity = quantitiesSO.numScalarQuantity;
		int numVectorQuantity = quantitiesSO.numVectorQuantity;
		int minimumMagnitudeValue = quantitiesSO.minimumMagnitudeValue;
		int maximumMagnitudeValue = quantitiesSO.maximumMagnitudeValue;
		List<string> intrinsicScalarMeasurements = quantitiesSO.intrinsicallyScalarMeasurements;
		List<string> vectorizableScalarMeasurements = quantitiesSO.vectorizableScalarMeasurements;
		List<string> directionDescriptors = quantitiesSO.directionDescriptors;

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

	private void OnEnable()
	{
		submitQuantitiesAnswerButton.onClick.AddListener(() => SubmitQuantitiesAnswer());
	}

	private void OnDisable()
	{
		submitQuantitiesAnswerButton.onClick.RemoveAllListeners();
	}

	private void SubmitQuantitiesAnswer()
	{
		DraggableQuantityText[] unsolvedQuantities = givenQuantitiesHolder.itemHolder.GetComponentsInChildren<DraggableQuantityText>();
		DraggableQuantityText[] scalarQuantities = scalarQuantitiesHolder.itemHolder.GetComponentsInChildren<DraggableQuantityText>();
		DraggableQuantityText[] vectorQuantities = vectorQuantitiesHolder.itemHolder.GetComponentsInChildren<DraggableQuantityText>();

		SubmitQuantitiesAnswerEvent?.Invoke(unsolvedQuantities, scalarQuantities, vectorQuantities);
	}
}
