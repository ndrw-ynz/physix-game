using TMPro;
using UnityEngine;

public class MassCoordinateProductDisplay : MonoBehaviour
{
	[Header("Text")]
	[SerializeField] private TextMeshProUGUI massNumberText;
	[Header("Input Fields")]
	[SerializeField] private TMP_InputField numberInputFieldOne;
	[SerializeField] private TMP_InputField numberInputFieldTwo;
	[SerializeField] private TMP_InputField resultInputField;
	public int? productValue { get; private set; }

	public void OnValueChange()
	{
		string inputOne = numberInputFieldOne.text;
		if (string.IsNullOrEmpty(inputOne)) inputOne = "0";
		string inputTwo = numberInputFieldTwo.text;
		if (string.IsNullOrEmpty(inputTwo)) inputTwo = "0";

		bool canEvaluate = ExpressionEvaluator.Evaluate($"{inputOne}*{inputTwo}", out int result);
		if (canEvaluate)
		{
			productValue = result;
			resultInputField.text = $"{result}";
		} else
		{
			productValue = null;
			resultInputField.text = "N/A";
		}
	}

	public void SetupText(int massNumber)
	{
		massNumberText.text += massNumber;
	}
}