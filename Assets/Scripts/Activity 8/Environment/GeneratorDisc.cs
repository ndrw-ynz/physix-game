using UnityEngine;

public class GeneratorDisc : MonoBehaviour
{
	public float floatAmplitude = 0.1f;
	public float floatFrequency = 1.0f;
	private Vector3 startPos;

	void Start()
    {
        startPos = transform.position;
    }

	void Update()
	{
		Vector3 tempPos = startPos;
		tempPos.y += Mathf.Sin(Time.time * Mathf.PI * floatFrequency) * floatAmplitude;
		transform.position = tempPos;
	}
}