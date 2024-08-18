using UnityEngine;

public class MomentOfInertiaObjectAnimate : MonoBehaviour
{
	public float rotationSpeed = 45.0f;

	public float floatAmplitude = 0.1f;
	public float floatFrequency = 1.0f;

	private Vector3 startPos;
	private bool rotateAlongFirstDiagonal = true;
	private float currentRotation = 0f;  
	private const float rotationThreshold = 180f; 

	void Start()
	{
		startPos = transform.position;
	}

	void Update()
	{
		Vector3 rotationAxis = rotateAlongFirstDiagonal ? new Vector3(1, 0, 1) : new Vector3(1, 0, -1);
		rotationAxis.Normalize();

		float rotationThisFrame = rotationSpeed * Time.deltaTime;

		transform.Rotate(rotationAxis * rotationThisFrame);

		currentRotation += rotationThisFrame;

		if (currentRotation >= rotationThreshold)
		{
			rotateAlongFirstDiagonal = !rotateAlongFirstDiagonal;

			currentRotation = 0f;
		}


		Vector3 tempPos = startPos;
		tempPos.y += Mathf.Sin(Time.time * Mathf.PI * floatFrequency) * floatAmplitude;
		transform.position = tempPos;
	}
}