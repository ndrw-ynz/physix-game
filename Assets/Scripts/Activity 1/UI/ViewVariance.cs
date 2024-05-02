using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewVariance : MonoBehaviour
{
	public static event Action<ViewVariance> OpenVarianceEvent;
	public List<Draggable> givenNumbers;
	public List<OperandButton> operandButtons;

	private void OnEnable()
	{
		// Initializes draggable given numbers.
		foreach (Draggable gn in givenNumbers)
		{
			gn.Initialize();
		}

		foreach(OperandButton ob in operandButtons)
		{
			ob.Initialize();
		}

		OpenVarianceEvent?.Invoke(this);
	}
}
