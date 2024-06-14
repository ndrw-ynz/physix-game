using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorInfo
{
	public int magnitudeValue;
	public int directionValue;
	public DirectionType directionType;
	public VectorDisplay vectorDisplay;
	// compute for x and y component?


	public VectorInfo(int magnitudeValue, int directionValue, DirectionType directionType, VectorDisplay vectorDisplay)
	{
		this.magnitudeValue = magnitudeValue;
		this.directionValue = directionValue;
		this.directionType = directionType;
		this.vectorDisplay = vectorDisplay;
		// here compute for x and y component
	}
}
