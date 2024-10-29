using UnityEngine;

public class NakalaisPlanetAnimate : MonoBehaviour
{
	public float rotationSpeed = 10f;

	private bool isMoving = false;
	private float targetXPosition;
	private float moveSpeed = 15f; 

	void Start()
	{
		targetXPosition = transform.position.x;
	}

	void Update()
	{
		// Rotate the planet continuously around the z-axis
		transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

		// Check if the position decrease has been triggered
		if (isMoving)
		{
			// Smoothly move the x position toward the target
			float newX = Mathf.MoveTowards(transform.position.x, targetXPosition, moveSpeed * Time.deltaTime);
			transform.position = new Vector3(newX, transform.position.y, transform.position.z);

			// Stop moving once the target position is reached
			if (Mathf.Approximately(transform.position.x, targetXPosition))
			{
				isMoving = false;
			}
		}
	}

	public void IncreaseXPosition(float value)
	{
		targetXPosition = transform.position.x + value;
		isMoving = true;
	}
}
