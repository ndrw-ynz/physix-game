using System;
using UnityEngine;

public class GraphEditButton : MonoBehaviour
{
    public static event Action<Camera> InitiateGraphEditViewSwitch;

    [Header("Interactive Camera")]
    [SerializeField] private Camera interactiveGraphCamera;

    public void TriggerGraphEditViewChange()
    {
        interactiveGraphCamera.enabled = true;
        InitiateGraphEditViewSwitch?.Invoke(interactiveGraphCamera);
    }
}