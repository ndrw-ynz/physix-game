using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MomentOfInertiaAnswerSubmission
{
	public InertiaObjectType? inertiaObjectType { get; private set; }
	public float? momentOfInertia { get; private set; }

	public MomentOfInertiaAnswerSubmission(
		InertiaObjectType? inertiaObjectType,
		float? momentOfInertia
		)
	{
		this.inertiaObjectType = inertiaObjectType;
		this.momentOfInertia = momentOfInertia;
	}
}

public class MomentOfInertiaView : MonoBehaviour
{
	public static event Action QuitViewEvent;
	public static event Action<MomentOfInertiaAnswerSubmission> SubmitAnswerEvent;
	public static event Action<InertiaObjectType> UpdateObjectDisplayEvent;

	[Header("Text")]
	[SerializeField] private TextMeshProUGUI calibrationTestText;
	[SerializeField] private TextMeshProUGUI objectTypeText;


	[Header("Prefabs")]
	[SerializeField] private GivenVariableDisplay givenVariableDisplayPrefab;


	[Header("Given Variables Container")]
	[SerializeField] private VerticalLayoutGroup givenVariablesContainer;


	[Header("Formula Display List")]
	[SerializeField] private List<MomentOfInertiaFormulaDisplay> formulaDisplays;

	private InertiaObjectType? selectedInertiaObjectType;

	private void Start()
	{
		InertiaObjectTypeButton.UpdateInertiaObjectTypeEvent += UpdateFormulaDisplay;
	}

	/// <summary>
	/// Setup state of <c>MomentOfInertiaView</c> in displaying information related to Moment of Inertia.
	/// </summary>
	/// <param name="data"></param>
	public void SetupMomentOfInertiaView(MomentOfInertiaData data)
	{
		// Update object type text
		objectTypeText.text = $"{data.inertiaObjectType}"; // CHANGE IN THE FUTURE TO MORE PROPER DISPLAY TEXT.

		// Clears contents of given variables container.
		ClearGivenVariablesContainer();

		// Add mass display
		GivenVariableDisplay massDisplay = Instantiate(givenVariableDisplayPrefab, givenVariablesContainer.transform, false);
		massDisplay.SetupGivenVariableDisplay("Mass: ", $"{data.mass} kg");

		// Add variables associated with each inertia object type.
		switch (data.inertiaObjectType)
		{
			case InertiaObjectType.SlenderRodCenter:
			case InertiaObjectType.SlenderRodEnd:
				// Add length display
				GivenVariableDisplay lengthDisplay = Instantiate(givenVariableDisplayPrefab, givenVariablesContainer.transform, false);
				lengthDisplay.SetupGivenVariableDisplay("Length: ", $"{data.length} m");
				break;
			case InertiaObjectType.RectangularPlateCenter:
			case InertiaObjectType.RectangularPlateEdge:
				// Add plate length A display
				GivenVariableDisplay plateLengthADisplay = Instantiate(givenVariableDisplayPrefab, givenVariablesContainer.transform, false);
				plateLengthADisplay.SetupGivenVariableDisplay("Length A: ", $"{data.plateLengthA} m");

				// Add plate length B display, only for Rectangular Plate Center
				if (data.inertiaObjectType == InertiaObjectType.RectangularPlateCenter)
				{
					GivenVariableDisplay plateLengthBDisplay = Instantiate(givenVariableDisplayPrefab, givenVariablesContainer.transform, false);
					plateLengthBDisplay.SetupGivenVariableDisplay("Length B: ", $"{data.plateLengthB} m");
				}
				break;
			case InertiaObjectType.HollowCylinder:
				// Add inner Radius display
				GivenVariableDisplay innerRadiusDisplay = Instantiate(givenVariableDisplayPrefab, givenVariablesContainer.transform, false);
				innerRadiusDisplay.SetupGivenVariableDisplay("Inner Radius: ", $"{data.innerRadius} m");

				// Add outer Radius display
				GivenVariableDisplay outerRadiusDisplay = Instantiate(givenVariableDisplayPrefab, givenVariablesContainer.transform, false);
				outerRadiusDisplay.SetupGivenVariableDisplay("Outer Radius: ", $"{data.outerRadius} m");
				break;
			case InertiaObjectType.SolidCylinder:
			case InertiaObjectType.ThinWalledHollowCylinder:
			case InertiaObjectType.SolidSphere:
			case InertiaObjectType.ThinWalledHollowSphere:
			case InertiaObjectType.SolidDisk:
				// Add radius display
				GivenVariableDisplay radiusDisplay = Instantiate(givenVariableDisplayPrefab, givenVariablesContainer.transform, false);
				radiusDisplay.SetupGivenVariableDisplay("Radius: ", $"{data.radius} m");
				break;
		}

		// Signal that object display in Moment of Inertia View must be updated.
		UpdateObjectDisplayEvent?.Invoke(data.inertiaObjectType);
	}

	/// <summary>
	/// Updates the currently displayed Moment of Inertia Formula in the view based on selected <c>InertiaObjectType</c>.
	/// </summary>
	/// <param name="inertiaObjectType"></param>
	public void UpdateFormulaDisplay(InertiaObjectType inertiaObjectType)
	{
		selectedInertiaObjectType = inertiaObjectType;
		foreach (MomentOfInertiaFormulaDisplay formulaDisplay in formulaDisplays)
		{
			if (formulaDisplay.inertiaObjectType == selectedInertiaObjectType)
			{
				formulaDisplay.gameObject.SetActive(true);
				formulaDisplay.ResetState();
			} else
			{
				formulaDisplay.gameObject.SetActive(false);
			}
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
		MomentOfInertiaFormulaDisplay formulaDisplay = GetFormulaDisplay(selectedInertiaObjectType);
		var resultValue = formulaDisplay == null ? null : formulaDisplay.resultValue;

		MomentOfInertiaAnswerSubmission submission = new MomentOfInertiaAnswerSubmission(
			inertiaObjectType: selectedInertiaObjectType,
			momentOfInertia: resultValue
			);

		SubmitAnswerEvent?.Invoke(submission);
	}

	/// <summary>
	/// Quit button click event action for quitting <c>MomentOfInertiaView</c>.
	/// </summary>
	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitViewEvent?.Invoke();
	}

	/// <summary>
	/// Fetches the <c>MomentOfInertiaFormulaDisplay</c> associated with a given <c>InertiaObjectType</c> in <c>MomentOfInertiaView</c>.
	/// </summary>
	/// <param name="inertiaObjectType"></param>
	/// <returns></returns>
	private MomentOfInertiaFormulaDisplay GetFormulaDisplay(InertiaObjectType? inertiaObjectType)
	{
		foreach (MomentOfInertiaFormulaDisplay formulaDisplay in formulaDisplays)
		{
			if (formulaDisplay.inertiaObjectType == inertiaObjectType)
			{
				return formulaDisplay;
			}
		}
		return null;
	}

	/// <summary>
	/// Destroys every single child component in given variables container.
	/// </summary>
	private void ClearGivenVariablesContainer()
	{
		foreach(Transform child in givenVariablesContainer.transform)
		{
			Destroy(child.gameObject);
		}
	}
}