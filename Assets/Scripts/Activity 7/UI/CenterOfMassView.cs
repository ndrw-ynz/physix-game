using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CenterOfMassView : MonoBehaviour
{
	[Header("Layout Containers")]
	[SerializeField] private VerticalLayoutGroup massCoordinateComponentContainer;
	[Header("Prefabs")]
	[SerializeField] private MassCoordinateComponent massCoordinateComponentPrefab;
	[Header("Calculation Displays")]
	[SerializeField] private GameObject MassTimesXCoords;
	[SerializeField] private GameObject MassTimesYCoords;
	[SerializeField] private GameObject CenterOfMassXDir;
	[SerializeField] private GameObject CenterOfMassYDir;
	[Header("Interactive Buttons")]
    [SerializeField] private Button leftPageButton;
	[SerializeField] private Button rightPageButton;

	#region View Setup
	public void SetupCenterOfMassView(List<MassCoordinatePair> massCoordinatePairs)
	{
		// Setting up mass coordinate components for UI display of given values
		for (int i = 0; i < massCoordinatePairs.Count; i++)
		{
			MassCoordinateComponent massCoordinate = Instantiate(massCoordinateComponentPrefab);
			massCoordinate.SetupInputFields(massCoordinatePairs[i], i + 1);
			massCoordinate.transform.SetParent(massCoordinateComponentContainer.transform, false);
		}
	}
	#endregion

	#region Buttons
	public void OnLeftPageButtonClick()
	{
		leftPageButton.gameObject.SetActive(false);
		rightPageButton.gameObject.SetActive(true);

		MassTimesXCoords.gameObject.SetActive(true);
		CenterOfMassXDir.gameObject.SetActive(true);

		MassTimesYCoords.gameObject.SetActive(false);
		CenterOfMassYDir.gameObject.SetActive(false);
	}

	public void OnRightPageButtonClick()
	{
		leftPageButton.gameObject.SetActive(true);
		rightPageButton.gameObject.SetActive(false);

		MassTimesXCoords.gameObject.SetActive(false);
		CenterOfMassXDir.gameObject.SetActive(false);

		MassTimesYCoords.gameObject.SetActive(true);
		CenterOfMassYDir.gameObject.SetActive(true);
	}
	#endregion
}