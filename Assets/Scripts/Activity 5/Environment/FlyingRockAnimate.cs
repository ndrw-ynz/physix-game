using System.Collections;
using UnityEngine;

public class FlyingRockAnimate : MonoBehaviour
{
	private Vector3 startingPosition;
	private Rigidbody rb;
	public float forceMagnitude = 500f;
	public float duration = 1f;

	void Start()
	{
		startingPosition = transform.position;

		rb = GetComponent<Rigidbody>();
		if (rb == null)
		{
			rb = gameObject.AddComponent<Rigidbody>();
		}

		StartCoroutine(ApplyUpwardForceAndResetLoop());
	}

	IEnumerator ApplyUpwardForceAndResetLoop()
	{
		while (true)
		{
			rb.AddForce(Vector3.right * forceMagnitude, ForceMode.Impulse);

			yield return new WaitForSeconds(duration);

			rb.velocity = Vector3.zero;
			transform.position = startingPosition;
		}
	}
}