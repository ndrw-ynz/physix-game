using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/Graphs")]
public class GraphsSubActivitySO : ScriptableObject
{
	[Header("Graph Datasets")]
	public List<GraphDataset> datasets;

	[Serializable]
	public class GraphDataset
	{
		public DatasetType datasetType;
		public List<string> dataset;
	}

	public enum DatasetType
	{
		Position,
		Velocity,
		Acceleration
	}
}