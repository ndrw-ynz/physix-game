using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewVariance : MonoBehaviour
{
	public static event Action<ViewVariance> OpenVarianceEvent;
	public List<Draggable> givenNumbers;
	public List<OperandButton> operandButtons;
	public List<OperatorButton> operatorButtons;
	private void OnEnable()
	{
		// Initializes draggable given numbers.
		foreach (Draggable gn in givenNumbers)
		{
			gn.Initialize();
		}
		// Initialize operand buttons.
		foreach (OperandButton ob in operandButtons)
		{
			ob.Initialize();
		}
		// Initialize operator buttons.
		foreach (OperatorButton ob in operatorButtons)
		{
			ob.Initialize();
		}

		OpenVarianceEvent?.Invoke(this);
	}
}
