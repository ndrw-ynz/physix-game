using UnityEngine;

public class InfiniteTerrainAnimate : MonoBehaviour
{
	public float numberOfTerrains = 3;
	public float terrainLength = 100f;
	public float scrollSpeed = 10f; 
	public Vector3 endPoint;

	void Update()
	{
		transform.Translate(Vector3.back * scrollSpeed * Time.deltaTime);

		if (transform.localPosition.z <= endPoint.z)
		{
			transform.localPosition = transform.localPosition + new Vector3(0, 0, terrainLength * numberOfTerrains);
		}
	}
}