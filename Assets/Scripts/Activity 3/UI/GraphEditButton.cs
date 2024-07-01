using System;
using UnityEngine;

public class GraphEditButton : MonoBehaviour
{
    public static event Action<Graph> InitiateGraphEditViewSwitch;

    [Header("Graph")]
    [SerializeField] private Graph graph;

    public void TriggerGraphEditViewChange()
    {
        graph.interactiveGraphCamera.enabled = true;
        InitiateGraphEditViewSwitch?.Invoke(graph);
    }
}