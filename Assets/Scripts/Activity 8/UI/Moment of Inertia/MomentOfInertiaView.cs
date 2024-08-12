using System.Collections.Generic;
using UnityEngine;

public class MomentOfInertiaView : MonoBehaviour
{
	[Header("Formula Display List")]
	[SerializeField] private List<MomentOfInertiaFormulaDisplay> formulaDisplays;

	private void Start()
	{
		InertiaObjectTypeButton.UpdateInertiaObjectTypeEvent += UpdateFormulaDisplay;
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
}