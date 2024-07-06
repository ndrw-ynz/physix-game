using System;
using UnityEngine;

public class CalcClearEntryButton : MonoBehaviour
{
    public static event Action ClearResultFieldEvent;

    public void OnClick()
    {
        ClearResultFieldEvent?.Invoke();
    }
}
