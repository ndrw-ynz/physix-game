using UnityEngine;

public class SpaceShuttleTiltAnimate : MonoBehaviour
{
	// Space movement parameters
	public float moveAmplitude = 2f;       
	public float moveSpeed = 1f;           

	// Rotation parameters
	public float rotateAmplitude = 15f;    
	public float rotateSpeed = 2f;         

	private Vector3 initialPosition;
	private Quaternion initialRotation;

	void Start()
	{
		initialPosition = transform.position;
		initialRotation = transform.rotation;
	}

	void Update()
	{
		// Calculate new y position
		float newY = initialPosition.y + moveAmplitude * Mathf.Sin(Time.time * moveSpeed);
		transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);

		// Calculate new x-axis rotation
		float newRotationX = rotateAmplitude * Mathf.Sin(Time.time * rotateSpeed);
		transform.rotation = Quaternion.Euler(initialRotation.eulerAngles.x, initialRotation.eulerAngles.y, newRotationX);
	}
}
