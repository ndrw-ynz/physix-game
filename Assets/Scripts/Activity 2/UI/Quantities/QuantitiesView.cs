using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class QuantitiesAnswerSubmission
{
	public DraggableQuantityText[] unsolvedQuantities { get; private set; }
	public DraggableQuantityText[] scalarQuantities { get; private set; }
	public DraggableQuantityText[] vectorQuantities { get; private set; }

	public QuantitiesAnswerSubmission(
		DraggableQuantityText[] unsolvedQuantities,
		DraggableQuantityText[] scalarQuantities,
		DraggableQuantityText[] vectorQuantities
		)
	{
		this.unsolvedQuantities = unsolvedQuantities;
		this.scalarQuantities = scalarQuantities;
		this.vectorQuantities = vectorQuantities;
	}
}

public class QuantitiesView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;
	public event Action<QuantitiesAnswerSubmission> SubmitAnswerEvent;

	[Header("Canvas")]
	[SerializeField] private Canvas canvas;

	[Header("Prefabs")]
	public DraggableQuantityText draggableQuantityTextPrefab;

	[Header("Containers")]
	public DraggableQuantityContainer givenQuantitiesHolder;
	public DraggableQuantityContainer scalarQuantitiesHolder;
	public DraggableQuantityContainer vectorQuantitiesHolder;

	private void OnEnable()
	{
		OpenViewEvent?.Invoke();
	}

	public void SetupQuantitiesView(QuantitiesSubActivitySO quantitiesSO)
    {
		List<string> intrinsicScalarMeasurements = quantitiesSO.intrinsicallyScalarMeasurements;
		List<string> vectorizableScalarMeasurements = quantitiesSO.vectorizableScalarMeasurements;
		List<string> directionDescriptors = quantitiesSO.directionDescriptors;

		for (int i = 0; i < quantitiesSO.numberOfQuantities; i++)
		{
			// 50/50 determine if scalar or vector
			bool isScalar = Random.Range(0, 2) == 0;

			if (isScalar)
			{
				// Generate Scalar quantity text
				DraggableQuantityText scalarQuantity = Instantiate(draggableQuantityTextPrefab);

				// Generate Scalar quantity text display
				int magnitudeValue = Random.Range(quantitiesSO.minimumMagnitudeValue, quantitiesSO.maximumMagnitudeValue);
				List<string> availableScalarMeasurements = intrinsicScalarMeasurements.Concat(vectorizableScalarMeasurements).ToList();
				string scalarMeasurementText = availableScalarMeasurements[Random.Range(0, availableScalarMeasurements.Count)];

				scalarQuantity.SetupQuantityDisplay(QuantityType.Scalar, $"{magnitudeValue} {scalarMeasurementText}", canvas);

				scalarQuantity.transform.SetParent(givenQuantitiesHolder.itemHolder.transform, false);
			} else
			{
				// Generate Vector quantity text
				DraggableQuantityText vectorQuantity = Instantiate(draggableQuantityTextPrefab);

				// Generate Vector quantity text display
				int magnitudeValue = Random.Range(quantitiesSO.minimumMagnitudeValue, quantitiesSO.maximumMagnitudeValue);
				string vectorMeasurementText = vectorizableScalarMeasurements[Random.Range(0, vectorizableScalarMeasurements.Count)];
				string directionDescriptorText = directionDescriptors[Random.Range(0, directionDescriptors.Count)];

				vectorQuantity.SetupQuantityDisplay(QuantityType.Vector, $"{magnitudeValue} {vectorMeasurementText} {directionDescriptorText}", canvas);

				vectorQuantity.transform.SetParent(givenQuantitiesHolder.itemHolder.transform, false);
			}
		}
	}

	public void OnSubmitButtonClick()
	{
		QuantitiesAnswerSubmission submission = new QuantitiesAnswerSubmission(
			unsolvedQuantities: givenQuantitiesHolder.itemHolder.GetComponentsInChildren<DraggableQuantityText>(),
			scalarQuantities: scalarQuantitiesHolder.itemHolder.GetComponentsInChildren<DraggableQuantityText>(),
			vectorQuantities: vectorQuantitiesHolder.itemHolder.GetComponentsInChildren<DraggableQuantityText>()
			);

		SubmitAnswerEvent?.Invoke(submission);
	}

	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitViewEvent?.Invoke();
	}
}
