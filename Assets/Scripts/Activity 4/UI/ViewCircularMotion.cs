using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ViewCircularMotion : MonoBehaviour
{
	public static event Action<float> SubmitCentripetalAccelerationAnswerEvent;

	[Header("Problem Display UI")]
	[SerializeField] private TextMeshProUGUI problemTypeText;
	[SerializeField] private TextMeshProUGUI givenValuesText;
	[SerializeField] private TextMeshProUGUI problemText;
	[Header("Prefabs")]
	[SerializeField] private CalcCompResult compResultPrefab;
	[Header("Input Field")]
	[SerializeField] private TMP_InputField inputField;
	[Header("Answer Area")]
	[SerializeField] private CalcAnswerArea answerArea;
	[Header("Layout Holders")]
	[SerializeField] private VerticalLayoutGroup compResultHolder;
	[Header("Calculation Status Indicator")]
	[SerializeField] private Image calcStatusImage;
	[SerializeField] private TextMeshProUGUI calcStatusText;
	[Header("Buttons")]
	[SerializeField] private Button centripetalAccelerationButton;
	[Header("Overlays")]
	[SerializeField] private Image calculatorAreaOverlay;
	[SerializeField] private Image satelliteModuleOverlay;

	private void OnEnable()
	{
		CalcCalculateButton.CalculateResultEvent += EvaluateInput;
		CalcInputField.UpdateInputField += UpdateCalcStatusIndicator;

		centripetalAccelerationButton.onClick.AddListener(() => SubmitCentripetalAccelerationAnswerEvent?.Invoke(answerArea.answerValue));
	}

	private void OnDisable()
	{
		centripetalAccelerationButton.onClick.RemoveAllListeners();
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

	#region Problem Display UI
	public void SetupCentripetalAccelerationProblemDisplay(int satelliteRadiusValue, int satelliteTimePeriodValue)
	{
		problemTypeText.text = "Circular Motion";
		givenValuesText.text = $"Satellite Radius = {satelliteRadiusValue} meters, Satellite Time Period = {satelliteTimePeriodValue} seconds";
		problemText.text = "What is the centripetal acceleration of the satellite?";
	}

	public void UpdateProblemText(string problemText)
	{
		this.problemText.text = problemText;
	}

	#endregion

	public void SetOverlays(bool value)
	{
		calculatorAreaOverlay.gameObject.SetActive(value);
		satelliteModuleOverlay.gameObject.SetActive(value);
	}


	public void ResetState()
	{
		answerArea.ResetAnswerArea();
	}
}