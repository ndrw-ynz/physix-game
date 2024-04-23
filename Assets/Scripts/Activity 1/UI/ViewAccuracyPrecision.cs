using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewAccuracyPrecision : MonoBehaviour
{
	public static event Action<ViewAccuracyPrecision> OpenViewEvent;

	private void OnEnable()
	{
		OpenViewEvent?.Invoke(this);
	}
}
