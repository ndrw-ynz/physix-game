using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewVariance : MonoBehaviour
{
	public static event Action<ViewVariance> OpenVarianceEvent;
	private void OnEnable()
	{
		OpenVarianceEvent?.Invoke(this);
	}
}
