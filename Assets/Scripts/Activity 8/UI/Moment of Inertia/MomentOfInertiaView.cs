using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MomentOfInertiaView : MonoBehaviour
{
	[Header("Text")]
	[SerializeField] private TextMeshProUGUI calibrationTestText;
	[SerializeField] private TextMeshProUGUI objectTypeText;


	[Header("Prefabs")]
	[SerializeField] private GivenVariableDisplay givenVariableDisplayPrefab;


	[Header("Given Variables Container")]
	[SerializeField] private VerticalLayoutGroup givenVariablesContainer;


	[Header("Formula Display List")]
	[SerializeField] private List<MomentOfInertiaFormulaDisplay> formulaDisplays;

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

				// Add plate length B display, only for Rectangular Plate Edge
				if (data.inertiaObjectType == InertiaObjectType.RectangularPlateEdge)
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
	}

	/// <summary>
	/// Updates the currently displayed Moment of Inertia Formula in the view based on selected <c>InertiaObjectType</c>.
	/// </summary>
	/// <param name="inertiaObjectType"></param>
	public void UpdateFormulaDisplay(InertiaObjectType inertiaObjectType)
	{
		foreach (MomentOfInertiaFormulaDisplay formulaDisplay in formulaDisplays)
		{
			if (formulaDisplay.inertiaObjectType == inertiaObjectType)
			{
				formulaDisplay.gameObject.SetActive(true);
				formulaDisplay.ResetState();
			} else
			{
				formulaDisplay.gameObject.SetActive(false);
			}
		}
	}

	public void UpdateCalibrationTestTextDisplay(int testNumber, int totalTests)
	{
		calibrationTestText.text = $"Calibration Test: {testNumber} / {totalTests}";
	}
}