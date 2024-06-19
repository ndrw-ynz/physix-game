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
		double directionRadians = directionValue * (System.Math.PI/180);

		double x = magnitudeValue * System.Math.Cos(directionRadians);
		double y = magnitudeValue * System.Math.Sin(directionRadians);

		double threshold = 1e-10;

		x = (System.Math.Abs(x) < threshold) ? 0 : x;
		y = (System.Math.Abs(y) < threshold) ? 0 : y;

		float xFloat = (float) x;
		float yFloat = (float) y;

		Debug.Log($"x: {xFloat} y: {yFloat}");
		vectorComponent = new Vector2(xFloat, yFloat);
	}
}
