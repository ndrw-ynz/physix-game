using System;
using UnityEngine;

public class CalcCalculateButton : MonoBehaviour
{
    public static event Action CalculateResultEvent;

    public void OnClick()
    {
        CalculateResultEvent?.Invoke();
    }
}
