using System.Collections.Generic;
using UnityEngine;

public class APGraphManager : MonoBehaviour
{
    [Header("Accuracy Precision Graphs")]
    [SerializeField] private List<APGraph> APGraphs;

	private void Start()
	{
		foreach (APGraph graph in APGraphs)
		{
			graph.InitializeGraphContent();
		}
	}

	public APGraphType GetGraphTypeFromGraphs(int graphNumber)
	{
		return APGraphs[graphNumber - 1].graphType;
	}
}
