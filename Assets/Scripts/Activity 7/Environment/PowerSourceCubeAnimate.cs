using UnityEngine;

public class PowerSourceCubeAnimate : MonoBehaviour
{
	public float rotationSpeed = 45.0f;

	public float floatAmplitude = 0.1f;
	public float floatFrequency = 1.0f;

	private Vector3 startPos;

	void Start()
	{
		startPos = transform.position;
	}

	void Update()
	{
		transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
		transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);


		Vector3 tempPos = startPos;
		tempPos.y += Mathf.Sin(Time.time * Mathf.PI * floatFrequency) * floatAmplitude;
		transform.position = tempPos;
	}
}