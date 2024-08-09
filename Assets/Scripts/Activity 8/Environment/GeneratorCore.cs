using UnityEngine;

public class GeneratorCore : MonoBehaviour
{
	public Transform orbitCenter;

	public float orbitRadius = 1.0f;
	public float orbitSpeed = 30.0f;
	public float rotationSpeed = 90.0f;
	public float floatAmplitude = 0.1f;
	public float floatFrequency = 1.0f;

	private Vector3 startPos;
	private float angle = 0f;
    void Start()
    {
		startPos = transform.position;
	}

	void Update()
    {
		transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
		transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);

		angle += orbitSpeed * Time.deltaTime;
		if (angle > 360f) angle -= 360f;

		Vector3 tempPos = startPos;
		tempPos.x = orbitCenter.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * orbitRadius;
		tempPos.z = orbitCenter.position.z + Mathf.Sin(angle * Mathf.Deg2Rad) * orbitRadius;
		tempPos.y = orbitCenter.position.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
		transform.position = tempPos;
	}
}