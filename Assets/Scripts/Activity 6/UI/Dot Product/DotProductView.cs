using System;
using TMPro;
using UnityEngine;

public class DotProductAnswerSubmission
{
	public float? xCoordScalarProduct { get; private set; }
	public float? yCoordScalarProduct { get; private set; }
	public float? zCoordScalarProduct { get; private set; }

	public float? dotProduct { get; private set; }

	public DotProductAnswerSubmission(
		float? xCoordScalarProduct,
		float? yCoordScalarProduct,
		float? zCoordScalarProduct,
		float? dotProduct
		)
	{
		this.xCoordScalarProduct = xCoordScalarProduct;
		this.yCoordScalarProduct = yCoordScalarProduct;
		this.zCoordScalarProduct = zCoordScalarProduct;
		this.dotProduct = dotProduct;
	}
}

public class DotProductView : MonoBehaviour
{
	public event Action OpenViewEvent;
	public event Action QuitViewEvent;
	public event Action<DotProductAnswerSubmission> SubmitAnswerEvent;

	[Header("Vector Given Fields")]
	[SerializeField] private TMP_InputField satelliteDishVector;
	[SerializeField] private TMP_InputField targetObjectVector;

	[Header("Dot Product Equation Displays")]
	[SerializeField] private ProductEquationDisplay xCoordScalarProductDisplay;
	[SerializeField] private ProductEquationDisplay yCoordScalarProductDisplay;
	[SerializeField] private ProductEquationDisplay zCoordScalarProductDisplay;
	[SerializeField] private SumEquationDisplay dotProductDisplay;

	private void OnEnable()
	{
		OpenViewEvent?.Invoke();
	}

	public void SetupDotProductView(DotProductData data)
	{
		ClearAllFields();
		satelliteDishVector.text = $"({data.satelliteDishVector.x}, {data.satelliteDishVector.y}, {data.satelliteDishVector.z})";
		targetObjectVector.text = $"({data.targetObjectVector.x}, {data.targetObjectVector.y}, {data.targetObjectVector.z})";
	}

	private void ClearAllFields()
	{
		xCoordScalarProductDisplay.ResetState();
		yCoordScalarProductDisplay.ResetState();
		zCoordScalarProductDisplay.ResetState();
		dotProductDisplay.ResetState();
	}

	public void OnSubmitButtonClick()
	{
		DotProductAnswerSubmission submission = new DotProductAnswerSubmission(
			xCoordScalarProduct: xCoordScalarProductDisplay.productValue,
			yCoordScalarProduct: yCoordScalarProductDisplay.productValue,
			zCoordScalarProduct: zCoordScalarProductDisplay.productValue,
			dotProduct: dotProductDisplay.sumValue
			);

		SubmitAnswerEvent?.Invoke(submission);
	}

	public void OnQuitButtonClick()
	{
		gameObject.SetActive(false);
		QuitViewEvent?.Invoke();
	}
}