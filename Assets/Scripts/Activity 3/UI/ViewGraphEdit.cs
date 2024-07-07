using System;
using UnityEngine;

public class ViewGraphEdit : MonoBehaviour
{
    public static event Action InitiateGraphViewSwitch;

    [Header("Interactive Graph Camera")]
    public Camera interactiveGraphCamera;

    public void TriggerGraphViewChange()
    {
        interactiveGraphCamera.enabled = false;
        InitiateGraphViewSwitch?.Invoke();
    }
}