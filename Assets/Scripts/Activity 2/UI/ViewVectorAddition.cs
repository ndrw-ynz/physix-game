using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewVectorAddition : MonoBehaviour
{
    [Header("Prefabs")]
    public VectorComponentDisplay vectorComponentDisplayPrefab;
    public TMP_InputField valueInputFieldPrefab;
    public TextMeshProUGUI plusSignTextPrefab;
    [Header("Containers")]
    public VerticalLayoutGroup vectorComponentDisplayContainer;
    public HorizontalLayoutGroup xInputFieldContainer;
    public HorizontalLayoutGroup yInputFieldContainer;
     public void SetupViewVectorAddition(List<VectorInfo> vectorInfoList)
    {
        // Setting up contents of vectorComponentDisplayContainer
        foreach (VectorInfo vectorInfo in vectorInfoList)
        {
            VectorComponentDisplay vectorComponentDisplay = Instantiate(vectorComponentDisplayPrefab);
            vectorComponentDisplay.SetupVectorComponentDisplay(vectorInfo);
            vectorComponentDisplay.transform.SetParent(vectorComponentDisplayContainer.transform, false);
        }

        // Setting up n-inputFieldContainer
        SetupInputFieldContainer(xInputFieldContainer, vectorInfoList.Count);
		SetupInputFieldContainer(yInputFieldContainer, vectorInfoList.Count);
	}

	private void SetupInputFieldContainer(HorizontalLayoutGroup inputFieldContainer, int numberOfVectors)
    {
		for (int i = 0; i < numberOfVectors; i++)
		{
			TMP_InputField valueInputField = Instantiate(valueInputFieldPrefab);
			valueInputField.transform.SetParent(inputFieldContainer.transform, false);

			if (i + 1 < numberOfVectors)
			{
				TextMeshProUGUI plusSignText = Instantiate(plusSignTextPrefab);
				plusSignText.transform.SetParent(inputFieldContainer.transform, false);
			}
		}
	}
}
