using System;
using TMPro;
using UnityEngine;

public class VectorInfoDisplay : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI descriptorText;
	[SerializeField] private TMP_InputField polarVectorInfoField;
	[SerializeField] private TMP_InputField xComponentField;
	[SerializeField] private TMP_InputField yComponentField;

	public void SetupVectorInfoDisplay(int vectorNumber, VectorData vectorData)
	{
		descriptorText.text = $"Vector No. {vectorNumber}";
		polarVectorInfoField.text = $"{vectorData.magnitude}m {vectorData.angleMeasure}°";
		// Determine x and y component
		ExpressionEvaluator.Evaluate($"{vectorData.magnitude} * cos({vectorData.angleMeasure}*(pi/180))", out float xComponent);
		xComponent = (float) Math.Round(xComponent, 4);
		ExpressionEvaluator.Evaluate($"{vectorData.magnitude} * sin({vectorData.angleMeasure}*(pi/180))", out float yComponent);
		yComponent = (float)Math.Round(yComponent, 4);
		xComponentField.text = $"{xComponent}";
		yComponentField.text = $"{yComponent}";
	}
}
