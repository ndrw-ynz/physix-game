using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewVariance : MonoBehaviour
{
	public static event Action<ViewVariance> OpenVarianceEvent;
	public static event Action<float> SubmitVarianceEvent;
	public List<Draggable> givenNumbers;
	public List<OperandButton> operandButtons;
	public List<OperatorButton> operatorButtons;
	public ComputationResultButton computationResultButton;
	public AnswerDropHandler answerDropHandler;
	public Button submitVarianceAnswerButton;
	private void OnEnable()
	{
		// Initializes draggable given numbers.
		foreach (Draggable gn in givenNumbers)
		{
			gn.Initialize();
		}
		// Initialize operand buttons.
		foreach (OperandButton ob in operandButtons)
		{
			ob.Initialize();
		}
		// Initialize operator buttons.
		foreach (OperatorButton ob in operatorButtons)
		{
			ob.Initialize();
		}
		// Initialize computation result button
		computationResultButton.Initialize();

		submitVarianceAnswerButton.onClick.AddListener(() => SubmitVarianceAnswer(answerDropHandler.answerValue));

		OpenVarianceEvent?.Invoke(this);
	}

	public void SubmitVarianceAnswer(float varianceAnswer)
	{
		SubmitVarianceEvent?.Invoke(varianceAnswer);
	}

	public void OnDisable()
	{
		submitVarianceAnswerButton.onClick.RemoveAllListeners();
	}
}
