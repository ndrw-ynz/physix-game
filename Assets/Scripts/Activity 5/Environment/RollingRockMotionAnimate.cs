using System.Collections;
using UnityEngine;

public class RollingRockMotionAnimate : MonoBehaviour
{
	private Vector3 startingPosition;
	private Rigidbody rb;
	public float resetDelay = 2.5f;
	public float forceMagnitude = 10f; 

	void Start()
	{
		startingPosition = transform.position;

		rb = GetComponent<Rigidbody>();
		if (rb == null)
		{
			rb = gameObject.AddComponent<Rigidbody>();
		}

		StartCoroutine(ApplyForceAndResetLoop());
	}

	IEnumerator ApplyForceAndResetLoop()
	{
		while (true)
		{
			rb.AddForce(Vector3.left * forceMagnitude, ForceMode.Impulse);

			yield return new WaitForSeconds(resetDelay);

			rb.useGravity = false;
			rb.velocity = Vector3.zero; 
			transform.position = startingPosition;

			rb.useGravity = true;
		}
	}
}