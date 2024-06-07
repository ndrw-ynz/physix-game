using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePanel : IInteractable
{
	[SerializeField] private InputReader _inputReader;
	[SerializeField] private GameObject view;
	[SerializeField] private ActivityTwoManager activityTwoManager;

	public override void Interact()
	{
		_inputReader.SetUI();
		view.gameObject.SetActive(true);
	}
}
