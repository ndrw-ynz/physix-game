using System;
using UnityEngine;
using UnityEngine.UI;

public enum UnitSymbol
{
    Meter,
    Second
}

public class CalcUnitSymbolButton : MonoBehaviour
{
    public static event Action UpdateClickedEvent;

    [SerializeField] private Image buttonImage;
    private bool isClicked;
    public UnitSymbol unitSymbol;

	private void OnEnable()
	{
		UpdateClickedEvent += ResetState;
	}

    public void ResetState()
    {
        isClicked = false;
        buttonImage.color = new Color32(255, 255, 255, 255);
    }

	public void OnClick()
    {
        // Implemented on all button instances.
        UpdateClickedEvent?.Invoke();

        // Afterwards, this portion is only implemented on clicked instance.
		isClicked = true;
		buttonImage.color = new Color32(175, 255, 155, 255);
	}
}