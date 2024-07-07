using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CalcAnswerArea : MonoBehaviour, IDropHandler
{
	[SerializeField] private TextMeshProUGUI answerText;
	public float answerValue;

	public void OnDrop(PointerEventData eventData)
	{
		CalcCompResult compResult = eventData.pointerDrag.GetComponent<CalcCompResult>();
		answerValue = compResult.result;
		answerText.text = answerValue.ToString();
	}

	public void ResetAnswerArea()
	{
		answerText.text = "N/A";
		answerValue = 0;
	}
}