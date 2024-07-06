using System;
using UnityEngine;

public class CalcOperatorButton : MonoBehaviour
{
    public static event Action<string> OperatorInsertEvent;
    private string _operator;
    private bool _isInitialized;
	private void Start()
	{
		if (!_isInitialized)
		{
			_operator = transform.name;
			_isInitialized = true;
		}
	}

	public void OnClick()
	{
		OperatorInsertEvent?.Invoke(_operator);
	}
}