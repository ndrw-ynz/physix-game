using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewProjectileMotion : MonoBehaviour
{
	[Header("Prefabs")]
	[SerializeField] private CalcCompResult compResultPrefab;
	[Header("Input Field")]
	[SerializeField] private TMP_InputField inputField;
	[Header("Layout Holders")]
	[SerializeField] private VerticalLayoutGroup compResultHolder;
	[Header("Calculation Status Indicator")]
	[SerializeField] private Image calcStatusImage;
	[SerializeField] private TextMeshProUGUI calcStatusText;

	private void OnEnable()
	{
		CalcCalculateButton.CalculateResultEvent += EvaluateInput;
		CalcInputField.UpdateInputField += UpdateCalcStatusIndicator;
	}

	#region Calculator

	private void EvaluateInput()
	{
		// Null check of inputField to counter unknown bug causing NullReferenceException.
		if (!inputField)
		{
			return;
		}

		// Evaluate expression from input field.
		string mathExpression = inputField.text.Replace('x', '*');
		bool canEvaluate = ExpressionEvaluator.Evaluate(mathExpression, out float result);
		if (canEvaluate)
		{
			// Create new CalcCompResult and add to compResultHolder.
			CalcCompResult compResult = Instantiate(compResultPrefab);
			compResult.transform.SetParent(compResultHolder.transform, false);
			compResult.SetupCompResult(result, inputField.text);
		}
		else
		{
			// TODO: show error or indicator
		}
	}

	private void UpdateCalcStatusIndicator(string mathExpression)
	{
		mathExpression = mathExpression.Replace('x', '*');
		bool canEvaluate = ExpressionEvaluator.Evaluate(mathExpression, out float _);
		if (canEvaluate)
		{
			calcStatusText.text = "Can Evaluate Expression";
			calcStatusImage.color = new Color32(175, 255, 155, 255);
		}
		else
		{
			calcStatusText.text = "Cannot Evaluate Expression";
			calcStatusImage.color = new Color32(200, 75, 55, 255);
		}
	}

	#endregion
}