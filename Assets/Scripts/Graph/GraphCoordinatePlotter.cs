using UnityEngine;

public class GraphCoordinatePlotter : MonoBehaviour
{
    [Header("Coordinate Point Prefab")]
    [SerializeField] private GameObject coordinatePointPrefab;

    public void PlacePoint(Vector2 coordinate)
    {
        GameObject point = Instantiate(coordinatePointPrefab);
        point.transform.position = new Vector3(coordinate.x, 0, coordinate.y);
        point.transform.SetParent(transform, false);
    }
}   