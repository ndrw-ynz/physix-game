using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewVariance : MonoBehaviour
{
	public static event Action<ViewVariance> OpenVarianceEvent;
	public List<Draggable> givenNumbers;

	private void OnEnable()
	{
		OpenVarianceEvent?.Invoke(this);
	}
}
