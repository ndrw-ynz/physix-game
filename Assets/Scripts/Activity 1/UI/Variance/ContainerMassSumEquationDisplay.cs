using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContainerMassSumEquationDisplay : MonoBehaviour
{
	[Header("Equation Container")]
    [SerializeField] private HorizontalLayoutGroup equationContainer;

	[Header("Result Field")]
	[SerializeField] private TMP_InputField resultField;

	[Header("Prefabs")]
	[SerializeField] private TMP_InputField numberInputFieldPrefab;
	[SerializeField] private TextMeshProUGUI plusSignTextPrefab;

	public float resultValue { get; private set; }

	public void SetupEquationDisplay(int addendsCount)
    {
		for (int i = 0; i < addendsCount; i++)
		{
			TMP_InputField numberInputField = Instantiate(numberInputFieldPrefab, equationContainer.transform, false);
			numberInputField.onValueChanged.AddListener((_) => UpdateEquationResultField());

			if (i + 1 < addendsCount)
			{
				TextMeshProUGUI plusSignText = Instantiate(plusSignTextPrefab, equationContainer.transform, false);
			}
		}
	}

	private void UpdateEquationResultField()
	{
		resultValue = 0;
		TMP_InputField[] numberInputFields = equationContainer.GetComponentsInChildren<TMP_InputField>();
		foreach (TMP_InputField numberInputField in numberInputFields)
		{
			resultValue += float.Parse(numberInputField.text);
		}
		resultField.text = $"{resultValue}";
	}
}
