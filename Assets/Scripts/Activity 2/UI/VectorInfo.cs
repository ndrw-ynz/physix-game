using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorInfo
{
	public int magnitudeValue;
	public int directionValue;
	public DirectionType directionType;
	public VectorDisplay vectorDisplay;
	public Vector2 vectorComponents;

	public VectorInfo(int magnitudeValue, int directionValue, DirectionType directionType, VectorDisplay vectorDisplay)
	{
		this.magnitudeValue = magnitudeValue;
		this.directionValue = directionValue;
		this.directionType = directionType;
		this.vectorDisplay = vectorDisplay;

		ComputeXYComponents(magnitudeValue, directionValue);
	}

	private void ComputeXYComponents(int magnitudeValue, int directionValue)
	{
		float directionRadians = directionValue * Mathf.Deg2Rad;

		float x = magnitudeValue * Mathf.Cos(directionRadians);
		float y = magnitudeValue * Mathf.Sin(directionRadians);

		vectorComponents = new Vector2(x, y);
	}
}
