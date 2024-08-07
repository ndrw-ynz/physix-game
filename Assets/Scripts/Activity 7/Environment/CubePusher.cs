using UnityEngine;

public class CubePusher : MonoBehaviour
{
	public GameObject cube;
	public float pushDistance = 4;
	public float pushSpeed = 4f;
	public float retractSpeed = 2f;
	public float waitTime = .4f;  // Time to wait before retracting and pushing again

	private Vector3 initialPosition;
	private bool isWorking;
	private bool isPushing = true;
	private bool isRetracting = false;
	private float pushStartTime;
	private float retractStartTime;

	private void Start()
	{
		initialPosition = transform.position;
	}

	private void Update()
	{
		if (isWorking == false) return;
		if (isPushing)
		{
			Push();
		}
		else if (isRetracting)
		{
			Retract();
		}
	}

	public void SetWorkState(bool isWorking)
	{
		this.isWorking = isWorking;
	}

	private void Push()
	{
		float step = pushSpeed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, initialPosition + -transform.right * pushDistance, step);

		if (Vector3.Distance(transform.position, initialPosition + -transform.right * pushDistance) < 0.001f)
		{
			isPushing = false;
			isRetracting = true;
			retractStartTime = Time.time + waitTime;
		}
	}

	private void Retract()
	{
		if (Time.time >= retractStartTime)
		{
			float step = retractSpeed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, initialPosition, step);

			if (Vector3.Distance(transform.position, initialPosition) < 0.001f)
			{
				isRetracting = false;
				isPushing = true;
				pushStartTime = Time.time + waitTime;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject != cube)
		{
			isPushing = false;
			isRetracting = true;
			retractStartTime = Time.time + waitTime;
		}
	}
}