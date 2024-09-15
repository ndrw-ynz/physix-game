using UnityEngine;

public class TerrainRotateAnimate : MonoBehaviour
{
	public Transform rotationCenter;
	public float rotationSpeed = 10f;

	void Update()
	{
		transform.RotateAround(rotationCenter.position, Vector3.up, rotationSpeed * Time.deltaTime);
	}
}