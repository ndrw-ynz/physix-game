using System;
using UnityEngine;

public class PowerSourceCube : IInteractableObject
{
	public static event Action RetrieveEvent;

	public float rotationSpeed = 45.0f;

	public float floatAmplitude = 0.1f;
	public float floatFrequency = 1.0f;

	private Vector3 startPos;
	public override void Interact()
	{
		Destroy(gameObject);
		RetrieveEvent?.Invoke();
	}

	public override string GetInteractionDescription()
	{
		return "Retrieve Power Source Cube";
	}

	private void Start()
	{
		startPos = transform.position;
		SetInteractable(false);
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