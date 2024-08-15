using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TorqueAnswerSubmission
{
	public float? torqueMagnitude { get; private set; }
	public TorqueDirection? torqueDirection { get; private set; }

	public TorqueAnswerSubmission(
		float? torqueMagnitude,
		TorqueDirection? torqueDirection
		)
	{
		this.torqueMagnitude = torqueMagnitude;
		this.torqueDirection = torqueDirection;
	}
}

/// <summary>
/// This class contains the components for displaying the
/// information related to the Torque subactivity for Activity Eight.
/// </summary>
public class TorqueView : MonoBehaviour
{
	public static event Action<List<TorqueAnswerSubmission>> SubmitAnswerEvent;

	[Header("Text")]
	[SerializeField] private TextMeshProUGUI calibrationTestText;


    [Header("Prefab")]
    [SerializeField] private BoltInfoDisplay boltInfoDisplayPrefab;


    [Header("Bolt Info Container")]
    [SerializeField] private HorizontalLayoutGroup boltInfoContainer;


	[Header("Result Fields")]
	[SerializeField] private List<TMP_InputField> torqueMagnitudeResultFields;


	[Header("Torque Direction Button Containers")]
	[SerializeField] private List<HorizontalLayoutGroup> torqueDirectionButtonContainers;

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
	/// Button event click action for submission of answer in <c>MomentOfInertiaView</c>.
	/// </summary>
	public void OnSubmitButtonClick()
	{
		List<TorqueAnswerSubmission> submission = new List<TorqueAnswerSubmission>();

		for (int i = 0; i < 3; i++)
		{
			float torqueMagnitude = float.Parse(torqueMagnitudeResultFields[i].text);
			TorqueDirection? torqueDirection = GetTorqueDirection(torqueDirectionButtonContainers[i]);

			TorqueAnswerSubmission answer = new TorqueAnswerSubmission(
				torqueMagnitude: torqueMagnitude,
				torqueDirection: torqueDirection
				);

			submission.Add(answer);
		}

		SubmitAnswerEvent?.Invoke(submission);
	}

	/// <summary>
	/// Fetches the selected <c>TorqueDirection</c> from all the <c>TorqueDirectionButton</c> 
	/// on a given Torque Direction Button Container.
	/// </summary>
	/// <param name="torqueDirectionButtonContainer"></param>
	private TorqueDirection? GetTorqueDirection(HorizontalLayoutGroup torqueDirectionButtonContainer)
	{
		TorqueDirectionButton[] directionButtons = torqueDirectionButtonContainer.GetComponentsInChildren<TorqueDirectionButton>();
		foreach (TorqueDirectionButton directionButton in directionButtons)
		{
			if (directionButton.isClicked) return directionButton.torqueDirection;
		}
		return null;
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