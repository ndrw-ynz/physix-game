using TMPro;
using UnityEngine;

public abstract class IInteractableObject : MonoBehaviour
{
	public bool isInteractable { get; private set; }
	public abstract void Interact();

	public abstract string GetInteractionDescription();

	public void SetInteractable(bool isInteractable)
	{
		this.isInteractable = isInteractable;
	}
}

public class InteractionHandler : MonoBehaviour
{
	[SerializeField] private InputReader _input;
	public Transform interactorSource;
	public float interactRange = 3.0f;

	public GameObject interactionUI;
	public TextMeshProUGUI interactionText;

	void Start()
	{
		_input.InteractEvent += HandleInteract;
	}

    private void OnDestroy()
    {
        _input.InteractEvent -= HandleInteract;
    }

    private void Update()
	{
		// Update for displaying interactionUI
		Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
		Ray r = Camera.main.ScreenPointToRay(screenCenter);

		bool hitInteractableObject = false;

		if (Physics.Raycast(r, out RaycastHit hit, interactRange))
		{
			IInteractableObject interactableObject = hit.collider.GetComponent<IInteractableObject>();

			if (interactableObject != null && interactableObject.isInteractable == true)
			{
				hitInteractableObject = true;
				interactionText.text = interactableObject.GetInteractionDescription();
			}
		}

		interactionUI.SetActive(hitInteractableObject);
	}

	private void HandleInteract()
	{
		Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
		Ray r = Camera.main.ScreenPointToRay(screenCenter);
		if (Physics.Raycast(r, out RaycastHit hit, interactRange))
		{
			if (hit.collider.gameObject.TryGetComponent(out IInteractableObject interactObj))
			{
				if (interactObj.isInteractable == true) {
					interactionUI.SetActive(false);
					interactObj.Interact(); 
				}
			}
		}
	}
}