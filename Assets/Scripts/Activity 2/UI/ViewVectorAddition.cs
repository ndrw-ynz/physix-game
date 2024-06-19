using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewVectorAddition : MonoBehaviour
{
    [Header("Component Fields")]
    public TMP_InputField xComponentResultField;
	public TMP_InputField yComponentResultField;
	[Header("Magnitude Fields")]
    public TMP_InputField magnitudeInputFieldOne;
	public TMP_InputField magnitudeInputFieldTwo;
    public TMP_InputField magnitudeResultField;
    [Header("Direction Fields")]
    public TMP_InputField directionInputNumerator;
	public TMP_InputField directionInputDenominator;
	public TMP_InputField directionResultField;
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

        // Setting up magnitude fields
        SetupMagnitudeFields();

        // Setting up direction fields
        SetupDirectionFields();
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

    private void SetupMagnitudeFields()
    {
        magnitudeInputFieldOne.onValueChanged.AddListener((_) => UpdateMagnitudeResultField());
        magnitudeInputFieldTwo.onValueChanged.AddListener((_) => UpdateMagnitudeResultField());
	}

    private void UpdateMagnitudeResultField()
    {
        double totalResult = 0;

        bool isInputOneEvaluated = ExpressionEvaluator.Evaluate($"({magnitudeInputFieldOne.text})^2", out double inputOneResult);
        if (isInputOneEvaluated) totalResult += inputOneResult;

        bool isInputTwoEvaluated = ExpressionEvaluator.Evaluate($"({magnitudeInputFieldTwo.text})^2", out double inputTwoResult);
        if (isInputTwoEvaluated) totalResult += inputTwoResult;

        totalResult = Math.Round(Math.Sqrt(totalResult), 4);

        magnitudeResultField.text = $"{totalResult}";
	}

    private void SetupDirectionFields()
    {
		directionInputNumerator.onValueChanged.AddListener((_) => UpdateDirectionResultField());
        directionInputDenominator.onValueChanged.AddListener((_) => UpdateDirectionResultField());
	}

	private void UpdateDirectionResultField()
    {
		bool isNumeratorEvaluated = ExpressionEvaluator.Evaluate($"{directionInputNumerator.text}", out double numeratorResult);
		bool isDenominatorEvaluated = ExpressionEvaluator.Evaluate($"{directionInputDenominator.text}", out double denominatorResult);

        if (isNumeratorEvaluated == true && isDenominatorEvaluated == true)
        {
            double result = Math.Atan2(numeratorResult, denominatorResult) * (180/Math.PI);
            result = Math.Round(result, 4);
            directionResultField.text = $"{result}";
        } else
        {
            directionResultField.text = "/";
        }
	}
}