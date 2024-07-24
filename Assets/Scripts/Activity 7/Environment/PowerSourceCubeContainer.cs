using System;
using UnityEngine;

public class PowerSourceCubeContainer : IInteractableObject
{
	public static event Action InteractEvent;
	public override void Interact()
	{
		InteractEvent?.Invoke();
	}
}