using System;
using UnityEngine;

public class BoatMotionAnimate : MonoBehaviour
{
	public float verticalAmplitude = 0.5f; 
	public float verticalFrequency = 1f;   
	public float horizontalVelocity = 1f;
	public float distanceLimit = 2f;
	public float resetDelay = 5.5f;

	private Vector3 startPosition;

	void Start()
	{
		startPosition = transform.position;
	}

	void Update()
	{
		Vector3 tempPos = transform.position;

		tempPos.x += horizontalVelocity * Time.deltaTime;

		if (Math.Abs(tempPos.x - startPosition.x) >= distanceLimit)
		{
			tempPos.x = startPosition.x;
		}

		tempPos.y = startPosition.y + Mathf.Sin(Time.time * Mathf.PI * verticalFrequency) * verticalAmplitude;

		transform.position = tempPos;
	}
}