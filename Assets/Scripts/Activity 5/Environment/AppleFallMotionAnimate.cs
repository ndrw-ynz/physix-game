using UnityEngine;

public class AppleFallMotionAnimate : MonoBehaviour
{
	public float moveDistance = 3f;
	public float speed = 2f;        

	private Vector3 startPosition; 
	private Vector3 endPosition;   

	private void Start()
	{
		startPosition = transform.position;

		// Calculate the target position (downward by moveDistance)
		endPosition = startPosition - new Vector3(0, moveDistance, 0);
	}

	private void Update()
	{
		// Move towards the end position
		transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
		
		transform.Rotate(Vector3.forward, speed * 60 * Time.deltaTime);
		transform.Rotate(Vector3.right, speed * 60 * Time.deltaTime);

		// Check if the object has reached the end position
		if (Vector3.Distance(transform.position, endPosition) <= 0.01f)
		{
			// Reset the position back to start
			transform.position = startPosition;
		}
	}
}