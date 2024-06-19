using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorInfo
{
	public int magnitudeValue;
	public int directionValue;
	public DirectionType directionType;
	public VectorInfoDisplay vectorInfoDisplay;
	public Vector2 vectorComponent;
	public bool isComponentSolved;

	public VectorInfo(int magnitudeValue, int directionValue, DirectionType directionType, VectorInfoDisplay vectorInfoDisplay)
	{
		this.magnitudeValue = magnitudeValue;
		this.directionValue = directionValue;
		this.directionType = directionType;
		this.vectorInfoDisplay = vectorInfoDisplay;
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
