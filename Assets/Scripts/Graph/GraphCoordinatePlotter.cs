using System.Collections.Generic;
using UnityEngine;

public class GraphCoordinatePlotter : MonoBehaviour
{
    [Header("Coordinate Point Prefab")]
    [SerializeField] private GameObject coordinatePointPrefab;

    private List<GameObject> coordinatePointsList = new List<GameObject>();

	public void PlacePoint(Vector2 coordinate)
    {
        GameObject point = Instantiate(coordinatePointPrefab);
        point.transform.position = new Vector3(coordinate.x, 0, coordinate.y);
        point.transform.SetParent(transform, false);

        coordinatePointsList.Add(point);
    }

    public void RemoveAllPoints()
    {
        for (int i = 0; i < coordinatePointsList.Count; i++)
        {
            Destroy(coordinatePointsList[i].gameObject);
        }
    }
}   