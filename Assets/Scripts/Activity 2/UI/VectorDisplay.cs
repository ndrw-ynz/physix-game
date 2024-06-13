using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VectorDisplay : MonoBehaviour
{
	// this just contains information about the VIEW DISPLAYS FOR UI
	// list all the elements and then link them up! then create methods for use of the display.
	// seven components here...
	[Header("Text Information")]
	public TextMeshProUGUI vectorTextInfo;
	[Header("Equation Holders")]
	public NumericalExpressionHolder xComponentEquationHolder;
	public NumericalExpressionHolder yComponentEquationHolder;
	[Header("Prefabs")]
	public DraggableNumericalExpression draggableNumericalExpressionPrefab;
	[Header("Choice Holders")]
	public NumericalExpressionHolder choiceHolderLeft;
	public NumericalExpressionHolder choiceHolderRight;
	[Header("n-Component Input Text")]
	public TMP_InputField xComponentInputText;
	public TMP_InputField yComponentInputText;
	[Header("Submit Button")]
	public Button submitButton;

	public void SetupVectorDisplay(int magnitudeValue, int directionValue)
	{
		vectorTextInfo.text = $"{magnitudeValue}m {directionValue}°";
	}
}
