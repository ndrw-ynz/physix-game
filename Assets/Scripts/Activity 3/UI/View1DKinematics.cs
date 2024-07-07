using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class View1DKinematics : MonoBehaviour
{
	public static event Action<float> SubmitAccelerationAnswerEvent;
	public static event Action<float> SubmitFreeFallAnswerEvent;

	[Header("Given Text")]
	[SerializeField] private TextMeshProUGUI accelerationGivenText;
	[SerializeField] private TextMeshProUGUI freeFallGivenText;
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
	[SerializeField] private Button freeFallButton;

	private void OnEnable()
	{
		CalcCalculateButton.CalculateResultEvent += EvaluateInput;
		accelerateButton.onClick.AddListener(() => SubmitAccelerationAnswerEvent?.Invoke(answerArea.answerValue));
		freeFallButton.onClick.AddListener(() => SubmitFreeFallAnswerEvent?.Invoke(answerArea.answerValue));
	}

	private void OnDisable()
	{
		accelerateButton.onClick.RemoveAllListeners();
		freeFallButton.onClick.RemoveAllListeners();
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

	public void SetupAccelerationGivenText(float initialVelocity, float finalVelocity, float totalTime)
	{
		accelerationGivenText.text = $"Initial Velocity = {initialVelocity} km/hr Final Velocity = {finalVelocity} km/hr Time = {totalTime} minute/s";
	}

	public void SetupFreeFallGivenText(int totalTime)
	{
		freeFallGivenText.text = $"Time = {totalTime} minute/s / {totalTime * 60} second/s";
	}

	public void SwitchToFreeFallView()
	{
		accelerationGivenText.gameObject.SetActive(false);
		freeFallGivenText.gameObject.SetActive(true);

		accelerateButton.gameObject.SetActive(false);
		freeFallButton.gameObject.SetActive(true);

		answerArea.ResetAnswerArea();
	}
}