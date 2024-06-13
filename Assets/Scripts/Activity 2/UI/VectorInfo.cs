using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorInfo
{
	public int magnitudeValue;
	public int directionValue;
	public VectorDisplay vectorDisplay;
	// compute for x and y component?

	public VectorInfo(int magnitudeValue, int directionValue, VectorDisplay vectorDisplay)
	{
		this.magnitudeValue = magnitudeValue;
		this.directionValue = directionValue;
		this.vectorDisplay = vectorDisplay;
		// here compute for x and y component
	}
}
