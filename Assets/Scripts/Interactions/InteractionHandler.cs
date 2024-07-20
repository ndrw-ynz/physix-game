using UnityEngine;

public abstract class IInteractableObject : MonoBehaviour
{
	public abstract void Interact();
}

public class InteractionHandler : MonoBehaviour
{
	[SerializeField] private InputReader _input;
	public Transform interactorSource;
	public float interactRange = 3.0f;

	private IInteractable _selectedInteractable;

	void Start()
	{
		_input.InteractEvent += HandleInteract;
	}

	private void HandleInteract()
	{
		Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
		Ray r = Camera.main.ScreenPointToRay(screenCenter);
		if (Physics.Raycast(r, out RaycastHit hit, interactRange))
		{
			if (hit.collider.gameObject.TryGetComponent(out IInteractableObject interactObj))
			{
				interactObj.Interact();
			}
		}
	}
}