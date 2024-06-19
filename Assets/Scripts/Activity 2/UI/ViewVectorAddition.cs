using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewVectorAddition : MonoBehaviour
{
    [Header("Result Fields")]
    public TMP_InputField xComponentResultField;
	public TMP_InputField yComponentResultField;
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
        SetupInputFieldContainer(xInputFieldContainer, xComponentResultField, vectorInfoList.Count);
		SetupInputFieldContainer(yInputFieldContainer, yComponentResultField, vectorInfoList.Count);
	}

	private void SetupInputFieldContainer(HorizontalLayoutGroup inputFieldContainer, TMP_InputField componentResultField, int numberOfVectors)
    {
		for (int i = 0; i < numberOfVectors; i++)
		{
			TMP_InputField valueInputField = Instantiate(valueInputFieldPrefab);
			valueInputField.transform.SetParent(inputFieldContainer.transform, false);
            valueInputField.onValueChanged.AddListener((_) => UpdateComponentResultField(componentResultField, inputFieldContainer));

			if (i + 1 < numberOfVectors)
			{
				TextMeshProUGUI plusSignText = Instantiate(plusSignTextPrefab);
				plusSignText.transform.SetParent(inputFieldContainer.transform, false);
			}
		}
	}

    private void UpdateComponentResultField(TMP_InputField componentResultField , HorizontalLayoutGroup inputFieldContainer)
    {
        float totalResult = 0;
        TMP_InputField[] valueInputFields = inputFieldContainer.GetComponentsInChildren<TMP_InputField>();
        foreach (TMP_InputField inputField in valueInputFields)
        {
            bool isEvaluated = ExpressionEvaluator.Evaluate(inputField.text, out float expressionResult);
            if (isEvaluated) totalResult += expressionResult;
        }
        componentResultField.text = $"{totalResult}";
    }
}