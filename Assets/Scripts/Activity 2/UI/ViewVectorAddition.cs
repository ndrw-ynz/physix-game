using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewVectorAddition : MonoBehaviour
{
    [Header("Prefabs")]
    public VectorComponentDisplay vectorComponentDisplayPrefab;
    [Header("Containers")]
    public VerticalLayoutGroup vectorComponentDisplayContainer;
     public void SetupViewVectorAddition(List<VectorInfo> vectorInfoList)
    {
        // Setting up contents of vectorComponentDisplayContainer
        foreach (VectorInfo vectorInfo in vectorInfoList)
        {
            VectorComponentDisplay vectorComponentDisplay = Instantiate(vectorComponentDisplayPrefab);
            vectorComponentDisplay.SetupVectorComponentDisplay(vectorInfo);
            vectorComponentDisplay.transform.SetParent(vectorComponentDisplayContainer.transform, false);
        }
    }
}
