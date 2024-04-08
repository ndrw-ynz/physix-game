using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientificNotation : IInteractable
{
    // button from UI needed here, or UI itself
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GameObject viewScientificNotationUI;
    public void Start()
    {
        return;
    }
    // need game object for UI.
    public override void Interact()
    {
        _inputReader.SetUI();
        viewScientificNotationUI.SetActive(true);
    }
}
