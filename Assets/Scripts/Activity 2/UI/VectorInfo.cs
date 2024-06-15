using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorInfo
{
	public int magnitudeValue;
	public int directionValue;
	public DirectionType directionType;
	public VectorDisplay vectorDisplay;
	public Vector2 vectorComponent;
	public bool isComponentSolved;

	public VectorInfo(int magnitudeValue, int directionValue, DirectionType directionType, VectorDisplay vectorDisplay)
	{
		this.magnitudeValue = magnitudeValue;
		this.directionValue = directionValue;
		this.directionType = directionType;
		this.vectorDisplay = vectorDisplay;
		isComponentSolved = false;

		ComputeXYComponents(magnitudeValue, directionValue);
	}

	private void ComputeXYComponents(int magnitudeValue, int directionValue)
	{
		float directionRadians = directionValue * Mathf.Deg2Rad;

		float x = magnitudeValue * Mathf.Cos(directionRadians);
		float y = magnitudeValue * Mathf.Sin(directionRadians);

		vectorComponent = new Vector2(x, y);
	}
}
