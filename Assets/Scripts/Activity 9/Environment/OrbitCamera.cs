using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
	public Transform orbitCenter;

	public float orbitRadius = 1.0f;
	public float orbitSpeed = 30.0f;

	private Vector3 startPos;
	private float angle = 0f;

	// Start is called before the first frame update
	void Start()
    {
		startPos = transform.position;
	}

	// Update is called once per frame
	void Update()
    {
		angle += orbitSpeed * Time.deltaTime;
		if (angle > 360f) angle -= 360f;

		Vector3 tempPos = startPos;
		tempPos.x = orbitCenter.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * orbitRadius;
		tempPos.z = orbitCenter.position.z + Mathf.Sin(angle * Mathf.Deg2Rad) * orbitRadius;

		transform.position = tempPos;

		transform.LookAt(orbitCenter);
	}
}