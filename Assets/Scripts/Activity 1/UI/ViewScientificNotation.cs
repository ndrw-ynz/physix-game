using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class ViewScientificNotation : MonoBehaviour
{
    private TMP_InputField _inputField;
    public TextMeshProUGUI measurementText;
    public static event Action<ViewScientificNotation> OpenViewEvent;
    public static event Action<string> SubmitAnswerEvent;
    private void OnEnable()
    {
        _inputField = GetComponentInChildren<TMP_InputField>();
        OpenViewEvent?.Invoke(this);
    }

    public void Submit()
    {
        Debug.Log("Submitted answer!");
        SubmitAnswerEvent?.Invoke(_inputField.text);
    }
}
