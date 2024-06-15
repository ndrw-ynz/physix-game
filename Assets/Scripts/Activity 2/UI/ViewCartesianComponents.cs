using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ViewCartesianComponents : MonoBehaviour
{
	public static event Action<VectorInfo> SubmitCartesianComponentsAnswerEvent;

	[Header("Vector Display Holder")]
	public GameObject vectorDisplayHolder;
	[Header("Line Renderer")]
	public VectorLineRenderer vectorLineRenderer;
	[Header("Prefabs")]
	public VectorDisplay vectorDisplayPrefab;
	public DraggableQuantityText draggableQuantityTextPrefab;
	public Button vectorSelectButtonPrefab;
	[Header("Containers")]
	public HorizontalLayoutGroup vectorSelectButtonContainer;
	[Header("Private Variables")]
	private List<VectorInfo> _vectorInfoList = new List<VectorInfo>();

	public void SetupViewCartesianComponents(VectorsSubActivitySO vectorsSO)
	{
		// Randomly generate vectors from SO
		int minimumMagnitudeValue = vectorsSO.minimumMagnitudeValue;
		int maximumMagnitudeValue = vectorsSO.maximumMagnitudeValue;
		int numberOfVectors = vectorsSO.numberOfVectors;
		DirectionType directionType = vectorsSO.directionType;

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

			AddVectorInfo(magnitudeValue, directionValue, directionType);
		}
	}

	private void AddVectorInfo(int magnitudeValue, int directionValue, DirectionType directionType)
	{
		// Setup Vector Display
		VectorDisplay vectorDisplay = Instantiate(vectorDisplayPrefab);
		vectorDisplay.transform.SetParent(vectorDisplayHolder.transform, false);
		vectorDisplay.gameObject.SetActive(false);

		// Setup Vector Info
		VectorInfo vectorInfo = new VectorInfo(magnitudeValue, directionValue, directionType, vectorDisplay);
		_vectorInfoList.Add(vectorInfo);
		vectorDisplay.SetupVectorDisplay(vectorInfo);

		// Setup Vector Select Button for accessing Vector Info
		Button vectorSelectButton = Instantiate(vectorSelectButtonPrefab);
		vectorSelectButton.transform.SetParent(vectorSelectButtonContainer.transform, false);

		TextMeshProUGUI buttonText = vectorSelectButton.GetComponentInChildren<TextMeshProUGUI>();
		buttonText.text = $"{magnitudeValue}m {directionValue}°";

		vectorSelectButton.onClick.AddListener(() => OnVectorSelectButtonClick(vectorInfo));
		vectorDisplay.submitButton.onClick.AddListener(() => SubmitComponentsAnswer(vectorInfo));
	}

	private void OnVectorSelectButtonClick(VectorInfo vectorInfo)
	{
		foreach (VectorInfo vi in _vectorInfoList)
		{
			vi.vectorDisplay.gameObject.SetActive(false);
		}
		vectorInfo.vectorDisplay.gameObject.SetActive(true);
		ChangeLineRenderer(vectorInfo.magnitudeValue, vectorInfo.directionValue);
	}

	private void ChangeLineRenderer(int magnitudeValue, int directionValue)
	{
		float directionRadians = directionValue * Mathf.Deg2Rad;

		float x = magnitudeValue * Mathf.Cos(directionRadians);
		float z = magnitudeValue * Mathf.Sin(directionRadians);
		Vector3 targetPoint = new Vector3(x, 0, z);
		vectorLineRenderer.SetupVector(targetPoint);
	}

	private void SubmitComponentsAnswer(VectorInfo vectorInfo)
	{
		if (vectorInfo.isComponentSolved == false)
		{
			SubmitCartesianComponentsAnswerEvent?.Invoke(vectorInfo);
		}
	}
}
