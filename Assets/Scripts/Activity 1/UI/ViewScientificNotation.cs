using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ViewScientificNotation : MonoBehaviour
{
    public TextMeshProUGUI measurementText;
    public static event Action<ViewScientificNotation> OpenViewEvent;

    private void OnEnable()
    {
        OpenViewEvent?.Invoke(this);
    }
}
