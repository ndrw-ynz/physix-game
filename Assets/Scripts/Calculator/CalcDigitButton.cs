using System;
using UnityEngine;

public class CalcDigitButton : MonoBehaviour
{
    public static event Action<int> DigitInsertEvent;
	private int _digit;
	private bool _isInitialized;
	private void OnEnable()
	{
		if (!_isInitialized)
		{
			_digit = int.Parse(transform.name);
			_isInitialized = true;
		}
	}

	public void OnClick()
    {
        DigitInsertEvent?.Invoke(_digit);
    }
}