using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EjectPanel : IInteractable
{
	[SerializeField] private InputReader _inputReader;
	[SerializeField] private GameObject endActivityUI;
	[SerializeField] private ActivityOneManager activityOneManager;
	public override void Interact()
	{
		if (true)
		{
			_inputReader.SetUI();
			endActivityUI.SetActive(true);
		}
	}
}
