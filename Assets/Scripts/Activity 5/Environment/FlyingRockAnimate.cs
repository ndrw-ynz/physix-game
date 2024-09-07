using System.Collections;
using UnityEngine;

public class FlyingRockAnimate : MonoBehaviour
{
	private Vector3 startingPosition;
	private Rigidbody rb;
	public float forceMagnitude = 500f;
	public float resetDelay = 1f;

	private float timer;

	void Start()
	{
		startingPosition = transform.position;

		rb = GetComponent<Rigidbody>();
		if (rb == null)
		{
			rb = gameObject.AddComponent<Rigidbody>();
		}
	}

	private void Update()
	{
		if (timer <= 0f)
		{
			rb.AddForce(Vector3.right * forceMagnitude, ForceMode.Impulse);
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