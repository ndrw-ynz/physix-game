using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class View1DKinematics : MonoBehaviour
{
	public static event Action<float> SubmitAccelerationAnswerEvent;

	[Header("Given Text")]
	[SerializeField] private TextMeshProUGUI accelerationGivenText;
    [Header("Prefabs")]
	[SerializeField] private CalcCompResult compResultPrefab;
    [Header("Input Field")]
    [SerializeField] private TMP_InputField inputField;
	[Header("Answer Area")]
	[SerializeField] private CalcAnswerArea answerArea;
	[Header("Layout Holders")]
	[SerializeField] private VerticalLayoutGroup compResultHolder;
	[Header("Buttons")]
	[SerializeField] private Button accelerateButton;

	private void OnEnable()
	{
		CalcCalculateButton.CalculateResultEvent += EvaluateInput;
		accelerateButton.onClick.AddListener(() => SubmitAccelerationAnswerEvent?.Invoke(answerArea.answerValue));
	}

	private void OnDisable()
	{
		accelerateButton.onClick.RemoveAllListeners();
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

	public void SetupGivenText(float initialVelocity, float finalVelocity, float totalTime)
	{
		accelerationGivenText.text = $"Initial Velocity = {initialVelocity} km/hr Final Velocity = {finalVelocity} km/hr Time = {totalTime} minutes";
	}
}