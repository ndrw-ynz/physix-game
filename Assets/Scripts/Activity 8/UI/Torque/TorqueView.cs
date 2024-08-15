using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class contains the components for displaying the
/// information related to the Torque subactivity for Activity Eight.
/// </summary>
public class TorqueView : MonoBehaviour
{
	[Header("Text")]
	[SerializeField] private TextMeshProUGUI calibrationTestText;


    [Header("Prefab")]
    [SerializeField] private BoltInfoDisplay boltInfoDisplayPrefab;


    [Header("Bolt Info Container")]
    [SerializeField] private HorizontalLayoutGroup boltInfoContainer;

	/// <summary>
	/// Setup state of <c>TorqueView</c> in displaying bolt information
	/// related to the calculation of each individual bolt's Torque magnitude
	/// and direction.
	/// </summary>
	/// <param name="data"></param>
	public void SetupTorqueView(List<TorqueData> data)
    {
		// Clear contents of boltInfoContainer
		ClearBoltInfoContainer();

		for (int i = 0; i < 3; i++)
		{
			TorqueData currentData = data[i];
			BoltInfoDisplay boltInfoDisplay = Instantiate(boltInfoDisplayPrefab, boltInfoContainer.transform, false);
			boltInfoDisplay.SetupBoltInfoDisplay(currentData, i + 1);
		}
    }

	/// <summary>
	/// Updates the content of Calibration Test Text display, used in updating current test number and total tests.
	/// </summary>
	/// <param name="testNumber"></param>
	/// <param name="totalTests"></param>
	public void UpdateCalibrationTestTextDisplay(int testNumber, int totalTests)
	{
		calibrationTestText.text = $"Calibration Test: {testNumber} / {totalTests}";
	}

	/// <summary>
	/// Destroys every single child component in given variables container.
	/// </summary>
	private void ClearBoltInfoContainer()
	{
		foreach (Transform child in boltInfoContainer.transform)
		{
			Destroy(child.gameObject);
		}
	}
}