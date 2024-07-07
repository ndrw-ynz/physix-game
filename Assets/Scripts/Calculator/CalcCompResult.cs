using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CalcCompResult : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	[Header("Prefabs")]
	public CalcDraggableNumber draggableNumberPrefab;
	[Header("Text")]
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI expressionText;
    [Header("Values")]
    public float result;
    public string expression;

	private CalcDraggableNumber currentDraggableNumber;

    public void SetupCompResult(float resultValue, string expressionValue)
    {
        resultText.text = resultValue.ToString();
        expressionText.text = expressionValue;

        result = resultValue;
        expression = expressionValue;
    }

	public void OnBeginDrag(PointerEventData eventData)
	{
		currentDraggableNumber = Instantiate(draggableNumberPrefab);
		currentDraggableNumber.transform.position = eventData.position;
		currentDraggableNumber.transform.SetParent(transform.root);
		currentDraggableNumber.SetupDraggableNumber(result);
	}

	public void OnDrag(PointerEventData eventData)
	{
		currentDraggableNumber.transform.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		currentDraggableNumber.Destroy();
	}
}