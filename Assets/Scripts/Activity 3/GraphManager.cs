using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public Camera interactiveGraphCamera;
    [SerializeField] private LayerMask placementLayerMask;

    private Vector3 lastMousePosition;

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = interactiveGraphCamera.nearClipPlane;
        RaycastHit hit;
        Ray ray = interactiveGraphCamera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out hit, 100, placementLayerMask))
        {
            lastMousePosition = hit.point;
        }
        return lastMousePosition;
    }
}