using System;
using UnityEngine;

public class VectorInfo
{
	public int magnitudeValue;
	public int directionValue;
	public VectorDirectionType directionType;
	public VectorInfoDisplay vectorInfoDisplay;
	public Vector2 vectorComponent;
	public bool isComponentSolved;

	public VectorInfo(int magnitudeValue, int directionValue, VectorDirectionType directionType, VectorInfoDisplay vectorInfoDisplay)
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
		double directionRadians = directionValue * (Math.PI/180);

		double x = magnitudeValue * Math.Cos(directionRadians);
		double y = magnitudeValue * Math.Sin(directionRadians);

		double threshold = 1e-10;

		x = (Math.Abs(x) < threshold) ? 0 : x;
		y = (Math.Abs(y) < threshold) ? 0 : y;

		float xFloat = (float) Math.Round(x, 4);
		float yFloat = (float) Math.Round(y, 4);

		Debug.Log($"x: {xFloat} y: {yFloat}");
		vectorComponent = new Vector2(xFloat, yFloat);
	}
}
