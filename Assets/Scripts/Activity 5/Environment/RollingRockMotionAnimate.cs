using System.Collections;
using UnityEngine;

public class RollingRockMotionAnimate : MonoBehaviour
{
	private Vector3 startingPosition;
	private Rigidbody rb;
	public float resetDelay = 2.5f;
	public float forceMagnitude = 10f;
	private float timer = 0f;

	void Start()
	{
		startingPosition = transform.position;

		rb = GetComponent<Rigidbody>();
		if (rb == null)
		{
			rb = gameObject.AddComponent<Rigidbody>();
		}
	}

	void Update()
	{
		// Apply force once at the start or after reset
		if (timer <= 0f)
		{
			rb.AddForce(Vector3.left * forceMagnitude, ForceMode.Impulse);
			timer = resetDelay; // Reset the timer
		}

		// Count down the timer
		timer -= Time.deltaTime;

		// Reset the position and physics properties after delay
		if (timer <= 0f)
		{
			rb.useGravity = false;
			rb.velocity = Vector3.zero;
			transform.position = startingPosition;
			rb.useGravity = true;
		}
	}
}