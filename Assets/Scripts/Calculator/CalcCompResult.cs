using TMPro;
using UnityEngine;

public class CalcCompResult : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI expressionText;
    [Header("Values")]
    public float result;
    public string expression;

    public void SetupCompResult(float resultValue, string expressionValue)
    {
        resultText.text = resultValue.ToString();
        expressionText.text = expressionValue;

        result = resultValue;
        expression = expressionValue;
    }
}