using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class View1DKinematics : MonoBehaviour
{
    [Header("Prefabs")]
	[SerializeField] private CalcCompResult compResultPrefab;
    [Header("Input Field")]
    [SerializeField] private TMP_InputField inputField;
	[Header("Layout Holders")]
	[SerializeField] private VerticalLayoutGroup compResultHolder;
	// insert the area where the draggable numbers be in bruh

	private void Start()
	{
		CalcCalculateButton.CalculateResultEvent += EvaluateInput;
	}

	private void EvaluateInput()
	{
		// Null check of inputField to counter unknown bug causing NullReferenceException.
		if (!inputField)
		{
			return;
		}

		// Evaluate expression from input field.
		string mathExpression = inputField.text.Replace('x', '*');
		bool evaluationResult = ExpressionEvaluator.Evaluate(mathExpression, out float result);
		if (evaluationResult)
		{
			// Create new CalcCompResult and add to compResultHolder.
			CalcCompResult compResult = Instantiate(compResultPrefab);
			compResult.transform.SetParent(compResultHolder.transform, false);
			compResult.SetupCompResult(result, inputField.text);
		} else
		{
			// TODO: popup message for invalid computation.
		}
	}
}