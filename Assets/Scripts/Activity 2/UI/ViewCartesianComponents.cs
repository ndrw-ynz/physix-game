using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ViewCartesianComponents : MonoBehaviour
{
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

	private void ChangeLineRenderer(int magnitudeValue, int directionValue)
	{
		float directionRadians = directionValue * Mathf.Deg2Rad;

		float x = magnitudeValue * Mathf.Cos(directionRadians);
		float z = magnitudeValue * Mathf.Sin(directionRadians);
		Vector3 targetPoint = new Vector3(x, 0, z);
		vectorLineRenderer.SetupVector(targetPoint);
	}

	public void AddVectorInfo(int magnitudeValue, int directionValue, DirectionType directionType)
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
}
