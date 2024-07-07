using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalcDraggableNumber : MonoBehaviour
{
	[SerializeField] private Image image;
	[SerializeField] private TextMeshProUGUI numberText;
	public float value;

	public void SetupDraggableNumber(float value)
	{
		image.raycastTarget = false;
		numberText.raycastTarget = false;
		numberText.text = value.ToString();
		this.value = value;
	}

	public void Destroy()
	{
		Destroy(gameObject);
	}
}