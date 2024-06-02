using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientificNotation : IInteractable
{

    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GameObject viewScientificNotationUI;
	[SerializeField] private ActivityOneManager activityOneManager;

	public void Start()
    {
        return;
    }

    public override void Interact()
    {
        if (activityOneManager.isScientificNotationFinished == false)
        {
            _inputReader.SetUI();
            viewScientificNotationUI.SetActive(true);
        }
    }
}
