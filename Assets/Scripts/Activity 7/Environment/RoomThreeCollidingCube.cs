using System;
using UnityEngine;

public class RoomThreeCollidingCube : MonoBehaviour
{
	public event Action ResetPosition;

	public float velocityBeforeCollision = 1f;
	public float velocityAfterCollision = 1f;

	private Vector3 originalPosition;
	private bool hasCollided = true;
	
	private void Start()
	{
		originalPosition = transform.position;
		ResetPosition += ResetCubePosition;
	}

	private void Update()
	{
		// Move the box based on velocity
		if (hasCollided)
		{
			transform.Translate(Vector3.right * velocityAfterCollision * Time.deltaTime);
		}
		else
		{
			transform.Translate(Vector3.right * velocityBeforeCollision * Time.deltaTime);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Cube"))
		{
			hasCollided = true;
		} else if (collision.gameObject.CompareTag("Wall"))
		{
			ResetPosition?.Invoke();
		}
	}

	private void ResetCubePosition()
	{
		hasCollided = false;
		transform.position = originalPosition;
	}
}